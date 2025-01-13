<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Laboratory Test Code" :showMandatory="true" :valErrors="valErrors.laboratoryTestCode">
                        <WInput type="text" required="true" v-model="form.laboratoryTestCode" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Laboratory Test Name" :showMandatory="true" :valErrors="valErrors.laboratoryTestName">
                        <WInput type="text" required="true" v-model="form.laboratoryTestName" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Specimen Code" :showMandatory="true" :valErrors="valErrors.specimenCode">
                        <TomSelectFetch v-if="cplIsFormLoaded" tsId="specimenCode" v-model="form.specimenCode" :cProps="{valueField:'Code',labelField:'Description',searchField:['Code','Description'],recordField:'SpecimenCode',webservice:'PgSpecimensSetup'}" :filter="header.Status == 'New'?'':`Code eq '${record.SpecimenCode}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Measuring Unit" :showMandatory="true" :valErrors="valErrors.unitOfMeasure">
                        <TomSelectFetch v-if="cplIsFormLoaded" tsId="unitOfMeasure" v-model="form.unitOfMeasure" :cProps="{valueField:'Code',labelField:'Description',searchField:['Code','Description'],recordField:'MeasuringUnitCode',webservice:'QyMeasuringUnitsSetup'}" :filter="header.Status == 'New'?'':`Code eq '${record.MeasuringUnitCode}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Count Value" :showMandatory="false" :valErrors="valErrors.countValue">
                        <WInput type="number" required="true" v-model="form.countValue" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Positive?" :showMandatory="false" :valErrors="valErrors.positive">
                        <WSelect required="true" v-model="form.positive" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="isPositive" v-for="option in isPositive" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
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
    import Actions from '@/modules/hmis/laboratory/test-line/Lab_TestLineActions.vue';
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
                    title: 'Lab Test Form',
                    pageType: "form",
                    keys: { recKey: 'SystemId', parentKey: 'Laboratory_No' },
                    controller: 'Lab_TestLines',
                    formRoute: "/hmis/laboratory/test-line/form",
                    listRoute: "/hmis/laboratory/test-line/list",
                },
                form: {
                    parentId: '',
                    recId: "",
                    myAction: '',
                    specimenCode: '',
                    unitOfMeasure: '',
                    countValue: 0,
                    positive: false,
                    remarks: '',
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                isPositive: [{ id: true, name: "Yes" }, { id: false, name: "No" }],
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
                                this.form.collectionDate = this.$root.cplFnNavDateObjToISODate(formData.CollectionDate);
                                this.form.specimenCode = formData.SpecimenCode;
                                this.form.laboratoryTestCode = formData.LaboratoryTestCode;
                                this.form.laboratoryTestName = formData.LaboratoryTestName;
                                this.form.unitOfMeasure = formData.MeasuringUnitCode;
                                this.form.countValue = formData.CountValue;
                                this.form.positive = this.isPositive.find(obj => obj.id == Boolean(formData.Positive)).id;
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