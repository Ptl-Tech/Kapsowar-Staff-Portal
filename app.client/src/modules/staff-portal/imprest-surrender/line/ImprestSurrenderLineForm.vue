<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-2">
                <grid-col>
                    <field-group label="Account No" :showMandatory="false" :valErrors="valErrors.accountNo">
                        <WInput type="text" required="true" :value="record.Account_No" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Account Name" :showMandatory="false" :valErrors="valErrors.accountName">
                        <WInput type="text" required="true" :value="record.Account_Name" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Imprest Amount" :showMandatory="false" :valErrors="valErrors.amount">
                        <WInput type="text" required="true" :value="record.Amount" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Actual Amount Spent" :showMandatory="true" :valErrors="valErrors.actualSpentAmount">
                        <WInput type="number" required="true" v-model="form.actualSpentAmount" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Cash Receipt No." :showMandatory="false" :valErrors="valErrors.cashReceiptNo">
                        <WInput type="text" required="true" v-model="form.cashReceiptNo" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Cash Receipt Amount" :showMandatory="false" :valErrors="valErrors.cashReceiptAmount">
                        <WInput type="number" required="true" v-model="form.cashReceiptAmount" :formMode="formMode" />
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
    import Actions from '@/modules/staff-portal/imprest-surrender/line/ImprestSurrenderLineActions.vue';
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
                    title: 'Surrender Line Form',
                    pageType: "form",
                    keys: { recKey: 'Line_No', parentKey: 'Surrender_Doc_No' },
                    controller: 'ImprestSurrenderLine',
                    formRoute: "/ess/imprest-surrender/line/form",
                    listRoute: "/ess/imprest-surrender/line/list",
                },
                form: {
                    parentId: '',
                    recId: 0,
                    myAction: '',
                    actualSpentAmount: 0,
                    cashReceiptAmount: 0,
                    cashReceiptNo: '',
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
                                this.form.actualSpentAmount = formData.Actual_Spent;
                                this.form.cashReceiptAmount = formData.Cash_Receipt_Amount;
                                this.form.cashReceiptAmount = formData.Cash_Receipt_Amount;
                                this.form.cashReceiptNo = formData.Cash_Receipt_No;
                                if (this.header.Status != "Pending") {
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