<template>
    <div>
        <FormPageTemplate title="My Profile">
            <ProfileForm v-if="record.No != undefined" :profile="record" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
  import ProfileForm from '@/re-usables/modules/profile/ProfileForm.vue'
    import { useRouter } from 'vue-router'
    export default {
        components: { ProfileForm,...W },
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                pageProps: {
                    title: 'Profile Form',
                    pageType: "form",
                    controller: 'Profile',
                    formRoute: "/hmis/profile",
                },
                record: {},
                formMode: 'view',
            }
        },
        created() {
            this.$root.title = this.pageProps.title;
            if (!this.profile) {
                this.FnFetchSetups();
            }
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/formdata', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.record = data.response.profile;
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
        }

    }
</script>
