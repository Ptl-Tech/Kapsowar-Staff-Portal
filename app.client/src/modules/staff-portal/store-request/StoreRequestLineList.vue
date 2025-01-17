<template>
    <h4 class="font-semibold py-2">Store Request Lines</h4>
    <div v-if="isNew && header.Status != undefined && header.Status == 'Open'">
        <WRouterLink :to="pageProps.formRoute+'/create/'+docNo+'/0'" class="flex items-center !bg-blue-500" title="Edit"><PlusIcon class="iconSmall" />New Line</WRouterLink>
    </div>
    <ListPartPageTemplate ref="tpList" :urlQuery="`?docNo=${docNo}`" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Line No.</WTh>
            <WTh>LPO/PR No.</WTh>
            <WTh>Type</WTh>
            <WTh>Item No.</WTh>
            <WTh>Description</WTh>
            <WTh>Quantity</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.Document_No+'/'+record.Line_No">{{record.Line_No}}</WTd>
            <WTd>{{record.LPO_No}}</WTd>
            <WTd>{{record.Type}}</WTd>
            <WTd>{{record.No}}</WTd>
            <WTd>{{record.Description}}</WTd>
            <WTd>{{record.Qty_Requested}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.Line_No">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/store-request/StoreRequestLineActions.vue';
    export default {
        props: { docNo: { default: "" }, isNew: {default:true} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Store Request Lines List',
                    pageType:"list",
                    pKey: 'No',
                    pKeyHeader: 'Line_No',
                    controller:'StoreRequestLine',
                    docNo: this.docNo,
                    formRoute: "/ess/store-request/line/form",
                },
                lineAction:"",
                lineNo:0,
                actionsProps: { isNew: false,isNewCaption:"New Line", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                isLineModal:false,
                header:{}
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
            }
        },
    }
</script>