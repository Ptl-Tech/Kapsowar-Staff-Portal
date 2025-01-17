<template>
    <div>
        <GuestLayout>
            <authentication-card>
                <template #title>
                    Register Portal Account
                </template>
                <template #body>
                  <form @submit.prevent="submitForm()">
                      <div class="flex flex-col gap-1">
                          <field-group label="Company Name" :valErrors="valErrors.companyName" :showMandatory="true">
                              <w-input type="text" v-model="form.companyName" required />
                          </field-group>
                          <field-group label="Company Pin No." :valErrors="valErrors.kraPinNo" :showMandatory="true">
                              <w-input type="text" v-model="form.kraPinNo" required />
                          </field-group>
                          <field-group label="Official Email" :valErrors="valErrors.companyEmail" :showMandatory="true">
                              <w-input type="email" v-model="form.companyEmail" required />
                          </field-group>
                          <field-group label="Password" :valErrors="valErrors.password" :showMandatory="true">
                              <w-input type="password" v-model="form.password" required />
                          </field-group>
                          <field-group label="Confirm Password" :valErrors="valErrors.confirmPassword" :showMandatory="true">
                              <w-input type="password" v-model="form.confirmPassword" required />
                          </field-group>
                      </div>
                    <div class="mt-4">
                      <w-button type="submit" class="w-full !text-center !rounded-full bg-theme-1 link hover:bg-theme-1">LOGIN</w-button>
                    </div>
                    <div class="text-center mt-2 sm:mt-4 flex gap-1 justify-center">
                      <router-link to="/auth/forgot-password" class="text-blue-500 text-sm">Forgot Password?</router-link>
                      <span> or </span>
                      <router-link to="/auth/register" class="text-blue-500 text-sm">Not Registered?</router-link>
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

    import Grid from '@/re-usables/components/Grid.vue';
import GridCol from '@/re-usables/components/GridCol.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { AuthenticationCard, FieldGroup, WInput, WButton, GuestLayout,Grid,GridCol },
        setup() {
            const router = useRouter()
            return {router };
        },
        data() {
            return {
                form: {
                    companyName: "",
                    kraPinNo: "",
                    companyEmail: "",
                    password: "",
                    confirmPassword:"",
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
                this.$root.loader.message = "Registering...";
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.form)
                };
                fetch(this.appConfig.baseApiRoute+'authentication/register', requestOptions)
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
                            this.$root.FnNotification({ type: "modal", theme: "red",title:"Error", message: data.errors });
                        } 
                        else {
                            this.$router.push({ name: "authLogin" });
                            var msg = "Registered successfully. However, we need to verify the email is actually yours before you login. We have sent a link to your email for verification.";
                            this.$root.FnNotification({ type: "modal", theme: "green", message: msg });
                        }
                        this.$root.loader.isLoading = false;
                        this.$root.loader.message = "";
                    }).catch((error) => {
                        this.$root.loader.isLoading = false;
                        this.$root.FnNotification({ type: "modal", theme: "red", title: "Error", message: String(error) });
                    });
            },
        },
    }
</script>
