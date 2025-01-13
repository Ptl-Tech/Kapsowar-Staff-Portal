<template>
    <div>
      <FormPageTemplate :title="$route.params.action+' '+pageProps.caption">
        <div>
            <grid>
                <grid-col v-if="form.recId != ''">
                    <field-group label="Visit No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Patient" :showMandatory="false" :valErrors="valErrors.patientNo">
                        <TomSelectFetch v-if="cplIsFormLoaded" :tsId="'patients'" v-model="form.patientNo" :cProps="{valueField:'PatientNo',labelField:'LastName',searchField:['PatientNo','LastName'],recordField:'PatientNo',webservice:'QyPatients'}" :initValue="form.patientNo" :filter="record.Status != 'Completed'?'':`PatientNo eq '${record.PatientNo}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <!---->
                <grid-col>
                    <field-group label="Clinic" :showMandatory="false" :valErrors="valErrors.clinic">
                        <WSelect required="true" v-model="form.clinic" id="clinics" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="clinics" v-for="option in clinics" :value="option.No">{{option.No}}-{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <!---->
                <grid-col>
                    <field-group label="Doctor" :showMandatory="false" :valErrors="valErrors.doctor">
                        <WSelect required="true" v-model="form.doctor" id="doctors" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="doctors" v-for="option in doctors" :value="option.DoctorID">{{option.DoctorID}} - {{option.DoctorsName}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
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
    import Actions from '@/modules/hmis/visit/VisitHeaderActions.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    import TomSelectFetch from '@/re-usables/components/TomSelectFetch.vue';
    export default {
        components: { Actions, ...W, TomSelectDims, TomSelectFetch },
        setup() {
            const { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData, cplIsFormLoaded, cplOnAfterFormLoaded } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData, cplIsFormLoaded, cplOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Visit Form',
                    caption: 'Visit',
                    pageType: "form",
                    keys: { recKey: 'AppointmnetNo' },
                    controller: 'VisitsHeader',
                    formRoute: "/hmis/" + this.$route.params.section +"/visit/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/visit/list",
                },
                form: {
                    docNo: '',
                    myAction: "",
                    patientNo: "",
                    clinic: "",
                    doctor:"",
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {}, 
                formMode: 'view',
                dims: {},
                dimNos: [],
              doctors: [],
                clinics:[],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : '';
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
            if (this.$route.query.patientNo != undefined) {
                this.form.patientNo = this.$route.query.patientNo;
            }
        },
        beforeRouteLeave(to, from, next) {
            this.cpl_FnAutoSaveFormData({ to: to, from: from, next: next, status: this.record.Status });
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
                        this.doctors = data.response.doctors;
                        this.clinics = data.response.clinics;
                        if (this.form.myAction != "create") {
                          var formData = data.response.formData;
                          this.record = formData;
                          this.form.recId = formData.PatientNo;
                          this.form.patientNo = formData.PatientNo;
                          this.form.clinic = formData.SpecialClinics;
                          this.form.doctor = formData.Doctor;
                          if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                              this.approvers = data.response.approvers;
                          }
                          if (this.record != null) {
                              this.formMode = "view";
                          }
                          this.cpl_FnSetInitialFormData();
                        }
                        this.cplOnAfterFormLoaded();
                      this.loadTomSelects("clinics");
                    }
                } catch (error) {
                    this.$root.errorModal.isShow = true;
                    this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                } finally {
                    this.$root.loader.isLoading = false;
                }
          },
        },
        watch:{
          "form.clinic":function(){
            if (this.form.clinic != "") {
              this.doctors = this.doctors.filter(obj => obj.Specialization == this.form.clinic).filter(obj => obj.GlobalDimension1Code == this.$root.authUser.branchCode);
              this.loadTomSelects("doctors");
            }
          }

        }
    }
</script>
