<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh v-if="dimNos[0] != undefined" v-for="dimNo in dimNos" class="hidden md:table-cell">{{$root.DimCaption(dimNo)}}</WTh>
            <WTh>Total Amount (VAT Inc.)</WTh>
            <WTh>Status</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.No">{{record.No}}</WTd>
            <WTd v-if="dimNos[0] != undefined" v-for="dimNo in dimNos" class="hidden md:table-cell">{{record["Shortcut_Dimension_"+dimNo+"_Code"]}}</WTd>
            <WTd>{{parseFloat(record.Amount_Including_VAT).toLocaleString()}}</WTd>
            <WTd>{{record.Status}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Open'">Open <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Pending Approval'">Pending <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Released'">Approved <span class="hidden sm:flex pl-1">Requests</span></MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/purchase-request/PurchaseRequestActions.vue';
    export default {
        components: { Actions, ...W },
        data(){
            return {
                records:[],
                dimNos:[],
                pageProps: {
                    title: 'Purchase Requisition List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'PurchaseRequestHeader',
                    formRoute: "/ess/purchase-request/form",
                    listRoute: "/ess/purchase-request/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true,isNewCaption:"New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true},
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.dimNos = response.dims.dimNos.split(",");
                this.$root.title = this.pageProps.title;
            }
        },
    }
</script>