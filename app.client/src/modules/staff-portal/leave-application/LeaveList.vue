<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #navigationTabs>
            <MenuTabs v-if="this.$route.fullPath.includes('/ess/leave/list?status=All-')">
                <MenuLink linkTo="/ess/leave/list?status=All-Open">Open <span class="hidden sm:flex pl-1">Applications</span></MenuLink>
                <MenuLink linkTo="/ess/leave/list?status=All-Approval Pending">Pending <span class="hidden sm:flex pl-1">Applications</span></MenuLink>
                <MenuLink linkTo="/ess/leave/list?status=All-Approved">Approved <span class="hidden sm:flex pl-1">Applications</span></MenuLink>
            </MenuTabs>
            <MenuTabs v-else>
                <MenuLink linkTo="/ess/leave/list?status=Open">Open <span class="hidden sm:flex pl-1">Applications</span></MenuLink>
                <MenuLink linkTo="/ess/leave/list?status=Approval Pending">Pending <span class="hidden sm:flex pl-1">Applications</span></MenuLink>
                <MenuLink linkTo="/ess/leave/list?status=Approved">Approved <span class="hidden sm:flex pl-1">Applications</span></MenuLink>
            </MenuTabs>
        </template>
        <template #thead>
            <WTh>Leave No.</WTh>
            <WTh class="hidden md:flex">Document Date</WTh>
            <WTh>Leave Type</WTh>
            <WTh>Start Date</WTh>
            <WTh class="hidden md:flex">End Date</WTh>
            <WTh>Leave Days</WTh>
            <WTh>Status</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.No">{{record.No}}</WTd>
            <WTd class="hidden md:flex">{{record.Document_Date.split('T')[0]}}</WTd>
            <WTd>{{record.Cause_of_Absence_Desc}}</WTd>
            <WTd>{{record.Start_Date.split('T')[0]}}</WTd>
            <WTd class="hidden md:flex">{{record.End_Date.split('T')[0]}}</WTd>
            <WTd>{{record.Quantity}}</WTd>
            <WTd>{{record.Approvals_Status}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
</ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/leave-application/LeaveActions.vue';
    export default {
        components: { Actions, ...W },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Leave Applications List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'LeaveApplication',
                    formRoute: "/ess/leave/form",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true, isNewCaption: "New Leave", isEdit: true, isDelete: false, isFilter: true, isExport: true },
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
            }
        },
    }
</script>