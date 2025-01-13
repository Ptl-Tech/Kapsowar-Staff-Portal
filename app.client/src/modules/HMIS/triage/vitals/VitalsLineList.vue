<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Pulse Rate</WTh>
            <WTh>Pain</WTh>
            <WTh>Blood Pressure</WTh>
            <WTh>Temperature</WTh>
            <WTh>Height</WTh>
            <WTh>Weight</WTh>
            <WTh>BMI</WTh>
            <WTh>SPO2</WTh>
            <WTh>Respiration Rate</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.PulseRate}}</WTd>
            <WTd>{{record.PulseRate}}</WTd>
            <WTd>{{record.Pain}}</WTd>
            <WTd>{{record.BloodPressure}}</WTd>
            <WTd>{{record.Height}}</WTd>
            <WTd>{{record.Weight}}</WTd>
            <WTd>{{Math.round(record.BMI*100)/100}}</WTd>
            <WTd>{{record.SPO2}}</WTd>
            <WTd>{{record.RespirationRate}}</WTd>
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
    import Actions from '@/modules/hmis/triage/vitals/VitalsLineActions.vue';

    export default {
        props: {props: {default:{}},filter:{default:""} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Patient Vitals',
                    pageType:"list",
                    keys: {recKey:'LineNo',parentKey:'ObservationNo'},
                    controller:'TriageVitals',
                    formRoute: "/hmis/" + this.$route.params.section +"/vitals/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/vitals/list",
                    filter:this.filter,
                    props:this.props,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Vitals", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
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
