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
            //
            {
                path: 'imprest-request',
                meta: { requiresAuth: true },
                children: [
                    {
                        path: 'header',
                        meta: { requiresAuth: true },
                        children: [
                            { path: 'list', name: "ImprestHeaderList", component: () => import('@/modules/staff-portal/imprest-request/header/ImprestHeaderList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/imprest-request/header/ImprestHeaderForm.vue') },
                        ]
                    },
                    {
                        path: 'line',
                        meta: { requiresAuth: true },
                        children: [
                            { path: 'list', name: "ImprestLineList", component: () => import('@/modules/staff-portal/imprest-request/line/ImprestLineList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/imprest-request/line/ImprestLineForm.vue') },
                        ]
                    },
                ]
            },
            //
            {
                path: 'imprest-surrender',
                meta: { requiresAuth: true },
                children: [
                    {
                        path: 'header',
                        children: [
                            { path: 'list', name: "ImprestSurrenderHeaderList", component: () => import('@/modules/staff-portal/imprest-surrender/header/ImprestSurrenderHeaderList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/imprest-surrender/header/ImprestSurrenderHeaderForm.vue') },
                        ]
                    },
                    {
                        path: 'line',
                        children: [
                            { path: 'list', name: "ImprestSurrenderLineList", component: () => import('@/modules/staff-portal/imprest-surrender/line/ImprestSurrenderLineList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/imprest-surrender/line/ImprestSurrenderLineForm.vue') },
                        ]
                    },
                ]
            },
            //
            {
                path: 'staff-claim',
                meta: { requiresAuth: true },
                children: [
                    {
                        path: 'header',
                        children: [
                            { path: 'list', name: "StaffClaimHeaderList", component: () => import('@/modules/staff-portal/staff-claim/header/StaffClaimHeaderList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/staff-claim/header/StaffClaimHeaderForm.vue') },
                        ]
                    },
                    {
                        path: 'line',
                        children: [
                            { path: 'list', name: "StaffClaimLineList", component: () => import('@/modules/staff-portal/staff-claim/line/StaffClaimLineList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/staff-claim/line/StaffClaimLineForm.vue') },
                        ]
                    },
                ]
            },
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
            {
                path: 'reports',
                meta: { requiresAuth: true },
                children: [
                    { path: 'payslip', component: () => import('@/modules/staff-portal/reports/Payslip.vue'), },
                    { path: 'p9', component: () => import('@/modules/staff-portal/reports/P9.vue') },
                ]
            },
            /**
             * PROCUREMENT & STORES ROUTES
             */
            //
            {
                path: 'purchase-request',
                meta: { requiresAuth: true },
                children: [
                    {
                        path: 'header',
                        children: [
                            { path: 'list', name: "PurchaseRequestHeaderList", component: () => import('@/modules/staff-portal/purchase-request/header/PurchaseRequestHeaderList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/purchase-request/header/PurchaseRequestHeaderForm.vue') },
                        ]
                    },
                    {
                        path: 'line',
                        children: [
                            { path: 'list', name: "PurchaseRequestLineList", component: () => import('@/modules/staff-portal/purchase-request/line/PurchaseRequestLineList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/purchase-request/line/PurchaseRequestLineForm.vue') },
                        ]
                    },
                ]
            },
            {
                path: 'store-request',
                meta: { requiresAuth: true },
                children: [
                    {
                        path: 'header',
                        children: [
                            { path: 'list', name: "StoreRequestHeaderList", component: () => import('@/modules/staff-portal/store-request/header/StoreRequestHeaderList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/store-request/header/StoreRequestHeaderForm.vue') },
                        ]
                    },
                    {
                        path: 'line',
                        children: [
                            { path: 'list', name: "StoreRequestLineList", component: () => import('@/modules/staff-portal/store-request/line/StoreRequestLineList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/staff-portal/store-request/line/StoreRequestLineForm.vue') },
                        ]
                    },
                ]
            },

        ]
    }
]
