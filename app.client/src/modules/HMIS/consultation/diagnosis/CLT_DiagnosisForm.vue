<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Diagnosis" :showMandatory="false" :valErrors="valErrors.diagnosisNo">
                        <WSelect required="true" v-model="form.diagnosisNo" id="diagnosisSetups" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="diagnosisSetups" v-for="option in diagnosisSetups" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Diagnosis Code" :showMandatory="true" :valErrors="valErrors.diagnosisCode">
                        <WInput type="text" required="true" v-model="form.diagnosisCode" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Remarks" :valErrors="valErrors.remarks" :showMandatory="true">
                        <WTextarea required="false" v-model="form.remarks" :formMode="formMode" :maxLength="250"></WTextarea>
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
    import Actions from '@/modules/hmis/consultation/diagnosis/CLT_DiagnosisActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    export default {
        components: { Actions, ...W, ModalPageTemplate },
        emits:["closeModal"],
        setup() {
            const { router, loadTomSelects } = useFormComposable();
            return { router, loadTomSelects };
        },
        data() {
            return {
                pageProps: {
                    title: 'Diagnosis Form',
                    pageType: "form",
                    keys: { recKey: 'DiagnosisNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Diagnosis',
                    formRoute: "/hmis/" + this.$route.params.section +"/diagnosis/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/diagnosis/list",
                    name:'consultation-diagnosis',
                },
                form: {
                    parentKey: '',
                    recKey: "",
                    myAction: '',
                    diagnosisNo: "",
                    diagnosisCode: "",
                    remarks:"",
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                diagnosisSetups: [],
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
                            this.diagnosisSetups = data.response.diagnosisSetups;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.diagnosisNo = formData.DiagnosisNo;
                                this.form.diagnosisCode = formData.DiagnosisCode;
                                this.form.remarks = formData.Remarks;
                                if (this.header.Status == "Completed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("diagnosisSetups");
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