<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
      <template #thead>
        <WTh>Visit No.</WTh>
        <WTh>Name</WTh>
        <WTh>ID Number</WTh>
        <WTh>Category</WTh>
        <WTh>Purpose</WTh>
        <WTh>Date</WTh>
        <WTh>Status</WTh>
        <WTh>Actions</WTh>
      </template>
      <template v-for="(record,index) in records" #[`tbody-${index}`]>
          <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
        <WTd>{{record.VisitorName}}</WTd>
        <WTd>{{record.IDNumber}}</WTd>
        <WTd>{{record.VisitorCategory}}</WTd>
        <WTd>{{record.PurposeofVisit}}</WTd>
        <WTd>{{record.InitiatedDate.Day}}-{{record.InitiatedDate.Month}}-{{record.InitiatedDate.Year}}</WTd>
        <WTd>{{record.Status}}</WTd>
        <WTd>
          <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
            <Actions :record="record" :pageProps="pageProps" />
          </EllipsisMenu>
        </WTd>
      </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Active-Today'">Active Today</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Active-All'">Active All</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Cleared'">Cleared</MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/visitor/VisitorHeaderActions.vue';
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
                    title: 'Visitors List',
                    pageType:"list",
                    keys: {recKey:'No'},
                    controller:'VisitorsHeader',
                    formRoute: "/hmis/"+this.$route.params.section+"/visitor/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/visitor/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true,isNewCaption:"New Visitor", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
