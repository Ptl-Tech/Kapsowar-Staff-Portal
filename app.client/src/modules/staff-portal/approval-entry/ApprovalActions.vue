<template>
    <div :class="[$route.query.entryNo == undefined? '':'py-1','flex gap-0.5 justify-center']">
        <WRouterLink v-if="$route.query.entryNo == undefined" :to="docLink" class="!bg-blue-500 rounded-sm !text-xs !py-0.5" title="Open Document"><EyeIcon class="iconSmall" /> <span class="hidden sm:flex">View</span></WRouterLink>
        <WButton v-if="record.Status == 'Open'" class="!bg-green-500 rounded-sm !text-xs !py-0.5" @click="OnApprove()"><CheckIcon class="iconSmall" />Approve</WButton>
        <WButton v-if="record.Status == 'Open'" class="!bg-red-500 rounded-sm !text-xs !py-0.5" @click="OnReject()"><XMarkIcon class="iconSmall" />Reject</WButton>
    </div>
    <ApprovalRejectionForm v-if="isReject" @onSubmitReject="onSubmitReject($event)" @close="isReject = false"/>
</template>
<script>
    import { W } from '@/re-usables/imports/ActionsPartComponents.js'
    import { MenuItem } from '@headlessui/vue'
    import ApprovalRejectionForm from '@/modules/staff-portal/approval-entry/ApprovalRejectionForm.vue';
    import { useRouter } from 'vue-router'
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import WButton from '@/re-usables/components/WButton.vue'
    import { EyeIcon,XMarkIcon } from '@heroicons/vue/24/outline'
    export default {
        props: { record: { default: null }, docLink: { default: "" }, formData: { default: "" }, pageProps: { default: {} }, recordLink: { default: "" } },
        components: { ...W, MenuItem, ApprovalRejectionForm, XMarkIcon, WRouterLink, WButton, EyeIcon },
        setup() {
            const router = useRouter();
            return { router };
        },
        data(){
            return{
                isPreview:false,
                base64: null,
                form: {
                    myAction: "",
                    docNo: "",
                    docType: "",
                    comments: "",
                    tableID: 0,
                    entryNo:"",
                },
                isReject: false
            }
        },
        methods:{
            OnApprove() {
                if (!confirm('Are you sure you want to approve this document?')) { return; }
                var url;
                url = this.appConfig.baseApiRoute + "ApprovalManagement" + "/DocumentApproval";
                this.$root.loader.isLoading = true;
                this.form.myAction = "approve";
                this.form.docNo = this.record.Document_No != undefined ? this.record.Document_No : this.record.No;
                this.form.docType = this.$route.params.docType == undefined ? this.$route.query.docType:this.$route.params.docType;
                this.form.tableID = this.record.Table_ID;
                this.form.entryNo = this.record.Entry_No != undefined ? String(this.record.Entry_No) : this.record.Employee_ID;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.form)
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {
                            var res;
                            var msg = "";
                            res = data.response;
                            if (res) {
                                msg = "Approved successfully.";
                                this.$root.FnNotification({ type: "poup", theme: "green", message: msg });
                                this.router.push("ess/approval-entry/list?status=Open&docType=" + this.$route.params.docType);
                                this.router.go();
                                this.ReducePendingApprovalCount();
                            } else {
                                var msg = this.appConfig.errors.generalFailure;
                                this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                            }
                           
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
            OnReject() {
                if (!confirm('Are you sure you want to reject this document?')) { return; }
                this.isReject = false;
                this.isReject = true;
            },
            onSubmitReject(response) {
                if (response.length > 0) {
                    this.form.comments = response;
                    this.isReject = false;
                    this.SubmitReject();
                }
            },
            SubmitReject() {
                var url;
                url = this.appConfig.baseApiRoute + "ApprovalManagement" + "/DocumentApproval";
                this.$root.loader.isLoading = true;
                this.form.myAction = "reject";
                this.form.docNo = this.record.Document_No != undefined ? this.record.Document_No : this.record.No;
                this.form.docType = this.$route.params.docType == undefined ? this.$route.query.docType : this.$route.params.docType;
                this.form.tableID = this.record.Table_ID;
                this.form.entryNo = this.record.Entry_No != undefined ? String(this.record.Entry_No) : this.record.Approver_ID;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.form)
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {
                            var res;
                            var msg = "";
                            res = data.response;
                            if (res) {
                                msg = "Rejected successfully.";
                                this.$root.FnNotification(msg, 'bg-green-500', false);
                                this.router.push("ess/approval-entry/list?status=Open&docType=" + this.$route.params.docType);
                                this.router.go();
                                this.ReducePendingApprovalCount();
                            } else {
                                var msg = this.appConfig.errors.generalFailure;
                                this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                            }

                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
            ReducePendingApprovalCount(){
                var authUser = localStorage.getItem("authUser");
                if (authUser != null && authUser != "undefined") {
                    var authUser2 = JSON.parse(authUser);
                    authUser2.PendingMyApproval = authUser2.PendingMyApproval - 1;
                    authUser2.PendingMyApproval = authUser2.PendingMyApproval < 0 ? 0 : authUser2.PendingMyApproval;
                    localStorage.setItem("authUser", JSON.stringify(authUser2));
                    this.$root.authUser.PendingMyApproval = authUser2.PendingMyApproval;
                }
            }
        }
    }
</script>
