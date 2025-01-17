<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Purchase Request'">
            <ApprovalForm v-if="$route.params.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="No.">
                        <WInput type="text" required="false" :value="form.docNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="dimNos" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="record != null && record.Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <div v-if="record.Status != undefined">
                <grid>
                    <grid-col>
                        <field-group label="Approval Status">
                            <WInput type="text" required="false" v-model="record.Status" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Amount Including VAT">
                            <WInput type="text" required="false" v-model="record.Amount_Including_VAT" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Order Created?">
                            <WInput type="text" required="false" :value="record.Order_Created? 'Yes':'No'" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <!---->
            <hr class="py-2" />
            <PurchaseLineList v-if="form.docNo != 'new'" :docNo="form.docNo" />
            <!---->
            <hr class="py-2" />
            <AttachmentList v-if="form.docNo != 'new'" :docNo="form.docNo" tableID="38" type="Purchase Request" :editable="record.Status != undefined && record.Status == 'Open'? true:false" />
            <!---->
            <ApproversList :records="approvers" v-if="record.Status != undefined && record.Status != 'Open'"></ApproversList>
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
            <!---->
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/ess/purchase-request/PurchaseRequestActions.vue';
    import PurchaseLineList from '@/modules/ess/purchase-request/PurchaseRequestLineList.vue';
    import AttachmentList from '@/modules/ess/attachment/AttachmentList.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, ...Apr, PurchaseLineList, AttachmentList, TomSelectDims },
        setup() {
            const { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Purchase Request Form',
                    pageType: "form",
                    controller: 'PurchaseRequestHeader',
                    formRoute: "/ess/purchase-request/form",
                    listRoute: "/ess/purchase-request/list",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    donor:'',
                    project: '',
                    budgetLine: '',
                    county: '',
                    dimensionSet: {},
                },
                record: {},
                valErrors: [],
                donors: [],
                projects: [],
                dims: [],
                dimNos: [],
                counties: [],
                approvers: [],
                approvalEntry: {}, 
                formMode: 'view',
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.params.no;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        beforeRouteLeave(to, from, next) {
            this.cpl_FnAutoSaveFormData(to, from, next, this.record.Status);
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/getformdata?myAction='+this.form.myAction+"&docNo="+this.form.docNo, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        } else {
                            this.setDropdownDims(data.response);
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.No;
                                this.form.donor = formData.Shortcut_Dimension_1_Code;
                                this.form.project = formData.Shortcut_Dimension_2_Code;
                                //this.form.budgetLine = formData.Shortcut_Dimension_3_Code;
                                this.form.county = formData.Shortcut_Dimension_7_Code;
                                if (formData.Status != "Open") {
                                    this.formMode = "view";
                                }
                                if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                                    this.approvers = data.response.approvers;
                                }
                            }
                            //this.loadTomSelects("donor,project,county,budgetLine");
                            this.cpl_FnSetInitialFormData();
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
        }
    }
</script>