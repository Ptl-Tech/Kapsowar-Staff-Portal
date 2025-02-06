<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Code</WTh>
            <WTh>Description</WTh>
            <WTh>Category</WTh>
            <WTh>Department Code</WTh>
            <WTh>Student Registered</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd>{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.Description}}</WTd>
            <WTd>{{record.Category}}</WTd>
            <WTd>{{record.Global_Dimension_2_Code}}</WTd>
            <WTd>{{record.Student_Registered}}</WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?category=All'">All</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?category=Certificate'">Certificate</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?category=Diploma'">Diploma</MenuLink>
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
                    title: 'Programmes List',
                    pageType:"list",
                    keys: { recKey: 'Code' },
                    controller:'Programmes',
                    formRoute: "/ess/academics/programme/form",
                    listRoute: "/ess/academics/programme/list",
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
