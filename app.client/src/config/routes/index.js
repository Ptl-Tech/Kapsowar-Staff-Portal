import { createRouter, createWebHistory } from 'vue-router'
import { defaultRoutes } from './default-routes';
import { supplierPortalRoutes } from './staff-portal-routes';
const routes = [
    ...defaultRoutes, ...supplierPortalRoutes
];
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
            if (session.isMFAVerified) {
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
