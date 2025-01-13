<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Injection No</WTh>
            <WTh>Item No.</WTh>
            <WTh>Location</WTh>
            <WTh>Injection Name</WTh>
            <WTh>Injection Given</WTh>
            <WTh>Unit of Measure</WTh>
            <WTh>Quantity</WTh>
            <WTh>Duration</WTh>
            <WTh>Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&recKey2='+record[pageProps.keys.recKey2]+'&subPart2='+pageProps.name">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.ItemNo}}</WTd>
            <WTd>{{record.Location}}</WTd>
            <WTd>{{record.InjectionName}}</WTd>
            <WTd>{{record.InjectionGiven}}</WTd>
            <WTd>{{record.InjectionUnitofMeasure}}</WTd>
            <WTd>{{record.InjectionQuantity}}</WTd>
            <WTd>{{record.Duration}}</WTd>
            <WTd>{{record.InjectionRemarks}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]+'#'+record[pageProps.keys.recKey2]">
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
    import Actions from '@/modules/hmis/consultation/injections/CLT_InjectionsActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Injections List',
                    pageType:"list",
                    keys: { recKey: 'InjectionNo', recKey2: 'ItemNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Injections',
                    formRoute: "/hmis/" + this.$route.params.section +"/clt-injections/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/clt-injections/list",
                    filter: this.filter,
                    name:"consultation-injections",
                },
                actionsProps: { isNew: true,isNewCaption:"Add Injection", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
