<template>
    <div :class="[pageProps.pageType == 'list'? 'flex-col':'flex-col sm:flex-row','flex gap-0.5']">
        <WButton v-if="pageProps.pageType == 'form' && (header.Approval_Status == 'Open') && formData.myAction != 'view'" class="!bg-blue-500 rounded-sm" @click="OnSubmit('save')"><CheckIcon class="iconSmall" />Save</WButton>
        <WRouterLink :to="pageProps.formRoute+'/edit/'+$route.params.no+'/'+record.No" v-if="record.No != undefined && header.Approval_Status == 'Open' && pageProps.pageType == 'list'" class="flex items-center !bg-blue-500" title="Edit"><PencilSquareIcon class="iconSmall" /><span> Edit</span></WRouterLink>
        <WRouterLink :to="pageProps.formRoute+'/view/'+$route.params.no+'/'+record.No" v-if="record.No != undefined && formData.myAction != 'view' && pageProps.pageType == 'list'" class="flex items-center !bg-gray-500" title="View"><EyeIcon class="iconSmall" /><span> View</span></WRouterLink>
        <WButton v-if="record.No != undefined && record.New && header.Approval_Status == 'Open'" @click="OnDeleteRecord()" class="flex items-center !bg-red-500" title="delete"><TrashIcon class="iconSmall" /><span> Delete</span></WButton>
    </div>
</template>
<script>
    import {W} from '@/re-usables/imports/ActionsPartComponents.js'
    import { useRouter } from 'vue-router'
    export default {
        props: {
            record: { default: null }, header: { default: null }, formData: { default: "" }, pageProps: { default: {} }},
        components: { ...W },
        emits: ["onAction"],
        setup() {
            const router = useRouter();
            return { router };
        },
        data() {
            return {
                isModal:false
            }
        },
        methods: {
            OnSubmit(action) {
                if (!confirm('Are you sure you want to ' + action +'?')) { return; }
                var body;
                var url;
                var docNo = "";
                if (this.pageProps.pageType == "list") {
                    if (action == "submit") {
                        url = this.appConfig.baseApiRoute + this.pageProps.controller + "/RequestApproval";
                        body = this.record;
                    }
                    docNo = this.record.No;
                } else {
                    if(action !== "cancel") {
                        url = this.appConfig.baseApiRoute + this.pageProps.controller + "/Store";
                        body = this.formData;
                        if (body.myAction.indexOf("#" + action) == -1) {
                            body.myAction = this.formData.myAction + "#" + action;
                        } 
                        docNo = this.formData.DocNo;
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
                                this.$router.go(-1);
                            }
                            this.$root.FnNotification(msg, 'bg-green-500', false);
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.$root.loader.isLoading = false;
                    });
            },
            OnDeleteRecord() {
                if (!confirm('Are you sure you want to delete this record?')) { return; }
                var url = this.appConfig.baseApiRoute + this.pageProps.controller + "/Delete";
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
                            if (this.pageProps.pageType == 'form') {
                                this.$router.go(-1);
                            } else {
                                this.$router.go();
                            }

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
