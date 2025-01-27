<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Type</WTh>
            <WTh>Issuing Store</WTh>
            <WTh>No.</WTh>
            <WTh>Description</WTh>
            <WTh>Quantity Requested</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.Type}}</WTd>
            <WTd>{{record.Issuing_Store}}</WTd>
            <WTd>{{record.No}}</WTd>
            <WTd>{{record.Description}}</WTd>
            <WTd>{{record.Quantity_Requested}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.parentKey]+'#'+record[pageProps.keys.recKey]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #summaryRow>
            <tr class="bg-yellow-100">
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd class="font-semibold text-right">TOTALS : </WTd>
                <WTd class="font-semibold">{{quantityTotal}}</WTd>
                <WTd></WTd>
            </tr>
        </template>
        <template #listMoreActions>
            <Actions :header="header" :actionsProps="{scope:'multipleRecords'}" />
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/store-request/line/StoreRequestLineActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Store Request Lines',
                    pageType: "list",
                    keys: { recKey: 'Line_No', parentKey: 'Requistion_No' },
                    controller: 'StoreReqLine',
                    formRoute: "/ess/store-request/line/form",
                    listRoute: "/ess/store-request/line/list",
                    filter: this.filter,
                },
                actionsProps: { isNew: true, isNewCaption: "New Line", isEdit: false, isDelete: false, isSearch: false, isFilter: false, isExport: false, isOrderBy: false, moreActions: true },
                header: {},
                quantityTotal: 0,
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
                if (this.records != null) {
                    for (var i = 0; i < this.records.length; i++) {
                        this.quantityTotal = this.quantityTotal + parseFloat(this.records[i].Quantity);
                    }
                }
            }
        },
    }
</script>
