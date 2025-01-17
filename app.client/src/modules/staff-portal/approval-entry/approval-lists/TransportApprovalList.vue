<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Date Submitted</WTh>
            <WTh>Donor</WTh>
            <WTh>Project</WTh>
            <WTh>Requester</WTh>
            <WTh>Destination</WTh>
            <WTh>Estimated Cost</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="FnGetRecordLink(record)">{{record.Document_No != undefined? record.Document_No:record.No}}</WTd>
            <WTd>{{record.Date_Time_Sent_for_Approval.split("T")[0]}} {{record.Date_Time_Sent_for_Approval.split("T")[1].split(".")[0]}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Global_Dimension_1_Code:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Global_Dimension_2_Code:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Requester:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? JSON.parse(record.DocDetails).Destination:""}}</WTd>
            <WTd>{{record.DocDetails != null && record.DocDetails != ""? parseFloat(JSON.parse(record.DocDetails).Estimated_Cost_of_Trip).toLocaleString():""}}</WTd>
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
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import { EyeIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { Actions, ...W, WRouterLink, EyeIcon, Menu },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Transport Approvals List',
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
                //transport
                if(recApprovalEntry.Table_ID == 52018777) {
                    link = "/ess/approval-entry/" + this.status + "/transport/view/" + recApprovalEntry.Document_No + "/" + recApprovalEntry.Entry_No +"/transport";
                }
                return link;
            }
        },
    }
</script>