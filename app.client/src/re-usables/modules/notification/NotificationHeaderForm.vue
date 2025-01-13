<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Notification'">
            <!---->
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Subject" :valErrors="valErrors.subject">
                        <WInput type="text" v-model="form.subject" :formMode="'view'"></WInput>
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Message" :valErrors="valErrors.content">
                        <WTextarea type="text" v-model="form.content" :formMode="'view'"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Date Published" :valErrors="valErrors.datePublished">
                        <WInput type="text" v-model="form.datePublished" :formMode="'view'"></WInput>
                    </field-group>
                </grid-col>
            </grid>
        </FormPageTemplate>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
  import Actions from '@/re-usables/modules/notification/NotificationHeaderActions.vue';
    export default {
        components: { Actions, ...W, defineAsyncComponent },
        setup() {
            const { router, loadTomSelects, setDropdownDims } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims };
        },
        data() {
            return {
                pageProps: {
                    title: 'Notification',
                    pageType: "form",
                    controller: 'NotificationHeader',
                    formRoute: "/hmis/notification/form",
                    listRoute: "/hmis/notification/list",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    viewedBy: '',
                },
                record: {},
                valErrors: [],
                formMode: 'view',
                isFetching: false,
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.recId = this.$route.params.no;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/getformdata?myAction='+this.form.myAction+"&docNo="+this.form.recId, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        } else {
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.recId = formData.No;
                                this.form.subject = formData.Subject;
                                this.form.content = formData.Content;
                                this.form.viewedBy = formData.Viewed_By_Txt;
                                this.form.datePublished = formData.Date_Published.split("T")[0];
                                if(this.form.viewedBy.includes(this.$root.authUser.EmployeeNo+";") == false){
                                    this.ViewNotification();
                                    if(this.$root.authUser.newNotifications != 0){
                                        var authUser = localStorage.getItem("authUser");
                                        if (authUser != null && authUser != "undefined") {
                                            var authUser2 = JSON.parse(authUser);
                                            authUser2.newNotifications = this.$root.authUser.newNotifications-1;
                                            localStorage.setItem("authUser", JSON.stringify(authUser2));
                                            this.$root.authUser.newNotifications = authUser2.newNotifications;
                                        }
                                    }
                                }
                            }
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
            ViewNotification() {
                const requestOptions = {
                    method: "POST",
                    headers: {'Content-Type':"application/json"},
                    body:JSON.stringify(this.form)
                };
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+"/Store", requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        } else {
                            //
                        }
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                    });
            }
        }
    }
</script>
