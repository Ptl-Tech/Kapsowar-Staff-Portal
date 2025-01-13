<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>System</WTh>
            <WTh>Symptom Code</WTh>
            <WTh>Description</WTh>
            <WTh>Duration</WTh>
            <WTh>Characteristics</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&recKey2='+record[pageProps.keys.recKey2]+'&subPart2=consultation-symptoms'">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.SymptomCode}}</WTd>
            <WTd>{{record.Description}}</WTd>
            <WTd>{{record.Duration}}</WTd>
            <WTd>{{record.Characteristics}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]+'#'+record[pageProps.keys.recKey2]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/consultation/symptoms/ConsultationSymptomsActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Consultation Symptoms',
                    pageType:"list",
                    keys: { recKey: 'System', recKey2: 'SymptomCode',parentKey:'TreatmentNo'},
                    controller:'ConsultationSymptoms',
                    formRoute: "/hmis/" + this.$route.params.section +"/symptoms/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/symptoms/list",
                    filter:this.filter,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Symptoms", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
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
