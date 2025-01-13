<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Date Referred" :showMandatory="true" :valErrors="valErrors.dateReferred">
                        <WInput type="date" required="true" v-model="form.dateReferred" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Contact Person" :showMandatory="true" :valErrors="valErrors.contactPerson">
                        <WInput type="text" required="true" v-model="form.contactPerson" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Hospital No" :showMandatory="false" :valErrors="valErrors.hospitalNo">
                        <TomSelectFetch v-if="cplIsFormLoaded" tsId="hospitalNo" v-model="form.hospitalNo" :cProps="{valueField:'No',labelField:'Name',searchField:['No','Name'],recordField:'HospitalNo',webservice:'QyVendors'}" :filter="record.Status != 'Completed'?'':`No eq '${record.HospitalNo}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Referral Reason" :valErrors="valErrors.referralReason" :showMandatory="true">
                        <WTextarea required="false" v-model="form.referralReason" :formMode="formMode" :maxLength="250"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Referral Remarks" :valErrors="valErrors.referralRemarks" :showMandatory="true">
                        <WTextarea required="false" v-model="form.referralRemarks" :formMode="formMode" :maxLength="250"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <Actions :record="record" :header="header" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </template>
        </ModalPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import ModalPageTemplate from '@/re-usables/page-templates/ModalPageTemplate.vue';
    import Actions from '@/modules/hmis/consultation/referral/CLT_ReferralActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    export default {
        components: { Actions, ...W, ModalPageTemplate, TomSelectFetch },
        emits:["closeModal"],
        setup() {
            const { router, cplIsFormLoaded,cplOnAfterFormLoaded } = useFormComposable();
            return { router, cplIsFormLoaded,cplOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Referral Form',
                    pageType: "form",
                    keys: { recKey: 'TreatmentNo', parentKey: 'TreatmentNo' },
                    controller: 'CLT_Referrals',
                    formRoute: "/hmis/" + this.$route.params.section + "/clt-referral/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/clt-referral/list",
                    name: 'consultation-referral',
                },
                form: {
                    parentKey: '',
                    recKey: "",
                    myAction: '',
                    dateReferred: new Date().toISOString().split("T")[0],
                    hospitalNo: "",
                    contactPerson: "",
                    referralReason:"",
                    referralRemarks:"",
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata' + this.$root.fnGetUrlQueryFromObject(this.$route.query) + '&myAction=' + this.form.myAction, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.header = data.response.header;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.dateReferred = this.$root.cplFnNavDateObjToISODate(formData.DateReferred);
                                this.form.hospitalNo = formData.HospitalNo;
                                this.form.contactPerson = formData.Contactperson;
                                this.form.referralReason = formData.ReferralReason;
                                this.form.referralRemarks = formData.ReferralRemarks;
                                if (this.header.Status == "Completed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.cplOnAfterFormLoaded();
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>