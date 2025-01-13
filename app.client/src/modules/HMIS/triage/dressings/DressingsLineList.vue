<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Process No.</WTh>
            <WTh>Item No.</WTh>
            <WTh>Unit of Measure</WTh>
            <WTh>Quantity</WTh>
            <WTh>Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.ProcessNo}}</WTd>
            <WTd>{{record.ItemNo}}</WTd>
            <WTd>{{record.UnitofMeasure}}</WTd>
            <WTd>{{record.Quantity}}</WTd>
            <WTd>{{record.Remarks}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/triage/dressings/DressingsLineActions.vue';

    export default {
        props: {props: {default:{}},filter:{default:""} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Patient Dressings',
                    pageType:"list",
                    controller: 'TriageDressings',
                    keys: { recKey: 'ProcessNo', parentKey: 'ObservationNo' },
                    formRoute: "/hmis/" + this.$route.params.section + "/dressings/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/dressings/list",
                    filter:this.filter,
                    props:this.props,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Dressing", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                lineAction:"",
                lineNo:0,
                isLineModal: false,
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
