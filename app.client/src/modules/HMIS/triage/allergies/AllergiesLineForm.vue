<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Reason for Visit" showMandatory="true" :valErrors="valErrors.reasonForVisit">
                        <WSelect required="true" v-model="form.reasonForVisit" :formMode="formMode">
                            <option v-if="reasonForVisits" v-for="option in reasonForVisits" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Complaints" :valErrors="valErrors.complaints" :showMandatory="true">
                        <WTextarea required="false" v-model="form.complaints" :formMode="formMode" :maxLength="300"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Food Allergy" :valErrors="valErrors.foodAllergy" :showMandatory="true">
                        <WTextarea required="false" v-model="form.foodAllergy" :formMode="formMode" :maxLength="300"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Drug Allergy" :valErrors="valErrors.drugAllergy" :showMandatory="true">
                        <WTextarea required="false" v-model="form.drugAllergy" :formMode="formMode" :maxLength="300"></WTextarea>
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
                    controller: 'TriageAllergies',
                    keys: { recKey: 'ObservationNo', parentKey: 'ObservationNo' },
                    formRoute: "/hmis/" + this.$route.params.section + "/allergies/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/allergies/list",
                },
                form: {
                    parentKey: '',
                    recKey: 0,
                    myAction: '',
                    patientNo: "",
                    complaints: "",
                    reasonForVisit: 0,//options: 0-Blank,1-Patient not improving,2-Patient Deteriorated,3-New Presentation,4-Followup
                    foodAllergy: "",
                    drugAllergy: "",
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                reasonForVisits: [{ id: "0", name: "--select--" },{ id: "1", name: "Patient not Improving" }, { id: "2", name: "Patient Deteriorated" }, { id: "3", name: "New Presentation" }, { id: "4", name: "Followup" }],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? Number(this.$route.query.recId) : this.form.recId;
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
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.reasonForVisit = this.reasonForVisits.find(obj => obj.name == formData.ReasonforRevisit).id;
                                this.form.complaints = formData.Complaints;
                                this.form.foodAllergy = formData.FoodAllergy;
                                this.form.drugAllergy = formData.DrugAllergy;
                                if (this.header.Status == "Closed") {
                                    this.formMode = "view";
                                }
                            }
                        }
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