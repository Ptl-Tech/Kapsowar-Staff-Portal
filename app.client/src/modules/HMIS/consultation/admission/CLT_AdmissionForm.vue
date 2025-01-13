<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Date Of Admission" :showMandatory="true" :valErrors="valErrors.dateOfAdmission">
                        <WInput type="date" required="true" v-model="form.dateOfAdmission" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Admission Reason" :valErrors="valErrors.admissionReason" :showMandatory="true">
                        <WTextarea required="false" v-model="form.admissionReason" :formMode="formMode" :maxLength="250"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Admission Remarks" :valErrors="valErrors.admissionRemarks" :showMandatory="true">
                        <WTextarea required="false" v-model="form.admissionRemarks" :formMode="formMode" :maxLength="250"></WTextarea>
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
    import Actions from '@/modules/hmis/consultation/admission/CLT_AdmissionActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    export default {
        components: { Actions, ...W, ModalPageTemplate },
        emits:["closeModal"],
        setup() {
            const { router, cplIsFormLoaded,cplOnAfterFormLoaded } = useFormComposable();
            return { router, cplIsFormLoaded,cplOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Admission Form',
                    pageType: "form",
                    keys: { recKey: 'TreatmentNo', parentKey: 'TreatmentNo' },
                    controller: 'CLT_Admissions',
                    formRoute: "/hmis/" + this.$route.params.section + "/clt-admission/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/clt-admission/list",
                    name: 'consultation-admissions',
                },
                form: {
                    parentKey: '',
                    recKey: "",
                    myAction: '',
                    dateOfAdmission: new Date().toISOString().split("T")[0],
                    admissionReason:"",
                    admissionRemarks:"",
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
                                this.form.dateOfAdmission = this.$root.cplFnNavDateObjToISODate(formData.DateOfAdmission);
                                this.form.admissionReason = formData.AdmissionReason;
                                this.form.admissionRemarks = formData.AdmissionRemarks;
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