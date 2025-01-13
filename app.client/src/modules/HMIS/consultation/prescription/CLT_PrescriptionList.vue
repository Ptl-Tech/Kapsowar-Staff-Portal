<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Drug Group</WTh>
            <WTh>Drug No.</WTh>
            <WTh>Drug Name</WTh>
            <WTh>Quantity</WTh>
            <WTh>Unit of Measure</WTh>
            <WTh>Dosage</WTh>
            <WTh>Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&subPart2='+pageProps.name">{{record.ProductGroup}}</WTd>
            <WTd>{{record.DrugNo}}</WTd>
            <WTd>{{record.DrugName}}</WTd>
            <WTd>{{record.Quantity}}</WTd>
            <WTd>{{record.UnitOfMeasure}}</WTd>
            <WTd>{{record.Dosage}}</WTd>
            <WTd>{{record.Remarks}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.parentKey]+'#'+record[pageProps.keys.recKey]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #listMoreActions>
            <Actions :header="header" :actionsProps="{scope:'multipleRecords'}" />
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/consultation/prescription/CLT_PrescriptionActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Prescriptions List',
                    pageType:"list",
                    keys: { recKey: 'LlineNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Prescriptions',
                    formRoute: "/hmis/" + this.$route.params.section +"/clt-prescription/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/clt-prescription/list",
                    filter: this.filter,
                    name:'consultation-prescriptions',
                },
                actionsProps: { isNew: true,isNewCaption:"Add Prescription", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
                header: {}
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
