<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' '+pageProps.caption">
            <grid>
                <grid-col v-if="form.recId != ''">
                    <field-group label="Visitor No.">
                        <WInput type="text" required="false" :value="form.recId" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Reason for Visit" showMandatory="true" :valErrors="valErrors.reasonForVisit">
                        <WSelect required="true" v-model="form.reasonForVisit" :formMode="formMode">
                            <option value="">--select--</option>
                            <option v-if="visitReasons" v-for="option in visitReasons" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Visit Category" showMandatory="true" :valErrors="valErrors.visitorCategory">
                        <WSelect required="true" v-model="form.visitorCategory" :formMode="formMode" @change="onChangeVisitCategory()">
                            <option value="">--select--</option>
                            <option v-if="visitorCategories" v-for="option in visitorCategories" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col v-if="form.visitorCategory == 1">
                    <field-group label="Patient to visit No." :showMandatory="false" :valErrors="valErrors.personToVisitNo">
                        <TomSelectFetch v-if="cplIsFormLoaded" tsId="patients" v-model="form.personToVisitNo" :cProps="{valueField:'PatientNo',labelField:'Surname',searchField:['PatientNo','Surname','FirstName','MiddleName'],recordField:'VisitorNumber',webservice:'QyPatients'}" :filter="record.Status != 'Cleared'?'':`PatientNo eq '${record.VisitorNumber}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col v-if="form.visitorCategory == 2">
                    <field-group label="Employee to visit No." :showMandatory="false" :valErrors="valErrors.personToVisitNo">
                        <TomSelectFetch v-if="cplIsFormLoaded" tsId="employees" v-model="form.personToVisitNo" :cProps="{valueField:'No',labelField:'LastName',searchField:['No','MiddleName','FirstName','LastName'],recordField:'VisitorNumber',webservice:'QyEmployees'}" :filter="record.Status != 'Cleared'?'':`No eq '${record.VisitorNumber}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Visitor ID Number/Passport No." showMandatory="true" @change="CheckVisitorIDNumber()" :valErrors="valErrors.idNumber">
                        <WInput type="text" required="true" v-model="form.idNumber" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="First Name" showMandatory="true" :valErrors="valErrors.firstName">
                        <WInput type="text" required="true" v-model="form.firstName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Middle Name" showMandatory="true" :valErrors="valErrors.middleName">
                        <WInput type="text" required="true" v-model="form.middleName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Last Name" showMandatory="true" :valErrors="valErrors.lastName">
                        <WInput type="text" required="true" v-model="form.lastName" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Purpose of Visit" showMandatory="true" :valErrors="valErrors.purposeOfVisit">
                        <WInput type="text" required="true" v-model="form.purposeOfVisit" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Phone Number" showMandatory="true" :valErrors="valErrors.phoneNumber">
                        <WInput type="text" required="true" v-model="form.phoneNumber" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Department" showMandatory="true" :valErrors="valErrors.department">
                        <TomSelectFetch v-if="cplIsFormLoaded" tsId="departments" v-model="form.department" :cProps="{valueField:'Code',labelField:'Name',searchField:['Code','Name'],recordField:'Department',webservice:'QyDimensionValue'}" :filter="record.Status != 'Cleared'? `GlobalDimensionNo eq 1`:`GlobalDimensionNo eq 1 and No eq '${record.Department}'`" :record="record" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Car Registration No." :showMandatory="false" :valErrors="valErrors.carRegistrationNo">
                        <WInput type="text" required="true" v-model="form.carRegistrationNo" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Visit Pass No." :showMandatory="false" :valErrors="valErrors.visitorPassNo">
                        <WInput type="text" required="true" v-model="form.visitorPassNo" :formMode="formMode" />
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
    import Actions from '@/modules/hmis/visitor/VisitorHeaderActions.vue';
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
                    title: 'Visitor Form',
                    caption: 'Visitor',
                    pageType: "form",
                    controller: 'VisitorsHeader',
                    formRoute: "/hmis/" + this.$route.params.section +"/visitor/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/visitor/list",
                    keys: { recKey: 'No' },
                },
                form: {
                    recId: '',
                    visitorNo: '',
                    myAction: '',
                    visitorCategory: 0,
                    reasonForVisit:0,
                    firstName: '',
                    lastName: '',
                    middleName:'',
                    purposeOfVisit:'',
                    idNumber:'',
                    phoneNumber:'',
                    carRegistrationNo:'',
                    personToVisit: '',
                    personToVisitNo:'',
                    department:'',
                    visitorPassNo:'',
                    dimensionSet: {},
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {}, 
                formMode: 'view',
                dims: {},
                dimNos: [],
                visitorCategories: [{ id: "0", name: "Other" }, { id: "1", name: "Patient" }, { id: "2", name: "Employee" }],
                visitReasons: [{ id: "0", name: "--select--" }, { id: "1", name: "Medication" },{ id: "2", name: "Official" }],
                employees: [],
                patients: [],
                departments: [],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.recId = this.form.myAction != 'create'? this.$route.query.recId:'';
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
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
                        this.dims = data.response.dims.dims;
                        this.dimNos = data.response.dims.dimNos.split(",");
                      //  this.employees = data.response.employees;
                      this.departments = data.response.departments;
                      //this.patients = data.response.patients;
                        if (this.form.myAction != "create") {
                          var formData = data.response.formData;
                          this.record = formData;
                          this.form.recId = formData.No;
                          this.form.visitorCategory = this.visitorCategories.find(obj => obj.name == formData.VisitorCategory).id;
                          this.onChangeVisitCategory();
                            this.form.firstName = formData.FirstName;
                            this.form.middleName = formData.MiddleName;
                            this.form.lastName = formData.LastName;
                            if (formData.ReasonForVisit != "0") {
                                this.form.reasonForVisit = this.visitReasons.find(obj => obj.id == formData.ReasonForVisit).id;
                            }
                          this.form.purposeOfVisit = formData.PurposeofVisit;
                          this.form.idNumber = formData.IDNumber;
                          this.form.phoneNumber = formData.PhoneNumber;
                          this.form.carRegistrationNo = formData.VisitorCarRegNumber;
                          this.form.personToVisitNo = formData.VisitorNo;
                          this.form.department = formData.Department;
                          this.form.visitorPassNo = formData.VisitorPassNo;
                          if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                              this.approvers = data.response.approvers;
                          }
                          if (this.record.Status == "Cleared") {
                              this.formMode = "view";
                          }
                          this.cpl_FnSetInitialFormData();
                        }
                        this.cplOnAfterFormLoaded();
                        this.loadTomSelects("departments");
                    }
                } catch (error) {
                    this.$root.errorModal.isShow = true;
                    this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                } finally {
                    this.$root.loader.isLoading = false;
                }
          },
          onChangeVisitCategory() {
            if (this.form.visitorCategory == 1) {
              this.loadTomSelects("patients");
            }
            else if (this.form.visitorCategory == 2) {
              this.loadTomSelects("employees");
            }
          }
        },
    }
</script>
