<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' '+pageProps.caption">
            <PatientCard v-if="patient.PatientNo != undefined" :data="patient"></PatientCard>
            <div>
                <grid>
                    <grid-col v-if="form.recId != ''">
                        <field-group label="Observation No." :isOneLine="true">
                            {{form.recId}}
                        </field-group>
                    </grid-col>
                    <grid-col v-if="record.ObservationDate != undefined">
                        <field-group label="Date" :isOneLine="true">
                            {{$root.cplFnNavDateObjToString(record.ObservationDate)}}
                        </field-group>
                    </grid-col>
                    <grid-col v-if="record.ObservationDate != undefined">
                        <field-group label="Completed?" :isOneLine="true">
                            {{record.Completed? 'Yes':'No'}}
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <div class="py-4">
                <MenuTabs>
                    <MenuButton name="vitals" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Vitals</MenuButton>
                    <MenuButton name="allergies" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Allergies</MenuButton>
                    <MenuButton name="injections" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Injections</MenuButton>
                    <MenuButton name="dressings" :activeTab="activeSubPart" @activeTab="activeSubPart = $event">Dressings</MenuButton>
                </MenuTabs>
            </div>
            <!---->
            <VitalsLineList v-if="form.myAction != 'create' && activeSubPart == 'vitals'" :filter="{parentId:form.recId,subPart:'vitals'}" />
            <!---->
            <AllergiesLineList v-if="form.myAction != 'create'  && activeSubPart == 'allergies'" :filter="{parentId:form.recId,subPart:'allergies'}" />
            <!---->
            <InjectionsLineList v-if="form.myAction != 'create'  && activeSubPart == 'injections'" :filter="{parentId:form.recId,subPart:'injections'}" />
            <!---->
            <DressingsLineList v-if="form.myAction != 'create'  && activeSubPart == 'dressings'" :filter="{parentId:form.recId,subPart:'dressings'}" />
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/hmis/triage/TriagePatientHeaderActions.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    import VitalsLineList from '@/modules/hmis/triage/vitals/VitalsLineList.vue';
    import AllergiesLineList from '@/modules/hmis/triage/allergies/AllergiesLineList.vue';
    import InjectionsLineList from '@/modules/hmis/triage/injections/InjectionsLineList.vue';
    import DressingsLineList from '@/modules/hmis/triage/dressings/DressingsLineList.vue';
    import PatientCard from '@/modules/hmis/patient/PatientCard.vue';
    import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
    import MenuButton from '@/re-usables/components/MenuTabs/MenuButton.vue';

    export default {
        components: { Actions, ...W, TomSelectDims, PatientCard, MenuTabs, MenuButton, VitalsLineList, AllergiesLineList, InjectionsLineList, DressingsLineList },
        setup() {
            const { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Triage Form',
                    caption: 'Triage',
                    pageType: "form",
                    keys: { recKey: 'ObservationNo' },
                    controller: 'TriagePatientsHeader',
                    formRoute: "/hmis/" + this.$route.params.section +"/triage-patient/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/triage-patient/list",
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
                activeSubPart:"vitals"
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
              if(from.query.subPart != undefined){
                vm.activeSubPart = from.query.subPart;
              }
            });
        },
        //beforeRouteLeave(to, from, next) {
        //    this.cpl_FnAutoSaveFormData({ to: to, from: from, next: next, status: this.record.Status });
        //},
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
