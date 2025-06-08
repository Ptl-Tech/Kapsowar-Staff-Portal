<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Leave App. No.</WTh>
            <WTh>Leave Type</WTh>
            <WTh>Days Applied</WTh>
            <WTh>Start Date</WTh>
            <WTh>End Date</WTh>
            <WTh>Return to Work Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.Leave_Type}}</WTd>
            <WTd>{{record.Days_Applied}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Start_Date)}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.End_Date)}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Return_to_Work_Date)}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Open'">Open</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Pending Approval'">Pending Approval</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Released'">Approved</MenuLink>
            </MenuTabs>
        </template>
        <template #listMoreActions>
            <WRouterLink :to="'/report-preview?'+encodeURI('src=Reports/LeaveStatementReport')" class="flex items-center !bg-blue-500" title="View"><PrinterIcon class="iconSmall" /><span> Leave Statement</span></WRouterLink>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/leave-application/LeaveHeaderActions.vue';
    import { useRouter } from 'vue-router'
    import { PrinterIcon } from '@heroicons/vue/24/outline';
    export default {
        components: { Actions, ...W, PrinterIcon },
        setup() {
            const router = useRouter();
            return { router };
        },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Leave Applicatoions List',
                    pageType: "list",
                    keys: { recKey: 'Document_No' },
                    controller: 'LeaveApplicationHeader',
                    formRoute: "/ess/leave-application/form",
                    listRoute: "/ess/leave-application/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true, isNewCaption: "New Leave", isEdit: true, isDelete: false, isFilter: true, isExport: true,moreActions:true },
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
            },
        }
    }
</script>
