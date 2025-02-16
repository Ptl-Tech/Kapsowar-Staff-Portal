<template>
    <div>
        <Sidebar>
            <sidebar-link to="/ess/dashboard" :active="this.$route.name == 'dashboard'?true:false" :isLast="true" class="border-b border-gray-500"><Squares2X2Icon class="iconSmall" /> Dashboard</sidebar-link>
            <!---->
            <!--<SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
    <template #caption>
        <DocumentCheckIcon class="iconSmall" /> Approval Entries-->
            <!--<label class="bg-red-500 rounded-full !px-1 font-normal">{{$root.authUser.PendingMyApproval}}</label>-->
            <!--<label class="bg-red-500 rounded-full !px-1 font-normal">0</label>
        </template>
        <template #content>
            <sidebar-link to="/ess/approval-entry/list?status=Open&docType=staff-claim" :active="this.$route.fullPath.includes('/ess/approval-entry/list?status=Open') || this.$route.fullPath.includes('/ess/approval-entry/Open')?true:false" :sidebarDropdown="true">Pending My Approval <label class="bg-white rounded-full !px-1 text-red-500 font-normal">{{$root.authUser.PendingMyApproval}}</label></sidebar-link>
            <sidebar-link to="/ess/approval-entry/list?status=Approved&docType=staff-claim" :active="decodeURI(this.$route.fullPath).includes('/ess/approval-entry/list?status=Approved') || this.$route.fullPath.includes('/ess/approval-entry/Approved')?true:false" :sidebarDropdown="true">Approved Documents</sidebar-link>
            <sidebar-link to="/ess/approval-entry/list?status=Rejected&docType=staff-claim" :active="this.$route.fullPath.includes('/ess/approval-entry/list?status=Rejected') || this.$route.fullPath.includes('/ess/approval-entry/Rejected')?true:false" :sidebarDropdown="true" isLast="true">Rejected Documents</sidebar-link>
        </template>
    </SidebarDropdown>-->
            <!---->
            <SidebarDropdown :active="true" :isLast="false">
                <template #caption>
                    <UserGroupIcon class="iconSmall" /> HR & Payroll Services
                </template>
                <template #content>
                    <sidebar-link to="/ess/leave-application/list?status=Open" :active="($route.name == 'leaveList' || $route.name == 'leaveForm')?true:false" :sidebarDropdown="true">Leave Applications</sidebar-link>
                    <!--<sidebar-link to="/ess/leave-recall/list?status=Open" :active="this.$route.path.includes('/ess/leave-recall') || $route.path.includes('/ess/leave-recall/form')?true:false" :sidebarDropdown="true">Leave Recalls</sidebar-link>-->
                    <sidebar-link to="/ess/reports/payslip" :active="this.$route.fullPath == '/ess/reports/payslip'?true:false" :sidebarDropdown="true" :isLast="false">Payslip</sidebar-link>
                    <sidebar-link to="/ess/reports/p9" :active="decodeURI(this.$route.fullPath) == '/ess/reports/p9'" :sidebarDropdown="true" :isLast="true">P9</sidebar-link>
                </template>
            </SidebarDropdown>
            <!---->
            <SidebarDropdown :active="true">
                <template #caption>
                    <CurrencyDollarIcon class="iconSmall" /> Finance Services
                </template>
                <template #content>
                    <sidebar-link to="/ess/imprest-request/header/list?status=Pending" :active="this.$route.path.includes('/ess/imprest-request/')?true:false" :sidebarDropdown="true" :isLast="false">Imprests</sidebar-link>
                    <sidebar-link to="/ess/imprest-surrender/header/list?status=Pending" :active="this.$route.path.includes('/ess/imprest-surrender')?true:false" :sidebarDropdown="true" :isLast="false">Imprest Surrenders</sidebar-link>
                    <sidebar-link to="/ess/staff-claim/header/list?status=Pending" :active="this.$route.path.includes('/ess/staff-claim')?true:false" :sidebarDropdown="true" :isLast="true">Staff Claims</sidebar-link>
                </template>
            </SidebarDropdown>
            <!---->
            <SidebarDropdown :active="true">
                <template #caption>
                    <DocumentCheckIcon class="iconSmall" /> Procurement & Stores
                </template>
                <template #content>
                    <sidebar-link to="/ess/purchase-request/header/list?status=Open" :active="this.$route.path.includes('/ess/purchase-request')?true:false" :sidebarDropdown="true" :isLast="false">Purchase Requests</sidebar-link>
                    <sidebar-link to="/ess/store-request/header/list?status=Open" :active="this.$route.path.includes('/ess/store-request')?true:false" :sidebarDropdown="true" :isLast="true">Store Requests</sidebar-link>
                </template>
            </SidebarDropdown>
            <!---->
            <SidebarDropdown :active="true" v-if="$root.authUser.isHOD">
                <template #caption>
                    <BookOpenIcon class="iconSmall" /> Academics ({{$root.authUser.departmentCode}})
                </template>
                <template #content>
                    <sidebar-link :to="{name:'studentsList',query:{status:'Current'}}" :active="$route.name == 'studentsList'?true:false" :sidebarDropdown="true" :isLast="false">Students List</sidebar-link>
                    <sidebar-link :to="{name:'lecturerList',query:{status:'Active'}}" :active="$route.name == 'lecturerList'?true:false" :sidebarDropdown="true" :isLast="false">Lecturers List</sidebar-link>
                    <sidebar-link :to="{name:'programmeList',query:{category:'All'}}" :active="$route.name == 'programmeList'?true:false" :sidebarDropdown="true" :isLast="false">Programmes List</sidebar-link>
                    <!--<sidebar-link to="/ess/store-request/header/list?status=Open" :active="this.$route.path.includes('/ess/store-request')?true:false" :sidebarDropdown="true" :isLast="true">Reports</sidebar-link>-->
                </template>
            </SidebarDropdown>
            <!---->
            <SidebarDropdown :active="true" v-if="$root.authUser.isLecturer">
                <template #caption>
                    <ArrowsPointingInIcon class="iconSmall" /> Lecturer Section
                </template>
                <template #content>
                    <sidebar-link :to="{name:'lecturerUnitForm',params:{action:'edit'}}" :active="this.$route.name == 'lecturerUnitForm'?true:false" :sidebarDropdown="true" :isLast="true">Lecturer Assigned Units</sidebar-link>
                </template>
            </SidebarDropdown>
            <!---->
            <!--<SidebarDropdown :active="true">
        <template #caption>
            <TruckIcon class="iconSmall" /> Transport & Logistics
        </template>
        <template #content>
            <sidebar-link to="/ess/transport-request/list?status=Open" :active="this.$route.fullPath == '/ess/transport-request/list?status=Open' || this.$route.fullPath == '/ess/transport-request/list?status=Pending Approval' || this.$route.fullPath == '/ess/transport-request/list?status=Released'?true:false" :sidebarDropdown="true" :isLast="$root.authUser.IsTransportManager || $root.authUser.IsDriver? false:true">Transport Requests</sidebar-link>
            <div v-if="$root.authUser.IsTransportManager">
                <SidebarHeading>Transport Manager</SidebarHeading>
                <sidebar-link to="/ess/transport-request/list?status=Unassigned" :active="this.$route.fullPath == '/ess/transport-request/list?status=Unassigned'?true:false" :sidebarDropdown="true" class="pl-2">Approved But Unassigned</sidebar-link>
                <sidebar-link to="/ess/transport-request/list?status=Assigned" :active="this.$route.fullPath == '/ess/transport-request/list?status=Assigned'?true:false" :sidebarDropdown="true" class="pl-2">Assigned</sidebar-link>
                <sidebar-link to="/ess/transport-request/list?status=Closed" :active="this.$route.fullPath == '/ess/transport-request/list?status=Closed'?true:false" :sidebarDropdown="true" class="pl-2">Closed</sidebar-link>
            </div>
            <div v-if="$root.authUser.IsDriver">
                <SidebarHeading>Driver</SidebarHeading>
                <sidebar-link to="/ess/transport-request/list?status=DriverUnclosed" :active="this.$route.fullPath == '/ess/transport-request/list?status=DriverUnclosed'?true:false" :sidebarDropdown="true" class="pl-2">Assigned But Unclosed</sidebar-link>
                <sidebar-link to="/ess/transport-request/list?status=DriverClosed" :active="this.$route.fullPath == '/ess/transport-request/list?status=DriverClosed'?true:false" :sidebarDropdown="true" class="pl-2" isLast="true">Closed</sidebar-link>
            </div>
        </template>
    </SidebarDropdown>-->
        </Sidebar>
    </div>
