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
                            <w-input type="text" v-model="form.userNo" required/>
                        </field-group>
                        <field-group label="Password" :valErrors="valErrors.Password" :showMandatory="true">
                            <w-input type="password" v-model="form.Password" required/>
                        </field-group>
                        <div class="mt-4">
                            <w-button type="submit" class="w-full !text-center !rounded-full bg-theme-1 link hover:bg-theme-1">LOGIN</w-button>
                        </div>
                        <div class="text-center mt-2 sm:mt-4 flex gap-1 justify-center">
                          <router-link to="/auth/forgot-password" class="text-blue-500 text-sm">Forgot Password?</router-link>
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
                    Password: "",
                    sessionToken:"",
                },
                valErrors: [],
            }
        },
        mounted() {
            if (this.$root.isAuthenticated) {
                this.router.push('/dashboard');
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
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = String(data.errors);
                        } 
                        else {
                            var authUser = data.authUser;
                            if(authUser != undefined){
                                localStorage.setItem("authUser", JSON.stringify(authUser));
                                localStorage.setItem("sessionToken", authUser.sessionToken);
                                this.$root.authUser = authUser;
                                if (authUser.IsMFAVerified) {
                                    this.router.push('/hmis/security/visitor/list?status=Active-Today');
                                } else {
                                    this.router.push('/auth/otp-login');
                                    var msg = data.msg
                                    this.$root.FnNotification(msg, 'bg-green-500', false);
                                }
                            }
                        }
                        this.$root.loader.isLoading = false;
                        this.$root.loader.message = "";
                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = String(error);
                    });
            },
        },
    }
</script>
