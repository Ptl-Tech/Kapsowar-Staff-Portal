<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' Purchase Request Line'" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Type" showMandatory="true" :valErrors="valErrors.type">
                        <WSelect required="true" v-model="form.type" :formMode="formMode" @change="onTypeChange()">
                            <option value="">--select--</option>
                            <option v-if="types" v-for="option in types" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="No." showMandatory="true" :valErrors="valErrors.itemNo">
                        <WSelect required="true" v-model="form.itemNo" id="itemNo" isTomSelect="true" :formMode="formMode" @change="onItemChange()">
                            <option value="">search</option>
                            <option v-if="items && (form.type == '2' || form.type == '4')" v-for="item in items" :value="item.No">{{item.No}} - {{item.Description}}</option>
                            <option v-if="items && form.type == '1'" v-for="item in items" :value="item.No">{{item.No}} - {{item.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Description" showMandatory="true" :valErrors="valErrors.description">
                        <WInput type="text" v-model="form.description" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Unit of Measure" showMandatory="true" :valErrors="valErrors.unitOfMeasure">
                        <WSelect required="true" v-model="form.unitOfMeasure" id="unitOfMeasure" isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="uoms" v-for="uom in uoms" :value="uom.Code">{{uom.Code}} - {{uom.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Unit Cost" showMandatory="true" :valErrors="valErrors.unitCost">
                        <WInput type="number" v-model="form.unitCost" :formMode="isUnitCostEditable?formMode:'view'" />
                    </field-group>
                </grid-col>

                <grid-col>
                    <field-group label="Quantity" showMandatory="true" :valErrors="valErrors.quantity">
                        <WInput type="number" v-model="form.quantity" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Total Amount" showMandatory="true" :valErrors="valErrors.totalAmount">
                        <WInput type="number" v-model="form.totalAmount" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Location Code" showMandatory="true" :valErrors="valErrors.location">
                        <WSelect required="true" v-model="form.location" id="location" isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="locations" v-for="location in locations" :value="location.Code">{{location.Code}} - {{location.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Specifications" showMandatory="true" :valErrors="valErrors.specifications">
                        <WTextarea v-model="form.specifications" :formMode="formMode"></WTextarea>
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
    import Actions from '@/modules/ess/purchase-request/PurchaseRequestLineActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    export default {
        components: { Actions, ...W,ModalPageTemplate},
        emits:["closeModal"],
        setup() {
            const { router, loadTomSelects } = useFormComposable();
            return { router, loadTomSelects };
        },
        data() {
            return {
                pageProps: {
                    title: 'Purchase Request Line Form',
                    pageType: "form",
                    controller: 'PurchaseRequestLine',
                    pKey: 'Line_No',
                    pKeyHeader: 'Document_No',
                    formRoute: "/ess/purchase-request/line/form",
                    docNo: this.docNo,
                },
                form: {
                    docNo: '',
                    lineNo: 0,
                    myAction: '',
                    type: '',
                    itemNo: '',
                    description: '',
                    unitOfMeasure: '',
                    location: '',
                    unitCost: 0,
                    quantity: 0,
                    totalAmount: 0,
                    specifications: '',
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                projects: [],
                donors: [],
                items: [],
                locations: [],
                uoms: [],
                isUnitCostEditable: false,
                types: [{ id: "1", name: "G/L Account" }, { id: "2", name: "Item" }, { id: "4", name: "Fixed Asset" }],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
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
                            this.header = data.response.header;
                            this.locations = data.response.locations;
                            this.uoms = data.response.uoms;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.Document_No;
                                this.form.lineNo = formData.Line_No;
                                this.form.type = this.types.find(obj => obj.name == formData.Type).id;
                                this.onTypeChange();
                                formData.Type =="Item"
                                this.form.itemNo = formData.No;
                                this.form.unitOfMeasure = formData.Unit_of_Measure_Code;
                                this.form.quantity = formData.Quantity;
                                this.form.unitCost = formData.Direct_Unit_Cost;
                                this.form.totalAmount = formData.Amount_Including_VAT;
                                this.form.description = formData.Description;
                                this.form.specifications = formData.Description_2;
                                this.form.expectedReceiptDate = formData.Expected_Receipt_Date.split('T')[0];
                                this.form.timeTaken = formData.Time_Taken;
                                this.form.location = formData.Location_Code;
                                if (this.header.Status != "Open") {
                                    this.formMode = "view";
                                }
                            }
                            this.loadTomSelects("location,unitOfMeasure");
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },            
            onItemChange() {
                if (this.form.type == 2) {//item
                    var item = this.items.find(x => x.No == this.form.itemNo);
                    this.form.description = item.Description;
                    this.form.unitOfMeasure = item.Base_Unit_of_Measure;
                    this.form.unitCost = item.Unit_Cost;
                }
                else if (this.form.type == 1) {//gl
                    var item = this.items.find(x => x.No == this.form.itemNo);
                    this.form.description = item.Name;
                    this.form.unitOfMeasure = item.Quantity_Unit_Code;
                }
                else if (this.form.type == 4) {//fixed-asset
                    var item = this.items.find(x => x.No == this.form.itemNo);
                    this.form.description = item.Description;
                    //this.form.unit = item.Base_Unit_of_Measure;
                    //this.form.unitCost = item.Unit_Cost;
                }
            },
            onTypeChange() {
                this.items = [];
                this.form.itemNo = "";
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                this.form.description = "";
                this.form.unitOfMeasure = "";
                this.form.unitCost = 0;
                this.isUnitCostEditable = false;
                var type = "";
                this.isUnitCostEditable = true;
                if (this.form.type == 1) {
                    type = "gl";
                    //this.isUnitCostEditable = true;
                }
                else if (this.form.type == 2) {
                    type = "item"
                }
                else if (this.form.type == 4) {
                    type = "fixed-asset";
                    //this.isUnitCostEditable = true;
                }
                fetch(this.appConfig.baseApiRoute + "General" + '/GetTypeItems?type=' + type,requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.items = data.records;
                            this.loadTomSelects("itemNo");
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
        },
        watch: {
            "form.quantity": function () {
                this.form.totalAmount = this.form.quantity * this.form.unitCost;
            },
            "form.unitCost": function () {
                this.form.totalAmount = this.form.quantity * this.form.unitCost;
            }
        }
    }
</script>