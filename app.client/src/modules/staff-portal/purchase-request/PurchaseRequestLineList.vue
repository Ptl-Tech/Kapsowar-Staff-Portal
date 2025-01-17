<template>
    <h4 class="font-semibold py-2">Purchase Request Lines</h4>
    <div v-if="isNew && header.Status != undefined && header.Status == 'Open'">
        <WRouterLink :to="pageProps.formRoute+'/create/'+docNo+'/0'" class="flex items-center !bg-blue-500" title="Edit"><PlusIcon class="iconSmall" />New Line</WRouterLink>
    </div>
    <ListPartPageTemplate ref="tpList" :urlQuery="`?docNo=${docNo}`" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Line No.</WTh>
            <WTh>Type</WTh>
            <WTh>No</WTh>
            <WTh>Description</WTh>
            <WTh>Specifications</WTh>
            <WTh>Location</WTh>
            <WTh>Unit of Meas.</WTh>
            <WTh>Unit Cost</WTh>
            <WTh>Quantity</WTh>
            <WTh>Amount (VAT Inc.)</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.Document_No+'/'+record.Line_No">{{record.Line_No}}</WTd>
            <WTd>{{record.Type}}</WTd>
            <WTd>{{record.No}}</WTd>
            <WTd>{{record.Description}}</WTd>
            <WTd>{{record.Description_2}}</WTd>
            <WTd>{{record.Location_Code}}</WTd>
            <WTd>{{record.Unit_of_Measure_Code}}</WTd>
            <WTd>{{record.Direct_Unit_Cost}}</WTd>
            <WTd>{{record.Quantity}}</WTd>
            <WTd>{{record.Amount_Including_VAT}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.Line_No">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #totalsRow>
            <WTr>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd class="font-semibold">TOTALS</WTd>
                <WTd class="font-semibold">{{linesTotal.toLocaleString()}}</WTd>
                <WTd class="font-semibold"></WTd>
            </WTr>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/purchase-request/PurchaseRequestLineActions.vue';
    export default {
        props: { docNo: { default: "" }, isNew: {default:true} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Purchase Request Lines List',
                    pageType:"list",
                    pKey: 'No',
                    pKeyHeader: 'Line_No',
                    controller:'PurchaseRequestLine',
                    docNo: this.docNo,
                    formRoute: "/ess/purchase-request/line/form",
                },
                lineAction:"",
                lineNo:0,
                actionsProps: { isNew: false,isNewCaption:"New Line", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                isLineModal: false,
                header: {},
                linesTotal:0
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
                if (this.records != null) {
                    for (var i = 0; i < this.records.length; i++) {
                        this.linesTotal = this.linesTotal + parseFloat(this.records[i].Amount_Including_VAT);
                    }
                }
            }
        },
    }
</script>