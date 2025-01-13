<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Process No." :showMandatory="true" :valErrors="valErrors.processNo">
                        <WSelect required="true" v-model="form.processNo" id="processes" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="processes" v-for="option in processes" :value="option.No">{{option.No}} - {{option.RequestDescription}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Item No" :showMandatory="true" :valErrors="valErrors.itemNo">
                        <WSelect required="true" v-model="form.itemNo" id="items" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="items" v-for="option in items" :value="option.No">{{option.No}} - {{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Unit Of Measure" :showMandatory="true" :valErrors="valErrors.unitOfMeasure">
                        <WSelect required="true" v-model="form.unitOfMeasure" id="units" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="units" v-for="option in units" :value="option.Code">{{option.Code}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Quantity" :valErrors="valErrors.quantity">
                        <WInput type="number" v-model="form.quantity" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Remarks" :valErrors="valErrors.remarks" :showMandatory="true">
                        <WTextarea required="false" v-model="form.remarks" :formMode="formMode" :maxLength="300"></WTextarea>
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
    import Actions from '@/modules/hmis/triage/dressings/DressingsLineActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    export default {
        components: { Actions, ...W, ModalPageTemplate },
        emits:["closeModal"],
        setup() {
            const { router, loadTomSelects } = useFormComposable();
            return { router, loadTomSelects };
        },
        data() {
            return {
                pageProps: {
                    title: 'Patient Dressings Form',
                    pageType: "form",
                    controller: 'TriageDressings',
                    keys: { recKey: 'ProcessNo', parentKey: 'ObservationNo' },
                    formRoute: "/hmis/" + this.$route.params.section + "/dressings/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/dressings/list",
                },
                form: {
                    parentKey: '',
                    recKey: "",
                    myAction: '',
                    observationNo: "",
                    patientNo: "",
                    processNo: "",//setup QyStoreRequisitionHeaders
                    itemNo: "",//Setup QyItems
                    unitOfMeasure: "",//Setup QyItemUnitsOfMeasure
                    quantity: 0,
                    remarks: ""

                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                processes: [],
                items: [],
                units: [],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ?this.$route.query.recId: this.form.recId;
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
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.header = data.response.header;
                            this.form.patientNo = this.header.PatientNo;
                            this.processes = data.response.processes;
                            this.units = data.response.units;
                            this.items = data.response.items;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.observationNo = formData.ObservationNo;
                                this.form.processNo = formData.ProcessNo;
                                this.form.itemNo = formData.ItemNo;
                                this.form.unitOfMeasure = formData.UnitofMeasure;
                                this.form.quantity = formData.Quantity;
                                this.form.remarks = formData.Remarks;
                                if (this.header.Status == "Closed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("processes,items,units");
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>