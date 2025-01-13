import { createRouter, createWebHistory } from 'vue-router'
const routes = [
  /**
   * Patterns
   * Main pages - module/pageType/action/docNo e.g leave-application/form/edit/DC123
   * Line pages - module/line/pageType/action/docNo/lineNo e.g leave-application/line/form/edit/DC123/123
   */
  { path: '/', component: () => import('@/re-usables/modules/auth/Login.vue'), },
  { path: '/dashboard', component: () => import('@/modules/HMIS/dashboards/Dashboard_Default.vue'), meta: { requiresAuth: true } },
  /**
  * AUTH ROUTES
  * */
  {
    path: '/auth',
    meta: { requiresAuth: false },
    children: [
      { path: 'login', name: 'authLogin', title: 'Login', component: () => import('@/re-usables/modules/auth/Login.vue'), },
      { path: 'forgot-password', component: () => import('@/re-usables/modules/auth/ForgotPassword.vue') },
      { path: 'reset-password/:no', component: () => import('@/re-usables/modules/auth/ResetPassword.vue') },
      { path: 'otp-login', component: () => import('@/re-usables/modules/auth/OTPLogin.vue') },
      { path: 'register', component: () => import('@/re-usables/modules/auth/Register.vue') },
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
  /**
  * HMIS ROUTES
  * */
  {
    path: '/hmis/:section',
    meta: { requiresAuth: true },
    children: [
      //visitor Routes
      {
        path: 'visitor',
        meta: { requiresAuth: true },
        children: [
          { path: 'list', name: "VisitorList", component: () => import('@/modules/hmis/visitor/VisitorHeaderList.vue'), },
          { path: 'form/:action', component: () => import('@/modules/hmis/visitor/VisitorHeaderForm.vue') },
        ]
      },
      //Patient Routes
      {
        path: 'patient',
        meta: { requiresAuth: true },
        children: [
          { path: 'list', name: "PatientList", component: () => import('@/modules/hmis/patient/PatientHeaderList.vue'), },
          { path: 'form/:action', component: () => import('@/modules/hmis/patient/PatientHeaderForm.vue') },
        ]
      },
      //Visit Routes
      {
        path: 'visit',
        meta: { requiresAuth: true },
        children: [
          { path: 'list', name: "VisitsList", component: () => import('@/modules/hmis/visit/VisitHeaderList.vue'), },
          { path: 'form/:action', component: () => import('@/modules/hmis/visit/VisitHeaderForm.vue') },
        ]
      },
      //Triage Patient Routes
      {
        path: 'triage-patient',
        meta: { requiresAuth: true },
        children: [
          { path: 'list', name: "TriagePatientsList", component: () => import('@/modules/hmis/triage/TriagePatientHeaderList.vue'), },
          { path: 'form/:action', component: () => import('@/modules/hmis/triage/TriagePatientHeaderForm.vue') },
        ]
       },
        //vitals
        {
            path: 'vitals',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "PatientVitalsList", component: () => import('@/modules/hmis/triage/vitals/VitalsLineList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/triage/vitals/VitalsLineForm.vue') },
            ]
        },
        //allergies
        {
            path: 'allergies',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "PatientAllergiesList", component: () => import('@/modules/hmis/triage/allergies/AllergiesLineList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/triage/allergies/AllergiesLineForm.vue') },
            ]
        },
        //injections
        {
            path: 'injections',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "PatientInjectionsList", component: () => import('@/modules/hmis/triage/injections/InjectionsLineList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/triage/injections/InjectionsLineForm.vue') },
            ]
        },
        //dressings
        {
            path: 'dressings',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "PatientDressingsList", component: () => import('@/modules/hmis/triage/dressings/DressingsLineList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/triage/dressings/DressingsLineForm.vue') },
            ]
        },
        /**
        * CONSULTATION ROUTES
        * */
        {
            path: 'consultation',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationList", component: () => import('@/modules/hmis/consultation/treatment-header/TreatmentHeaderList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/treatment-header/TreatmentHeaderForm.vue') },
            ]
        },
        //signs
        {
            path: 'signs',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationSignsList", component: () => import('@/modules/hmis/consultation/signs/ConsultationSignsList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/signs/ConsultationSignsForm.vue') },
            ]
        },
        //symptoms
        {
            path: 'symptoms',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationSymptomsList", component: () => import('@/modules/hmis/consultation/symptoms/ConsultationSymptomsList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/symptoms/ConsultationSymptomsForm.vue') },
            ]
        },
        //laboratory
        {
            path: 'laboratory',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationLaboratoryList", component: () => import('@/modules/hmis/consultation/laboratory/ConsultationLaboratoryList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/laboratory/ConsultationLaboratoryForm.vue') },
            ]
        },
        //radiology
        {
            path: 'radiology',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationRadiologyList", component: () => import('@/modules/hmis/consultation/radiology/CLT_RadiologyList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/radiology/CLT_RadiologyForm.vue') },
            ]
        },
        //diagnosis
        {
            path: 'diagnosis',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationDiagnosisList", component: () => import('@/modules/hmis/consultation/diagnosis/CLT_DiagnosisList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/diagnosis/CLT_DiagnosisForm.vue') },
            ]
        },
        //injections
        {
            path: 'clt-injections',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationInjectionsList", component: () => import('@/modules/hmis/consultation/injections/CLT_InjectionsList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/injections/CLT_InjectionsForm.vue') },
            ]
        },
        //prescription
        {
            path: 'clt-prescription',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationPrescriptionsList", component: () => import('@/modules/hmis/consultation/prescription/CLT_PrescriptionList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/prescription/CLT_PrescriptionForm.vue') },
            ]
        },
        //admission
        {
            path: 'clt-admission',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationAdmissionList", component: () => import('@/modules/hmis/consultation/admission/CLT_AdmissionList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/admission/CLT_AdmissionForm.vue') },
            ]
        },
        //referral
        {
            path: 'clt-referral',
            meta: { requiresAuth: true },
            children: [
                { path: 'list', name: "ConsultationReferralList", component: () => import('@/modules/hmis/consultation/referral/CLT_ReferralList.vue'), },
                { path: 'form/:action', component: () => import('@/modules/hmis/consultation/referral/CLT_ReferralForm.vue') },
            ]
        }
    ]
    },
    /**
    * HMIS 2 (Latest)
    * */
    {
        path: '/hmis',
        meta: { requiresAuth: true },
        children: [
            {
                /**
                  * LABORATORY ROUTES
                * */
                path: 'laboratory',
                meta: { requiresAuth: true },
                children: [
                   
                    {
                        path: 'test-header',
                        children: [
                            { path: 'list', name: "LaboratoryTestsList", component: () => import('@/modules/hmis/laboratory/test-header/Lab_TestHeaderList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/hmis/laboratory/test-header/Lab_TestHeaderForm.vue') },
                        ]
                    },
                    //
                    {
                        path: 'test-line',
                        children: [
                            { path: 'list', name: "LaboratoryTestLinesList", component: () => import('@/modules/hmis/laboratory/test-line/Lab_TestLineList.vue') },
                            { path: 'form/:action', component: () => import('@/modules/hmis/laboratory/test-line/Lab_TestLineForm.vue') },
                        ]
                    },
                ]
            }
        ]
    }
    
];
/**
 * ADD CONTROLLER,FORM ROUTE, LIST ROUTE, TITLE as META DATA FOR ROUTES
 */
const router =    new createRouter({
    history: createWebHistory(),
    routes, // short for `routes: routes`
});
//check those that requires Authentication
//const session = JSON.parse(localStorage.getItem('authUser'));
router.beforeEach((to, from, next) => {
    if (to.meta.requiresAuth) {
        const session = JSON.parse(localStorage.getItem('authUser'));
        if (session != null) {
            if (session.IsMFAVerified) {
                next();
            }else{
                next('/auth/otp-login');
            }
        } else {
            next('/auth/login');
        }
    } else {
        next();
    }
});
export default router;
