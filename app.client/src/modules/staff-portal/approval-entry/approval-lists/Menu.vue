<template>
    <HMContainer v-if="this.$route.fullPath.includes('status=Open')">
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=LeaveApplication" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.leavesPendingMyApproval:0">Leave</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=PettyCash" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.pettyCashPendingMyApproval:0">Petty Cash</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=StaffClaim" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.claimsPendingMyApproval:0">Claims</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=ImprestRequest" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.imprestsPendingMyApproval:0">Imprests</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=ImprestSurrender" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.imprestSurrendersPendingMyApproval:0">Surrenders</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=PurchaseRequest" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.purchaseReqsPendingMyApproval:0">Purchase Reqs</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Open&docType=StoreRequest" :isTotal="true" :total="pendingStatistics != null? pendingStatistics.storeReqsPendingMyApproval:0">Store Reqs</HMItem>
    </HMContainer>
    <HMContainer v-if="this.$route.fullPath.includes('status=Approved')">
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=LeaveApplication">Leave</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=PettyCash">Petty Cash</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=StaffClaim">Claims</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=ImprestRequest">Imprests</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=ImprestSurrender">Surrenders</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=PurchaseRequest">Purchase Reqs</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Approved&docType=StoreRequest">Store Reqs</HMItem>
    </HMContainer>
    <HMContainer v-if="this.$route.fullPath.includes('status=Rejected')">
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=LeaveApplication">Leave</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=PettyCash">Petty Cash</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=StaffClaim">Claims</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=ImprestRequest">Imprests</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=ImprestSurrender">Surrenders</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=PurchaseRequest">Purchase Reqs</HMItem>
        <HMItem linkTo="/ess/approval-entry/list?status=Rejected&docType=StoreRequest">Store Reqs</HMItem>
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