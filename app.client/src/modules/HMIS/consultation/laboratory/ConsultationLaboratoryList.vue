<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Test Package Code</WTh>
            <!--<WTh>Package Name</WTh>-->
            <WTh>Due Date</WTh>
            <WTh>Results</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&subPart2=consultation-laboratory'">{{record.LaboratoryTestPackageCode}}</WTd>
            <!--<WTd>{{record.LaboratoryTestPackageName}}</WTd>-->
            <WTd>{{record.DateDue.Day}}-{{record.DateDue.Month}}-{{record.DateDue.Year}}</WTd>
            <WTd>{{record.Results}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #listMoreActions>
            <Actions :header="header" :actionsProps="{scope:'multipleRecords'}"/>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/consultation/laboratory/ConsultationLaboratoryActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Laboratory',
                    pageType:"list",
                    keys: { recKey: 'LineNo',parentKey:'TreatmentNo'},
                    controller:'ConsultationLaboratory',
                    formRoute: "/hmis/" + this.$route.params.section +"/laboratory/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/laboratory/list",
                    filter:this.filter,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Lab Test", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
