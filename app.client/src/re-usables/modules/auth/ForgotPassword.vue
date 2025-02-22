<template>
    <div>
        <GuestLayout>
            <authentication-card>
                <template #title>
                    Forgot Password
                </template>
                <template #body>
                    <form @submit.prevent="submitForm()">
                        <field-group label="Staff No." :valErrors="valErrors.userNo" :showMandatory="true">
                            <w-input type="text" v-model="form.userNo" required="true" />
                        </field-group>
                        <field-group label="Select Company" :valErrors="valErrors.NavCompany">
                            <NavCompaniesSelect v-model="form.NavCompany" />
                        </field-group>
                        <div class="mt-4">
                            <w-button type="submit" class="w-full !text-center !rounded-full bg-theme-1 hover:bg-theme-1 link">SEND RESET TOKEN</w-button>
                        </div>
                        <div class="text-center mt-2 sm:mt-4 flex justify-center">
                            <router-link to="/auth/login" class="text-blue-500 text-sm">Go to Login</router-link>
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
    import NavCompaniesSelect from '@/re-usables/components/NavCompaniesSelect.vue';
    export default {
        components: { AuthenticationCard, FieldGroup, WInput, WButton, GuestLayout, NavCompaniesSelect },
        setup() {
            const router = useRouter()
            return {router };
        },
        data() {
            return {
                form: {
                    userNo: "",
                    NavCompany: "",
                },
                valErrors: [],
            }
        },
        
        methods: {
            submitForm() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.form)
                };
                fetch('/api/authentication/ForgotPassword', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        this.$root.loader.isLoading = false;
                        if (data && data.valErrors) {
                            if (data.valErrors != null) {
                                this.valErrors = data.valErrors;
                            }
                        }
                        else if (data && data.errors) {
                            this.$root.FnNotification({ type: "modal", theme: "red", message: String(data.errors) });
                        } 
                        else {
                            this.router.push('/auth/reset-password/' + this.form.userNo + '?navCompany=' + this.form.NavCompany);
                            var msg = "A reset token code has been sent to your phone number via SMS. Kindly use the code below to reset your password.";
                            this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                        }
                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: String(error) });
                    });
            }
        },
    }
</script>
