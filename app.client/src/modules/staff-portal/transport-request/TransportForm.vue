<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Transport Request'">
            <ApprovalForm v-if="$route.params.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="Request No.">
                        <WInput type="text" required="false" :value="form.docNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group v-if="dimNos[0] != null" :label="$root.DimCaption(dimNos[0])" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNos[0]" :dimNo="dimNos[0]" :record="record" :loadAll="record != null && record.Approval_Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNos[0]+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Start Date & Time when vehicle is required" showMandatory="true" :valErrors="valErrors.startDateTime">
                        <WInput type="datetime-local" required="true" v-model="form.startDateTime" :formMode="formMode" :min="new Date().toISOString().slice(0, -8)" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="End Date & Time when vehicle is required" showMandatory="true" :valErrors="valErrors.endDateTime">
                        <WInput type="datetime-local" required="true" v-model="form.endDateTime" :formMode="formMode" :min="new Date().toISOString().slice(0, -8)" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="!grid-cols-1">
                <grid-col>
                    <field-group label="Purpose for which the vehicle is requested" showMandatory="true" :valErrors="valErrors.purpose">
                        <WTextarea required="true" v-model="form.purpose" :formMode="formMode"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid>
                <grid-col>
                    <field-group v-if="dimNos[1] != null" :label="$root.DimCaption(dimNos[1])" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNos[1]" :dimNo="dimNos[1]" :record="record" :loadAll="record != null && record.Approval_Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNos[1]+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group v-if="dimNos[2] != null" :label="$root.DimCaption(dimNos[2])" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNos[2]" :dimNo="dimNos[2]" :record="record" :loadAll="record != null && record.Approval_Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNos[2]+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group v-if="dimNos[3] != null" :label="$root.DimCaption(dimNos[3])" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNos[3]" :dimNo="dimNos[3]" :record="record" :loadAll="record != null && record.Approval_Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNos[3]+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group v-if="dimNos[4] != null" :label="$root.DimCaption(dimNos[4])" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNos[4]" :dimNo="dimNos[4]" :record="record" :loadAll="record != null && record.Approval_Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNos[4]+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Destination" showMandatory="false" :valErrors="valErrors.destination">
                        <WInput type="text" required="true" v-model="form.destination" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Class of vehicle" showMandatory="true" :valErrors="valErrors.classOfVehicle">
                        <WSelect required="true" v-model="form.classOfVehicle" id="vehicleClass" isTomSelect="true" :formMode="formMode" @change="calculateEstimatedCost()">
                            <option value="">search</option>
                            <option v-if="vehicleClasses" v-for="classOfVehicle in vehicleClasses" :value="classOfVehicle.Code">{{classOfVehicle.Vehicle_Class_Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Estimated Distance of trip in Kms" showMandatory="false" :valErrors="valErrors.estimatedDistance" @input="calculateEstimatedCost()">
                        <WInput type="number" required="true" v-model="form.estimatedDistance" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Estimated Cost of trip" showMandatory="false" :valErrors="valErrors.estimatedCost">
                        <WInput type="number" required="true" v-model="form.estimatedCost" :formMode="'view'" />
                    </field-group>
                </grid-col>
            </grid>
            <!---->
            <div v-if="record != null">
                <grid>
                    <grid-col>
                        <field-group label="Approval Status">
                            <WInput type="text" required="false" v-model="record.Approval_Status" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Status">
                            <WInput type="text" required="false" v-model="record.Status" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Driver Assigned">
                            <WInput type="text" required="false" v-model="record.Driver_Assigned" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Vehicle">
                            <WInput type="text" required="false" v-model="record.Vehicle_Assigned" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                    <grid-col>
                        <field-group label="Vehicle">
                            <WTextarea required="false" v-model="record.Transport_Manager_Comments" :formMode="'view'"></WTextarea>
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <!---->
            <div v-if="$root.authUser.IsTransportManager && record != null && record.Status == 'UnAssigned' && record.Approval_Status == 'Released'">
                <hr class="py-2" />
                <h4 class="text-md pb1 sm:pb-2 font-semibold">Assign Driver & Vehicle</h4>
                <form>
                    <grid class="sm:!grid-cols-1">
                        <grid-col>
                            <field-group label="Vehicle" showMandatory="true" :valErrors="valErrors.vehicle">
                                <WSelect required="true" v-model="assign.vehicle" id="vehicle" :tomSelect="true">
                                    <option value="">search</option>
                                    <option v-if="vehicles" v-for="vehicle in vehicles" :value="vehicle.Vehicle_Reg_No">{{vehicle.Vehicle_Reg_No}} - {{vehicle.Vehicle_Class}}</option>
                                </WSelect>
                            </field-group>
                        </grid-col>
                    </grid>
                    <grid class="sm:!grid-cols-2">
                        <grid-col>
                            <field-group label="Driver" showMandatory="true" :valErrors="valErrors.driver">
                                <WSelect required="true" v-model="assign.driver" id="driver" :tomSelect="true">
                                    <option value="">search</option>
                                    <option v-if="drivers" v-for="driver in drivers" :value="driver.Telephone_No">{{driver.Telephone_No}}-{{driver.Name}}</option>
                                </WSelect>
                            </field-group>
                        </grid-col>
                    </grid>
                    <grid class="sm:!grid-cols-2">
                        <grid-col>
                            <field-group label="Transport Manager comments" showMandatory="true" :valErrors="valErrors.managerComments">
                                <WTextarea required="true" v-model="assign.comments"></WTextarea>
                            </field-group>
                        </grid-col>
                    </grid>
                    <div class="flex justify-left py-2">
                        <WButton @click="onAssignDetails" class="!bg-blue-500">Assign</WButton>
                    </div>
                </form>
            </div>
            <!---->
            <!---->
            <div v-if="record != null && record.Status != 'UnAssigned' && record.Approval_Status == 'Released'">
                <LogLineList :docNo="form.docNo" :isNew="record != null && record.Status == 'Assigned'?true:false" />
                <div v-if="$root.authUser.IsDriver && record != null && record.Status == 'Assigned'" class="flex justify-left py-2">
                    <WButton @click="onCloseTrip()" class="!bg-green-500">Close Trip</WButton>
                </div>
            </div>
            <hr class="py-2" />
            <!---->
            <ApproversList :records="approvers" v-if="record.Approval_Status != undefined && record.Approval_Status != 'Open'"></ApproversList>
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import WButton from '@/re-usables/components/WButton.vue';
    import Actions from '@/modules/ess/transport-request/TransportActions.vue';
    import LogLineList from '@/modules/ess/transport-request/LogLineList.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, ...Apr, WButton, LogLineList, TomSelectDims },
         setup() {
             const { router, loadTomSelects, setDropdownDims,cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
             return { router, loadTomSelects, setDropdownDims,cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
         },
        data() {
            return {
                pageProps: {
                    title: 'Transport Request Form',
                    pageType: "form",
                    controller: 'TransportRequest',
                    formRoute: "/ess/transport-request/form",
                    listRoute: "/ess/transport-request/list",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    startDateTime: '',
                    endDateTime: '',
                    purpose: '',
                    destination: "", 
                    classOfVehicle: "",
                    estimatedDistance: 0,
                    estimatedCost: 0,
                    dimensionSet: {}
                },
                assign: {
                    docNo: "",
                    driver: "",
                    vehicle: "",
                    comments:"",
                },
                valErrors: [],
                dims: [],
                dimNos: [],
                approvers: [],
                vehicleClasses: [], 
                drivers: [], 
                vehicles: [], 
                record: {},
                approvalEntry: {}, 
                formMode: 'view',
                counter: 0
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.params.no;
            this.assign.docNo = this.$route.params.no;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        beforeRouteLeave(to, from, next) {
            this.cpl_FnAutoSaveFormData(to, from, next, this.record.Approval_Status);
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/getformdata?myAction='+this.form.myAction+"&docNo="+this.form.docNo, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        } else {
                            this.vehicleClasses = data.response.vehicleClasses;
                            this.form.balance = data.response.customerBalance;
                            this.setDropdownDims(data.response);
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.No;
                                this.form.startDateTime = formData.Request_Start_Date.split('T')[0] + " " + formData.Request_Start_Time;
                                this.form.endDateTime = formData.Request_End_Date.split('T')[0] + " " + formData.Request_End_Time;
                                this.form.purpose = formData.Purpose_of_Trip;
                                this.form.destination = formData.Destination;
                                this.form.classOfVehicle = formData.Vehicle_Class_Requested;
                                this.form.estimatedDistance = formData.Estimated_Distance;
                                this.form.estimatedCost = formData.Estimated_Cost_of_Trip;
                                if (formData.Approval_Status != "Open") {
                                    this.formMode = "view";
                                }
                                if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                                    this.approvers = data.response.approvers;
                                }
                                if (formData.Approval_Status == "Released" && formData.Status == "UnAssigned" && this.$root.authUser.IsTransportManager) {
                                    this.drivers = data.response.drivers;
                                    this.vehicles = data.response.vehicles;
                                    this.assign.docNo = data.response.No;
                                }
                            }
                            this.loadTomSelects("department,donor,project,budgetLine,region,vehicleClass,driver,vehicle");
                            this.cpl_FnSetInitialFormData();
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
            calculateEstimatedCost() {
                if(this.form.classOfVehicle != ""){
                    var vClass = this.vehicleClasses.find(obj => obj.Code == this.form.classOfVehicle);
                    if(vClass != null){
                        this.form.estimatedCost = this.form.estimatedDistance * vClass.Rate_per_KM;
                    }
                }
            },
            onAssignDetails() {
                this.$root.loader.isLoading = true;
                this.assign.docNo = this.$route.params.no;
                const requestOptions = {
                    method: "POST",
                    headers: {'Content-Type':"application/json"},
                    body: JSON.stringify(this.assign),
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + "/AssignVehicle", requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.$root.FnNotification("Submitted successfully.", "bg-green-500", false);
                            this.router.push("/ess/transport-request/list?status=Unassigned");
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
            //
            onCloseTrip() {
                if (!confirm('Are you sure you want close this trip?')) { return; }
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: {'Content-Type':"application/json"},
                    body: JSON.stringify(this.assign),
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + "/Closetrip", requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.$root.FnNotification("Closed successfully.", "bg-green-500", false);
                            this.router.go();
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            },
        }
    }
</script>