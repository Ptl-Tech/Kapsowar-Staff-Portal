<template>
    <div>
        <grid>
            <grid-col>
                <field-group label="Staff No.">
                    <WInput type="text" v-model="profile.No" formMode="view" />
                </field-group>
            </grid-col>
            <grid-col>
                <field-group label="Name">
                    <WInput type="text" v-model="profile.Full_Name" formMode="view" />
                </field-group>
            </grid-col>
            <grid-col>
                <field-group label="National ID">
                    <WInput type="text" v-model="profile.ID_Number" formMode="view" />
                </field-group>
            </grid-col>
            <grid-col>
                <field-group label="Date of Birth">
                    <WInput type="text" :value="profile.Date_Of_Birth != undefined?profile.Date_Of_Birth.split('T')[0]:''" formMode="view" />
                </field-group>
            </grid-col>
            <grid-col>
                <field-group label="Email">
                    <WInput type="text" v-model="profile.E_Mail" formMode="view" />
                </field-group>
            </grid-col>
            <grid-col>
                <field-group label="Phone Number">
                    <WInput type="text" v-model="profile.Home_Phone_Number" formMode="view" />
                </field-group>
            </grid-col>
        </grid>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useRouter } from 'vue-router'
    export default {
        components: { ...W },
        props: { profile: {}, isLoading: false },
        setup() {
            const router = useRouter()
            return { router };
        },
        //data() {
        //    return {
        //        pageProps: {
        //            title: 'Profile Form',
        //            pageType: "form",
        //            controller: 'Profile',
        //            formRoute: "/hmis/profile",
        //        },
        //        record: {},
        //        formMode: 'view',
        //    }
        //},
        //created() {
        //    if (!this.profile) {
        //        this.FnFetchSetups();
        //    }
        //},
        //methods: {
        //    FnFetchSetups() {
        //        this.$root.loader.isLoading = true;
        //        const requestOptions = {
        //            method: "GET",
        //        };
        //        fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/getformdata', requestOptions)
        //            .then(response => {
        //                return response.json();
        //            })
        //            .then(data => {
        //                if (data && data.errors) {
        //                    this.$root.errorModal.isShow = true;
        //                    this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
        //                } else {
        //                    this.profile = data.response.profile;
        //                }
        //                this.$root.loader.isLoading = false;
        //            }).catch((error) => {
        //                this.$root.errorModal.isShow = true;
        //                this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
        //                this.$root.loader.isLoading = false;
        //            });
        //    },
        //}

    }
</script>