<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Account No.</WTh>
            <WTh>Account Name</WTh>
            <WTh>Issued Amount</WTh>
            <WTh>Amount Spent</WTh>
            <WTh>Cash Receipt No.</WTh>
            <WTh>Cash Receipt Amount</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.Account_No}}</WTd>
            <WTd>{{record.Account_Name}}</WTd>
            <WTd>{{record.Amount}}</WTd>
            <WTd>{{record.Actual_Spent}}</WTd>
            <WTd>{{record.Cash_Receipt_No}}</WTd>
            <WTd>{{record.Cash_Receipt_Amount}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.parentKey]+'#'+record[pageProps.keys.recKey]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #summaryRow>
            <tr class="bg-yellow-100">
                <WTd></WTd>
                <WTd class="font-semibold text-right">TOTALS : </WTd>
                <WTd class="font-semibold">{{issuedAmountTotal.toLocaleString()}}</WTd>
                <WTd class="font-semibold">{{spentAmountTotal.toLocaleString()}}</WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
            </tr>
            <tr class="bg-yellow-100">
                <WTd></WTd>
                <WTd class="font-semibold text-right">BALANCE : </WTd>
                <WTd class="font-semibold">{{balanceAmount.toLocaleString()}}</WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
                <WTd></WTd>
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
    import Actions from '@/modules/staff-portal/imprest-surrender/line/ImprestSurrenderLineActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Surrender Lines',
                    pageType: "list",
                    keys: { recKey: 'Line_No', parentKey: 'Surrender_Doc_No' },
                    controller: 'ImprestSurrenderLine',
                    formRoute: "/ess/imprest-surrender/line/form",
                    listRoute: "/ess/imprest-surrender/line/list",
                    filter: this.filter,
                },
                actionsProps: { isNew: false, isNewCaption: "New Line", isEdit: false, isDelete: false, isSearch: false, isFilter: false, isExport: false, isOrderBy: false, moreActions: true },
                header: {},
                issuedAmountTotal: 0,
                spentAmountTotal: 0,
                balanceAmount: 0,
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
                if (this.records != null) {
                    for (var i = 0; i < this.records.length; i++) {
                        this.issuedAmountTotal = this.issuedAmountTotal + parseFloat(this.records[i].Amount);
                        this.spentAmountTotal = this.spentAmountTotal + parseFloat(this.records[i].Actual_Spent);
                    }
                    this.balanceAmount = this.issuedAmountTotal - this.spentAmountTotal;
                }
            }
        },
    }
</script>
