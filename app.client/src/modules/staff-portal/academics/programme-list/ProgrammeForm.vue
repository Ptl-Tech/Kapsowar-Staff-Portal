<template>
    <div>
        <FormPageTemplate :title="pageProps.caption+' - '+$route.params.action">
            <grid>
                <grid-col v-if="$route.params.action != 'create'">
                    <field-group label="Request No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="dimNos[0] != undefined" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="record != null && record.Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Expected Receipt Date" :showMandatory="true" :valErrors="valErrors.expectedReceiptDate">
                        <WInput type="date" required="true" v-model="form.expectedReceiptDate" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid v-if="record[pageProps.keys.recKey] != undefined">
                <grid-col >
                    <field-group label="Status">
                        <WInput type="text" required="false" :value="record.Status" formMode="view" />
                    </field-group>
                </grid-col>
            </grid>
            <!--<subparts-->
            <div v-if="record[pageProps.keys.recKey] != undefined">
                <MenuTabs class="py-2">
                    <MenuButton name="lines" :activeTab="activeSubpart" @activeTab="activeSubpart = $event">Purchase Request Lines</MenuButton>
                </MenuTabs>
                <Lines v-if="activeSubpart == 'lines'" :filter="{parentId:record[pageProps.keys.recKey]}" />
            </div><!--</subparts-->
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
        </FormPageTemplate>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/staff-portal/purchase-request/header/PurchaseRequestHeaderActions.vue';
    import Lines from '@/modules/staff-portal/purchase-request/line/PurchaseRequestLineList.vue';
    import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
    import MenuButton from '@/re-usables/components/MenuTabs/MenuButton.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, MenuTabs, MenuButton, TomSelectDims, Lines },
        setup() {
            const { router, xIsFormLoaded, xOnAfterFormLoaded, xFnAutoSaveFormData } = useFormComposable();
            return { router, xIsFormLoaded, xOnAfterFormLoaded, xFnAutoSaveFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Purchase Request Form',
                    caption: 'Purchase Request',
                    pageType: "form",
                    keys: { recKey: 'No' },
                    controller:'PurchaseReqHeader',
                    formRoute: "/ess/purchase-request/header/form",
                    listRoute: "/ess/purchase-request/header/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                    parentId: "",
                    expectedReceiptDate: null,
                    dimensionSet: {}
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
            this.xFnAutoSaveFormData({ to: to, from: from, next: next, status:'Open', saveAlways: true });
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
                            this.dimNos = data.response.dims.dimNos.split(",");
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.expectedReceiptDate = this.$root.xFnNavDateObjToISODate(formData.Requested_Receipt_Date);
                                if (this.record.Status != "Open") {
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
