<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Injection No." :showMandatory="true" :valErrors="valErrors.injectionNo">
                        <WSelect required="true" v-model="form.injectionNo" id="injections" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="injections" v-for="option in injections" :value="option.Code">{{option.Code}} - {{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Quantity" :valErrors="valErrors.quantity">
                        <WInput type="number" v-model="form.quantity" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Injection Date" :valErrors="valErrors.injectionDate">
                        <WInput type="date" v-model="form.injectionDate" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Injection Time" :valErrors="valErrors.injectionTimeStr">
                        <WInput type="time" v-model="form.injectionTimeStr" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Injection Remarks" :valErrors="valErrors.injectionRemarks" :showMandatory="true">
                        <WTextarea required="false" v-model="form.injectionRemarks" :formMode="formMode" :maxLength="300"></WTextarea>
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
    import Actions from '@/modules/hmis/triage/allergies/AllergiesLineActions.vue';
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
                    title: 'Patient Allergies Form',
                    pageType: "form",
                    controller: 'TriageInjections',
                    keys: { recKey: 'InjectionNo', parentKey: 'ObservationNo' },
                    formRoute: "/hmis/" + this.$route.params.section + "/allergies/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/allergies/list",
                },
                form: {
                    parentKey: '',
                    recKey: "",
                    myAction: '',
                    injectionNo: "",//setup QyInjectionsSetup
                    quantity: 0,
                    injectionDate:new Date().toISOString().split("T")[0],
                    injectionTimeStr: new Date().toTimeString().slice(0, 5),
                    injectionRemarks: "",
                    posted: true
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                reasonForVisits: [{ id: "0", name: "--select--" },{ id: "1", name: "Patient not Improving" }, { id: "2", name: "Patient Deteriorated" }, { id: "3", name: "New Presentation" }, { id: "4", name: "Followup" }],
                injections: [],

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
                            this.injections = data.response.injections;
                             this.form.patientNo = this.header.PatientNo;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.injectionNo = formData.InjectionNo;
                                this.form.quantity = formData.Quantity;
                                this.form.injectionDate = new Date(formData.InjectionDate.Year,formData.InjectionDate.Month,formData.InjectionDate.Day).toISOString().split("T")[0];
                                this.form.injectionTime = formData.injectionTime;
                                this.form.injectionRemarks = formData.InjectionRemarks;
                                if (this.header.Status == "Closed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("injections");
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