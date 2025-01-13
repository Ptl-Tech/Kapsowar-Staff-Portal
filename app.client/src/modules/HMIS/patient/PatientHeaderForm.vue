<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' '+pageProps.caption">
            <grid>
                <grid-col v-if="form.recId != ''">
                    <field-group label="Patient No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="First Name" :showMandatory="true" :valErrors="valErrors.firstName">
                        <WInput type="text" required="true" v-model="form.firstName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Middle Name" :showMandatory="true" :valErrors="valErrors.middleName">
                        <WInput type="text" required="true" v-model="form.middleName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Last Name" :showMandatory="true" :valErrors="valErrors.lastName">
                        <WInput type="text" required="true" v-model="form.lastName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Gender" :showMandatory="true" :valErrors="valErrors.gender">
                        <WSelect required="true" v-model="form.gender" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="genders" v-for="option in genders" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Date of Birth" :showMandatory="true" :valErrors="valErrors.dobStr">
                        <WInput type="date" required="true" v-model="form.dobStr" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Age" :showMandatory="true" :valErrors="valErrors.age">
                        <WInput type="text" required="true" :value="form.age" :formMode="'view'" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Email" :showMandatory="true" :valErrors="valErrors.email">
                        <WInput type="email" required="true" v-model="form.email" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Residence" :showMandatory="true" :valErrors="valErrors.residence">
                        <WInput type="text" required="true" v-model="form.residence" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Nationality" :showMandatory="false" :valErrors="valErrors.nationality">
                        <WSelect required="true" v-model="form.nationality" id="nationalities" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="nationalities" v-for="option in nationalities" :value="option.Code">{{option.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="County" :showMandatory="false" :valErrors="valErrors.county">
                        <WSelect required="true" v-model="form.county" id="counties" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="counties" v-for="option in counties" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Sub County" :showMandatory="false" :valErrors="valErrors.subcounty">
                        <WSelect required="true" v-model="form.subcounty" id="subcounties" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="subcounties" v-for="option in subcounties" :value="option.SubCountyCode">{{option.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="County Ward" :showMandatory="false" :valErrors="valErrors.countyWard">
                        <WSelect required="true" v-model="form.countyWard" id="countyWards" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="countyWards" v-for="option in countyWards" :value="option.Code">{{option.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="ID Number/Passport No." :showMandatory="true" :valErrors="valErrors.idNumber">
                        <WInput type="text" required="true" v-model="form.idNumber" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Phone Number" :showMandatory="true" :valErrors="valErrors.phoneNumber">
                        <WInput type="text" required="true" v-model="form.phoneNumber" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Payment Mode" :showMandatory="true" :valErrors="valErrors.paymentMode">
                        <WSelect required="true" v-model="form.paymentMode" :formMode="formMode">
                            <option value="">--select--</option>
                            <option v-if="paymentModes" v-for="option in paymentModes" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Next of Kin Full Name" :showMandatory="true" :valErrors="valErrors.nextOfKinFullName">
                        <WInput type="text" required="true" v-model="form.nextOfKinFullName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Next of Kin Phone Number" :showMandatory="true" :valErrors="valErrors.nextOfKinPhoneNo">
                        <WInput type="text" required="true" v-model="form.nextOfKinPhoneNo" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Next of Kin Relationship" :showMandatory="true" :valErrors="valErrors.nextOfKinRelationship">
                        <WSelect required="true" v-model="form.nextOfKinRelationship" id="relationships" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="relationships" v-for="option in relationships" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
          <!---->
          <div  v-if="form.paymentMode == 1">
            <h3 class="font-semibold">Insurance Details</h3>
            <grid>
              <grid-col>
                <field-group label="Insurance Name" :showMandatory="true" :valErrors="valErrors.insuranceNo">
                  <WSelect required="true" v-model="form.insuranceNo" id="insurances" :isTomSelect="true" :formMode="formMode">
                    <option value="">search</option>
                    <option v-if="insurances" v-for="option in insurances" :value="option.No">{{option.Name}}</option>
                  </WSelect>
                </field-group>
              </grid-col>
              <grid-col>
                <field-group label="Insurance Prinicipal Member Name" :showMandatory="true" :valErrors="valErrors.insurancePrinicipalMemberName">
                  <WInput type="text" required="true" v-model="form.insurancePrinicipalMemberName" :formMode="formMode" />
                </field-group>
              </grid-col>
              <grid-col>
                <field-group label="Prinicipal Member?" :showMandatory="true" :valErrors="valErrors.isPrincipleMember">
                  <WSelect required="true" v-model="form.isPrincipleMember" :formMode="formMode">
                    <option value="0">--select--</option>
                    <option v-if="isPrincipleMember" v-for="option in isPrincipleMember" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                  </WSelect>
                </field-group>
              </grid-col>
              <grid-col>
                <field-group label="Membership No." :showMandatory="true" :valErrors="valErrors.membershipNo">
                  <WInput type="text" required="true" v-model="form.membershipNo" :formMode="formMode" />
                </field-group>
              </grid-col>
              <grid-col>
                <field-group label="Scheme Name" :showMandatory="true" :valErrors="valErrors.schemeName">
                  <WInput type="text" required="true" v-model="form.schemeName" :formMode="formMode" />
                </field-group>
              </grid-col>
            </grid>
          </div>
          <grid>
            <grid-col>
              <field-group label="How you knew about us?" :showMandatory="false" :valErrors="valErrors.howYouKnewABoutUs">
                <WSelect required="true" v-model="form.howYouKnewABoutUs" id="howYouKnewABoutUs" :isTomSelect="true" :formMode="formMode">
                  <option value="">search</option>
                  <option v-if="marketingStrategies" v-for="option in marketingStrategies" :value="option.Code">{{option.Description}}</option>
                </WSelect>
              </field-group>
            </grid-col>
          </grid>
            <!---->
            <div v-if="record.Status != undefined">
                <grid>
                    <grid-col>
                        <field-group label="Status">
                            <WInput type="text" required="false" v-model="record.Status" :formMode="'view'"></WInput>
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
    import Actions from '@/modules/hmis/patient/PatientHeaderActions.vue';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, TomSelectDims },
        setup() {
            const { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData, } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Patient Form',
                    caption: 'Patient',
                    pageType: "form",
                    keys: { recKey: 'PatientNo' },
                    controller: 'PatientsHeader',
                    formRoute: "/hmis/" + this.$route.params.section +"/patient/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/patient/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                    patientNo: "",
                    branchCode: "",
                    firstName: "",
                    middleName: "",
                    lastName: "",
                    gender: 0,//1= Male, 2=Female
                    dobStr: "",
                    age: 0,
                    nationality: "",//setup QyCountries
                    county: "",//setup QyCounties
                    countyWard: "",//setup QyCountyWards
                    idNumber: "",
                    phoneNumber: "",
                    paymentMode: 0,//0,1=Corporate,2=Cash
                    nextOfKinRelationship: "",//setup QyRelationships
                    nextOfKinFullName: "",
                    nextOfKinPhoneNo: "",
                    insuranceNo: "", //setup QyCustomers
                    insuranceName: "",
                    insurancePrinicipalMemberName: "",
                    isPrincipleMember: false,
                    membershipNo: "",
                    schemeName: "",
                    howYouKnewABoutUs: "", //setup QyMarketingStrategies
                    subcounty: "",//QySubcounties
                    email: "",
                    residence: "",
                    patientStatus: 0 //0=Alive,1=Dead,2=Transfer
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {}, 
                formMode: 'view',
                dims: {},
                dimNos: [],
                genders: [{ id: "1", name: "Male" },{ id: "2", name: "Female" }],
                paymentModes: [{ id: "1", name: "Corporate" },{ id: "2", name: "Cash" }],
                isPrincipleMember: [{ id: true, name: "Yes" },{ id: false, name: "No" }],
                patientStatuses: [{ id: "0", name: "Alive" },{ id: "1", name: "Dead" },{ id: "2", name: "Transfer" }],
                nationalities: [],
                counties: [],
                marketingStrategies:[],
                subcounties: [],
                relationships: [],
                insurances:[],
                countyWards:[],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : '';
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        beforeRouteLeave(to, from, next) {
            this.cpl_FnAutoSaveFormData({ to:to, from:from, next:next, status:"Open",saveAlways:true});
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
                        this.nationalities = data.response.nationalities;
                        this.counties = data.response.counties;
                        this.subcounties = data.response.subcounties;
                        this.marketingStrategies = data.response.marketingStrategies;
                        this.relationships = data.response.relationships;
                        this.insurances = data.response.insurances;
                        this.countyWards = data.response.countyWards;
                        if (this.form.myAction != "create") {
                          var formData = data.response.formData;
                          this.record = formData;
                          this.form.recId = formData.PatientNo;
                          this.form.patientNo = formData.PatientNo;
                          if (formData.Gender != " ") {
                            this.form.gender = this.genders.find(obj => obj.name == formData.Gender).id;
                          }
                          if (formData.PatientType != " ") {
                            this.form.paymentMode = this.paymentModes.find(obj => obj.name == formData.PatientType).id;
                          }
                          if (formData.Prinicipal != " ") {
                            this.form.isPrincipleMember = this.isPrincipleMember.find(obj => obj.id == Boolean(formData.Prinicipal)).id;
                          }
                          if (formData.PatientStatus != " ") {
                            this.form.patientStatus = this.patientStatuses.find(obj => obj.name == formData.PatientStatus).id;
                          }
                          this.form.firstName = formData.Surname;
                          this.form.middleName = formData.MiddleName;
                          this.form.lastName = formData.LastName;
                          this.form.dobStr = new Date(formData.DateOfBirth.Year,formData.DateOfBirth.Month,formData.DateOfBirth.Day).toISOString().split("T")[0];
                          this.form.nationality = formData.Nationality;
                          this.form.county = formData.PlaceofBirthDistrict;
                          this.form.idNumber = formData.IDNumber;
                          this.form.phoneNumber = formData.TelephoneNo1;
                          this.form.nextOfKinRelationship = formData.NextofkinRelationship;
                          this.form.nextOfKinFullName = formData.NextOfkinFullName;
                          this.form.nextOfKinPhoneNo = formData.NextOfkinAddress1;
                          this.form.insuranceNo = formData.InsuranceNo;
                          this.form.insurancePrinicipalMemberName = formData.Principal;
                          this.form.insurancePrinicipalMemberName = formData.MotherFullName;
                          this.form.membershipNo = formData.MembershipNo;
                          this.form.schemeName = formData.SchemeName;
                          this.form.howYouKnewABoutUs = formData.HowyouKnewAboutUs;
                          this.form.subcounty = formData.SubCountycode;
                          this.form.email = formData.Email;
                          this.form.countyWard = formData.CountyWard;
                          this.form.residence = formData.PlaceofBirthVillage;
                          if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                              this.approvers = data.response.approvers;
                          }
                          //if (this.record.Status == "Cleared") {
                          //    this.formMode = "view";
                          //}
                          this.cpl_FnSetInitialFormData();
                        }
                        this.loadTomSelects("nationalities,counties,countyWards,subcounties,relationships,howYouKnewABoutUs");
                    }
                } catch (error) {
                    this.$root.errorModal.isShow = true;
                    this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                } finally {
                    this.$root.loader.isLoading = false;
                }
          },
          getAge(dateString) {
                var today = new Date();
                var birthDate = new Date(dateString);
                var age = today.getFullYear() - birthDate.getFullYear();
                var m = today.getMonth() - birthDate.getMonth();
                if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                    age--;
                }
                this.form.age = age;
            }
        },
        watch:{
          "form.paymentMode":function(){
            if(this.form.paymentMode == 1){
               this.loadTomSelects("insurances");
            }
          },

           "form.dobStr":function(){
                 this.getAge(this.form.dobStr);
           }

        }
    }
</script>
