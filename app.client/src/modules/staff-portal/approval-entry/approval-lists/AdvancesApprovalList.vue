<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Date Submitted</WTh>
            <WTh v-if="$route.query.docType == 'advance'">TOR</WTh>
            <WTh>Donor</WTh>
            <WTh>Project</WTh>
            <WTh>Requester</WTh>
            <WTh>Mission Summary</WTh>
            <WTh>Total Amount</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="FnGetRecordLink(record)">{{record.Document_No != undefined? record.Document_No:record.No}}</WTd>
            <WTd>{{record.Date_Time_Sent_for_Approval.split("T")[0]}} {{record.Date_Time_Sent_for_Approval.split("T")[1].split(".")[0]}}</WTd>
            <WTd v-if="$route.query.docType == 'advance'"><router-link class="text-blue-500 underline" :to="'/ess/terms-of-reference/form/view/'+(record.DocDetails != null && record.DocDetails != ''? JSON.parse(record.DocDetails).TOR_No:'')" target="_blank">{{record.DocDetails != null && record.DocDetails != ''? JSON.parse(record.DocDetails).TOR_No:''}}</router-link></WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Global_Dimension_1_Code:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Global_Dimension_2_Code:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Requester_Name:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Mission_Summary:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? parseFloat(JSON.parse(record.DocDetails).Total_Amount_Requested).toLocaleString():""}}</WTd>
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
    import Actions from '@/modules/ess/approval-entry/ApprovalActions.vue';
    import Menu from '@/modules/ess/approval-entry/approval-lists/Menu.vue';
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import { EyeIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { Actions, ...W, WRouterLink, EyeIcon, Menu },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Claim/Advance Approvals List',
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
                pendingStatistics:null
            }
        },
        mounted(){
            this.status = new URLSearchParams(window.location.search).get('status');
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
                if(response.pendingStatistics != undefined){
                    localStorage.setItem("pendingStatistics", JSON.stringify(response.pendingStatistics));
                    this.pendingStatistics = response.pendingStatistics;
                }
            },
            FnGetRecordLink(recApprovalEntry) {
                var link = "";
                //staff claim
                if (recApprovalEntry.Table_ID == 52018772 && recApprovalEntry.Document_Type == "Order") {
                    link = "/ess/approval-entry/"+this.status+"/staff-claim/view/" + recApprovalEntry.Document_No + "/" + recApprovalEntry.Entry_No+"/staff-claim";
                }
                //advance request
                if (recApprovalEntry.Table_ID == 52018772 && recApprovalEntry.Document_Type == "Quote") {
                    link = "/ess/approval-entry/"+this.status+"/advance-request/view/" + recApprovalEntry.Document_No + "/" + recApprovalEntry.Entry_No+"/advance";
                }
                //advance surrender
                if (recApprovalEntry.Table_ID == 52018772 && recApprovalEntry.Document_Type == "Invoice") {
                    link = "/ess/approval-entry/" + this.status +"/advance-surrender/view/" + recApprovalEntry.Document_No + "/" + recApprovalEntry.Entry_No+"/advance-surrender";
                }
                return link;
            }
        },
    }
</script>