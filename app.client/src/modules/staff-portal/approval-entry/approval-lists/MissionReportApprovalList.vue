<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Date Submitted</WTh>
            <WTh>TOR No.</WTh>
            <WTh>Requester</WTh>
            <WTh>Start Date</WTh>
            <WTh>End Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="FnGetRecordLink(record)">{{record.Document_No != undefined? record.Document_No:record.No}}</WTd>
            <WTd>{{record.Date_Time_Sent_for_Approval.split("T")[0]}} {{record.Date_Time_Sent_for_Approval.split("T")[1].split(".")[0]}}</WTd>
            <WTd><router-link class="text-blue-500 underline" :to="'/ess/terms-of-reference/form/view/'+(record.DocDetails != null && record.DocDetails != ''? JSON.parse(record.DocDetails).TOR_No:'')" target="_blank">{{record.DocDetails != null && record.DocDetails != ''? JSON.parse(record.DocDetails).TOR_No:''}}</router-link></WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Employee_Name:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Start_Date.split("T")[0]:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).End_Date.split("T")[0]:""}}</WTd>
            <WTd>
                <Actions :docLink="FnGetRecordLink(record)" :record="record"></Actions>
            </WTd>
        </template>
        <template #navigationTabs>
            <Menu></Menu>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/approval-entry/ApprovalActions.vue';
    import Menu from '@/modules/ess/approval-entry/approval-lists/Menu.vue';
    export default {
        components: { Actions, ...W, Menu },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Mission Report Approvals List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'ApprovalManagement',
                    formRoute: "/ess/approval-entry/form",
                    listRoute: "/ess/approval-entry/list",
                    isLinkTabs: true,
                    isRowDoubleClick:false
                },
                actionsProps: { isNew: false, isNewCaption: "", isEdit: false, isDelete: false, isFilter: true, isExport: true},
                status:"",
            }
        },
        mounted(){
            this.status = new URLSearchParams(window.location.search).get('status');
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
            },
            FnGetRecordLink(recApprovalEntry) {
                var link = "";
                if (recApprovalEntry.Table_ID == 52018814 && recApprovalEntry.Details.includes("Mission")) {
                    link = "/ess/approval-entry/"+this.status+"/mission-report/view/" + recApprovalEntry.Document_No + "/" + recApprovalEntry.Entry_No + "/Mission-Report";
                }
                return link;
            }
        },
    }
</script>