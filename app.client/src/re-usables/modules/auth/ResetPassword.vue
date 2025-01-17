<template>
    <div>
        <GuestLayout>
            <authentication-card>
                <template #title>
                    Reset Password
                </template>
                <template #body>
                    <form @submit.prevent="submitForm()">
                        <field-group label="Reset Token Code" :valErrors="valErrors.ResetTokenCode">
                            <w-input type="text" v-model="form.ResetTokenCode" />
                        </field-group>
                        <field-group label="New Password" :valErrors="valErrors.NewPassword">
                            <w-input type="password" v-model="form.NewPassword"/>
                        </field-group>
                        <field-group label="Confirm Password" :valErrors="valErrors.ConfirmPassword">
                            <w-input type="password" v-model="form.ConfirmPassword"/>
                        </field-group>
                        <div class="mt-4">
                            <w-button type="submit" class="w-full !text-center !rounded-full bg-theme-1 hover:bg-theme-1 link">SUBMIT</w-button>
                        </div>
                        <div class="text-center mt-2 sm:mt-4 flex justify-center">
                            <router-link to="/auth/forgot-password" class="text-blue-500 text-sm">Resend Reset Token</router-link>
                        </div>
                    </form>
                </template>
            </authentication-card>
        </GuestLayout>
    </div>
</template>
<script>
    import AuthenticationCard from '@/re-usables/components/AuthenticationCard.vue';
    import FieldGroup from '@/re-usables/components/FieldGroup.vue';
    import WInput from '@/re-usables/components/WInput.vue';
    import WButton from '@/re-usables/components/WButton.vue';
    import GuestLayout from '@/re-usables/page-templates/Guest.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { AuthenticationCard, FieldGroup, WInput, WButton, GuestLayout },
        setup() {
            const router = useRouter()
            return {router };
        },
        data() {
            return {
                form: {
                    userNo: "",
                    NewPassword: "",
                    ConfirmPassword: "",
                    ResetTokenCode: "",
                },
                valErrors:[],
            }
        },
        mounted() {
            this.form.userNo = this.$route.params.no;
        },
        methods: {
            submitForm() {
                var validatePass = this.validatePassword();
                if (validatePass != "success") {
                    this.$root.errorModal.isShow = true;
                    this.$root.errorModal.message = "<b>The Password must contain:</b><br/> - at least one capital letter.<br/>-at least one special character.<br/>-at least 8 characters long.<br/>-both letters and numbers.";
                    return;
                }
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.form)
                };
                fetch('/api/authentication/resetpassword', requestOptions)
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
                            this.$root.FnNotification({ type: "modal", theme: "red", message: String(data.errors) });
                        } 
                        else {
                            var authUser = data;
                            if(authUser != undefined){
                                localStorage.setItem("authUser",JSON.stringify(authUser.authUser));
                                this.router.push('/auth/login');
                                var msg = "Password reset successfully. Use the new password to login.";
                                this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                            }
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
        },
        watch: {
            "form.ResetTokenCode": function () {
                this.form.ResetTokenCode = this.form.ResetTokenCode.replaceAll(" ", "");
            }
        }
    }
</script>
