<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Request Start Date</WTh>
            <WTh>Request End Date</WTh>
            <WTh>Vehicle Class Requested</WTh>
            <WTh>Requester</WTh>
            <WTh>Rejection Comment</WTh>
            <WTh>Driver Assigned</WTh>
            <WTh>Status</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.No">{{record.No}}</WTd>
            <WTd>{{record.Request_Start_Date.split('T')[0]}}</WTd>
            <WTd>{{record.Request_End_Date.split('T')[0]}}</WTd>
            <WTd>{{record.Vehicle_Class_Requested}}</WTd>
            <WTd>{{record.Requester_Name != ""? record.Requester_Name:record.Requester}}</WTd>
            <WTd>{{record.Rejection_Comment}}</WTd>
            <WTd>{{record.Driver_Assigned}}</WTd>
            <WTd>{{record.Approval_Status}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs v-if="this.$route.fullPath == '/ess/transport-request/list?status=Open' || this.$route.fullPath == '/ess/transport-request/list?status=Pending Approval' || this.$route.fullPath == '/ess/transport-request/list?status=Released'">
                <MenuLink linkTo="/ess/transport-request/list?status=Open">Open <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
                <MenuLink linkTo="/ess/transport-request/list?status=Pending Approval">Pending <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
                <MenuLink linkTo="/ess/transport-request/list?status=Released">Approved <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/transport-request/TransportActions.vue';
    export default {
        components: { Actions, ...W },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Transport Request List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'TransportRequest',
                    formRoute: "/ess/transport-request/form",
                    listRoute: "/ess/transport-request/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true,isNewCaption:"New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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