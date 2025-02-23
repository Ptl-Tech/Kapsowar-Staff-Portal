<template>
    <ImprestsApprovalList v-if="$route.query.docType == 'ImprestRequest'"></ImprestsApprovalList>
    <ImprestSurrendersApprovalList v-if="$route.query.docType == 'ImprestSurrender'"></ImprestSurrendersApprovalList>
    <PettyCashApprovalList v-if="$route.query.docType == 'PettyCash'"></PettyCashApprovalList>
    <StaffClaimApprovalList v-if="$route.query.docType == 'StaffClaim'"></StaffClaimApprovalList>
    <LeaveApprovalList v-else-if="$route.query.docType == 'LeaveApplication'"></LeaveApprovalList>
    <PurchaseRequestApprovalList v-else-if="$route.query.docType == 'PurchaseRequest'"></PurchaseRequestApprovalList>
    <StoreRequestApprovalList v-else-if="$route.query.docType == 'StoreRequest'"></StoreRequestApprovalList>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/approval-entry/ApprovalActions.vue';
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import { EyeIcon } from '@heroicons/vue/24/outline'
    const ImprestsApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/ImprestRequestApprovalList.vue'));
    const ImprestSurrendersApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/ImprestSurrenderApprovalList.vue'));
    const PettyCashApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/PettyCashApprovalList.vue'));
    const LeaveApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/LeaveApprovalList.vue'));
    const StaffClaimApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/StaffClaimApprovalList.vue'));
    const PurchaseRequestApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/PurchaseRequestApprovalList.vue'));
    const StoreRequestApprovalList = defineAsyncComponent(() => import('@/modules/staff-portal/approval-entry/approval-lists/StoreRequestApprovalList.vue'));
    export default {
        components: {
            Actions, ...W, WRouterLink, EyeIcon, ImprestsApprovalList, ImprestSurrendersApprovalList, PettyCashApprovalList,
            StaffClaimApprovalList, LeaveApprovalList, PurchaseRequestApprovalList, StoreRequestApprovalList
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