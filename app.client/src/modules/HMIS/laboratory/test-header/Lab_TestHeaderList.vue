<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Test No.</WTh>
            <WTh>Link Type</WTh>
            <WTh>Link No.</WTh>
            <WTh>Date</WTh>
            <WTh>Patient No.</WTh>
            <WTh>Surname</WTh>
            <WTh>Middle Name</WTh>
            <WTh>Last Name</WTh>
            <WTh>ID Number</WTh>
            <WTh>Status</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.LinkType}}</WTd>
            <WTd>{{record.LinkNo}}</WTd>
            <WTd>{{$root.cplFnNavDateObjToString(record.LaboratoryDate)}}</WTd>
            <WTd>{{record.PatientNo}}</WTd>
            <WTd>{{record.Surname}}</WTd>
            <WTd>{{record.MiddleName}}</WTd>
            <WTd>{{record.LastName}}</WTd>
            <WTd>{{record.IDNumber}}</WTd>
            <WTd>{{record.Status}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Pending-Today'">Pending Tests (Today)</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Completed-Today'">Completed Tests (Today)</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=All'">All Tests</MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/laboratory/test-header/Lab_TestHeaderActions.vue';
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
                    title: 'Lab Tests List',
                    pageType:"list",
                    keys: { recKey: 'LaboratoryNo' },
                    controller:'Lab_TestHeaders',
                    formRoute: "/hmis/laboratory/test-header/form",
                    listRoute: "/hmis/laboratory/test-header/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: false,isNewCaption:"New Test", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
