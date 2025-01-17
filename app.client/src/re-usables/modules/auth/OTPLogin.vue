<template>
    <div>
        <GuestLayout>
            <authentication-card>
                <template #title>
                    OTP Login
                </template>
                <template #body>
                    <form @submit.prevent="submitForm()">
                        <field-group label="OTP Code" :valErrors="valErrors.OTPCode">
                            <w-input type="text" v-model="form.OTPCode" />
                        </field-group>
                        <div class="mt-4">
                            <w-button type="submit" class="w-full !text-center !rounded-full bg-theme-1 hover:bg-theme-1 link">SUBMIT</w-button>
                        </div>
                    </form>
                    <form @submit.prevent="FnResendOTP()">
                        <div class="text-center mt-2 sm:mt-4 flex justify-center">
                            <button type="submit" class="text-blue-500 text-xs">Resend OTP Code</button>
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
            return { router };
        },
        data() {
            return {
                form: {
                    userNo: "",
                    OTPCode: "",
                },
                valErrors: [],
            }
        },
        mounted() {
            this.form.userNo = this.$root.authUser.userNo;
        },
        methods: {
            submitForm() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.form)
                };
                fetch(this.appConfig.baseApiRoute + 'authentication/OTPLogin', requestOptions)
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
                            if (authUser != undefined) {
                                localStorage.setItem("authUser", JSON.stringify(authUser));
                                localStorage.setItem("sessionToken", authUser.sessionToken);
                                this.$root.authUser = authUser;
                                this.router.push({ name: "dashboard" });
                            }
                        }
                        this.$root.loader.isLoading = false;

                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: String(error) });
                    });
            },
            FnResendOTP() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: null
                };
                fetch('/api/authentication/ResendOTP', requestOptions)
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
                        } else {
                            var msg = "An OTP Code has been sent to your email. Use the code for authentication below.";
                            this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: String(error) });
                    });
            }
        },
        watch: {
            "form.OTPCode": function () {
                this.form.OTPCode = this.form.OTPCode.replaceAll(" ", "");
            }
        }
    }
</script>
