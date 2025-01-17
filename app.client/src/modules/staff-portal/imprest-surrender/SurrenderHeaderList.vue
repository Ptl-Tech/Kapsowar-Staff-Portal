<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Mission Start Date</WTh>
            <WTh>Mission Return Date</WTh>
            <WTh>Mission Summary</WTh>
            <WTh>Rejection Comment</WTh>
            <WTh>Status</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.No">{{record.No}}</WTd>
            <WTd>{{record.Date_of_Request.split('T')[0]}}</WTd>
            <WTd>{{record.Date_Due.split('T')[0]}}</WTd>
            <WTd>{{record.Mission_Summary.length > 100? record.Mission_Summary.substring(0,70)+'...':record.Mission_Summary}}</WTd>
            <WTd>{{record.Rejection_Comment}}</WTd>
            <WTd>{{record.Approval_Status}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink linkTo="/ess/staff-advance-surrender/list?status=Open">Open <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
                <MenuLink linkTo="/ess/staff-advance-surrender/list?status=Pending Approval">Pending <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
                <MenuLink linkTo="/ess/staff-advance-surrender/list?status=Released">Approved <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/staff-advance/AdvanceActions.vue';
    export default {
        components: { Actions, ...W },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Advance Surrender List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'StaffAdvanceSurrender',
                    formRoute: "/ess/staff-advance-surrender/form",
                    listRoute: "/ess/staff-advance-surrender/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true,isNewCaption:"New Surrender", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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