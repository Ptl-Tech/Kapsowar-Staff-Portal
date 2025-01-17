<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Leave Application'">
            <ApprovalForm v-if="$route.params.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="Leave No.">
                        <WInput type="text" required="false" :value="form.docNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave Type" showMandatory="true" :valErrors="valErrors.leaveType">
                        <WSelect v-model="form.leaveType" id="leaveType" :isTomSelect="true" @change="onTypeChange()" :formMode="formMode">
                            <option value="">search</option>
                            <option v-for="type in leaveTypes" :value="type.Code">{{type.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave Opening Balance" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.OpeningBalance" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Days Entitled" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.Entitled" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Accrued Days" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.Accrued" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Days Taken" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.LeaveTaken" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave Balance" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.Remaining" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <span class="text-sm font-semibold">Leave Details:</span>
            <grid>
                <grid-col>
                    <field-group label="Leave Start Date" showMandatory="true" @change="GetLeaveDaysAndReturnDate()" :valErrors="valErrors.startDate">
                        <WInput type="date" required="true" v-model="form.startDate" :formMode="formMode" :min="!$root.authUser.IsApplyBackdatedLeave? new Date().toISOString().split('T')[0]:''" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave End Date" showMandatory="true" :valErrors="valErrors.endDate">
                        <WInput type="date" required="true" v-model="form.endDate" @change="GetLeaveDaysAndReturnDate()" :formMode="formMode" :min="!$root.authUser.IsApplyBackdatedLeave? new Date().toISOString().split('T')[0]:''"/>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="No. of Days" showMandatory="true" :valErrors="valErrors.appliedDays">
                        <WSelect required="true" v-model="form.appliedDays" @change="GetLeaveEndAndReturnDates()" :valErrors="valErrors.appliedDays" :formMode="formMode">
                            <option value="">--select--</option>
                            <option v-if="form.leaveType != 'ANNUAL' && balances.Remaining != undefined && balances.Remaining > 0" v-for="index in parseFloat(balances.Remaining)" :value="index">{{index}}</option>
                            <option v-else-if="form.leaveType == 'ANNUAL' && balances.Accrued != undefined && ((balances.Accrued - balances.LeaveTaken) > 0)" v-for="index in (parseFloat(balances.Accrued)-parseFloat(balances.LeaveTaken))" :value="index">{{index}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave Return Date" :showLoader="isFetchingDates">
                        <WInput type="date" required="true" v-model="form.returnDate" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="form.isAttachment">
                    <field-group label="Attachment File" :showMandatory="true" :valErrors="valErrors.attachmentFile">
                        <WInput type="file" accept="image/jpeg,image/png,application/pdf,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/msword" @input="onFileChange($event)" formMode="formMode" />
                    </field-group>
                    <span v-if="attachmentB64 != ''">
                        <button @click="OnDownloadAttachment()" class="text-sm py-1 text-blue-500 underline" title="View">View Attachment</button>
                    </span>
                </grid-col>
                <grid-col>
                    <field-group label="Comments (if any)">
                        <WTextarea required="false" v-model="form.comments" :formMode="formMode"></WTextarea>
                        <span class="!pt-0 text-xs">{{200-form.comments.length}} remaining</span>
                    </field-group>
                </grid-col>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="Approvals Status">
                        <WInput type="text" required="true" v-model="record.Approvals_Status" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="Applied By">
                        <WInput type="text" required="true" v-model="record.Applied_By" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <ApproversList :records="approvers" v-if="record.Approvals_Status != undefined && record.Approvals_Status != 'Open'"></ApproversList>
            <template #submission>
                <Actions :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </template>
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/ess/leave-application/LeaveActions.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    import { useActionsComposable } from '@/re-usables/composables/ActionsComposable.js'

    export default {
        components: { Actions, ...W, ...Apr },
        setup() {
            const { router, loadTomSelects } = useFormComposable();
            const { viewAttachment } = useActionsComposable();
            return { router, loadTomSelects, viewAttachment};
        },
        data() {
            return {
                pageProps: {
                    title: 'Leave Application',
                    pageType: "form",
                    controller: 'LeaveApplication',
                    formRoute: "/ess/leave/form",
                    listRoute: "/ess/leave/list",
                },
                form: {
                    docNo: '',
                    leaveType: '',
                    startDate: '',
                    endDate: '',
                    returnDate: '',
                    appliedDays: 0,
                    comments: '',
                    myAction: '',
                    isAttachment: false,
                    isAttachmentMandatory: false,
                    attachmentFile: '',
                    filename:''
                },
                balances: {},
                valErrors: [],
                leaveTypes: [],
                formMode: 'view',
                record: {},
                approvers: {},
                isFetchingBalances: false,
                isFetchingDates: false,
                approvalEntry: {},
                attachmentB64: "",
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.params.no;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        methods: {
            FnFetchSetups() {
                if (this.form.myAction != 'create') {
                    this.$root.loader.isLoading = true;
                }
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/formdata?myAction=' + this.form.myAction + "&docNo=" + this.form.docNo, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        } else {
                            this.leaveTypes = data.response.leaveTypes;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.Document_No;
                                this.form.leaveType = formData.Cause_of_Absence_Code;
                                this.form.startDate = formData.Start_Date.split('T')[0];
                                this.form.appliedDays = formData.Quantity;
                                this.form.endDate = formData.End_Date.split('T')[0];
                                this.form.returnDate = formData.Return_Date.split('T')[0];
                                this.form.appliedDays = formData.Quantity;
                                this.onTypeChange();
                                this.form.comments = formData.Description;
                                this.record = data.response.record;
                                if (this.record.Approvals_Status != "Open") {
                                    this.formMode = "view";
                                }
                                if (data.response.approvers != undefined && data.response.approvers != 'undefined') {
                                    this.approvers = data.response.approvers;
                                }
                                if (data.response.attachment != undefined && data.response.attachment != 'undefined') {
                                    this.attachmentB64 = data.response.attachment;
                                    this.form.attachmentFile = data.response.attachment;
                                    this.form.filename = this.record.Attachment_FileName;
                                }
                            }
                            this.loadTomSelects("leaveType");
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
            onTypeChange() {
                this.balances = {};
                if (this.form.leaveType != "") {
                    this.isFetchingBalances = true;
                    const requestOptions = {
                        method: "GET",
                    };
                    fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/LeaveBalances?leaveType=' + this.form.leaveType, requestOptions)
                        .then(response => {
                            return response.json();
                        })
                        .then(data => {
                            if (data && data.errors) {
                                this.$root.errorModal.isShow = true;
                                this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                            } else {
                                var balances = JSON.parse(data.response);
                                if (balances && parseFloat(balances.Remaining) < 1 && this.record != null && this.record.Approvals_Status == 'Open') {
                                    this.$root.FnNotification("Your " + this.form.leaveType + " leave balance is less than 1", 'bg-red-500', false);
                                } else {
                                    this.balances = balances;
                                }
                            }
                            this.isFetchingBalances = false;
                        }).catch((error) => {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                            this.isFetchingBalances = false;
                        });
                    //
                    var leaveType = this.leaveTypes.find(obj => obj.Code == this.form.leaveType);
                    if (leaveType != null) {
                        if (leaveType.Requires_Attachment || leaveType.Attachment_Mandatory) {
                            this.form.isAttachment = true;
                        } else {
                            this.form.isAttachment = false;
                        }
                        this.form.isAttachmentMandatory = leaveType.Attachment_Mandatory;
                    }
                }
            },
            GetLeaveDaysAndReturnDate() {
                this.form.returnDate = "";
                this.form.appliedDays = 0;
                if (this.form.startDate == "" || this.form.endDate == "") {
                    return;
                }
                this.isFetchingDates = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.form)
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/LeaveDaysAndReturnDate', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.valErrors) {
                            this.valErrors = data.valErrors;
                        }
                        else {
                            var res = JSON.parse(data.response);
                            this.form.appliedDays = parseInt(res.EndDate);
                            this.form.returnDate = `${res.ReturnDate.slice(4, 8)}-${res.ReturnDate.slice(2, 4)}-${res.ReturnDate.slice(0, 2)}`;
                            var maxDays = parseFloat(this.balances.Remaining);
                            if (this.form.appliedDays > maxDays) {
                                this.$root.errorModal.isShow = true;
                                this.$root.errorModal.message = "The maximum number of leave days you can apply for is " + maxDays;
                                this.form.appliedDays = 0;
                            }
                        }
                        this.isFetchingDates = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.isFetchingDates = false;
                    });
            },
            GetLeaveEndAndReturnDates() {
                if (this.form.appliedDays == 0 && this.form.startDate != "") {
                    return;
                }
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.form)
                };
                this.isFetchingDates = true;
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/LeaveEndAndReturnDates', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.valErrors) {
                            this.valErrors = data.valErrors;
                        }
                        else {
                            var res = JSON.parse(data.response);
                            this.form.endDate = `${res.EndDate.slice(4, 8)}-${res.EndDate.slice(2, 4)}-${res.EndDate.slice(0, 2)}`;
                            this.form.returnDate = `${res.ReturnDate.slice(4, 8)}-${res.ReturnDate.slice(2, 4)}-${res.ReturnDate.slice(0, 2)}`;
                        }
                        this.isFetchingDates = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.isFetchingDates = false;
                    });
            },
            onFileChange(event) {
                let files = event.target.files;
                var file = files[0];
                if (files.length) {
                    this.form.filename = file.name;
                    var self = this;
                    this.getBase64(file).then(data => {
                        self.form.attachmentFile = data;
                    });
                }
            },
            getBase64(file) {
                return new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = () => resolve(reader.result);
                    reader.onerror = error => reject(error);
                });
            },
            OnDownloadAttachment() {
                this.viewAttachment(this.record.Attachment_Extension,this.attachmentB64,this.record.Attachment_FileName);
            }
        },
        watch: {
            "form.comments": function (newValue,oldValue) {
                if (this.form.comments.length > 200) {
                    this.form.comments = this.form.comments.substring(0,199);
                }
            }
        }
    
    
    }
</script>