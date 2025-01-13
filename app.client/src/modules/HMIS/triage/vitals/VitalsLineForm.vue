<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Pulse Rate" :valErrors="valErrors.pulseRate">
                        <WInput type="text" v-model="form.pulseRate" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Pain" :valErrors="valErrors.pain">
                        <WInput type="number" v-model="form.pain" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Blood Pressure" :valErrors="valErrors.bloodPreasure">
                        <WInput type="text" v-model="form.bloodPreasure" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Temperature" :valErrors="valErrors.temperature">
                        <WInput type="number" v-model="form.temperature" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Height" :valErrors="valErrors.height">
                        <WInput type="number" v-model="form.height" :formMode="formMode" @input="calculateBMI()"/>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Weight" :valErrors="valErrors.weight">
                        <WInput type="number" v-model="form.weight" :formMode="formMode" @input="calculateBMI()"/>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="BMI" :valErrors="valErrors.bmi">
                        <WInput type="number" v-model="form.bmi" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="SP02" :valErrors="valErrors.sP02">
                        <WInput type="text" v-model="form.sP02" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Respiration Rate" :valErrors="valErrors.respirationRate">
                        <WInput type="text" v-model="form.respirationRate" :formMode="formMode" />
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
    import Actions from '@/modules/hmis/triage/vitals/VitalsLineActions.vue';
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
                    title: 'Patient Vitals Form',
                    pageType: "form",
                    controller: 'TriageVitals',
                    keys: { recKey: 'LineNo', parentKey: 'ObservationNo' },
                    formRoute: "/hmis/" + this.$route.params.section + "/vitals/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/vitals/list",
                },
                form: {
                    parentKey: '',
                    recKey: 0,
                    myAction: '',
                    patientNo: "",
                    pulseRate: "",
                    pain: 0,
                    bloodPreasure: "",
                    temperature: 0,
                    height: 0,
                    weight: 0,
                    bmi: 0,
                    sP02: "",
                    respirationRate: ""

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
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.header = data.response.header;
                             this.form.patientNo = this.header.PatientNo;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.pulseRate = formData.PulseRate;
                                this.form.pain = formData.Pain;
                                this.form.bloodPreasure = formData.BloodPressure;
                                this.form.temperature = formData.Temperature;
                                this.form.height = formData.Height;
                                this.form.weight = formData.Weight;
                                this.form.bmi = Math.round(formData.BMI*100)/100;
                                this.form.sP02 = formData.SP02;
                                this.form.respirationRate = formData.RespirationRate;
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
            },
            calculateBMI(){
                if(this.form.height != 0 && this.form.weight != 0){
                    this.form.bmi = Math.round((this.form.weight/ ((this.form.height/100) * (this.form.height/100)))*100)/100;
                }
            }
        }
    }
</script>