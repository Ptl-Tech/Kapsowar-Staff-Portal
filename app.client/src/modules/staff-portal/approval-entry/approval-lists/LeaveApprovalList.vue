<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Leave App. No.</WTh>
            <WTh>Date Submitted</WTh>
            <WTh>Leave Type</WTh>
            <WTh>Days Applied</WTh>
            <WTh>Start Date</WTh>
            <WTh>End Date</WTh>
            <WTh>Return to Work Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="FnGetRecordLink(record)">{{record.Document_No != undefined? record.Document_No:record.No}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.DocDetails.Date_Time_Sent_for_Approval)}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? record.DocDetails.Leave_Type:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? record.DocDetails.Days_Applied:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? $root.xFnNavDateObjToString(record.DocDetails.Start_Date):""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? $root.xFnNavDateObjToString(record.DocDetails.End_Date):""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? $root.xFnNavDateObjToString(record.DocDetails.Return_to_Work_Date):""}}</WTd>
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
    import Actions from '@/modules/staff-portal/approval-entry/ApprovalActions.vue';
    import Menu from '@/modules/staff-portal/approval-entry/approval-lists/Menu.vue';
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import { EyeIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { Actions, ...W, WRouterLink, EyeIcon, Menu },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Leave Approvals List',
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
                if (response.pendingStatistics != undefined) {
                    localStorage.setItem("pendingStatistics",response.pendingStatistics);
                    this.pendingStatistics = response.pendingStatistics;
                }
            },
            FnGetRecordLink(recApprovalEntry) {
                var link = "";
                link = "/ess/approval-entry/"+this.status+"/leave/view/" + recApprovalEntry.No + "/" + recApprovalEntry.Employee_ID+"/LeaveApplication";
                return link;
            }
        },
    }
</script>