/**
 * Patterns
 * Main pages - module/pageType/action/docNo e.g leave-application/form/edit/DC123
 * Line pages - module/line/pageType/action/docNo/lineNo e.g leave-application/line/form/edit/DC123/123
 */

/**
* Supplier Portal
* */
export const supplierPortalRoutes = [
    {
        path: '/ess',
        meta: { requiresAuth: true },
        children: [
            { path: 'dashboard', name: "dashboard", component: () => import('@/modules/staff-portal/dashboards/DefaultDashboard.vue') },
            //approval-entry
            //{
            //    path: 'approval-entry',
            //    meta: { requiresAuth: true },
            //    children: [
            //        { path: 'list', name: "ApprovalList", component: () => import('@/modules/staff-portal/approval-entry/ApprovalList.vue'), },
            //        { path: ':status/imprest/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/imprest/ImprestHeaderForm.vue') },
            //        { path: ':status/imprest-surrender/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/imprest-surrender/SurrenderHeaderForm.vue') },
            //        { path: ':status/leave/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/leave-application/LeaveHeaderForm.vue') },
            //        { path: ':status/leave-recall/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/leave-recall/RecallForm.vue') },
            //        { path: ':status/staff-claim/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/staff-claim/ClaimHeaderForm.vue') },
            //        { path: ':status/transport/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/transport-request/TransportForm.vue') },
            //        { path: ':status/purchase-request/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/purchase-request/PurchaseRequestForm.vue') },
            //        { path: ':status/store-request/:action/:no/:entryNo/:docType', component: () => import('@/modules/staff-portal/store-request/StoreRequestHeaderForm.vue') },
            //    ]
            //},
            /**
             * 
             */
            //staff-claim
            //{
            //    path: 'staff-claim',
            //    meta: { requiresAuth: true },
            //    children: [
            //        { path: 'list', name: "StaffClaimHeaderList", component: () => import('@/modules/staff-portal/staff-claim/ClaimHeaderList.vue'), },
            //        { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/staff-claim/ClaimHeaderForm.vue'), },
            //        { path: 'line/form/:lineAction/:no/:lineNo', component: () => import('@/modules/staff-portal/staff-claim/ClaimLineForm.vue'), },
            //    ]
            //},
            ////staff-advance
            //{
            //    path: 'imprest',
            //    meta: { requiresAuth: true },
            //    children: [
            //        { path: 'list', name: "ImprestHeaderList", component: () => import('@/modules/staff-portal/imprest/ImprestHeaderList.vue'), },
            //        { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/imprest/ImprestHeaderForm.vue') },
            //        { path: 'line/form/:lineAction/:no/:lineNo', component: () => import('@/modules/staff-portal/imprest/ImprestLineForm.vue'), },
            //    ]
            //},
            ////imprest-surrender
            //{
            //    path: 'imprest-surrender',
            //    meta: { requiresAuth: true },
            //    children: [
            //        { path: 'list', name: "SurrenderHeaderList", component: () => import('@/modules/staff-portal/imprest-surrender/SurrenderHeaderList.vue'), },
            //        { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/imprest-surrender/SurrenderHeaderForm.vue'), },
            //        { path: 'line/form/:lineAction/:no/:lineNo', component: () => import('@/modules/staff-portal/imprest-surrender/SurrenderLineForm.vue'), },
            //    ]
            //},
            /**
             * HR ROUTES
             */

            //leave
            {
                path: 'leave-application',
                meta: { requiresAuth: true },
                children: [
                    { path: 'list', name: "leaveList", component: () => import('@/modules/staff-portal/leave-application/LeaveHeaderList.vue'), },
                    { path: 'form/:action', name: "leaveForm", component: () => import('@/modules/staff-portal/leave-application/LeaveHeaderForm.vue'), },
                ]
            },
            //
        //    {
        //        path: 'leave-recall',
        //        meta: { requiresAuth: true },
        //        children: [
        //            { path: 'list', name: "LeaveRecallList", component: () => import('@/modules/staff-portal/leave-recall/RecallList.vue'), },
        //            { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/leave-recall/RecallForm.vue'), },
        //        ]
        //    },
        //    {
        //        path: 'reports',
        //        meta: { requiresAuth: true },
        //        children: [
        //            { path: 'payslip', component: () => import('@/modules/staff-portal/reports/Payslip.vue'), },
        //            { path: 'p9', component: () => import('@/modules/staff-portal/reports/P9.vue') },
        //        ]
        //    },
        //    /**
        //     * PROCUREMENT & STORES ROUTES
        //     */
        //    {
        //        path: 'purchase-request',
        //        meta: { requiresAuth: true },
        //        children: [
        //            { path: 'list', name: "PurchaseReqList", component: () => import('@/modules/staff-portal/purchase-request/PurchaseRequestList.vue'), },
        //            { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/purchase-request/PurchaseRequestForm.vue') },
        //            { path: 'line/form/:action/:no/:lineNo', component: () => import('@/modules/staff-portal/purchase-request/PurchaseRequestLineForm.vue') },
        //        ]
        //    },
        //    {
        //        path: 'store-request',
        //        meta: { requiresAuth: true },
        //        children: [
        //            { path: 'list', name: "StoreReqList", component: () => import('@/modules/staff-portal/store-request/StoreRequestHeaderList.vue'), },
        //            { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/store-request/StoreRequestHeaderForm.vue') },
        //            { path: 'line/form/:action/:no/:lineNo', component: () => import('@/modules/staff-portal/store-request/StoreRequestLineForm.vue') },
        //        ]
        //    },
        //    /**
        //     * TRANSPORT ROUTES
        //     */
        //    {
        //        path: 'transport-request',
        //        meta: { requiresAuth: true },
        //        children: [
        //            { path: 'list', name: "TransportReqList", component: () => import('@/modules/staff-portal/transport-request/TransportList.vue'), },
        //            { path: 'form/:action/:no', component: () => import('@/modules/staff-portal/transport-request/TransportForm.vue') },
        //            { path: 'log/form/:action/:no/:lineNo', component: () => import('@/modules/staff-portal/transport-request/LogLineForm.vue') },
        //        ]
        //    },
        ]
    }
]
