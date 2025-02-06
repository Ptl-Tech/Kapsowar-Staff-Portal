<template>
    <div>
        <!--Admin Services-->
        <!--<h4 class="font-semibold text-sm text-gray-500">Admin & General Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <grid-col>
                <Tile class="bg-red-500" :showLoader="isLoading">
                    <template #icon>
                        <DocumentCheckIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/approval-entry/list?status=Open&docType=advance" class="underline">Approval Entries</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/approval-entry/list?status=Open&docType=advance">{{$root.authUser.PendingMyApproval}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/approval-entry/list?status=Approved&docType=advance">{{record.approvedDocumentsHR + record.approvedDocuments}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>-->
        <!--Finance Services-->
        <h4 class="font-semibold text-sm py-0.5 text-gray-500">Finance Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <!---->
            <grid-col>
                <Tile class="bg-orange-500" :showLoader="isLoading">
                    <template #icon>
                        <CurrencyDollarIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/staff-claim/header/list?status=Pending" class="underline">Staff Claims</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/staff-claim/list?status=Pending">{{record.openStaffClaims}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/staff-claim/list?status=Pending Approval">{{record.pendingStaffClaims}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/staff-claim/list?status=Approved">{{record.approvedStaffClaims}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-blue-500" :showLoader="isLoading">
                    <template #icon>
                        <BanknotesIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/imprest-request/header/list?status=Pending" class="underline">Imprest Requests</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-request/header/list?status=Pending">{{record.openAdvanceRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-request/header/list?status=Pending Approval">{{record.pendingAdvanceRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-request/header/list?status=Approved">{{record.approvedAdvanceRequests}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-teal-500" :showLoader="isLoading">
                    <template #icon>
                        <ReceiptRefundIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/imprest-surrender/header/list?status=Pending" class="underline">Imprest Surrenders</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-surrender/header/list?status=Pending">{{record.openAdvanceSurrenders}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-surrender/header/list?status=Pending Approval">{{record.pendingAdvanceSurrenders}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-surrender/header/list?status=Approved">{{record.approvedAdvanceSurrenders}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--HR Services-->
        <h4 class="font-semibold text-sm py-0.5 text-gray-500">HR Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <!---->
            <grid-col>
                <Tile class="bg-green-500" :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <CalendarDaysIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/leave-application/list?status=Open" class="underline">Leave Applications</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-application/list?status=Open">{{record.openLeaves}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-application/list?status=Pending Approval">{{record.pendingLeaves}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-application/list?status=Released">{{record.approvedLeaves}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-rose-500" :showLoader="isLoading">
                    <template #icon>
                        <ArrowUturnDownIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/leave-recall/list?status=Open" class="underline">Leave Recalls</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-recall/list?status=Open">{{record.openLeaveRecalls}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-recall/list?status=Approval Pending">{{record.pendingLeaveRecalls}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-recall/list?status=Approved">{{record.approvedLeaveRecalls}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--Procurement Services-->
        <h4 class="font-semibold text-sm py-0.5 text-gray-500">Procurement & Store Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <!---->
            <grid-col>
                <Tile class="bg-cyan-500" :showLoader="isLoading">
                    <template #icon>
                        <ArrowPathRoundedSquareIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/purchase-request/list?status=Open" class="underline">Purchase Requests</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/purchase-request/list?status=Open">{{record.openPurchaseRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/purchase-request/list?status=Pending Approval">{{record.pendingPurchaseRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/purchase-request/list?status=Released">{{record.approvedPurchaseRequests}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-amber-500" :showLoader="isLoading">
                    <template #icon>
                        <CircleStackIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/store-request/list?status=Open" class="underline">Store Requests</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/store-request/list?status=Open">{{record.openStoreRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/store-request/list?status=Pending Approval">{{record.pendingStoreRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/store-request/list?status=Released">{{record.approvedStoreRequests}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--Transport Services-->
        <!--<h4 class="font-semibold text-sm py-0.5 text-gray-500">Transport & Logistics Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">-->
            <!---->
            <!--<grid-col>
                <Tile class="bg-indigo-500" :showLoader="isLoading">
                    <template #icon>
                        <TruckIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/transport-request/list?status=Open" class="underline">Transport Requests</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/transport-request/list?status=Open">{{record.openTransportRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/transport-request/list?status=Pending Approval">{{record.pendingTransportRequests}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/transport-request/list?status=Released">{{record.approvedTransportRequests}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>-->
    
    </div>
</template>
<script>
    import Tile from '@/re-usables/components/Tile.vue';
    import Grid from '@/re-usables/components/Grid.vue';
    import GridCol from '@/re-usables/components/GridCol.vue';
    import { DocumentCheckIcon, UserPlusIcon,DocumentTextIcon,DocumentChartBarIcon,DocumentPlusIcon,DocumentArrowDownIcon,FaceSmileIcon,ArrowPathRoundedSquareIcon,CircleStackIcon, CalendarDaysIcon, ClockIcon, CurrencyDollarIcon, ArrowsPointingInIcon, ClipboardDocumentCheckIcon, ArrowUturnDownIcon, TruckIcon, ReceiptRefundIcon, BanknotesIcon } from '@heroicons/vue/24/outline'
    export default {
        components: {
            Tile, Grid, GridCol, UserPlusIcon, DocumentTextIcon, DocumentChartBarIcon, DocumentPlusIcon, DocumentArrowDownIcon, FaceSmileIcon,ArrowPathRoundedSquareIcon,
           DocumentCheckIcon, CurrencyDollarIcon, ArrowsPointingInIcon,CircleStackIcon,
            ClipboardDocumentCheckIcon, ArrowUturnDownIcon, TruckIcon,
            ReceiptRefundIcon, BanknotesIcon, CalendarDaysIcon,ClockIcon
        },
        data() {
            return {
                pageProps: {
                    title: 'Dashboard Form',
                    pageType: "form",
                    controller: 'General',
                    formRoute: "/ess/dashboard",
                },
                record: {},
                profile: {},
                formMode: 'view',
                isLoading: false,
            }
        },
        created() {
            this.$root.title = "Dashboard"
            //this.FnFetchSetups();
            var isWelcomed = localStorage.getItem("isWelcomed");
            if (isWelcomed == undefined || isWelcomed == null) {
                var msg = "Hi " + this.$root.authUser.firstName + ", welcome to Kapsowar staff Portal";
                this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
            }
        },
        mounted() {
            this.$root.FnGetDimensionsSetup();
        },
        methods: {
            FnFetchSetups() {
                this.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/Dashboard', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.record = data.response.dashboard;
                            if (data.response.dashboard != undefined) {
                                localStorage.setItem("isWelcomed", JSON.stringify(true));
                                var authUser = localStorage.getItem("authUser");
                                if (authUser != null && authUser != "undefined") {
                                    var authUser2 = JSON.parse(authUser);
                                    authUser2.PendingMyApproval = this.record.pendingMyApprovalHR + this.record.pendingMyApproval + this.record.pendingMyApprovalEmployee;
                                    authUser2.newNotifications = this.record.newNotifications;
                                    localStorage.setItem("authUser", JSON.stringify(authUser2));
                                    this.$root.authUser.PendingMyApproval = authUser2.PendingMyApproval;
                                    this.$root.authUser.newNotifications = authUser2.newNotifications;
                                }
                            }
                        }
                        this.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.isLoading = false;
                    });
            },
        }

    }
</script>