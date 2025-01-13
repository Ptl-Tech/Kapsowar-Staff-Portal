<template>
    <div :class="[pageProps.pageType == 'list'? 'flex-col':'flex-col sm:flex-row','flex gap-0.5']">
        <WButton v-if="pageProps.pageType == 'form' && $route.params.action != 'view' && editable" class="!bg-blue-500 rounded-sm" @click="OnSubmit('save')"><CheckIcon class="iconSmall" />Save</WButton>
        <WButton v-if="record.No != 'undefined' && $route.params.action != 'create' && editable" @click="OnDeleteRecord()" class="flex items-center !bg-red-500" title="delete"><TrashIcon class="iconSmall" /><span> Delete</span></WButton>
        <WButton v-if="record.No != 'undefined' && $route.params.action != 'create'" @click="OnDownloadAttachment()" class="flex items-center !bg-green-500" title="download"><ArrowDownTrayIcon class="iconSmall" /><span> View</span></WButton>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/ActionsPartComponents.js'
    import { useActionsComposable } from '@/re-usables/composables/ActionsComposable.js'
    import { useRouter } from 'vue-router'
    export default {
        props: {
            record: { default: null }, header: { default: null }, formData: { default: "" }, pageProps: { default: {} },editable: { default: true }
},
        components: { ...W },
        emits: ["onAction"],
        setup() {
            const router = useRouter();
            const { viewAttachment } = useActionsComposable();
            return { router, viewAttachment };
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
                var url = this.appConfig.baseApiRoute + this.pageProps.controller+"/Delete";
                this.$root.loader.isLoading = true;
                this.record.isEditable = this.editable;
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
            OnDownloadAttachment() {
                var url = this.appConfig.baseApiRoute + this.pageProps.controller+"/Download";
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
                            this.viewAttachment(this.record.Attachment_Extension, data.response, this.record.Attachment_FileName);
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
