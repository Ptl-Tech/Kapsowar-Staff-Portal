<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Injection No.</WTh>
            <WTh>Quantity</WTh>
            <WTh>Date</WTh>
            <WTh>Time</WTh>
            <WTh>Remarks</WTh>
            <WTh>Posted</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.InjectionNo}}</WTd>
            <WTd>{{record.Quantity}}</WTd>
            <WTd>{{record.InjectionDate.Day}}-{{record.InjectionDate.Month}}-{{record.InjectionDate.Year}}</WTd>
            <WTd>{{record.InjectionTime}}</WTd>
            <WTd>{{record.InjectionRemarks}}</WTd>
            <WTd>{{record.Posted}}</WTd>
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
    import Actions from '@/modules/hmis/triage/injections/InjectionsLineActions.vue';

    export default {
        props: {props: {default:{}},filter:{default:""} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Patient Injections',
                    pageType:"list",
                    keys: { recKey:'InjectionNo',parentKey:'ObservationNo'},
                    controller:'TriageInjections',
                    formRoute: "/hmis/" + this.$route.params.section +"/injections/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/injections/list",
                    filter:this.filter,
                    props:this.props,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Injection", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
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
