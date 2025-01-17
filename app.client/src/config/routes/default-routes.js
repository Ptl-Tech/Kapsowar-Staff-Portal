export const defaultRoutes = [
    /**
     * Patterns
     * Main pages - module/pageType/action/docNo e.g leave-application/form/edit/DC123
     * Line pages - module/line/pageType/action/docNo/lineNo e.g leave-application/line/form/edit/DC123/123
     */
    { path: '/', name: 'home', component: () => import('@/re-usables/modules/auth/Login.vue'), },
    /**
    * AUTH ROUTES
    * */
    {
        path: '/auth',
        meta: { requiresAuth: false },
        children: [
            { path: 'login', name: 'authLogin', title: 'Login', component: () => import('@/re-usables/modules/auth/Login.vue'), },
            { path: 'forgot-password', name: 'authForgotPassword', component: () => import('@/re-usables/modules/auth/ForgotPassword.vue') },
            { path: 'reset-password/:no', name: 'authResetPassword', component: () => import('@/re-usables/modules/auth/ResetPassword.vue') },
            { path: 'otp-login', name: 'authOTPLogin', component: () => import('@/re-usables/modules/auth/OTPLogin.vue') },
            { path: 'register', name: 'authRegister', component: () => import('@/re-usables/modules/auth/Register.vue') },
        ],
    },
    /**
    * GENERAL ROUTES
    * */
    //notification
    {
        path: '/notification',
        meta: { requiresAuth: true },
        children: [
            { path: 'list', name: "NotificationList", component: () => import('@/re-usables/modules/notification/NotificationHeaderList.vue'), },
            { path: 'form/:action', component: () => import('@/re-usables/modules/notification/NotificationHeaderForm.vue') },
        ]
    },
    //profile
    { path: '/profile', component: () => import('@/re-usables/modules/profile/Profile.vue'), meta: { requiresAuth: true } },
    { path: '/change-password', component: () => import('@/re-usables/modules/auth/ChangePassword.vue') },
    //WSystem
    {
        path: '/wsystem',
        meta: { requiresAuth: true },
        children: [
            { path: 'file-preview', component: () => import('@/re-usables/page-templates/FilePreviewPageTemplate.vue'), },
        ]
    },

];