<template>
    <div :class="[pageProps.pageType == 'list'? 'flex-col':'flex-col sm:flex-row','flex gap-0.5']">
        <WButton v-if="record != null && record.Status != 'Completed'" class="!bg-green-500 rounded-sm" @click="OnMarkAsCompleted()"><CheckIcon class="iconSmall" />Mark as Completed</WButton>
        <WRouterLink v-if="record != null && $route.params.action != 'view' && $route.params.action != 'create'" :to="pageProps.formRoute+'/view?recId='+record[pageProps.keys.recKey]" class="flex items-center !bg-gray-500" title="View"><EyeIcon class="iconSmall" /><span> View</span></WRouterLink>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/ActionsPartComponents.js'
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
                        url = this.appConfig.baseApiRoute + this.pageProps.controller + "/CancelApproval";
                        body = this.record;
                        docNo = this.record.No;
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
                            if (action == "submit") {
                                msg = "Sent to Triage successfully.";
                                this.pageProps.docNo = res;
                                if (source.GuiAllowed) {
                                  this.router.push("/hmis/reception/patient/list");
                                } else {
                                    source.next();
                                }
                            }
                            else if (action == "cancel") {
                                msg = "Reopened successfully.";
                                this.router.go();
                            }
                            else if (action == "submit") {
                                msg = "Submitted successfully.";
                                if (res != "") {
                                    this.router.push(this.pageProps.formRoute + "/edit?recId=" + res);
                                    this.router.go();
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
                if (!confirm('Are you sure you want to delete?')) { return; }
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
                            this.router.push({ path: this.pageProps.listRoute, query: { status: 'Active' }, replace: true })
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.$root.loader.isLoading = false;
                    });
            },
          OnMarkAsCompleted() {
            if (!confirm('Are you sure you want to mark as completed?')) { return; }
            var url = this.appConfig.baseApiRoute + this.pageProps.controller + "/MarkAsCompleted";
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
                  this.$root.FnNotification("Completed successfully", 'bg-green-500', false);
                    this.router.push("/hmis/consultation/consultation/list?status=New-Today")
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
