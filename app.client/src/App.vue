<template>
    <div class="font-sans" @click="onBodyClick($event)">
        <div v-if="isMounted != null">
            <div v-if="!isAuthenticated">
                <img :src="appConfig.client.bgImagePath" alt="royal" class="opacity-25 w-full max-h-screen min-h-screen">
                <div class="top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 flex justify-center text-center items-center opacityx-0 absolute text-black w-full">
                    <router-view></router-view>
                </div>
            </div>
            <div v-else-if="isAuthenticated">
                <!--NavBar-->
                <div>
                    <navbar ref="navbar"></navbar>
                </div>
                <div class="relative sm:flex w-full">
                    <!--Sidebar-->
                    <div id="sidebar" class="z-40 sm:30 bg-theme-1 top-0 sm:top-11 w-50 sm:w-48 hidden sm:block absolute sm:relative borderx-t border-gray-400">
                        <Sidebar></Sidebar>
                    </div>
                    <main class="containerx w-full">
                        <!--Body-->
                        <div class="text-gray-900 pt-2 mt-12 bg-grayx-100 pl-1 pr-0.5 sm:pl-2 sm:pr-1 min-h-screen">
                            <router-view></router-view>
                        </div>
                    </main>
                </div>
            </div>
            <!-- Notification Modal -->
            <notification-modal v-if="notificationModal.isShow != undefined && notificationModal.isShow" :notification="notificationModal" />
            <!-- Notification -->
            <NotificationPopup v-if="notificationPopup.isShow" :notification="notificationPopup.props" />
            <!-- Show loading page when loading is true -->
            <div v-if="loader.isLoading" class="flex">
                <LoadingPage v-if="loader.message == null" />
                <LoadingPage v-if="loader.message != null" :message="loader.message" />
            </div>
        </div>
        <div v-else>
            <div class="flex flex-col items-center ">
                <LoadingPage message="Loading...Please wait." />
            </div>
        </div>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import Navbar from '@/re-usables/components/Navbar.vue';
    import WFooter from '@/re-usables/components/Footer.vue';
    import LoadingPage from '@/re-usables/components/Loading.vue';
    const NotificationModal = defineAsyncComponent(() => import('@/re-usables/components/NotificationModal.vue'));
    import Sidebar from '@/modules/HMIS/profiles/Profile_SuperUser.vue'
    const NotificationPopup = defineAsyncComponent(() => import('@/re-usables/components/NotificationPopup.vue'))
    import { useRouter } from 'vue-router'
    import { useGeneralComposable } from '@/re-usables/composables/GeneralComposable.js';
    export default {
        setup() {
            const router = useRouter();
            const { cplFnNavDateObjToString, cplFnNavDateObjToISODate } = useGeneralComposable();
            return { router, cplFnNavDateObjToString, cplFnNavDateObjToISODate };
        },
        props: {
            layoutProps: { Default: [] }
        },
        components: { NotificationModal, Navbar, WFooter, LoadingPage, NotificationPopup, Sidebar },
        data() {
            return {
                isAuthenticated: false,
                isMounted: null,
                notificationModal: {},
                loader: { isLoading: false, message: '' },
                notificationPopup: { isShow: false, props: {} },
                authUser: null,
                dimsSetup: null,
                dims: null,
                activeActionRef: "",
                title: "",
                section: "",
                inactivityTimeout: null,
                timeoutDuration: 3480000, // 58 Mins in Miliseconds
            }
        },
        mounted() {
            var authUser = localStorage.getItem("authUser");
            if (authUser != null && authUser != "undefined") {
                this.authUser = JSON.parse(authUser);
                var dimsSetup = localStorage.getItem("dimsSetup");
                if (dimsSetup != null && dimsSetup != "") {
                    this.dimsSetup = JSON.parse(dimsSetup);
                }
            }
            this.section = this.$route.params.section;
        },
        methods: {
            FnValidateUserSession() {
                this.isMounted = null;
                var authUser2 = this.getAuthUser();
                if (authUser2 != 'undefined' && authUser2 != null && authUser2 != undefined) {
                    const requestOptions = {
                        method: "POST",
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(authUser2)
                    };
                    fetch('/api/authentication/ValidateUserSession', requestOptions)
                        .then(response => {
                            return response.json();
                        }).then(data => {
                            if (data.isExpired) {
                                this.isAuthenticated = false;
                                this.router.push('/auth/login');
                                this.ClearSession();
                                this.router.go();
                            }
                            else if (!data.isExpired && !this.authUser.IsMFAVerified) {
                                this.isAuthenticated = false;
                                this.router.push('/auth/otp-login');
                            }
                            else {
                                this.isAuthenticated = true;
                                this.resetTimer();
                            }
                            this.isMounted = true;
                        }).catch((error) => {
                            this.isAuthenticated = false;
                            this.isMounted = true;
                        });
                } else {
                    this.isMounted = true;
                }

            },
            //FnNotification(message, styles, noTimeout) {
            FnNotification(notification) {
                this.notificationModal = {};
                this.notificationPopup = {};
                if (notification.type == 'popup') {
                    this.notificationPopup.props = notification;
                    this.notificationPopup.isShow = true;
                    if (notification.isNoTimeout == undefined || notification.isNoTimeout == false) {
                        var self = this;
                        setTimeout(function () { self.notificationPopup.isShow = false }, 5000);
                    }
                } else if (notification.type == 'modal') {
                    this.notificationModal = {};
                    this.notificationModal = notification;
                    this.notificationModal.isShow = true;
                }
            },
            onBodyClick(event) {
                if (event != undefined && !event.target.classList.contains('ellipsis-menu')) {
                    this.activeActionRef = '';
                }
            },
            FnGetDimensionsSetup() {
                var dimsSetup = localStorage.getItem("dimsSetup");
                if (dimsSetup == null || dimsSetup == undefined || dimsSetup == "") {
                    const requestOptions = {
                        method: "GET",
                    };
                    fetch('/api/Dimensions/GetDimensionsSetup', requestOptions)
                        .then(response => {
                            return response.json();
                        }).then(data => {
                            var res = null;
                            if (data.response2 != undefined) {
                                res = data.response2;
                            } else {
                                res = data;
                            }
                            localStorage.setItem("dimsSetup", JSON.stringify(res.setup));
                            this.dimsSetup = res.setup;
                        }).catch((error) => {
                            //
                        });
                } else {
                    this.dimsSetup = JSON.parse(dimsSetup);
                }
            },
            DimCaption(dimNo) {
                if (this.authUser != null) {
                    if (this.dimsSetup != null) {
                        if (this.dimsSetup["Shortcut_Dimension_" + dimNo + "_Caption"] != undefined) {
                            return this.dimsSetup["Shortcut_Dimension_" + dimNo + "_Caption"];
                        }
                    }
                }
                return "Dimension " + dimNo;
            },
            ClearSession() {
                localStorage.removeItem("authUser");
                localStorage.removeItem("isWelcomed");
                localStorage.removeItem("pendingStatistics");
                localStorage.removeItem("dimsSetup");
            },
            resetTimer() {
                // Clear existing timer
                clearTimeout(this.inactivityTimeout);
                // Set a new timer
                this.inactivityTimeout = setTimeout(() => {
                    this.forceLogoutUser();
                }, this.timeoutDuration);
            },
            forceLogoutUser() {
                if (this.$route.path != "/auth/login" && this.$route.path != "/") {
                    this.ClearSession();
                    this.router.go();
                }
            },
            getAuthUser() {
                var authUser = localStorage.getItem("authUser");
                if (authUser != null && authUser != "undefined") {
                    return JSON.parse(authUser);
                }
                else if (this.authUser.isExpired != undefined) {
                    return this.authUser;
                }
                return null;
            },
            fnGetUrlQueryFromObject(filter) {
                if (filter == undefined || filter == '') {
                    return '';
                }
                const params = new URLSearchParams();
                // Iterate over the object and append each key-value pair to URLSearchParams
                for (const [key, value] of Object.entries(filter)) {
                    params.append(key, value);
                }
                // Return the query string (with the leading ?)
                return "?" + params.toString();
            }
        },
        watch: {
            $route(to, from) {
                //this.notificationModal = {};
                var requiresAuth = this.$route.meta.requiresAuth;
                if (requiresAuth != "undefined" && requiresAuth) {
                    this.FnValidateUserSession();
                } else {
                    this.isMounted = true;
                }
                this.activeActionRef = "";
            },
            title: function () {
                if (this.title != undefined) {
                    document.title = this.title + ' - ' + "CHG";
                } else {
                    document.title = "Staff Portal" + ' - ' + "CHG";
                }
            }
        }
    }
</script>
