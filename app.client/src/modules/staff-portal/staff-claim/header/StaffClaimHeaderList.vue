<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Claim No.</WTh>
            <WTh>Date</WTh>
            <WTh>Payee</WTh>
            <WTh>Total Amount</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Date)}}</WTd>
            <WTd>{{record.Payee}}</WTd>
            <WTd>{{record.Total_Net_Amount_LCY}}</WTd>
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
    import Actions from '@/modules/staff-portal/staff-claim/header/StaffClaimHeaderActions.vue';
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
                    title: 'Imprests List',
                    pageType:"list",
                    keys: { recKey: 'No' },
                    controller:'StaffClaimHeader',
                    formRoute: "/ess/staff-claim/header/form",
                    listRoute: "/ess/staff-claim/header/list",
                },
                actionsProps: { isNew: true,isNewCaption:"New Claim", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
