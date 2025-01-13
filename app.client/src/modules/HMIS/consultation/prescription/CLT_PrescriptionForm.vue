<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Drug Group" :showMandatory="false" :valErrors="valErrors.drugGroup">
                        <TomSelectFetch v-if="cplIsFormLoaded" :tsId="'drugGroup'" v-model="form.drugGroup" :cProps="{valueField:'Code',labelField:'Description',searchField:['Code','Description'],recordField:'ProductGroup',webservice:'QyDiagnosisSetup'}" :filter="record.Status != 'Completed'?'':`Code eq '${record.ProductGroup}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Drug No." :showMandatory="false" :valErrors="valErrors.drugNo">
                        <TomSelectFetch v-if="cplIsFormLoaded" :tsId="'drugNo'" v-model="form.drugNo" :cProps="{valueField:'No',labelField:'Description',searchField:['No','Description'],recordField:'DrugNo',webservice:'QyItems'}" :filter="record.Status != 'Completed'?'':`Code eq '${record.DrugNo}'`"  :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Quantity" :showMandatory="true" :valErrors="valErrors.quantity">
                        <WInput type="number" required="true" v-model="form.quantity" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Unit Of Measure" :showMandatory="false" :valErrors="valErrors.unitOfMeasure">
                        <TomSelectFetch v-if="cplIsFormLoaded" :tsId="'unitOfMeasure'" v-model="form.unitOfMeasure" :cProps="{valueField:'Code',labelField:'Code',searchField:['Code'],recordField:'UnitOfMeasure',webservice:'QyUnitsOfMeasure'}" :filter="record.Status != 'Completed'?'':`Code eq '${record.UnitOfMeasure}'`"  :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Dosage" :valErrors="valErrors.dosage" :showMandatory="true">
                        <WTextarea required="false" v-model="form.dosage" :formMode="formMode" :maxLength="250"></WTextarea>
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
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    export default {
        components: { Actions, ...W, ModalPageTemplate,TomSelectFetch },
        emits:["closeModal"],
        setup() {
            const { router,cplIsFormLoaded,cplOnAfterFormLoaded } = useFormComposable();
            return { router, cplIsFormLoaded,cplOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Prescription Form',
                    pageType: "form",
                    keys: { recKey: 'LlineNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Prescriptions',
                    formRoute: "/hmis/" + this.$route.params.section +"/clt-prescription/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/clt-prescription/list",
                    name:'consultation-prescriptions',
                },
                form: {
                    parentKey: '',
                    recKey: 0,
                    myAction: '',
                    drugGroup: "",
                    drugNo: "",
                    quantity:0,
                    unitOfMeasure:"",
                    dosage:"",
                    remarks:"",
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
            this.form.recId = this.form.myAction != 'create' ? parseInt(this.$route.query.recId) : this.form.recId;
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
                                this.form.drugGroup = formData.ProductGroup;
                                this.form.drugNo = formData.DrugNo;
                                this.form.quantity = formData.Quantity;
                                this.form.unitOfMeasure = formData.UnitOfMeasure;
                                this.form.dosage = formData.Dosage;
                                this.form.remarks = formData.Remarks;
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