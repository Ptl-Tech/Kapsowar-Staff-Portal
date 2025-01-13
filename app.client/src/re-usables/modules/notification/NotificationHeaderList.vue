<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Notification Subject</WTh>
            <WTh class="hidden sm:flex">Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/view/'+record.No">{{record.Subject}}</WTd>
            <WTd class="hidden sm:flex">{{record.Date_Published.split("T")[0]}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=not-viewed'">Not Viewed</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=viewed'">Viewed</MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
  import Actions from '@/re-usables/modules/notification/NotificationHeaderActions.vue';
    export default {
        components: { Actions, ...W },
        data(){
            return {
                records: [],
                pageProps: {
                    title: 'Notification List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'NotificationHeader',
                    formRoute: "/hmis/notification/form",
                    listRoute: "/hmis/notification/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: false, isNewCaption: "New Notification", isEdit: false, isDelete: false, isFilter: false, isExport: false },
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
