<template>
    <AdvancesApprovalList v-if="$route.query.docType == 'staff-claim' || $route.query.docType == 'advance' || $route.query.docType == 'advance-surrender'"></AdvancesApprovalList>
    <TransportApprovalList v-else-if="$route.query.docType == 'transport'"></TransportApprovalList>
    <TimesheetApprovalList v-else-if="$route.query.docType == 'timesheet'"></TimesheetApprovalList>
    <LeaveApprovalList v-else-if="$route.query.docType == 'leave'"></LeaveApprovalList>
    <PurchaseRequestApprovalList v-else-if="$route.query.docType == 'purchase-request'"></PurchaseRequestApprovalList>
    <StoreRequestApprovalList v-else-if="$route.query.docType == 'store-request'"></StoreRequestApprovalList>
    <TORApprovalList v-else-if="$route.query.docType == 'TOR'"></TORApprovalList>
    <MissionReportApprovalList v-else-if="$route.query.docType == 'Mission-Report'"></MissionReportApprovalList>
    <MemoApprovalList v-else-if="$route.query.docType == 'memo'"></MemoApprovalList>
    <StaffRequestApprovalList v-else-if="$route.query.docType == 'staff-request'"></StaffRequestApprovalList>
    <AccessRequestApprovalList v-else-if="$route.query.docType == 'access-request'"></AccessRequestApprovalList>
    <MEProjectsApprovalList v-else-if="$route.query.docType == 'ME-projects-report'"></MEProjectsApprovalList>
    <MEBranchApprovalList v-else-if="$route.query.docType == 'ME-branch-report'"></MEBranchApprovalList>
    <MEAnnualApprovalList v-else-if="$route.query.docType == 'ME-annual-report'"></MEAnnualApprovalList>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/approval-entry/ApprovalActions.vue';
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import { EyeIcon } from '@heroicons/vue/24/outline'
    const AdvancesApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/AdvancesApprovalList.vue'));
    const TransportApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/TransportApprovalList.vue'));
    const TimesheetApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/TimesheetApprovalList.vue'));
    const LeaveApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/LeaveApprovalList.vue'));
    const PurchaseRequestApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/PurchaseRequestApprovalList.vue'));
    const StoreRequestApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/StoreRequestApprovalList.vue'));
    const TORApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/TORApprovalList.vue'));
    const MissionReportApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/MissionReportApprovalList.vue'));
    const MemoApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/MemoApprovalList.vue'));
    const StaffRequestApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/StaffRequestApprovalList.vue'));
    const MEProjectsApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/MEProjectsApprovalList.vue'));
    const MEBranchApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/MEBranchApprovalList.vue'));
    const MEAnnualApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/MEAnnualApprovalList.vue'));
    const AccessRequestApprovalList = defineAsyncComponent(() => import('@/modules/ess/approval-entry/approval-lists/AccessRequestApprovalList.vue'));
    export default {
        components: {
            Actions, ...W, WRouterLink, MissionReportApprovalList, EyeIcon, MEAnnualApprovalList, MEBranchApprovalList, MEProjectsApprovalList, StaffRequestApprovalList,
            MemoApprovalList, AdvancesApprovalList, TransportApprovalList, TimesheetApprovalList, LeaveApprovalList, PurchaseRequestApprovalList, StoreRequestApprovalList,
            TORApprovalList, AccessRequestApprovalList
        },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Approvals List',
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
        }
    }
</script>