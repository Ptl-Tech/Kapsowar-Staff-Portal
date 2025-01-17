<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' Store Request Line'" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Request Source" showMandatory="true" :valErrors="valErrors.sourceType">
                        <WSelect required="true" v-model="form.sourceType" :formMode="formMode" @change="onSourceChange()">
                            <option value="">--select--</option>
                            <option v-if="sources" v-for="option in sources" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group :label="form.source == 1? 'LPO No.':form.source == 2? 'PR No.':'No.'" showMandatory="true" :showLoader="isLoadingReqs" :valErrors="valErrors.reqNo">
                        <WSelect required="true" v-model="form.reqNo" id="reqNo" isTomSelect="true" :formMode="formMode" @change="onReqNoChange()">
                            <option value="">search</option>
                            <option v-if="lposPrs" v-for="doc in lposPrs" :value="doc.No">{{doc.No}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Item No." showMandatory="true" :valErrors="valErrors.itemNo" :showLoader="isLoadingItems">
                        <WSelect required="true" v-model="form.itemNo" id="itemNo" isTomSelect="true" :formMode="formMode" @change="onItemNoChange()">
                            <option value="">search</option>
                            <option v-if="items" v-for="item in items" :value="item.No">{{item.No}} {{item.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Item Type" showMandatory="true" :valErrors="valErrors.source">
                        <WSelect required="true" v-model="form.type" :formMode="'view'" @change="onSourceChange()">
                            <option v-if="types" v-for="option in types" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Description" showMandatory="true" :valErrors="valErrors.description">
                        <WInput type="text" v-model="form.description" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Quantity Ordered" showMandatory="true" :valErrors="valErrors.quantityOrdered">
                        <WInput type="number" v-model="form.quantityOrdered" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Request Quantity" showMandatory="true" :valErrors="valErrors.quantity">
                        <WInput type="number" v-model="form.quantity" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Remarks" :valErrors="valErrors.remarks">
                        <WTextarea v-model="form.remarks" :formMode="formMode"></WTextarea>
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
    import Actions from '@/modules/ess/store-request/StoreRequestLineActions.vue';
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
                    title: 'Store Request Line Form',
                    pageType: "form",
                    controller: 'StoreRequestLine',
                    pKey: 'Line_No',
                    pKeyHeader: 'Document_No',
                    formRoute: "/ess/store-request/line/form",
                    docNo: this.docNo,
                },
                form: {
                    docNo: '',
                    lineNo: 0,
                    myAction: '',
                    sourceType: '',
                    type: 0,
                    itemNo: '',
                    reqNo: '',
                    quantityOrdered: 0,
                    quantity: 0,
                    remarks: '',
                    description: '',
                    reqLineNo: 0,
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                items: [],
                lposPrs:[],
                isLoadingReqs: false,
                isLoadingItems: false,
                sources: [{ id: "1", name: "LPO" }, { id: "2", name: "Purchase Request" }],
                types: [{ id: "1", name: "G/L Account" }, { id: "2", name: "Item" }, { id: "4", name: "Fixed Asset" }, { id: "5", name: "Charge (Item)" }],
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
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.Document_No;
                                this.form.lineNo = formData.Line_No;
                                this.form.sourceType = formData.LPO_No != "" ? "1" : "2";
                                this.onSourceChange();
                                this.form.reqNo = formData.LPO_No != "" ? formData.LPO_No : formData.Lot_No;
                                this.onReqNoChange();
                                //this.onItemNoChange();
                                this.form.type = this.types.find(obj => obj.name == formData.Type).id;
                                this.form.quantityOrdered = formData.Qty_Requested;
                                this.form.quantity = formData.Quantity;
                                this.form.remarks = formData.Remarks;
                                this.form.itemNo = formData.No;
                                this.form.description = formData.Description;
                                if (this.header.Status != "Open") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },            
            onSourceChange() {
                this.form.reqNo = "";
                this.lposPrs = [];
                if (this.form.sourceType == "") { return };
                this.isLoadingReqs = true;
                const requestOptions = {
                    method: "GET",
                };
                var action = "";
                if (this.form.sourceType == 1) {
                    action = "GetLPOList";
                }
                else if (this.form.sourceType == 2) {
                    action = "GetPRList"
                }
                fetch(this.appConfig.baseApiRoute + "StoreRequestLine/" + action)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.lposPrs = data.records;
                            this.loadTomSelects("reqNo");
                        }
                        this.isLoadingReqs = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.isLoadingReqs = false;
                    });
            },
            onReqNoChange() {
                this.form.reqLine = "";
                this.items = [];
                this.form.itemNo = "";
                if (this.form.source == "") { return };
                this.isLoadingItems = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + "StoreRequestLine/GetRequestDocLines?sourceType=" + this.form.sourceType +"&sourceNo="+this.form.reqNo)
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
                        this.isLoadingItems = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.isLoadingItems = false;
                    });
            },
            onItemNoChange() {
                if (this.form.itemNo == "") { return };
                var item = this.items.find(x => x.No == this.form.itemNo);
                this.form.description = item.Description;
                this.form.quantityOrdered = item.Quantity;
                this.form.quantity = item.Quantity;
                this.form.type = this.types.find(obj => obj.name == item.Type).id;
                this.form.reqLineNo = item.Line_No;
            },
        },
        watch: {
            "form.quantity": function () {
                if (this.form.quantity > this.form.quantityOrdered) {
                    this.$root.errorModal.message = "The request quantity cannot be greater than quantity ordered.";
                    this.$root.errorModal.isShow = true;
                    this.form.quantity = 0;
                }
            }
        }
    }
</script>