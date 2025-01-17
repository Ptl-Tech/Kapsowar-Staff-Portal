<template>
    <div class="flex justify-center">
        <FormPageTemplate :title="'Change Password'" class="grow sm:max-w-2xl">
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Current Password">
                        <w-input type="password" v-model="form.currentPassword" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="New Password" :valErrors="valErrors.NewPassword">
                        <w-input type="password" v-model="form.NewPassword" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Confirm Password" :valErrors="valErrors.ConfirmPassword">
                        <w-input type="password" v-model="form.ConfirmPassword" />
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <w-button @click="submitForm" class="!text-center bg-theme-1 hover:bg-theme-1 link">SUBMIT</w-button>
            </template>
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useRouter } from 'vue-router'
    export default {
        components: { ...W },
        setup() {
            const router = useRouter()
            return {router };
        },
        data() {
            return {
                form: {
                    currentPassword: "",
                    NewPassword: "",
                    ConfirmPassword: "",
                },
                valErrors:[],
            }
        },
        mounted() {
            this.form.EmployeeNo = this.$route.params.no;
        },
        methods: {
            submitForm() {
                var validatePass = this.validatePassword();
                if (validatePass != "success") {
                    var msg = "<b>The Password must contain:</b><br/> - at least one capital letter.<br/>-at least one special character.<br/>-at least 8 characters long.<br/>-both letters and numbers.";
                    this.$root.FnNotification({ type: "modal", theme: "red", message:msg });
                    return;
                }
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.form)
                };
                fetch('/api/authentication/changepassword', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        
                        if (data && data.valErrors) {
                            if (data.valErrors != null) {
                                this.valErrors = data.valErrors;
                            }
                        }
                        else if (data && data.errors) {
                            this.$root.FnNotification({ type: "modal", theme: "red", message:String(data.errors) });
                        } 
                        else {
                            localStorage.removeItem("authUser");
                            this.router.push('/auth/login');
                            this.router.go();
                            var msg = "Password changed successfully. Use the new password to login.";
                            this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                        }
                        this.$root.loader.isLoading = false;
                        
                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: String(error) });
                    });
            },
            validatePassword() {
                var password = this.form.NewPassword;
                // Regular expressions for each criteria
                const hasCapital = /[A-Z]/.test(password);// At least one capital letter
                const hasSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password);  // At least one special character
                const hasMinimumLength = password.length >= 8?true:false;     // Minimum 8 characters
                const hasNumberAndLetter = /[A-Za-z].*\d|\d.*[A-Za-z]/.test(password);  // Must have both letters and numbers
                // Check all criteria
                if (!hasCapital) {
                    return "Password must contain at least one capital letter.";
                }
                if (!hasSpecialChar) {
                    return "Password must contain at least one special character.";
                }
                if (!hasMinimumLength) {
                    return "Password must be at least 8 characters long.";
                }
                if (!hasNumberAndLetter) {
                    return "Password must contain both letters and numbers.";
                }
                // Password meets all criteria
                return "success";
            },
        }
    }
</script>
