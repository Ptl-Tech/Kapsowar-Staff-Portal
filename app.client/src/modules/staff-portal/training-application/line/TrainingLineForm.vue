<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-1 min-h-40">
                <grid-col>
                    <field-group label="Staff No." :showMandatory="false" :valErrors="valErrors.staffNo">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="staffNo" v-model="form.staffNo" :cProps="{valueField:'No',labelField:'Full_Name',searchField:['No'],recordField:'Staff_No',webservice:'QyEmployees'}" :filter="record.Status != 'Released'? ``:`No eq '${record.Staff_No}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <Actions :record="record" :header="header" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </template>
        </ModalPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import ModalPageTemplate from '@/re-usables/page-templates/ModalPageTemplate.vue';
    import Actions from '@/modules/staff-portal/training-application/line/TrainingLineActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    export default {
        components: { Actions, ...W, ModalPageTemplate, TomSelectFetch },
        emits:["closeModal"],
        setup() {
            const { router, xIsFormLoaded,xOnAfterFormLoaded } = useFormComposable();
            return { router, xIsFormLoaded,xOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Training Participant Form',
                    pageType: "form",
                    keys: { recKey: 'Line_No', parentKey: 'Application_No' },
                    controller: 'TrainingLine',
                    formRoute: "/ess/training/line/form",
                    listRoute: "/ess/training/line/list",
                },
                form: {
                    parentId: '',
                    recId: 0,
                    myAction: '',
                    staffNo: '',
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata' + this.$root.fnGetUrlQueryFromObject(this.$route.query) + '&myAction=' + this.form.myAction, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = this.appConfig.errors.dataFetchFailure;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.header = data.response.header;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.staffNo = formData.Staff_No;
                                if (this.header.Status != "Open") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.xOnAfterFormLoaded();
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>