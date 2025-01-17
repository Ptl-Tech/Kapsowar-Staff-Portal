<template>
    <h4 class="font-semibold py-2">Staff Claim Lines</h4>
    <div v-if="header.Approval_Status != undefined && header.Approval_Status == 'Open'">
        <WRouterLink :to="pageProps.formRoute+'/create/'+docNo+'/0'" class="flex items-center !bg-blue-500" title="Edit"><PlusIcon class="iconSmall" />Add Staff Claim Line</WRouterLink>
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
            <WTh>Amount (LCY)</WTh>
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
                <WTd class="font-semibold">Totals</WTd>
                <WTd class="font-semibold">{{linesTotal.toLocaleString()}}</WTd>
                <WTd class="font-semibold"></WTd>
            </WTr>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/staff-claim/ClaimLineActions.vue';
    export default {
        props: { docNo: {default:""} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Staff Claim List',
                    pageType: "list",
                    formRoute:"/ess/staff-claim/line/form",
                    pKey: 'No',
                    pKeyHeader: 'Advance_Request_Hdr_No',
                    controller:'StaffClaimLine',
                    docNo:this.docNo
                },
                lineAction:"",
                lineNo:0,
                actionsProps: { isNew: false,isNewCaption:"Add Staff Claim Line", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                isLineModal: false,
                header: {},
                linesTotal: 0,
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
                        this.linesTotal = this.linesTotal + parseFloat(this.records[i].Amount_LCY);
                    }
                }
            }
        },
    }
</script>