<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Leave App. No.</WTh>
            <WTh>Document Date</WTh>
            <WTh>Staff No.</WTh>
            <WTh>Staff Name</WTh>
            <WTh>Leave Type</WTh>
            <WTh>Days Applied</WTh>
            <WTh>Start Date</WTh>
            <WTh>End Date</WTh>
            <WTh>Return to Work Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="FnGetRecordLink(record)">{{record.Document_No != undefined? record.Document_No:record.No}}</WTd>
            <WTd>{{record.Date_Time_Sent_for_Approval.split("T")[0]}} {{record.Date_Time_Sent_for_Approval.split("T")[1].split(".")[0]}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Employee_No","")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Empoyee_Name","")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Leave_Type","")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Days_Applied","")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Start_Date","date")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"End_Date","date")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Return_to_Work_Date","date")}}</WTd>
            <WTd>
                <Actions :docLink="FnGetRecordLink(record)" :record="record"></Actions>
            </WTd>
        </template>
        <template #navigationTabs>
            <Menu :pendingStats="pendingStatistics"></Menu>
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
                records: [],
                entryDocDetails:[],
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
                status: "",
                routeDocType: "leave-application",
                approvalDocType: "LeaveApplication",
                pendingStatistics:null
            }
        },
        mounted(){
            this.status = new URLSearchParams(window.location.search).get('status');
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.entryDocDetails = response.entryDocDetails;
                this.$root.title = this.pageProps.title;
                if (response.pendingStatistics != undefined) {
                    localStorage.setItem("pendingStatistics", JSON.stringify(response.pendingStatistics));
                    this.pendingStatistics = response.pendingStatistics;
                }
            },
            FnGetRecordLink(recApprovalEntry) {
                var link = "";
                link = "/ess/approval-entry/" + this.status + "/" + this.routeDocType + "/view/" + this.approvalDocType + '?recId=' + recApprovalEntry.Document_No + "&entryNo=" + recApprovalEntry.Entry_No+'&isApproval='+true;
                return link;
            },
            FnGetEntryDocDetails(record, field,type) {
                var value = "";
                if (this.entryDocDetails && this.entryDocDetails[record.Document_No + "_" + record.Entry_No] != undefined) {
                    var obj = JSON.parse(this.entryDocDetails[record.Document_No + "_" + record.Entry_No]);
                    if (type == "") {
                        value = obj[field];
                    } else {
                        if (type == "date") {
                            value = this.$root.xFnNavDateObjToString(obj[field])
                        }
                    }
                }
                return value;
            }
        },
    }
</script>