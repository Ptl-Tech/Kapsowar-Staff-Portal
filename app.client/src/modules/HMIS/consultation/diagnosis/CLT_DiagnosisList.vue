<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Diagnosis No.</WTh>
            <WTh>Diagnosis Code</WTh>
            <WTh>Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&subPart2='+pageProps.name">{{record.DiagnosisNo}}</WTd>
            <WTd>{{record.DiagnosisCode}}</WTd>
            <WTd>{{record.Remarks}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.parentKey]+'#'+record[pageProps.keys.recKey]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/consultation/diagnosis/CLT_DiagnosisActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Diagnosis',
                    pageType:"list",
                    keys: { recKey: 'DiagnosisNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Diagnosis',
                    formRoute: "/hmis/" + this.$route.params.section +"/diagnosis/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/diagnosis/list",
                    filter: this.filter,
                    name:'consultation-diagnosis',
                },
                actionsProps: { isNew: true,isNewCaption:"Add Diagnosis", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
