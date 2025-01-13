<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' '+pageProps.caption">
            <PatientCard v-if="patient.PatientNo != undefined" :data="patient"></PatientCard>
            <div>
                <grid v-if="patient.DateOfBirth != undefined">
                    <grid-col v-if="form.recId != ''">
                        <field-group label="Treatment No." :isOneLine="true">
                            {{form.recId}}
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Status" :isOneLine="true">
                            {{record.Status}}
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Doctor ID" :isOneLine="true">
                            {{record.DoctorID}}
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Treatment Type" :isOneLine="true">
                            {{record.TreatmentType}}
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Treatment Date" :isOneLine="true">
                            {{$root.cplFnNavDateObjToString(record.TreatmentDate)}}
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Treatment Time" :isOneLine="true">
                            {{record.TreatmentTime}}
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <div class="py-4">
                <h3 class="font-semibold pb-2 text-gray-500 underline italic">Triage Details</h3>
                <MenuTabs>
                    <MenuButton name="vitals" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Vitals</MenuButton>
                    <MenuButton name="allergies" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Allergies</MenuButton>
                    <MenuButton name="injections" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Injections</MenuButton>
                    <MenuButton name="dressings" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Dressings</MenuButton>
                </MenuTabs>
            </div>
            <div v-if="record.ObservationNo != undefined">
                <!---->
                <VitalsLineList v-if="activeSubPart == 'vitals'" :filter="{parentId:record.ObservationNo,subPart:'vitals',source:'consultation'}" :props="{isNew:true}"/>
                <!---->
                <AllergiesLineList v-if="activeSubPart == 'allergies'" :filter="{parentId:record.ObservationNo,subPart:'allergies',source:'consultation'}" :props="{isNew:true}"/>
                <!---->
                <InjectionsLineList v-if="activeSubPart == 'injections'" :filter="{parentId:record.ObservationNo,subPart:'injections',source:'consultation'}" :props="{isNew:false}"/>
                <!---->
                <DressingsLineList v-if="activeSubPart == 'dressings'" :filter="{parentId:record.ObservationNo,subPart:'dressings',source:'consultation'}" :props="{isNew:true}"/>
                <!---->
            </div>
            <hr />
            <div class="pb-4">
                <h3 class="font-semibold pb-2 text-gray-500 underline italic">Consultation Details</h3>
                <MenuTabs>
                    <MenuButton name="consultation-signs" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Signs</MenuButton>
                    <MenuButton name="consultation-symptoms" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Symptoms</MenuButton>
                    <MenuButton name="consultation-laboratory" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Laboratory</MenuButton>
                    <MenuButton name="consultation-radiology" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Radiology</MenuButton>
                    <MenuButton name="consultation-diagnosis" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Diagnosis</MenuButton>
                    <MenuButton name="consultation-injections" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Injections</MenuButton>
                    <MenuButton name="consultation-admissions" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Admission</MenuButton>
                    <MenuButton name="consultation-referral" :activeTab="activeSubPart2" @activeTab="activeSubPart2 = $event">Referral</MenuButton>
                </MenuTabs>
            </div>
            <div v-if="record.TreatmentNo != undefined">
                <ConsultationSigns v-if="activeSubPart2 == 'consultation-signs'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-signs'}" />
                <ConsultationSymptoms v-if="activeSubPart2 == 'consultation-symptoms'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-symptoms'}" />
                <ConsultationLaboratory v-if="activeSubPart2 == 'consultation-laboratory'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-laboratory'}" />
                <ConsultationRadiology v-if="activeSubPart2 == 'consultation-radiology'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-radiology'}" />
                <ConsultationDiagnosis v-if="activeSubPart2 == 'consultation-diagnosis'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-diagnosis'}" />
                <ConsultationInjections v-if="activeSubPart2 == 'consultation-injections'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-injections'}" />
                <ConsultationPrescriptions v-if="activeSubPart2 == 'consultation-prescriptions'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-prescriptions'}" />
                <ConsultationAdmissions v-if="activeSubPart2 == 'consultation-admissions'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-admissions'}" />
                <ConsultationReferral v-if="activeSubPart2 == 'consultation-referral'" :filter="{parentId:record.TreatmentNo,subPart2:'consultation-referral'}" />
            </div>
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
    import Actions from '@/modules/hmis/consultation/treatment-header/TreatmentHeaderActions.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    const VitalsLineList = defineAsyncComponent(() => import('@/modules/hmis/triage/vitals/VitalsLineList.vue'));
    const AllergiesLineList = defineAsyncComponent(() => import('@/modules/hmis/triage/allergies/AllergiesLineList.vue'));
    const InjectionsLineList = defineAsyncComponent(() => import('@/modules/hmis/triage/injections/InjectionsLineList.vue'));
    const DressingsLineList = defineAsyncComponent(() => import('@/modules/hmis/triage/dressings/DressingsLineList.vue'));
    const ConsultationSigns = defineAsyncComponent(() => import('@/modules/hmis/consultation/signs/ConsultationSignsList.vue'));
    const ConsultationSymptoms = defineAsyncComponent(() => import('@/modules/hmis/consultation/symptoms/ConsultationSymptomsList.vue'));
    const ConsultationLaboratory = defineAsyncComponent(() => import('@/modules/hmis/consultation/laboratory/ConsultationLaboratoryList.vue'));
    const ConsultationRadiology = defineAsyncComponent(() => import('@/modules/hmis/consultation/radiology/CLT_RadiologyList.vue'));
    const ConsultationDiagnosis = defineAsyncComponent(() => import('@/modules/hmis/consultation/diagnosis/CLT_DiagnosisList.vue'));
    const ConsultationInjections = defineAsyncComponent(() => import('@/modules/hmis/consultation/injections/CLT_InjectionsList.vue'));
    const ConsultationPrescriptions = defineAsyncComponent(() => import('@/modules/hmis/consultation/prescription/CLT_PrescriptionList.vue'));
    const ConsultationAdmissions = defineAsyncComponent(() => import('@/modules/hmis/consultation/admission/CLT_AdmissionList.vue'));
    const ConsultationReferral = defineAsyncComponent(() => import('@/modules/hmis/consultation/referral/CLT_ReferralList.vue'));
    import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
    import MenuButton from '@/re-usables/components/MenuTabs/MenuButton.vue';
    import PatientCard from '@/modules/hmis/patient/PatientCard.vue';

    export default {
        components: { Actions, ...W, TomSelectDims, PatientCard, MenuTabs, MenuButton, VitalsLineList, AllergiesLineList, InjectionsLineList, DressingsLineList, ConsultationSigns, ConsultationSymptoms, ConsultationLaboratory, ConsultationRadiology, ConsultationDiagnosis, ConsultationInjections, ConsultationPrescriptions, ConsultationAdmissions,ConsultationReferral },
        setup() {
            const { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Consultation Form',
                    caption: 'Consultation',
                    pageType: "form",
                    keys: { recKey: 'TreatmentNo' },
                    controller: 'TreatmentsHeader',
                    formRoute: "/hmis/" + this.$route.params.section + "/consultation/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/consultation/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                    observationNo: "",
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {}, 
                formMode: 'view',
                dims: {},
                dimNos: [],
                patient: {},
                activeSubPart: "vitals",
                activeSubPart2:"consultation-signs"
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : '';
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        beforeRouteEnter(to, from, next) {
            next((vm) => {
                if (from.query.subPart != undefined) {
                    vm.activeSubPart = from.query.subPart;
                }
                if (from.query.subPart2 != undefined) {
                    vm.activeSubPart2 = from.query.subPart2;
                }
            });
        },
        methods: {
          async FnFetchSetups() {
                try {
                    this.$root.loader.isLoading = true;
                    const requestOptions = {
                        method: "GET",
                    };
                    var response = await fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/getformdata?myAction='+this.form.myAction+"&recId="+this.form.recId, requestOptions);
                    const data = await response.json();
                    if (data && data.errors) {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = data.errors;
                    } else {
                        //this.dims = data.response.dims.dims;
                        //this.dimNos = data.response.dims.dimNos.split(",");
                        if (this.form.myAction != "create") {
                          var formData = data.response.formData;
                          this.patient = data.response.patient;
                          this.record = formData;
                          this.form.recId = formData[this.pageProps.keys.recKey];
                          this.form.patientNo = formData.PatientNo;
                          if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                              this.approvers = data.response.approvers;
                          }
                          //if (this.record != null) {
                          //    this.formMode = "view";
                          //}
                          this.cpl_FnSetInitialFormData();
                        }
                    }
                } catch (error) {
                    this.$root.errorModal.isShow = true;
                    this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                } finally {
                    this.$root.loader.isLoading = false;
                }
          },
          OnActiveSubPart(section){
              this.activeSubPart = section;
          }
        },
    }
</script>
