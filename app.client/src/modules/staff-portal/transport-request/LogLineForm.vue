<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' Vehicle Log'" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Destination" showMandatory="true" :valErrors="valErrors.destination">
                        <WInput type="text" required="true" v-model="form.destination" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-2">
                <grid-col>
                    <field-group label="Start Date & Time" showMandatory="true" :valErrors="valErrors.startDateTime">
                        <WInput type="datetime-local" required="true" v-model="form.startDateTime" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Start Odometer Reading" showMandatory="true" :valErrors="valErrors.startOdometerReading">
                        <WInput type="number" required="true" v-model="form.startOdometerReading" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-2">
                <grid-col>
                    <field-group label="End Date & Time" showMandatory="true" :valErrors="valErrors.endDateTime">
                        <WInput type="datetime-local" required="true" v-model="form.endDateTime" :formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="End Odometer Reading" showMandatory="true" :valErrors="valErrors.endOdometerReading">
                        <WInput type="number" required="true" v-model="form.endOdometerReading" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Notes" showMandatory="true" :valErrors="valErrors.notes">
                        <WTextarea required="true" v-model="form.notes" :formMode="formMode"></WTextarea>
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
    import Actions from '@/modules/ess/transport-request/LogLineActions.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { Actions, ...W,ModalPageTemplate},
        emits:["closeModal"],
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                pageProps: {
                    title: 'Vehicle Log Line',
                    pageType: "form",
                    controller: 'VehicleLogLine',
                    formRoute: "/ess/transport-request/log/form",
                    pKey: 'No',
                    pKeyHeader: 'Transport_Request_No',
                },
                form: {
                    docNo: '',
                    lineNo: 0,
                    myAction: '',
                    destination: '',
                    startDateTime: '',
                    startOdometerReading: 0,
                    endDateTime: '',
                    endOdometerReading:0,
                    notes: '',
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.params.no;
            this.form.lineNo = this.$route.params.lineNo;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata/' + this.form.myAction + "/" + this.form.docNo + "/" + this.form.lineNo, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.header = data.response.header;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.Transport_Request_No;
                                this.form.lineNo = formData.No;
                                this.form.destination = formData.Destination_Route;
                                this.form.startDateTime = formData.Starting_Date.split('T')[0] + " " + formData.Start_Time;
                                this.form.startOdometerReading = formData.Start_Time_Odometer_Reading;
                                this.form.endDateTime = formData.Ending_Date.split('T')[0] + " " + formData.End_Time;
                                this.form.endOdometerReading = formData.End_Time_Odometer_Reading;
                                this.form.notes = formData.Notes;
                                if (!(this.header.Approval_Status == "Released" && this.header.Status == "Assigned" && this.$root.authUser.IsDriver)){
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
            onItemChange(){
                var item = this.advanceTypes.find(x => x.Code == this.form.item);
                this.form.description = item.Description;
                this.form.unitOfMeasure = item.Unit_of_Measure;
                this.form.unitCost = item.Unit_Cost;
            },
        }
    }
</script>