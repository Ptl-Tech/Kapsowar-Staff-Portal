<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-2">
                <grid-col>
                    <field-group label="Type" :showMandatory="true" :valErrors="valErrors.type">
                        <WSelect required="true" v-model="form.type" :formMode="formMode">
                            <option v-if="types" v-for="option in types" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Issuing Store" :showMandatory="true" :valErrors="valErrors.issuingStore">
                        <TomSelectFetch v-if="xIsFormLoaded" tsId="issuingStore" v-model="form.issuingStore" :cProps="{webservice:'QyLocations',valueField:'Code',labelField:'Name',searchField:['Code','Name'],recordField:'Issuing_Store'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`Code eq '${record.Issuing_Store}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="No" :showMandatory="true" :valErrors="valErrors.no">
                        <div v-if="form.type == 1"><TomSelectFetch v-if="xIsFormLoaded" tsId="Items" v-model="form.no" :cProps="{webservice:'QyItems',valueField:'No',labelField:'Description',searchField:['No','Description'],recordField:'No'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`No eq '${record.No}'`" :record="record" :formMode="formMode" /></div>
                        <div v-if="form.type == 2 && form.issuingStore != ''"><TomSelectFetch v-if="xIsFormLoaded" tsId="Items" v-model="form.no" :cProps="{webservice:'QyFixedAssets',valueField:'No',labelField:'Description',searchField:['No','Description'],recordField:'No'}" :filter="header.Status != undefined && header.Status == 'Open'? ``:`No eq '${record.No}'`" :record="record" :formMode="formMode" /></div>
                    </field-group>
                </grid-col>

                <grid-col>
                    <field-group label="Quantity" :showMandatory="false" :valErrors="valErrors.quantity">
                        <WInput type="number" required="true" v-model="form.quantity" :formMode="formMode" />
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
    import Actions from '@/modules/staff-portal/store-request/line/StoreRequestLineActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    export default {
        components: { Actions, ...W, ModalPageTemplate, TomSelectFetch },
        emits: ["closeModal"],
        setup() {
            const { router, xIsFormLoaded, xOnAfterFormLoaded } = useFormComposable();
            return { router, xIsFormLoaded, xOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Store request Line Form',
                    pageType: "form",
                    keys: { recKey: 'Line_No', parentKey: 'Requistion_No' },
                    controller: 'StoreReqLine',
                    formRoute: "/ess/store-request/line/form",
                    listRoute: "/ess/store-request/line/list",
                },
                form: {
                    parentId: '',
                    recId: 0,
                    myAction: '',
                    type: 0,
                    no: "",
                    procurementPlan: "",
                    requestReason: "",
                    location: "",
                    quantity: 0,
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                types: [{ id: "0", name: "--select--" }, { id: "1", name: "Item" }, { id: "2", name: "Minor Asset" }],
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
                                this.form.type = this.types.find(obj => obj.name == formData.Type).id;
                                this.form.no = formData.No;
                                this.form.issuingStore = formData.Issuing_Store;
                                this.form.quantity = formData.Quantity;
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