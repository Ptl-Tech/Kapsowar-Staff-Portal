<template>
    <div>
        <FormPageTemplate :title="pageProps.title+'-'+$route.params.action" @close="$emit('closeModal')">
            <ApprovalForm v-if="$route.query.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="$route.params.action != 'create'">
                    <field-group label="Leave Application No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave Type" :showMandatory="false" :valErrors="valErrors.leaveType">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="leaveTypes" v-model="form.leaveType" :cProps="{valueField:'Code',labelField:'Code',searchField:['Code'],recordField:'Leave_Type',webservice:'QyLeaveTypes'}" :filter="record.Status != 'Released'? ``:`Code eq '${record.LeaveType}'`" :record="record" :formMode="formMode" @change="FnOnLeaveTypeChange()" />
                    </field-group>
                </grid-col>
                <!--<grid-col>
                    <field-group label="Days Entitled" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.allocatedDays" formMode="view" />
                    </field-group>
                </grid-col>-->
                <grid-col>
                    <field-group label="Days Taken" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.daysTaken" formMode="view" />
                    </field-group>
                </grid-col>
                <!--<grid-col>
                    <field-group label="Leave brought forward" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.reimbursedDays" formMode="view" />
                    </field-group>
                </grid-col>-->
                <grid-col>
                    <field-group label="Leave Balance" :showLoader="isFetchingBalances">
                        <WInput type="number" required="false" :value="balances.balance" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <div class="text-sm font-semibold !py-2">Leave Details:</div>
            <grid>
                <grid-col>
                    <field-group label="Leave Start Date" showMandatory="true" @change="GetLeaveEndAndReturnDates()" :valErrors="valErrors.startDate">
                        <WInput type="date" required="true" v-model="form.startDate" :formMode="formMode" :min="!$root.authUser.IsApplyBackdatedLeave? new Date().toISOString().split('T')[0]:''" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="No. of Days" showMandatory="true" :valErrors="valErrors.noOfDays">
                        <WSelect required="true" v-model="form.noOfDays" @change="GetLeaveEndAndReturnDates()" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="balances.balance != undefined" v-for="index in parseFloat(balances.balance)" :value="index">{{index}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave End Date" showMandatory="true" :valErrors="valErrors.endDate">
                        <WInput type="date" required="true" v-model="form.endDate" @change="GetLeaveDaysAndReturnDate()" :formMode="'view'" :min="!$root.authUser.IsApplyBackdatedLeave? new Date().toISOString().split('T')[0]:''" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Leave Return Date" :showLoader="isFetchingDates">
                        <WInput type="date" required="true" v-model="form.returnDate" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Reliever" :showMandatory="false" :valErrors="valErrors.reliever">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="reliever" v-model="form.reliever" :cProps="{valueField:'No',labelField:'Full_Name',searchField:['No'],recordField:'Reliever_No',webservice:'QyEmployees'}" :filter="record.Status != 'Released'? `Responsibility_Center eq '${$root.authUser.responsibilityCenter}'`:`No eq '${record.Reliever_No}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Comments (if any)">
                        <WTextarea required="false" v-model="form.comments" :formMode="formMode"></WTextarea>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Attachment File (if applicable)" :showMandatory="false" :valErrors="valErrors.attachmentFile">
                        <WInput type="file" accept="image/jpeg,image/png,application/pdf,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/msword" @input="onFileChange($event)" formMode="formMode" />
                    </field-group>
                    <span v-if="attachmentB64 != ''">
                        <button @click="OnDownloadAttachment()" class="text-sm py-1 text-blue-500 underline" title="View">View Attachment</button>
                    </span>
                </grid-col>
                <grid-col v-if="$route.params.action != 'create'">
                    <field-group label="Approvals Statu">
                        <WInput type="text" required="true" v-model="record.Status" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="$route.params.action != 'create'">
                    <field-group label="Applied By">
                        <WInput type="text" required="true" v-model="record.Employee_No" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <ApproversList :records="approvers" v-if="record.Status != undefined && record.Status != 'Open'"></ApproversList>
            <template #submission>
                <Actions :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </template>
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import ModalPageTemplate from '@/re-usables/page-templates/ModalPageTemplate.vue';
    import Actions from '@/modules/staff-portal/leave-application/LeaveHeaderActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    export default {
        components: { Actions, ...W,...Apr, ModalPageTemplate, TomSelectFetch },
        emits: ["closeModal"],
        setup() {
            const { router, xIsFormLoaded, xOnAfterFormLoaded } = useFormComposable();
            return { router, xIsFormLoaded, xOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Leave Application Form',
                    pageType: "form",
                    keys: { recKey: 'Document_No' },
                    controller: 'LeaveApplicationHeader',
                    formRoute: "/ess/leave-application/form",
                    listRoute: "/ess/leave-application/list",
                },
                form: {
                    recId: '',
                    myAction: '',
                    leaveBalance: 0,
                    startDate: '',
                    endDate: '',
                    returnDate: '',
                    noOfDays: 0,
                    comments: '',
                    reliever: '',
                    isAttachment: false,
                    isAttachmentMandatory: false,
                    attachmentFile: '',
                    filename: ''
                },
                valErrors: [],
                record: {},
                formMode: 'view',
                approvers: {},
                approvalEntry: {},
                isFetchingBalances: false,
                isFetchingDates: false,
                balances: {},
                attachmentB64: ''
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
        },
        mounted() {
            if (this.record.Days_Applied != undefined) {
                this.form.noOfDays = this.record.Days_Applied;
            }
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                var url = this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata' + this.$root.fnGetUrlQueryFromObject(this.$route.query) + '&myAction=' + this.form.myAction;
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.header = data.response.header;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.leaveType = formData.Leave_Type;
                                this.form.startDate = this.$root.xFnNavDateObjToISODate(formData.Start_Date);
                                this.form.endDate = this.$root.xFnNavDateObjToISODate(formData.End_Date);
                                this.form.returnDate = this.$root.xFnNavDateObjToISODate(formData.Return_to_Work_Date);
                                this.form.reliever = formData.Reliever_No;
                                this.form.comments = formData.Reson_for_Request;
                                this.form.noOfDays = parseFloat(formData.Days_Applied);
                                if (this.record.Status != "Open") {
                                    this.formMode = "view";
                                }
                                if (data.response.approvers != undefined) {
                                    this.approvers = data.response.approvers;
                                }
                                this.GetLeaveDaysAndReturnDate();
                            }
                        }
                        this.xOnAfterFormLoaded();
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
            FnOnLeaveTypeChange() {
                this.isFetchingBalances = true;
                const requestOptions = {
                    method: "GET",
                };
                var url = this.appConfig.baseApiRoute + this.pageProps.controller + '/GetLeaveBalances?leaveType=' + this.form.leaveType;
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.balances = data;
                        }
                        this.isFetchingBalances = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.isFetchingBalances = false;
                    });
            },
            GetLeaveDaysAndReturnDate() {
                this.form.returnDate = "";
                this.form.noOfDays = 0;
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
                        else if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {
                            var res = data.response;
                            this.form.noOfDays = parseInt(res.noOfDays);
                            this.form.returnDate = new Date(res.returnDate).toISOString().split("T")[0];
                            var maxDays = parseFloat(this.balances.balance);
                            if (this.form.noOfDays > maxDays) {
                                var msg = "The maximum number of leave days you can apply for is " + maxDays;
                                this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                                this.form.noOfDays = 0;
                            }
                        }
                        this.isFetchingDates = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.isFetchingDates = false;
                    });
            },
            GetLeaveEndAndReturnDates() {
                if (this.form.noOfDays == 0 && this.form.startDate != "") {
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
                        else if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {
                            var res = data.response;
                            this.form.endDate = new Date(res.endDate).toISOString().split("T")[0];
                            this.form.returnDate = new Date(res.returnDate).toISOString().split("T")[0];
                        }
                        this.isFetchingDates = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
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
                this.viewAttachment(this.record.Attachment_Extension, this.attachmentB64, this.record.Attachment_FileName);
            }
        }
    }
</script>