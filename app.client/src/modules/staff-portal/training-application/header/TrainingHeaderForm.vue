<template>
    <div>
        <FormPageTemplate :title="pageProps.caption+' - '+$route.params.action">
            <ApprovalForm v-if="$route.query.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="$route.params.action != 'create'">
                    <field-group label="Imprest No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Training Need" :showMandatory="true" :valErrors="valErrors.trainingNeed">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="trainingNeed" v-model="form.trainingNeed" :cProps="{webservice:'QyTrainingNeeds',valueField:'Code',labelField:'Description',searchField:['Code','Description'],recordField:'Training_Need_Code'}" :filter="record.Status == undefined || record.Status == 'Open'? `Status eq 'Approved' and Blocked eq false`:`Code eq '${record.Training_Need_Code}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Start Date" :showMandatory="true" :valErrors="valErrors.startDate">
                        <WInput type="date" required="true" v-model="form.startDate" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Reliever" :showMandatory="false" :valErrors="valErrors.relieverNo">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="relieverNo" v-model="form.relieverNo" :cProps="{valueField:'No',labelField:'Full_Name',searchField:['No'],recordField:'Reliever_no',webservice:'QyEmployees'}" :filter="record.Status != 'Released'? `Responsibility_Center eq '${$root.authUser.responsibilityCenter}'`:`No eq '${record.Reliever_no}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Comments" :showMandatory="true" :valErrors="valErrors.comments">
                        <WTextarea required="false" v-model="form.comments" :formMode="formMode"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid v-if="record[pageProps.keys.recKey] != undefined">
                <grid-col>
                    <field-group label="Status">
                        <WInput type="text" required="false" :value="record.Status" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <!--<subparts-->
            <div v-if="record[pageProps.keys.recKey] != undefined">
                <MenuTabs class="py-2">
                    <MenuButton name="lines" :activeTab="activeSubpart" @activeTab="activeSubpart = $event">Training Participants</MenuButton>
                </MenuTabs>
                <Lines v-if="activeSubpart == 'lines'" :header="record" :filter="{parentId:record[pageProps.keys.recKey]}" />
            </div><!--</subparts-->
            <ApproversList :records="approvers" v-if="record.Status != undefined && record.Status != 'Open'"></ApproversList>
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/staff-portal/training-application/header/TrainingHeaderActions.vue';
    import Lines from '@/modules/staff-portal/training-application/line/TrainingLineList.vue';
    import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
    import MenuButton from '@/re-usables/components/MenuTabs/MenuButton.vue';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    export default {
        components: { Actions, ...W, ...Apr, MenuTabs, MenuButton, TomSelectFetch, Lines },
        setup() {
            const { router, xIsFormLoaded, xOnAfterFormLoaded, xFnAutoSaveFormData } = useFormComposable();
            return { router, xIsFormLoaded, xOnAfterFormLoaded, xFnAutoSaveFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Training Form',
                    caption: 'Training',
                    pageType: "form",
                    keys: { recKey: 'Application_No' },
                    controller: 'TrainingHeader',
                    formRoute: "/ess/training/header/form",
                    listRoute: "/ess/training/header/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                    parentId: "",
                    dimensionSet: {},
                    comments: "",
                    trainingNeed: "",
                    startDate: null,
                    relieverNo: "",
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {},
                formMode: 'view',
                activeSubpart: "lines",
                dims: [],
                dimNos: []
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        beforeRouteLeave(to, from, next) {
            this.xFnAutoSaveFormData({ to: to, from: from, next: next, status: 'Open', saveAlways: true });
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
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "popup", theme: "red", message: msg });
                            this.router.go(-1);
                        } else {
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.comments = formData.Comments;
                                this.form.trainingNeed = formData.Training_Need_Code;
                                this.form.relieverNo = formData.Reliever_no;
                                this.form.startDate = this.$root.xFnNavDateObjToISODate(formData.Start_Date);
                                if (this.record.Status != "Open") {
                                    this.formMode = "view";
                                }
                                if (data.response.approvers != undefined) {
                                    this.approvers = data.response.approvers;
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
