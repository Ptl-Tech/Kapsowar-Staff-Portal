<template>
    <div>
        <ModalPageTemplate :title="pageProps.title+' - '+$route.params.action" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Injection" :showMandatory="false" :valErrors="valErrors.injectionNo">
                        <WSelect required="true" v-model="form.injectionNo" id="injectionSetups" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="injectionSetups" v-for="option in injectionSetups" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Location" :showMandatory="false" :valErrors="valErrors.location">
                        <WSelect required="true" v-model="form.location" id="locations" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="locations" v-for="option in locations" :value="option.Code">{{option.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Injection Given?" :showMandatory="true" :valErrors="valErrors.injectionGiven">
                        <WSelect required="true" v-model="form.injectionGiven" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="injectionGiven" v-for="option in injectionGiven" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Injection Unit Of Measure" :showMandatory="false" :valErrors="valErrors.injectionUnitOfMeasure">
                        <WSelect required="true" v-model="form.injectionUnitOfMeasure" id="unitsOfMeasure" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="unitsOfMeasure" v-for="option in unitsOfMeasure" :value="option.Code">{{option.Code}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Injection Quantity" :showMandatory="true" :valErrors="valErrors.injectionQuantity">
                        <WInput type="number" required="true" v-model="form.injectionQuantity" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Duration" :showMandatory="true" :valErrors="valErrors.duration">
                        <WInput type="text" required="true" v-model="form.duration" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Remarks" :valErrors="valErrors.remarks" :showMandatory="true">
                        <WTextarea required="false" v-model="form.remarks" :formMode="formMode" :maxLength="250"></WTextarea>
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
    import Actions from '@/modules/hmis/consultation/injections/CLT_InjectionsActions.vue';
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
                    title: 'Injection Form',
                    pageType: "form",
                    keys: { recKey: 'InjectionNo', recKey2: 'ItemNo', parentKey: 'TreatmentNo' },
                    controller: 'CLT_Injections',
                    formRoute: "/hmis/" + this.$route.params.section + "/clt-injections/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/clt-injections/list",
                    name: "consultation-injections",
                },
                form: {
                    parentKey: '',
                    recId: '',
                    recKey2: '',
                    myAction: '',
                    treatmentNo: "",
                    injectionNo: "",
                    location: "",
                    injectionGiven: true,
                    duration: "",
                    injectionUnitOfMeasure: "",
                    injectionQuantity: 0,
                    remarks: "",
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                injectionSetups: [],
                locations: [],
                unitsOfMeasure: [],
                injectionGiven: [{ id: true, name: "Yes" },{ id: false, name: "No" }],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.form.recKey2 = this.form.myAction != 'create' ? this.$route.query.recKey2 : this.form.recKey2;
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
                            this.injectionSetups = data.response.injectionSetups;
                            this.items = data.response.items;
                            this.locations = data.response.locations;
                            this.unitsOfMeasure = data.response.unitsOfMeasure;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.recKey2 = formData[this.pageProps.keys.recKey2];
                                this.form.treatmentNo = formData[this.pageProps.keys.parentKey];
                                this.form.injectionNo = formData.InjectionNo;
                                this.form.location = formData.Location;
                                this.form.injectionGiven = this.injectionGiven.find(obj => obj.id == Boolean(formData.InjectionGiven)).id;
                                this.form.duration = formData.Duration;
                                this.form.injectionUnitOfMeasure = formData.InjectionUnitofMeasure;
                                this.form.injectionQuantity = formData.InjectionQuantity;
                                this.form.remarks = formData.InjectionRemarks;
                                if (this.header.Status == "Completed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("injectionSetups,items,locations,unitsOfMeasure");
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