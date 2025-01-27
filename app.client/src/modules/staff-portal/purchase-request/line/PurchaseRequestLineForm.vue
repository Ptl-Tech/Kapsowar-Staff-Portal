<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-2">
                <grid-col>
                    <field-group label="Type" :showMandatory="true" :valErrors="valErrors.type">
                        <WSelect required="true" v-model="form.type" :formMode="formMode">
                            <option v-if="types" v-for="option in types" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="No" :showMandatory="true" :valErrors="valErrors.no">
                        <div v-if="form.type == 1"><TomSelectFetch v-if="xIsFormLoaded" tsId="GLs" v-model="form.no" :cProps="{webservice:'QyGLAccounts',valueField:'No',labelField:'Name',searchField:['No','Name'],recordField:'No'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`No eq '${record.No}'`" :record="record" :formMode="formMode" /></div>
                        <div v-if="form.type == 2"><TomSelectFetch v-if="xIsFormLoaded" tsId="GLs" v-model="form.no" :cProps="{webservice:'QyItems',valueField:'No',labelField:'Description',searchField:['No','Description'],recordField:'No'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`No eq '${record.No}'`" :record="record" :formMode="formMode" /></div>
                        <div v-if="form.type == 4"><TomSelectFetch v-if="xIsFormLoaded" tsId="GLs" v-model="form.no" :cProps="{webservice:'QyFixedAssets',valueField:'No',labelField:'Description',searchField:['No','Description'],recordField:'No'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`No eq '${record.No}'`" :record="record" :formMode="formMode" /></div>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Procurement Plan" :showMandatory="true" :valErrors="valErrors.procurementPlan">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="procurementPlans" v-model="form.procurementPlan" :cProps="{webservice:'QyProcurementPlans',valueField:'Activity_Code',labelField:'Activity_Description',searchField:['Activity_Code','Activity_Description'],recordField:'Procurement_Plan'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`Activity_Code eq '${record.Procurement_PLan}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Request Reason" :showMandatory="true" :valErrors="valErrors.requestReason">
                        <WInput type="text" required="true" v-model="form.requestReason" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Location" :showMandatory="true" :valErrors="valErrors.location">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="location" v-model="form.location" :cProps="{webservice:'QyLocations',valueField:'Code',labelField:'Name',searchField:['Code','Name'],recordField:'Location_Code'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`Code eq '${record.Location_Code}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Quantity" :showMandatory="false" :valErrors="valErrors.quantity">
                        <WInput type="number" required="true" v-model="form.quantity" :formMode="formMode" />
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
    import Actions from '@/modules/staff-portal/purchase-request/line/PurchaseRequestLineActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    export default {
        components: { Actions, ...W, ModalPageTemplate, TomSelectFetch },
        emits: ["closeModal"],
        setup() {
            const { router, xIsFormLoaded, xOnAfterFormLoaded } = useFormComposable();
            return { router, xIsFormLoaded, xOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Purchase request Line Form',
                    pageType: "form",
                    keys: { recKey: 'Line_No', parentKey: 'Document_No' },
                    controller: 'PurchaseReqLine',
                    formRoute: "/ess/purchase-request/line/form",
                    listRoute: "/ess/purchase-request/line/list",
                },
                form: {
                    parentId: '',
                    recId: 0,
                    myAction: '',
                    type: 0,
                    no: "",
                    procurementPlan: "",
                    requestReason: "",
                    location: "",
                    quantity: 0,
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                types: [{ id: "0", name: "--select--" }, { id: "1", name: "G/L Account" }, { id: "2", name: "Item" },{ id: "4", name: "Fixed Asset" }],
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
                            var msg = this.appConfig.errors.dataFetchFailure;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.header = data.response.header;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.type = this.types.find(obj => obj.name == formData.Type).id;
                                this.form.no = formData.No;
                                this.form.procurementPlan = formData.Procurement_Plan;
                                this.form.requestReason = formData.Reason_for_Request;
                                this.form.claimReceiptNo = formData.Claim_Receipt_No;
                                this.form.location = formData.Location_Code;
                                this.form.quantity = formData.Quantity;
                                if (this.header.Status != "Open") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.xOnAfterFormLoaded();
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>