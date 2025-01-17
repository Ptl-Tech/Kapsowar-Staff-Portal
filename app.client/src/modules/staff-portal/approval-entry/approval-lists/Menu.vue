<template>
    <HMContainer v-if="this.$route.fullPath.includes('status=Open')">
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=staff-claim" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalClaim:0"><span class="hidden sm:flex">Staff </span>Claim </HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=advance" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalAdvance:0">Advances</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=advance-surrender" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalSurrender:0">Surrender</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=transport" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalTransport:0">Transport</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=timesheet" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalTimesheet:0">Timesheet</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=leave" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalLeave:0">Leave</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=purchase-request" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalPurchReq:0">Purchase</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=store-request" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalStoreReq:0">Store</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=TOR" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalTOR:0">TOR</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=Mission-Report" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalMissionReport:0">Mission Report</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=memo" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalMemo:0">Memo</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=staff-request" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalStaffReq:0">Staff Req</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=access-request" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalAccessReq:0">Access Req</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=ME-projects-report" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalMEProject:0">M&E Projects</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=ME-branch-report" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalMEBranch:0">M&E Branch</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=ME-annual-report" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pendingMyApprovalMEAnn:0">M&E Annual</HMItem>
    </HMContainer>
    <HMContainer v-if="this.$route.fullPath.includes('status=Approved')">
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=staff-claim"><span class="hidden sm:flex">Staff </span>Claim</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=advance">Advances</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=advance-surrender">Surrender</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=transport">Transport</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=timesheet">Timesheet</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=leave">Leave</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=purchase-request">Purchase</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=store-request">Store</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=TOR">TOR</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=Mission-Report">Mission Report</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=memo">Memo</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=staff-request">Staff Req</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=access-request">Access Req</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=ME-projects-report">M&E Projects</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=ME-branch-report">M&E Branch</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=ME-annual-report">M&E Annual</HMItem>
    </HMContainer>
    <HMContainer v-if="this.$route.fullPath.includes('status=Rejected')">
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=staff-claim"><span class="hidden sm:flex">Staff </span>Claim</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=advance">Advances</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=advance-surrender">Surrender</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=transport">Transport</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=timesheet">Timesheet</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=leave">Leave</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=purchase-request">Purchase</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=store-request">Store</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=TOR">TOR</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=Mission-Report">Mission Report</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=memo">Memo</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=staff-request">Staff Req</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=access-request">Access Req</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=ME-projects-report">M&E Projects</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=ME-branch-report">M&E Branch</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=ME-annual-report">M&E Annual</HMItem>
    </HMContainer>
</template>
<script>
    import HMContainer from '@/re-usables/components/HorizontalMenu/HMContainer.vue'
    import HMItem from '@/re-usables/components/HorizontalMenu/HMItem.vue'
    export default {
        components: { HMContainer, HMItem },
        props:{pendingStats:{default:null}},
        data(){
            return{
                pendingStatistics:{}
            }
        },
        mounted() {
            if(this.$route.fullPath.includes('status=Open')){
                var stats = localStorage.getItem("pendingStatistics");
                if (stats != null) {
                    this.pendingStatistics = JSON.parse(stats);
                }
            }
        },
        watch:{
            pendingStats:function(){
                this.pendingStatistics = this.pendingStats;
            }
        }
    }
</script>