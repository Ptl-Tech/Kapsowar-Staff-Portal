<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' Store Request'">
            <ApprovalForm v-if="$route.params.entryNo != undefined" @onFetchApprovalEntry="approvalEntry = $event" />
            <grid>
                <grid-col v-if="form.docNo != 'new'">
                    <field-group label="No.">
                        <WInput type="text" required="false" :value="form.docNo" formMode="view" />
                    </field-group>
                </grid-col>
                <grid-col v-if="dimNos[0] != undefined" v-for="dimNo in dimNos">
                    <field-group :label="$root.DimCaption(dimNo)" showMandatory="true" :valErrors="valErrors.dimensionSet">
                        <TomSelectDims :tsId="'dimNo'+dimNo" :dimNo="dimNo" :record="record" :loadAll="record != null && record.Status != 'Open'?false:true" v-model="form.dimensionSet['Dim_'+dimNo+'_Value']" :formMode="formMode"></TomSelectDims>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Location Code" showMandatory="true" :valErrors="valErrors.location">
                        <WSelect required="true" v-model="form.location" id="location" isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="locations" v-for="location in locations" :value="location.Code">{{location.Code}} - {{location.Name}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Consignee Name" showMandatory="true" :valErrors="valErrors.consigneeName">
                        <WInput type="text" v-model="form.consigneeName" :formMode="formMode"></WInput>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Transport Type" showMandatory="true" :valErrors="valErrors.transportType">
                        <WSelect required="true" v-model="form.transportType" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="typesOfTransport" v-for="option in typesOfTransport" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Transporter Name & Phone (optional)" :showMandatory="false" :valErrors="valErrors.transporterNamePhone">
                        <WInput type="text" v-model="form.transporterNamePhone" :formMode="formMode"></WInput>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Vehicle No.(optional)" :showMandatory="false" :valErrors="valErrors.vehicleNo">
                        <WInput type="text" v-model="form.vehicleNo" :formMode="formMode"></WInput>
                    </field-group>
                </grid-col>
                <field-group label="Means Of Transport (optional)" :showMandatory="false" :valErrors="valErrors.meansOfTransport">
                    <WSelect required="true" v-model="form.meansOfTransport" :formMode="formMode">
                        <option value="">--select--</option>
                        <option v-if="meansOfTransport" v-for="option in meansOfTransport" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
                    </WSelect>
                </field-group>
            </grid>
            <!---->
            <div v-if="record.Status != undefined">
                <grid>
                    <grid-col>
                        <field-group label="Approval Status">
                            <WInput type="text" required="false" v-model="record.Status" :formMode="'view'"></WInput>
                        </field-group>
                    </grid-col>
                </grid>
            </div>
            <!---->
            <hr class="py-2" />
            <StoreLineList v-if="form.docNo != 'new'" :docNo="form.docNo" />
            <!---->
            <hr class="py-2" />
            <AttachmentList v-if="form.docNo != 'new'" :docNo="form.docNo" tableID="39005759" type="Store Request" :editable="record.Status != undefined && record.Status == 'Open'? true:false" />
            <!---->
            <ApproversList :records="approvers" v-if="record.Status != undefined && record.Status != 'Open'"></ApproversList>
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
            <!---->
            <ApprovalActions v-if="approvalEntry != null && approvalEntry.Status != undefined && approvalEntry.Status == 'Open'" :record="approvalEntry" />
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/ess/store-request/StoreRequestHeaderActions.vue';
    import StoreLineList from '@/modules/ess/store-request/StoreRequestLineList.vue';
    import AttachmentList from '@/modules/ess/attachment/AttachmentList.vue';
    import { Apr } from '@/re-usables/imports/ApprovalComponents.js';
    import TomSelectDims from '@/re-usables/components/TomSelectDims.vue';
    export default {
        components: { Actions, ...W, ...Apr, StoreLineList, AttachmentList, TomSelectDims },
        setup() {
            const { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData } = useFormComposable();
            return { router, loadTomSelects, setDropdownDims, cpl_FnAutoSaveFormData, cpl_FnSetInitialFormData };
        },
        data() {
            return {
                pageProps: {
                    title: 'Store Request Form',
                    pageType: "form",
                    controller: 'StoreRequestHeader',
                    formRoute: "/ess/store-request/form",
                    listRoute: "/ess/store-request/list",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    dimensionSet: {},
                    location: '',
                    consigneeName: '',
                    transportType: 0,
                    transporterNamePhone: '',
                    vehicleNo: '',
                    meansOfTransport: 0,
                },
                record: {},
                valErrors: [],
                donors: [],
                projects: [],
                budgetLines: [],
                counties: [],
                regions: [],
                departments: [],
                locations: [],
                approvers: [],
                dimNos: [],
                approvalEntry: {}, 
                formMode: 'view',
                typesOfTransport: [{ id: "1", name: "KRCS" }, { id: "2", name: "Commercial" }],
                meansOfTransport: [{ id: "0", name: "Road" }, { id: "1", name: "Rail" }, { id: "2", name: "Air" }, { id: "3", name: "Sea" }],
                counter: 0
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.params.no;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        beforeRouteLeave(to, from, next) {
            this.cpl_FnAutoSaveFormData(to, from, next, this.record.Status);
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
                            this.setDropdownDims(data.response);
                            this.locations = data.response.locations;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.docNo = formData.No;
                                this.form.donor = formData.Shortcut_Dimension_1_Code;
                                this.form.project = formData.Shortcut_Dimension_2_Code;
                                this.form.county = formData.Shortcut_Dimension_7_Code;
                                this.form.budgetLine = formData.Shortcut_Dimension_3_Code;
                                this.form.region = formData.Shortcut_Dimension_4_Code;
                                this.form.department = formData.Shortcut_Dimension_6_Code;
                                this.form.location = formData.Location_Code;
                                this.form.consigneeName = formData.Consignee_Name;
                                this.form.transportType = formData.Type_of_transport != " "? this.typesOfTransport.find(obj => obj.name == formData.Type_of_transport).id:"";
                                this.form.transporterNamePhone = formData.Transporter_Name_and_Phone;
                                this.form.vehicleNo = formData.Vehicle_No;
                                this.form.meansOfTransport = formData.Means_Of_Transport != ""? this.meansOfTransport.find(obj => obj.name == formData.Means_Of_Transport).id:"";
                                if(data.response.approvers != undefined && data.response.approvers != 'undefined') {
                                    this.approvers = data.response.approvers;
                                }
                                if (this.record.Status != "Open") {
                                    this.formMode = "view";
                                }
                            }
                            this.loadTomSelects("location");
                            this.cpl_FnSetInitialFormData();
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