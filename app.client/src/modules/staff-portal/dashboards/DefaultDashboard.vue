<template>
    <div>
        <!--Admin Services-->
        <h4 class="font-semibold text-sm text-gray-500">Admin Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <grid-col>
                <Tile class="bg-red-500" :showLoader="isLoading">
                    <template #icon>
                        <DocumentCheckIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/approval-entry/list?status=Open&docType=LeaveApplication" class="underline">Approval Entries</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/approval-entry/list?status=Open&docType=LeaveApplication">{{$root.authUser.pendingMyApproval}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/approval-entry/list?status=Open&docType=LeaveApplication">{{record.approvedDocuments}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--Finance Services-->
        <h4 v-if="$root.authUser.navCompany == appConfig.client.collegeCompany && $root.authUser.isHOD" class="font-semibold text-sm py-0.5 text-gray-500">Academic HOD Services:</h4>
        <grid v-if="$root.authUser.navCompany == appConfig.client.collegeCompany && $root.authUser.isHOD" class="sm:!grid-cols-2 lg:!grid-cols-5">
            <!---->
            <grid-col>
                <Tile class="bg-blue-500" :showLoader="isLoading">
                    <template #icon>
                        <UserGroupIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link :to="{name:'studentsList',query:{status:'Current'}}" class="underline">Students List</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Current</span>
                                <span class="underline">
                                    <router-link :to="{name:'studentsList',query:{status:'Current'}}">{{record.studentsCountCurrent}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Registration</span>
                                <span class="underline">
                                    <router-link :to="{name:'studentsList',query:{status:'Registration'}}">{{record.studentsCountRegistration}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-green-500" :showLoader="isLoading">
                    <template #icon>
                        <UserCircleIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link :to="{name:'lecturerList',query:{status:'Active'}}" class="underline">Lecturers List</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Active</span>
                                <span class="underline">
                                    <router-link :to="{name:'lecturerList',query:{status:'Active'}}">{{record.lecturersCountActive}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Inactive</span>
                                <span class="underline">
                                    <router-link :to="{name:'lecturerList',query:{status:'Active'}}">{{record.lecturersCountInactive}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-orange-500" :showLoader="isLoading">
                    <template #icon>
                        <NumberedListIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link :to="{name:'programmeList',query:{category:'All'}}" class="underline">Programmes List</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">All</span>
                                <span class="underline">
                                    <router-link :to="{name:'programmeList',query:{category:'All'}}">{{record.programmesCountAll}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--Finance Services-->
        <h4 v-if="$root.authUser.navCompany == appConfig.client.collegeCompany" class="font-semibold text-sm py-0.5 text-gray-500">Finance Services:</h4>
        <grid v-if="$root.authUser.navCompany == appConfig.client.collegeCompany" class="sm:!grid-cols-2 lg:!grid-cols-5">
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
                                    <router-link to="/ess/staff-claim/header/list?status=Pending">{{record.staffClaimsOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/staff-claim/header/list?status=Pending Approval">{{record.staffClaimsPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/staff-claim/header/list?status=Approved">{{record.staffClaimsApproved}}</router-link>
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
                                    <router-link to="/ess/imprest-request/header/list?status=Pending">{{record.imprestsOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-request/header/list?status=Pending Approval">{{record.imprestsPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-request/header/list?status=Approved">{{record.imprestsApproved}}</router-link>
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
                                    <router-link to="/ess/imprest-surrender/header/list?status=Pending">{{record.imprestSurrendersOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-surrender/header/list?status=Pending Approval">{{record.imprestSurrendersPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/imprest-surrender/header/list?status=Approved">{{record.imprestSurrendersApproved}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile class="bg-gray-600" :showLoader="isLoading">
                    <template #icon>
                        <ReceiptRefundIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/petty-cash/header/list?status=Pending" class="underline">Petty Cash</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/petty-cash/header/list?status=Pending">{{record.pettyCashOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/petty-cash/header/list?status=Pending Approval">{{record.pettyCashPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/petty-cash/header/list?status=Approved">{{record.pettyCashApproved}}</router-link>
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
                                    <router-link to="/ess/leave-application/list?status=Open">{{record.leavesOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-application/list?status=Pending Approval">{{record.leavesPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/leave-application/list?status=Released">{{record.leavesApproved}}</router-link>
                                </span>
                            </span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--Procurement Services-->
        <h4 v-if="$root.authUser.navCompany == appConfig.client.collegeCompany" class="font-semibold text-sm py-0.5 text-gray-500">Procurement & Store Services:</h4>
        <grid v-if="$root.authUser.navCompany == appConfig.client.collegeCompany" class="sm:!grid-cols-2 lg:!grid-cols-5">
            <!---->
            <grid-col>
                <Tile class="bg-cyan-500" :showLoader="isLoading">
                    <template #icon>
                        <ArrowPathRoundedSquareIcon class="h-6 w-6" />
                    </template>
                    <template #label>
                        <router-link to="/ess/purchase-request/header/list?status=Open" class="underline">Purchase Requests</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/purchase-request/header/list?status=Open">{{record.purchaseReqsOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/purchase-request/header/list?status=Pending Approval">{{record.purchaseReqsPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/purchase-request/header/list?status=Released">{{record.purchaseReqsApproved}}</router-link>
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
                        <router-link to="/ess/store-request/header/list?status=Open" class="underline">Store Requests</router-link>
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Open</span>
                                <span class="underline">
                                    <router-link to="/ess/store-request/header/list?status=Open">{{record.storeReqsOpen}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Pending</span>
                                <span class="underline">
                                    <router-link to="/ess/store-request/header/list?status=Pending Approval">{{record.storeReqsPending}}</router-link>
                                </span>
                            </span>
                            <span class="border-r h-6"></span>
                            <span class="text-xs flex flex-col items-center">
                                <span class="">Approved</span>
                                <span class="underline">
                                    <router-link to="/ess/store-request/header/list?status=Released">{{record.storeReqsApproved}}</router-link>
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
    import { DocumentCheckIcon, UserPlusIcon, NumberedListIcon, DocumentTextIcon, DocumentChartBarIcon, DocumentPlusIcon, DocumentArrowDownIcon, FaceSmileIcon, ArrowPathRoundedSquareIcon, CircleStackIcon, CalendarDaysIcon, ClockIcon, CurrencyDollarIcon, ArrowsPointingInIcon, ClipboardDocumentCheckIcon, ArrowUturnDownIcon, TruckIcon, ReceiptRefundIcon, BanknotesIcon, UserGroupIcon, UserCircleIcon } from '@heroicons/vue/24/outline'
    export default {
        components: {
            Tile, Grid, GridCol, UserPlusIcon, DocumentTextIcon, DocumentChartBarIcon, DocumentPlusIcon, DocumentArrowDownIcon, FaceSmileIcon, ArrowPathRoundedSquareIcon,
            DocumentCheckIcon, CurrencyDollarIcon, ArrowsPointingInIcon, CircleStackIcon,
            ClipboardDocumentCheckIcon, ArrowUturnDownIcon, TruckIcon, UserCircleIcon,
            ReceiptRefundIcon, BanknotesIcon, CalendarDaysIcon, ClockIcon, UserGroupIcon, NumberedListIcon
        },
        data() {
            return {
                pageProps: {
                    title: 'Dashboard Form',
                    pageType: "form",
                    controller: 'ESS_General',
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
            this.FnFetchSetups();
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
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/Dashboard', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = this.appConfig.errors.dataFetchFailure;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.record = data.response;
                            if (data.response != undefined) {
                                localStorage.setItem("isWelcomed", JSON.stringify(true));
                                var authUser = localStorage.getItem("authUser");
                                if (authUser != null && authUser != "undefined") {
                                    var authUser2 = JSON.parse(authUser);
                                    authUser2.PendingMyApproval = this.record.pendingMyApprovalCount;
                                    authUser2.newNotifications = this.record.newNotifications;
                                    localStorage.setItem("authUser", JSON.stringify(authUser2));
                                    this.$root.authUser.PendingMyApproval = authUser2.PendingMyApproval;
                                    this.$root.authUser.newNotifications = authUser2.newNotifications;
                                }
                            }
                        }
                        this.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.isLoading = false;
                    });
            },
        }

    }
</script>