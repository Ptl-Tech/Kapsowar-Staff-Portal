<template>
    <div>
        <FormPageTemplate :title="pageProps.caption+' - '+$route.params.action">
            <ApprovalForm v-if="$route.query.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="$route.params.action != 'create'">
                    <field-group label="Surrender No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Imprest No." :showMandatory="false" :valErrors="valErrors.imprestNo">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="imprestNo" v-model="form.imprestNo" :cProps="{webservice:'QyImprestHeaders',valueField:'No',labelField:'No',searchField:['No'],recordField:'Imprest_Issue_Doc_No'}" :filter="record.Status != 'Released'? `Account_No eq '${$root.authUser.customerNo}' and Posted eq true`:`No eq '${record.Imprest_Issue_Doc_No}'`" :record="record" :formMode="record.Status != undefined? 'view':formMode" />
                    </field-group>
                </grid-col>
                <grid-col v-if="record[pageProps.keys.recKey] != undefined">
                    <field-group label="Imprest Issue date">
                        <WInput type="text" required="false" :value="$root.xFnNavDateObjToISODate(record.Imprest_Issue_Date)" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="record[pageProps.keys.recKey] != undefined">
                    <field-group label="Imprest Amount">
                        <WInput type="text" required="false" :value="record.Amount" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="record[pageProps.keys.recKey] != undefined && dimNos[0] != undefined" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="record != null && record.Status != 'Pending'?false:true" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="'view'"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col v-if="record[pageProps.keys.recKey] != undefined">
                    <field-group label="Status">
                        <WInput type="text" required="false" :value="record.Status" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <!--<subparts-->
            <div v-if="record[pageProps.keys.recKey] != undefined">
                <MenuTabs class="py-2">
                    <MenuButton name="lines" :activeTab="activeSubpart" @activeTab="activeSubpart = $event">Surrender Lines</MenuButton>
                </MenuTabs>
                <Lines v-if="activeSubpart == 'lines'" :filter="{parentId:record[pageProps.keys.recKey]}" />
            </div><!--</subparts-->
            <ApproversList :records="approvers" v-if="record.Status != undefined && record.Status != 'Pending'"></ApproversList>
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
    import Actions from '@/modules/staff-portal/imprest-surrender/header/ImprestSurrenderHeaderActions.vue';
    import Lines from '@/modules/staff-portal/imprest-surrender/line/ImprestSurrenderLineList.vue';
    import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
    import MenuButton from '@/re-usables/components/MenuTabs/MenuButton.vue';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    export default {
        components: { Actions, ...W,...Apr, MenuTabs, MenuButton, TomSelectFetch, TomSelectDims, Lines },
        setup() {
            const { router, xIsFormLoaded, xOnAfterFormLoaded, xFnAutoSaveFormData } = useFormComposable();
            return { router, xIsFormLoaded, xOnAfterFormLoaded, xFnAutoSaveFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Imprest Surrender Form',
                    caption: 'Imprest Surrender',
                    pageType: "form",
                    keys: { recKey: 'No' },
                    controller: 'ImprestSurrenderHeader',
                    formRoute: "/ess/imprest-surrender/header/form",
                    listRoute: "/ess/imprest-surrender/header/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                    parentId: "",
                    imprestNo: "",
                    dimensionSet: {},
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
        //beforeRouteLeave(to, from, next) {
        //    this.xFnAutoSaveFormData({ to: to, from: from, next: next, status: 'Open', saveAlways: true });
        //},
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
                            this.dimNos = data.response.dims.dimNos.split(",");
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.imprestNo = formData.Imprest_Issue_Doc_No;
                                if (this.record.Status != "Pending") {
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
