<template>
    <div>
        <ModalPageTemplate :title="$route.params.lineAction+' Staff Advance Line'" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Item" showMandatory="true" :valErrors="valErrors.item">
                        <WSelect required="true" v-model="form.item" id="item" :isTomSelect="true" :formMode="formMode" @change="onItemChange()">
                            <option value="">search</option>
                            <option v-if="advanceTypes" v-for="advanceType in advanceTypes" :value="advanceType.Code">{{advanceType.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Description" :valErrors="valErrors.description">
                        <WInput type="text" required="true" v-model="form.description" :formMode="isPortalEditable?formMode:'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Unit of Measure" showMandatory="true" :valErrors="valErrors.unitOfMeasure">
                        <WSelect required="true" v-model="form.unitOfMeasure" id="unitOfMeasure" isTomSelect="true" :formMode="isPortalEditable?formMode:'view'">
                            <option value="">search</option>
                            <option v-if="uoms" v-for="uom in uoms" :value="uom.Code">{{uom.Code}} - {{uom.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Unit Cost" :valErrors="valErrors.unitCost">
                        <WInput type="number" required="true" v-model="form.unitCost" :formMode="isPortalEditable?formMode:'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="No. of Units" showMandatory="true" :valErrors="valErrors.noOfUnits">
                        <WInput type="number" required="true" v-model="form.noOfUnits" :formMode="formMode" @input="fnUpdateAmounts()"/>
                    </field-group>
                </grid-col>
                <!--<grid-col v-if="form.dimNos" v-for="dimNo in form.dimNos">
        <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors['dim'+dimNo]">
            <WSelect required="true" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :id="'dim'+dimNo" isTomSelect="true" :formMode="formMode">
                <option value="">search</option>
                <option v-if="dims['dims'+dimNo]" v-for="dim in dims['dims'+dimNo]" :value="dim.Code">{{dim.Code}} - {{dim.Name}}</option>
            </WSelect>
        </field-group>
    </grid-col>-->
                <grid-col v-if="dimNos" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="false" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Purpose" showMandatory="true" :valErrors="valErrors.purpose">
                        <WTextarea required="true" v-model="form.purpose" :formMode="formMode"></WTextarea>
                    </field-group>
                </grid-col>
                <grid-col v-if="form.currency != 'KES'">
                    <field-group label="Currency" :valErrors="valErrors.currency">
                        <WInput type="text" required="true" v-model="form.currency" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Amount" :valErrors="valErrors.amount">
                        <WInput type="number" required="true" v-model="form.amount" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="form.currency != 'KES'">
                    <field-group label="Amount LCY" :valErrors="valErrors.amountLCY">
                        <WInput type="number" required="true" v-model="form.amountLCY" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <Actions :record="record" :header="header" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" @closeModal="$emit('closeModal')"/>
            </template>
        </ModalPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import ModalPageTemplate from '@/re-usables/page-templates/ModalPageTemplate.vue';
    import Actions from '@/modules/ess/staff-advance/AdvanceLineActions.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, ModalPageTemplate, TomSelectDims },
        emits:["closeModal"],
        setup() {
            const { router, loadTomSelects, setDropdownDims, setFormDims, updateDimensionSet } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, setFormDims, updateDimensionSet };
        },
        data() {
            return {
                pageProps: {
                    title: 'Staff Advance Line',
                    pageType: "form",
                    controller: 'StaffAdvanceLine',
                },
                form: {
                    docNo: '',
                    lineNo: 0,
                    myAction: '',
                    item: '',
                    description: '',
                    unitOfMeasure: '',
                    unitCost: 0,
                    noOfUnits:0,
                    purpose: "",
                    amount: 0,
                    dimensionSet: {},
                },
                valErrors: [],
                advanceTypes: [],
                budgetLines: [],
                projects: [],
                regions: [],
                record: {},
                header: {},
                formMode: 'view',
                dims: [],
                dimNos: [],
                uoms: [],
                isPortalEditable:false
            }
        },
        created() {
            this.form.myAction = this.$route.params.lineAction;
            this.form.docNo = this.$route.params.no;
            this.form.lineNo = this.$route.params.lineNo;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata/' + this.form.myAction + "/" + this.form.docNo + "/" + this.form.lineNo, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.advanceTypes = data.response.advanceTypes;
                            this.uoms = data.response.uoms;
                            this.setDropdownDims(data.response);
                            this.header = data.response.header;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.Advance_Request_Hdr_No;
                                this.form.lineNo = formData.No;
                                this.form.item = formData.Item;
                                this.form.description = formData.Item_Description;
                                this.form.unitOfMeasure = formData.Unit_Of_Measure;
                                this.form.unitCost = formData.Unit_Cost;
                                this.form.noOfUnits = formData.No_of_Units;
                                this.setFormDims(data.response.formData);
                                this.form.purpose = formData.Purpose;
                                this.form.amount = formData.Amount;
                                this.form.amountLCY = formData.Amount_LCY;
                                this.form.currency = formData.Currency;
                                if (this.header.Approval_Status != "Open") {
                                    this.formMode = "view";
                                }
                            }
                            this.loadTomSelects("item");
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },            
            onItemChange(){
                var item = this.advanceTypes.find(x => x.Code == this.form.item);
                this.form.description = item.Description;
                this.form.unitOfMeasure = item.Unit_of_Measure;
                this.form.currency = item.Currency;
                this.form.unitCost = item.Unit_Cost;
                this.isPortalEditable = false;
                if (item.Editable_on_Portal == true) {
                    this.isPortalEditable = true;
                    this.loadTomSelects("unitOfMeasure");
                }
                this.fnUpdateAmounts();
            },
            fnUpdateAmounts() {
                this.form.amount = this.form.unitCost * this.form.noOfUnits;
                if (this.form.currency != "KES") {
                    this.form.amountLCY = 0;
                }
            }
        }
    }
</script>