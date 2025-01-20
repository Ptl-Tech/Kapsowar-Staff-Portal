import { defineAsyncComponent, markRaw } from "vue";
export function useListPageComposable() {
    //used to prefetch components that are most likely to be loaded next. This loads and cache the component which makes it load fast when accessed. 
    function prefetchComponent() {
        var routeName = this.$route.name;
        if (routeName == 'ApprovalList') {
            routeName = this.$route.query.docType + "Approval";
        }
        //setTimeout(function () {
        //    if (routeName != undefined && routeName != "") {
        //        var prefetch;
        //        switch (routeName) {
        //            case 'LeaveList':
        //            case 'leaveApproval':
        //                //var comp = "@/modules/ess/leave-application/LeaveForm.vue";
        //                //prefetch = () => { defineAsyncComponent(() => import(`${comp}`)) }
        //                prefetch = () => { import("@/modules/ess/leave-application/LeaveForm.vue") };
        //                break;
        //            case 'LeaveRecallList':
        //            case 'leave-recallApproval':
        //                prefetch = () => { import('@/modules/ess/leave-recall/RecallForm.vue'); };
        //                break;
        //            case 'StaffClaimList':
        //            case 'staff-claimApproval':
        //                prefetch = () => { import('@/modules/ess/staff-claim/ClaimForm.vue'); };
        //                break;
        //            case 'StaffAdvanceList':
        //            case 'staff-advanceApproval':
        //                prefetch = () => { import('@/modules/staff-portal/imprest/ImprestForm.vue'); };
        //                break;
        //            case 'SurrenderList':
        //            case 'advance-surrenderApproval':
        //                prefetch = () => { import('@/modules/staff-portal/imprest-surrender/SurrenderForm.vue'); };
        //                break;
        //            case 'TransportReqList':
        //            case 'transportApproval':
        //                prefetch = () => { import('@/modules/ess/transport-request/TransportForm.vue'); };
        //                break;
        //            case 'TimesheetList':
        //            case 'timesheetApproval':
        //                prefetch = () => { import('@/modules/ess/timesheet/TimesheetForm.vue'); };
        //                break;
        //            case 'PurchaseReqList':
        //            case 'purchase-requestList':
        //                prefetch = () => { import('@/modules/ess/purchase-request/PurchaseRequestForm.vue'); };
        //                break;
        //            case 'StoreReqList':
        //            case 'store-requestList':
        //                prefetch = () => { import('@/modules/ess/store-request/StoreRequestHeaderForm.vue'); };
        //                break;
        //            case 'MEBranchReList': 
        //            case 'ME-branch-reportApproval':
        //                prefetch = () => { import('@/modules/ess/monitoring-evaluation/branch-reporting/ME_BranchReportingHeaderForm.vue'); };
        //                break;
        //            case 'MEProjectsReList':
        //            case 'ME-projects-reportApproval':
        //                prefetch = () => { import('@/modules/ess/monitoring-evaluation/projects-reporting/ME_ProjectsReportingHeaderForm.vue'); };
        //                break;
        //            case 'MEAnnualReList':
        //            case 'ME-annual-reportApproval':
        //                prefetch = () => { import('@/modules/ess/monitoring-evaluation/annual-reporting/ME_AnnualReportingHeaderForm.vue'); };
        //                break;
        //            case 'MemoList':
        //            case 'memoApproval':
        //                prefetch = () => { import('@/modules/ess/memo/MemoHeaderForm.vue'); };
        //                break;
        //            case 'TORList':
        //            case 'TORApproval':
        //                prefetch = () => { import('@/modules/ess/terms-of-reference/TORHeaderForm.vue'); };
        //                break;
        //            case 'StaffReqList':
        //            case 'staff-requestApproval':
        //                prefetch = () => { import('@/modules/ess/staff-request/StaffRequestHeaderForm.vue'); };
        //                break;
        //            case 'MissionReportList':
        //            case 'MissionReportApproval':
        //                prefetch = () => { import('@/modules/ess/mission-report/MissionReportHeaderForm.vue'); };
        //                break;
        //        }
        //        if (prefetch != null) {
        //            prefetch();
        //        }
        //    }
        //}, 400);//miliseconds
    }
    return { prefetchComponent };
}
