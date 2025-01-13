<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Radiology Type Code</WTh>
            <WTh>Radiology Name</WTh>
            <WTh>Due Date</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&subPart2='+pageProps.name">{{record.RadiologyTypeCode}}</WTd>
            <WTd>{{record.RadiologyTypeName}}</WTd>
            <WTd>{{record.DateDue.Day}}-{{record.DateDue.Month}}-{{record.DateDue.Year}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.parentKey]+'#'+record[pageProps.keys.recKey]">
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
    import Actions from '@/modules/hmis/consultation/radiology/CLT_RadiologyActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Radiology',
                    pageType:"list",
                    keys: { recKey: 'RadiologyTypeCode',parentKey:'TreatmentNo'},
                    controller:'ConsultationRadiology',
                    formRoute: "/hmis/" + this.$route.params.section +"/radiology/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/radiology/list",
                    filter: this.filter,
                    name:'consultation-radiology',
                },
                actionsProps: { isNew: true,isNewCaption:"Add Radiology", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
