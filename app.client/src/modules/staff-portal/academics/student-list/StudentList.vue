<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Name</WTh>
            <WTh>Balance</WTh>
            <WTh>Phone No.</WTh>
            <WTh>Gender</WTh>
            <WTh>ID Number</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd>{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.Name}}</WTd>
            <WTd>{{record.Balance}}</WTd>
            <WTd>{{record.Phone_No}}</WTd>
            <WTd>{{record.Gender}}</WTd>
            <WTd>{{record.ID_No}}</WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Current'">Current</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Registration'">Registration</MenuLink>
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
                    title: 'Students List',
                    pageType:"list",
                    keys: { recKey: 'No' },
                    controller:'Students',
                    formRoute: "/ess/academics/students-list/form",
                    listRoute: "/ess/academics/students-list/list",
                },
                actionsProps: { isNew: false,isNewCaption:"New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
