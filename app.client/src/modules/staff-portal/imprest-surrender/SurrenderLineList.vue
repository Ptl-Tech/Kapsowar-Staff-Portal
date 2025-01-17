<template>
    <h4 class="font-semibold py-2">Advance Surrender Lines</h4>
    <div v-if="header.Approval_Status != undefined && header.Approval_Status == 'Open'">
        <WRouterLink :to="'/ess/staff-advance-surrender/line/form/create/'+docNo+'/0'" class="flex items-center !bg-blue-500" title="Edit"><PlusIcon class="iconSmall" />Advance Surrender Line</WRouterLink>
    </div>
    <ListPartPageTemplate ref="tpList" :urlQuery="`?docNo=${docNo}`" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Item Description</WTh>
            <WTh v-if="dimNos[0] != undefined" v-for="dimNo in dimNos">{{$root.DimCaption(dimNo)}}</WTh>
            <WTh>Purpose</WTh>
            <WTh>Unit of Measure</WTh>
            <WTh>Unit Cost</WTh>
            <WTh>No. of Units</WTh>
            <WTh>Currency</WTh>
            <WTh>Advance Amount</WTh>
            <WTh>Advanced Amount LCY</WTh>
            <WTh>Amount LCY</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.Advance_Request_Hdr_No+'/'+record.No">{{record.Item_Description}}</WTd>
            <WTd v-if="dimNos[0] != undefined" v-for="dimNo in dimNos">{{record["Shortcut_Dimension_"+dimNo+"_Code"]}}</WTd>
            <WTd>{{record.Purpose}}</WTd>
            <WTd>{{record.Unit_Of_Measure}}</WTd>
            <WTd>{{record.Unit_Cost}}</WTd>
            <WTd>{{record.No_of_Units}}</WTd>
            <WTd>{{record.Currency}}</WTd>
            <WTd>{{record.Advanced_Amount}}</WTd>
            <WTd>{{record.Advanced_Amount_LCY}}</WTd>
            <WTd>{{record.Amount_LCY}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :header="header" :pageProps="pageProps" @onAction="onAction($event)" @closeModal="onCloseModal()" />
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
                <WTd class="font-semibold">Totals</WTd>
                <WTd class="font-semibold">{{totalAdvancedAmount.toLocaleString()}}</WTd>
                <WTd class="font-semibold">{{totalActualAmount.toLocaleString()}}</WTd>
            </WTr>
            <WTr>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd class="font-semibold">Balance</WTd>
                <WTd class="font-semibold">{{(totalAdvancedAmount-totalActualAmount).toLocaleString()}}</WTd>
                <WTd></WTd>
                <WTd></WTd>
            </WTr>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/staff-advance-surrender/SurrenderLineActions.vue';
    export default {
        props: { docNo: { default: "" }, isNew: {default:true} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Advance Surrender List',
                    pageType:"list",
                    pKey: 'No',
                    pKeyHeader: 'Advance_Request_Hdr_No',
                    controller: 'StaffAdvanceSurrenderLine',
                    formRoute: "/ess/staff-advance-surrender/line/form",
                    docNo:this.docNo
                },
                lineAction:"",
                lineNo:0,
                actionsProps: { isNew: false,isNewCaption:"Add Advance Surrender Line", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                isLineModal: false,
                header: {},
                totalAdvancedAmount: 0,
                totalActualAmount: 0,
                dimNos: [],
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
                this.dimNos = response.dims.dimNos.split(",");
                if (this.records != null) {
                    for (var i = 0; i < this.records.length; i++) {
                        this.totalAdvancedAmount = this.totalAdvancedAmount + parseFloat(this.records[i].Advanced_Amount_LCY);
                        this.totalActualAmount = this.totalActualAmount + parseFloat(this.records[i].Amount_LCY);
                    }
                }
            }
        },
    }
</script>