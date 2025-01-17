<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Advance Surrender'">
            <ApprovalForm v-if="$route.params.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="Surrender No.">
                        <WInput type="text" required="false" :value="form.docNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Advance Request Selection" showMandatory="true" :valErrors="valErrors.advance">
                        <WSelect v-if="form.advanceNo == '' || (record.Approval_Status == undefined)" required="true" v-model="form.advanceNo" id="advanceNo" isTomSelect="true" :formMode="formMode" @change="onAdvanceChange()">
                            <option value="">search</option>
                            <option v-if="advances" v-for="advance in advances" :value="advance.No">{{advance.No}}</option>
                        </WSelect>
                        <WInput v-else type="text" required="false" v-model="form.advanceNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Actual Mission Start Date" showMandatory="true" :valErrors="valErrors.actualStartDate">
                        <WInput type="date" required="true" v-model="form.actualStartDate" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Actual Date of return from Mission" showMandatory="true" :valErrors="valErrors.actualReturnDate">
                        <WInput type="date" required="true" v-model="form.actualReturnDate" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="dimNos" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="false" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="'view'"></TomSelectDims>
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
                        <WTextarea required="true" v-model="form.summary" formMode="view"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="!gsrid-cols-1">
                <grid-col>
                    <field-group label="Preferred Payment Method" showMandatory="true" :valErrors="valErrors.paymentMethod">
                        <WSelect required="true" v-model="form.paymentMethod" formMode="view">
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
                <grid class="sm:!grid-cols-1">
                    <grid-col v-if="record.Rejection_Comment != ''">
                        <field-group label="Last Rejection Comment">
                            <WTextarea required="false" v-model="record.Rejection_Comment" :formMode="'view'"></WTextarea>
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <hr class="py-2" />
            <SurrenderLinesList v-if="form.docNo != 'new'" :docNo="form.docNo" :key="form.docNo" :isNew="false" />
            <AdvanceAttachmentsList v-if="$route.params.no != 'new' && form.advanceNo != ''" :docNo="form.advanceNo" :key="form.advanceNo" :isNew="false" />
            <MpesaSheetLines v-if="form.myAction != 'create' && form.docNo != 'new'" :docNo="form.docNo" :isNew="record.Approval_Status != undefined && record.Approval_Status == 'Open'?true:false" />
            <SurrenderAttachmentsList v-if="$route.params.no != 'new'" :docNo="$route.params.no" />
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
    import Actions from '@/modules/ess/staff-advance/AdvanceActions.vue';
    import SurrenderLinesList from '@/modules/ess/staff-advance-surrender/SurrenderLineList.vue';
    import AdvanceAttachmentsList from '@/modules/ess/staff-advance/AdvanceAttachmentList.vue';
    import SurrenderAttachmentsList from '@/modules/ess/staff-advance-surrender/SurrenderAttachmentList.vue';
    import { useRouter } from 'vue-router'
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    import MpesaSheetLines from '@/modules/ess/mpesa-sheet-line/MpesaSheetLineList.vue';
    export default {
        components: { Actions, ...W, ...Apr, TomSelectDims, MpesaSheetLines, AdvanceAttachmentsList, SurrenderLinesList, SurrenderAttachmentsList },
        setup() {
            const { router, loadTomSelects, setDropdownDims, setFormDims, updateDimensionSet, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, setFormDims, updateDimensionSet, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Advance Surrender',
                    pageType: "form",
                    controller: 'StaffAdvanceSurrender',
                    formRoute: "/ess/staff-advance-surrender/form",
                    listRoute: "/ess/staff-advance-surrender/list",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    actualStartDate: '',
                    actualReturnDate: '',
                    summary: '',
                    paymentMethod:0,
                    balance: 0,
                    advanceNo: '',
                    dimensionSet: {},
                },
                valErrors: [],
                donors: [],
                projects: [],
                regions: [],
                advances: [],
                approvers:[],
                record: {},
                approvalEntry: {}, 
                formMode: 'view',
                dims: [],
                dimNos: [],
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
                            this.advances = data.response.advances;
                            this.form.balance = data.response.customerBalance;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.actualStartDate = formData.Date_of_Request.split('T')[0];
                                this.form.actualReturnDate = formData.Date_Due.split('T')[0];
                                this.form.summary = formData.Mission_Summary;
                                this.form.advanceNo = formData.Staff_Advance_Header_No;
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
                            this.loadTomSelects("advanceNo");
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
            onAdvanceChange() {
                var advance = this.advances.find(obj => obj.No === this.form.advanceNo);
                if(advance != null){
                    this.form.dimensionSet.Dim_1_Value = advance.Global_Dimension_1_Code;
                    this.form.dimensionSet.Dim_2_Value = advance.Global_Dimension_2_Code;
                    this.form.dimensionSet.Dim_3_Value = advance.Shortcut_Dimension_3_Code;
                    this.form.dimensionSet.Dim_4_Value = advance.Shortcut_Dimension_4_Code;
                    this.form.dimensionSet.Dim_5_Value = advance.Shortcut_Dimension_5_Code;
                    this.form.dimensionSet.Dim_6_Value = advance.Shortcut_Dimension_6_Code;
                    this.form.dimensionSet.Dim_7_Value = advance.Shortcut_Dimension_7_Code;
                    this.form.dimensionSet.Dim_8_Value = advance.Shortcut_Dimension_8_Code;
                    this.form.dimensionSet.Dim_9_Value = advance.Shortcut_Dimension_9_Code;
                    this.form.dimensionSet.Dim_10_Value = advance.Shortcut_Dimension_10_Code;
                    this.form.dimensionSet.Dim_11_Value = advance.Shortcut_Dimension_11_Code;
                    this.form.dimensionSet.Dim_12_Value = advance.Shortcut_Dimension_12_Code;
                    this.form.actualStartDate = advance.Date_of_Request.split('T')[0];
                    this.form.actualReturnDate = advance.Date_Due.split('T')[0];
                    this.form.summary = advance.Mission_Summary;
                    if(advance.Preferred_Payment_Method == "Cheque"){
                        this.form.paymentMethod = 1;
                    }
                    if(advance.Preferred_Payment_Method == "MPesa"){
                        this.form.paymentMethod = 2;
                    }
                    if(this.form.docNo == "new" || (this.form.myAction == "edit" && this.form.advanceNo != this.record.Staff_Advance_Header_No)){
                        this.OnSubmit();
                    }
                }
            },
            OnSubmit() {
                var body;
                var url;
                var docNo = "";
                url = this.appConfig.baseApiRoute + this.pageProps.controller + "/Store";
                this.form.myAction = "create#save";
                body = this.form;
                docNo = this.record.No;
                 //
                 this.$root.loader.isLoading = true;
                 const requestOptions = {
                     method: "POST",
                     headers: { 'Content-Type': "application/json" },
                     body: JSON.stringify(body)
                 };
                 fetch(url, requestOptions)
                     .then(response => {
                         return response.json();
                     })
                     .then(data => {
                         if (data && data.valErrors) {
                             this.$emit('onValErrors', data.valErrors);
                         }
                         else if (data && data.errors) {
                             this.$root.errorModal.isShow = true;
                             this.$root.errorModal.message = data.errors;
                         }
                         else {
                             var res;
                             res = data.response;
                             this.router.push(this.pageProps.formRoute + "/edit/" + res);
                         }
                         this.$root.loader.isLoading = false;
                     }).catch((error) => {
                         this.$root.errorModal.isShow = true;
                         this.$root.errorModal.message = error;
                         this.$root.loader.isLoading = false;
                     });
             },
        }
    }
</script>