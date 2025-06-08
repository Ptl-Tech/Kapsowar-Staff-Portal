<template>
    <div>
        <div class="container mx-auto px-4 py-4">
            <!-- Header Section -->
            <div class="text-center mb-12">
                <h1 class="text-2xl font-bold text-gray-800">What We Stand For</h1>
                <p class="text-md text-gray-600 mt-4">Excellence In Service Provision.</p>
            </div>

            <!-- Cards Section -->
            <div class="flex flex-wrap -mx-4">
                <!-- Vision Card -->
                <div class="w-full md:w-1/3 px-4 mb-8">
                    <div class="bg-white rounded-lg shadow-lg p-8 text-center hover:shadow-xl transition-shadow duration-300">
                        <div class="flex justify-center">
                            <!-- Icon for Vision -->
                            <svg class="w-12 h-12 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"
                                 xmlns="http://www.w3.org/2000/svg">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                      d="M3 9a2 2 0 012-2h.93a2 2 0 001.664-.89l.812-1.22A2 2 0 0110.07 4h3.86a2 2 0 011.664.89l.812 1.22A2 2 0 0018.07 7H19a2 2 0 012 2v9a2 2 0 01-2 2H5a2 2 0 01-2-2V9z">
                                </path>
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                      d="M15 13a3 3 0 11-6 0 3 3 0 016 0z"></path>
                            </svg>
                        </div>
                        <h3 class="text-2xl font-semibold text-gray-800 mt-6">Vision</h3>
                        <p class="text-gray-600 mt-4 leading-relaxed">
                            Improve the physical and spiritual health of our community while training others to do the same
                        </p>
                    </div>
                </div>

                <!-- Mission Card -->
                <div class="w-full md:w-1/3 px-4 mb-8">
                    <div class="bg-white rounded-lg shadow-lg p-8 text-center hover:shadow-xl transition-shadow duration-300">
                        <div class="flex justify-center">
                            <!-- Icon for Mission -->
                            <svg class="w-12 h-12 text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"
                                 xmlns="http://www.w3.org/2000/svg">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                      d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                            </svg>
                        </div>
                        <h3 class="text-2xl font-semibold text-gray-800 mt-6">Mission</h3>
                        <p class="text-gray-600 mt-4 leading-relaxed">
                            Provide underserved communities with access to quality health services while they experience the love of Jesus
                        </p>
                    </div>
                </div>

                <!-- Values Card -->
                <div class="w-full md:w-1/3 px-4 mb-8">
                    <div class="bg-white rounded-lg shadow-lg p-8 text-center hover:shadow-xl transition-shadow duration-300">
                        <div class="flex justify-center">
                            <!-- Icon for Values -->
                            <svg class="w-12 h-12 text-purple-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"
                                 xmlns="http://www.w3.org/2000/svg">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                      d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z">
                                </path>
                            </svg>
                        </div>
                        <h3 class="text-2xl font-semibold text-gray-800 mt-6">Values</h3>
                        <p class="text-gray-600 mt-4 leading-relaxed">
                            Humility, Faith, Service, Love
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <!--Leave Balances-->
        <h4 class="font-semibold text-sm py-0.5 text-gray-500">Leave Balances:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <!---->
            <grid-col>
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-pink-500"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Annual Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.annualLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-blue-400"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Compassion Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.compassionLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col v-if="$root.authUser.gender == 'Female'">
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-orange-500"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Maternity Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.maternityLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col v-if="$root.authUser.gender == 'Male'">
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-orange-500"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Paternity Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.paternityLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-gray-600"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Sick Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.sickLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-green-500"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Study Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.studyLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
            <!---->
            <grid-col>
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-teal-500"><CalendarDaysIcon class="h-4 w-4" /></div>
                    </template>
                    <template #label>
                        Unpaid Leave Balance
                    </template>
                    <template #value>
                        <div class="flex gap-1 items-center">
                            <span class="!text-lg">{{record.unpaidLeaveBalance}}</span> <span class="text-xs">Days</span>
                        </div>
                    </template>
                </Tile>
            </grid-col>
        </grid>
        <!--Admin Services-->
        <h4 class="font-semibold text-sm text-gray-500">Admin Services:</h4>
        <grid class="sm:!grid-cols-2 lg:!grid-cols-5">
            <grid-col>
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-red-500"><DocumentCheckIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-blue-400"><UserGroupIcon class="h-4 w-4" /></div>
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
                        <div class="p-1 rounded-full bg-green-500"><UserCircleIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-orange-500"><NumberedListIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-orange-500"><CurrencyDollarIcon class="h-4 w-4" /></div>
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
                        <div class="p-1 rounded-full bg-blue-400"><BanknotesIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-teal-500"><ReceiptRefundIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-gray-600"><ReceiptRefundIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading" :isStatus="true">
                    <template #icon>
                        <div class="p-1 rounded-full bg-green-500"><CalendarDaysIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-cyan-500"><ArrowPathRoundedSquareIcon class="h-4 w-4" /></div>
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
                <Tile :showLoader="isLoading">
                    <template #icon>
                        <div class="p-1 rounded-full bg-cyan-500"><CircleStackIcon class="h-4 w-4" /></div>
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
                    <TruckIcon class="h-4 w-4" />
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