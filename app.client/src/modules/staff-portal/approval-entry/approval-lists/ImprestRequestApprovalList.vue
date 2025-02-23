<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Imprest No.</WTh>
            <WTh>Document Date</WTh>
            <WTh>Staff No</WTh>
            <WTh>Total Net Amount</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="FnGetRecordLink(record)">{{record.Document_No != undefined? record.Document_No:record.No}}</WTd>
            <WTd>{{record.Date_Time_Sent_for_Approval.split("T")[0]}} {{record.Date_Time_Sent_for_Approval.split("T")[1].split(".")[0]}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Employee_No","")}}</WTd>
            <WTd>{{FnGetEntryDocDetails(record,"Total_Net_Amount","")}}</WTd>
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
                records: [],
                entryDocDetails:[],
                pageProps: {
                    title: 'Imprests Approvals List',
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
                routeDocType: "imprest-request",
                approvalDocType:"ImprestRequest",
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