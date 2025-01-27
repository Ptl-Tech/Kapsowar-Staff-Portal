<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>PR No.</WTh>
            <WTh>Document Date</WTh>
            <WTh>Expected Receipt Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Document_Date)}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Requested_Receipt_Date)}}</WTd>
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
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/purchase-request/header/PurchaseRequestHeaderActions.vue';
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
                    title: 'Purchase Request List',
                    pageType:"list",
                    keys: { recKey: 'No' },
                    controller:'PurchaseReqHeader',
                    formRoute: "/ess/purchase-request/header/form",
                    listRoute: "/ess/purchase-request/header/list",
                },
                actionsProps: { isNew: true,isNewCaption:"New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
