<template>
    <div>
        <GuestLayout>
            <authentication-card>
                <template #title>
                    Staff Portal Login
                </template>
                <template #body>
                    <form @submit.prevent="submitForm()">
                        <field-group label="Staff No." :valErrors="valErrors.userNo" :showMandatory="true">
                            <w-input type="text" v-model="form.userNo" required />
                        </field-group>
                        <field-group label="Password" :valErrors="valErrors.password" :showMandatory="true">
                            <w-input type="password" v-model="form.password" required />
                        </field-group>
                        <field-group label="Select Company" :valErrors="valErrors.NavCompany">
                            <NavCompaniesSelect v-model="form.NavCompany" />
                        </field-group>
                        <div class="mt-4">
                            <w-button type="submit" class="w-full !text-center !rounded-full bg-theme-1 link hover:bg-theme-1">LOGIN</w-button>
                        </div>
                        <div class="text-center mt-2 sm:mt-4 flex gap-1 justify-center">
                            <router-link to="/auth/forgot-password" class="text-blue-500 text-sm">Forgot Password?/First time login?</router-link>
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
    import NavCompaniesSelect from '@/re-usables/components/NavCompaniesSelect.vue';
    import { useRouter } from 'vue-router'
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
                    password: "",
                    sessionToken: "",
                    NavCompany: "",
                },
                valErrors: [],
            }
        },
        mounted() {
            if (this.$root.isAuthenticated) {
                this.router.push('/dashboard');
            }
            if (this.$route.query.verification != undefined && this.$route.query.verification) {
                var msg = "Account verified successfully. You can now login and access the portal services.";
                this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
            }
        },
        methods: {
            submitForm() {
                this.$root.ClearSession();
                //
                var sessionToken = localStorage.getItem("sessionToken");
                if (sessionToken != null && sessionToken != "undefined") {
                    this.form.sessionToken = sessionToken;
                }
                this.$root.loader.isLoading = true;
                this.$root.loader.message = "Authenticating/sending OTP";
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.form)
                };
                fetch(this.appConfig.baseApiRoute+'authentication/login', requestOptions)
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
                            var authUser = data.authUser;
                            if(authUser != undefined){
                                localStorage.setItem("authUser", JSON.stringify(authUser));
                                this.$root.authUser = authUser;
                                if (authUser.isMFAVerified) {
                                    this.$router.push({name:"dashboard"});
                                } else {
                                    this.router.push({name:"authOTPLogin"});
                                    var msg = data.msg
                                    this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                                }
                            }
                        }
                        this.$root.loader.isLoading = false;
                        this.$root.loader.message = "";
                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: String(error) });
                    });
            },
        },
    }
</script>