</template>
<script>
    import Sidebar from "@/re-usables/components/Sidebar.vue";
    import SidebarLink from "@/re-usables/components/SidebarLink.vue";
    import SidebarHeading from "@/re-usables/components/SidebarHeading.vue";
    import SidebarDropdown from "@/re-usables/components/SidebarDropdown.vue";
    import SidebarMenu from "@/re-usables/components/SidebarMenu.vue";
    import {
        UserGroupIcon, Squares2X2Icon, ArrowPathRoundedSquareIcon, DocumentTextIcon, DocumentPlusIcon, DocumentArrowDownIcon, CircleStackIcon,
        DocumentCheckIcon, DocumentChartBarIcon, CalendarDaysIcon, ClockIcon, CurrencyDollarIcon, ArrowsPointingInIcon, ClipboardDocumentCheckIcon, UserPlusIcon,
        GiftTopIcon, ArrowUturnDownIcon,BookOpenIcon, TruckIcon, ReceiptRefundIcon, BanknotesIcon, AdjustmentsVerticalIcon
    } from '@heroicons/vue/24/outline'
    export default {
        components: {
            Sidebar, SidebarMenu, Squares2X2Icon, CircleStackIcon, DocumentPlusIcon, DocumentTextIcon, DocumentArrowDownIcon, ArrowPathRoundedSquareIcon, DocumentCheckIcon, SidebarLink, SidebarDropdown, UserGroupIcon, CurrencyDollarIcon, ArrowsPointingInIcon,
            ClipboardDocumentCheckIcon, UserPlusIcon, GiftTopIcon, ArrowUturnDownIcon, TruckIcon, AdjustmentsVerticalIcon,
            ReceiptRefundIcon, BanknotesIcon, BookOpenIcon, DocumentChartBarIcon, CalendarDaysIcon, SidebarHeading, ClockIcon
        },
    }
</script>