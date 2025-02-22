<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Petty Cash No.</WTh>
            <WTh>Date</WTh>
            <WTh>Staff No</WTh>
            <WTh>Total Net Amount</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Date)}}</WTd>
            <WTd>{{record.PF_No}}</WTd>
            <WTd>{{record.Total_Net_Amount}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Pending'">Open</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Pending Approval'">Pending Approval</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Approved'">Approved</MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/petty-cash/header/PettyCashHeaderActions.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { Actions, ...W },
        setup() {
            const router = useRouter();
            return { router };
        },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Petty Cash List',
                    pageType:"list",
                    keys: { recKey: 'No' },
                    controller:'PettyCashHeader',
                    formRoute: "/ess/petty-cash/header/form",
                    listRoute: "/ess/petty-cash/header/list",
                },
                actionsProps: { isNew: true,isNewCaption:"New Petty Cash", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
