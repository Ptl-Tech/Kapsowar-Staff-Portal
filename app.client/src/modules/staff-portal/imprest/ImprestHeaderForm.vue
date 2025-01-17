<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Staff Advance'">
            <ApprovalForm v-if="$route.params.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="Advance No.">
                        <WInput type="text" required="false" :value="form.docNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Approved TOR No." showMandatory="true" :valErrors="valErrors.TOR">
                        <WInput type="text" required="true" v-model="form.TOR" placeholder="e.g TOR000XXX" :formMode="formMode" />
                        <span v-if="form.TOR != '' && record.Approval_Status != undefined && record.Approval_Status != 'Open'"><router-link :to="'/ess/terms-of-reference/form/view/'+form.TOR" class="text-blue-500 underline text-xs">View TOR {{form.TOR}}</router-link></span>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Actual Mission Start Date" showMandatory="true" :valErrors="valErrors.actualStartDate">
                        <WInput type="date" required="true" v-model="form.actualStartDate" :formMode="formMode" :min="new Date().toISOString().split('T')[0]" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Actual Date of return from Mission" showMandatory="true" :valErrors="valErrors.actualReturnDate">
                        <WInput type="date" required="true" v-model="form.actualReturnDate" :formMode="formMode" :min="new Date().toISOString().split('T')[0]" />
                    </field-group>
                </grid-col>
                <grid-col v-if="dimNos" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="record != null && record.Approval_Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Balance" showMandatory="false" :valErrors="valErrors.balance">
                        <WInput type="text" required="true" v-model="form.balance" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="!grid-cols-1">
                <grid-col>
                    <field-group label="Mission Summary (Type activity name and county visited)" showMandatory="true" :valErrors="valErrors.summary">
                        <WTextarea required="true" v-model="form.summary" :formMode="formMode"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="!grid-cols-1">
                <grid-col>
                    <field-group label="Preferred Payment Method" showMandatory="true" :valErrors="valErrors.paymentMethod">
                        <WSelect required="true" v-model="form.paymentMethod" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option value="1">Cheque</option>
                            <option value="2">M-Pesa</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <div v-if="record.Approval_Status != undefined">
                <grid>
                    <grid-col>
                        <field-group label="Approval Status">
                            <WInput type="text" required="false" v-model="record.Approval_Status" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Employee No.">
                            <WInput type="text" required="false" v-model="record.Requester_Employee_No" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                </grid>
                <grid class="sm:!grid-cols-1" v-if="record.Rejection_Comment != ''">
                    <grid-col>
                        <field-group label="Last Rejection Comment">
                            <WInput type="text" required="false" v-model="record.Rejection_Comment" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <hr class="py-2" />
            <AdvanceLinesList v-if="form.myAction != 'create' && form.docNo != 'new'" :docNo="form.docNo" :isNew="record.Approval_Status != undefined && record.Approval_Status == 'Open'?true:false" />
            <MpesaSheetLines v-if="form.myAction != 'create' && form.docNo != 'new'" :docNo="form.docNo" :isNew="record.Approval_Status != undefined && record.Approval_Status == 'Open'?true:false" />
            <AdvanceAttachmentsList v-if="form.myAction != 'create' && form.docNo != 'new'" :docNo="form.docNo" :isNew="record.Approval_Status != undefined && record.Approval_Status == 'Open'?true:false" />
            <ApproversList :records="approvers" v-if="record.Approval_Status != undefined && record.Approval_Status != 'Open'"></ApproversList>
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js'
    import Actions from '@/modules/ess/staff-advance/AdvanceActions.vue';
    import AdvanceLinesList from '@/modules/ess/staff-advance/AdvanceLineList.vue';
    import AdvanceAttachmentsList from '@/modules/ess/staff-advance/AdvanceAttachmentList.vue';
    import MpesaSheetLines from '@/modules/ess/mpesa-sheet-line/MpesaSheetLineList.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, ...Apr, AdvanceLinesList, MpesaSheetLines, AdvanceAttachmentsList,TomSelectDims },
        setup() {
            const { router, loadTomSelects, setDropdownDims, setFormDims, updateDimensionSet, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, setFormDims, updateDimensionSet, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Staff Advance',
                    pageType: "form",
                    controller: 'StaffAdvance',
                    formRoute: "/ess/staff-advance/form",
                    listRoute: "/ess/staff-advance/list",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    TOR: '',
                    actualStartDate: '',
                    actualReturnDate: '',
                    summary: '',
                    paymentMethod:0,
                    balance: 0,
                    dimensionSet: {},

                },
                dimNos: [],
                valErrors: [],
                donors: [],
                projects: [],
                regions: [],
                approvers: [],
                TORs:[],
                record: {}, 
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
            this.cpl_FnAutoSaveFormData(to, from, next, this.record.Approval_Status);
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
                            this.form.balance = data.response.customerBalance;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.No;
                                this.form.TOR = formData.TOR_No;
                                this.form.actualStartDate = formData.Date_of_Request.split('T')[0];
                                this.form.actualReturnDate = formData.Date_Due.split('T')[0];
                                this.form.summary = formData.Mission_Summary;
                                if(formData.Preferred_Payment_Method == "Cheque"){
                                    this.form.paymentMethod = 1;
                                }
                                if(formData.Preferred_Payment_Method == "MPesa"){
                                    this.form.paymentMethod = 2;
                                }
                                if (formData.Approval_Status != "Open") {
                                    this.formMode = "view";
                                }
                                if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                                    this.approvers = data.response.approvers;
                                }
                                this.cpl_FnSetInitialFormData();
                            }
                        }
                         this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
            onFetchApprovalEntry(response) {
                this.approvalEntry = response;
            }
        }
    }
</script>