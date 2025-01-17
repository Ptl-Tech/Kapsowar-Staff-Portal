<template>
    <div :class="[pageProps.pageType == 'list'? 'flex-col':'flex-col sm:flex-row','flex gap-0.5']">
        <WButton v-if="pageProps.pageType == 'form' && (formData.docNo == 'new' || record.Approval_Status == 'Open') && $route.params.action != 'view'" class="!bg-blue-500 rounded-sm" @click="OnSubmit('save')"><CheckIcon class="iconSmall" />Save</WButton>
        <WRouterLink v-if="record != null && record.Approval_Status == 'Open' && $route.params.action != 'edit'" :to="pageProps.formRoute+'/edit/'+record.No" class="flex items-center !bg-blue-500" title="Edit"><PencilSquareIcon class="iconSmall" /><span> Edit</span></WRouterLink>
        <WButton v-if="record != null && record.Approval_Status == 'Open'" @click="OnSubmit('submit')" class="flex items-center !bg-green-500" title="submit"><PaperAirplaneIcon class="iconSmall" /><span> Submit</span></WButton>
        <WButton v-if="record != null && record.Approval_Status == 'Open'" @click="OnDeleteRecord()" class="flex items-center !bg-red-500" title="delete"><TrashIcon class="iconSmall" /><span> Delete</span></WButton>
        <WRouterLink v-if="record != null && $route.params.action != 'view' && $route.params.action != 'create'" :to="pageProps.formRoute+'/view/'+record.No" class="flex items-center !bg-gray-500" title="View"><EyeIcon class="iconSmall" /><span> View</span></WRouterLink>
        <WButton v-if="record != null && record.Approval_Status == 'Pending Approval' && $root.authUser.EmployeeNo == record.Requester_Employee_No" class="!bg-red-500 rounded-sm" @click="OnSubmit('cancel')" title="cancel approval"><ArrowUturnLeftIcon class="iconSmall" /><span> Cancel Approval</span></WButton>
        <WButton v-if="record != null && record.Approval_Status == 'Pending Approval' && $root.authUser.EmployeeNo == record.Requester_Employee_No" class="!bg-teal-500 rounded-sm" @click="OnDelegate()" title="delegate approval"><ArrowUturnLeftIcon class="iconSmall" /><span> Delegate</span></WButton>
        <WButton v-if="record != null && record.Approval_Status == 'Released'" class="!bg-green-500 rounded-sm" @click="OnGetReport()" title="Print/Download"><PrinterIcon class="iconSmall" /><span> Print/Download</span></WButton>
    </div>
</template>
<script>
    import {W} from '@/re-usables/imports/ActionsPartComponents.js'
    import { useRouter } from 'vue-router'
    export default {
        props: { record: { default: null },formData: { default: "" }, pageProps: {default:{}} },
        components: { ...W },
        setup() {
            const router = useRouter();
            return { router };
        },
        data(){
            return{
                isPreview:false,
                base64:null,
            }

        },
        methods:{
            OnSubmit(action, source = { GuiAllowed: true, next: null }) {
                if (source.GuiAllowed && (!confirm('Are you sure you want to ' + action + '?'))) { return; }
                var body;
                var url;
                var docNo = "";
                this.$emit('onValErrors', []);
                if (this.pageProps.pageType == "list") {
                    if (action == "submit") {
                        url = this.appConfig.baseApiRoute + this.pageProps.controller + "/RequestApproval";
                        body = this.record;
                    }
                    if (action == "cancel") {
                        url = this.appConfig.baseApiRoute + this.pageProps.controller + "/CancelApproval";
                        body = this.record;
                    }
                    docNo = this.record.No;
                } else {
                    if(action !== "cancel") {
                        url = this.appConfig.baseApiRoute + this.pageProps.controller + "/Store";
                        body = this.formData;
                        if (body.myAction.indexOf("#" + action) == -1) {
                            body.myAction = this.$route.params.action + "#" + action;
                        } 
                        docNo = this.formData.DocNo;
                    } else {
                        if (action == "cancel") {
                            url = this.appConfig.baseApiRoute + this.pageProps.controller + "/CancelApproval";
                            body = this.record;
                            docNo = this.record.No;
                        }
                    }
                }
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
                            //this.$parent.valErrors = data.valErrors;
                            this.$emit('onValErrors', data.valErrors);
                            this.$root.FnNotification(this.appConfig.errors.validationErrors, 'bg-red-500', false);
                        }
                        else if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        }
                        else {
                            var res;
                            var msg = "";
                            res = data.response;
                            if (action == "save") {
                                msg = "saved successfully.";
                                this.pageProps.docNo = res;
                                if (source.GuiAllowed) {
                                    this.router.push(this.pageProps.formRoute + "/edit/" + res);
                                } else {
                                    source.next();
                                }
                            }
                            else if (action == "cancel") {
                                msg = "Reopened successfully.";
                                this.router.push(this.pageProps.formRoute + "/edit/" + docNo);
                            }
                            else if (action == "submit") {
                                msg = "Submitted successfully.";
                                if (res != "") {
                                    this.router.push(this.pageProps.formRoute + "/view/" + res);
                                }
                            } else {
                                this.router.push(this.pageProps.listRoute + "?status=Open");
                            }
                            if (source.GuiAllowed) {
                                this.$root.FnNotification(msg, 'bg-green-500', false);
                            }
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.$root.loader.isLoading = false;
                    });
            },
            OnDeleteRecord() {
                if (!confirm('Are you sure you want to delete this recall?')) { return; }
                var url = this.appConfig.baseApiRoute + this.pageProps.controller+"/Delete";
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.record)
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.valErrors) {
                            this.valErrors = data.valErrors;
                        }
                        else if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        }
                        else {
                            this.$root.FnNotification("Deleted successfully", 'bg-green-500', false);
                            this.router.push({ path: this.pageProps.listRoute, query: { status: 'Open' }, replace: true })
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.$root.loader.isLoading = false;
                    });
            },
            OnGetReport() {
                var route = this.appConfig.baseApiRoute + this.pageProps.controller+"/Report/"+this.record.No;
                route = route.replaceAll("/","__");
                var url =  encodeURI("/ess/file-preview/"+route);
                this.router.push(url);
            },
            OnDelegate() {
                var url = this.appConfig.baseApiRoute + 'ApprovalManagement' + "/DelegateDocumentApproval";
                var body = { docNo: '', docType:''};
                body.docNo = this.record.No;
                body.docType = 'staffAdvance';
                this.$root.loader.isLoading = true;
                //
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
                            this.$root.FnNotification(this.appConfig.errors.validationErrors, 'bg-red-500', false);
                        }
                        else if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        }
                        else {
                            var res;
                            res = data.response;
                            if (res) {
                                this.$router.go();
                                this.$root.FnNotification("Delegated Successfully", 'bg-green-500', false);
                            }
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>
