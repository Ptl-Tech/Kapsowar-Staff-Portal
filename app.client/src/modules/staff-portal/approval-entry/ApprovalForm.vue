<template>
    <div v-if="record != null && (record.Entry_No != undefined || record.No != undefined)">
        <h4 class="font-semibold">a) Approval Details</h4>
        <grid class="sm:!grid-cols-2">
            <grid-col v-if="record.Details != undefined">
                <field-group label="Document Description" :showMandatory="false">
                    <WInput type="text" required="false" v-model="record.Details" :formMode="'view'" />
                </field-group>
            </grid-col>
            <grid-col v-if="record.Sender_ID != undefined">
                <field-group label="Document Owner" :showMandatory="false">
                    <WInput type="text" required="false" v-model="record.Sender_ID" :formMode="'view'" />
                </field-group>
            </grid-col>
            <grid-col v-if="record.Employee_ID != undefined">
                <field-group label="Document Owner" :showMandatory="false">
                    <WInput type="text" required="false" v-model="record.Employee_ID" :formMode="'view'" />
                </field-group>
            </grid-col>
            <grid-col>
                <field-group label="Date & Time Sent for Approval" :showMandatory="false">
                    <WInput type="text" required="false" v-model="record.Date_Time_Sent_for_Approval.split('.')[0]" :formMode="'view'" />
                </field-group>
            </grid-col>
            <grid-col v-if="record.Due_Date != undefined">
                <field-group label="Due date" :showMandatory="false">
                    <WInput type="text" required="false" :value="$root.xFnNavDateObjToString(record.Due_Date)" :formMode="'view'" />
                </field-group>
            </grid-col>
        </grid>
        <h4 class="font-semibold">b) Document Details</h4>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import Actions from '@/modules/staff-portal/approval-entry/ApprovalActions.vue';
    import { useRouter } from 'vue-router'
    export default {
        emits: ["onFetchApprovalEntry"],
        components: { Actions, ...W},
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                pageProps: {
                    title: 'Approval Details',
                    pageType: "form",
                    controller: 'ApprovalManagement',
                    formRoute: "",
                    listRoute: "",
                },
                form: {
                    docNo: '',
                    myAction: '',
                    entryNo: null,
                    docType:null,
                },
                record: {},
                formMode: 'view',
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.query.recId;
            this.form.entryNo = this.$route.query.entryNo;
            this.form.docType = this.$route.params.docType;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata?myAction=' + this.form.myAction + "&entryNo=" + this.form.entryNo + "&docNo=" + this.form.docNo + "&docType=" + this.form.docType+'&isApproval='+true, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = this.appConfig.errors.dataFetchFailure;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.record = data.response.formData;
                            this.$emit("onFetchApprovalEntry", this.record);
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
        }
    }
</script>