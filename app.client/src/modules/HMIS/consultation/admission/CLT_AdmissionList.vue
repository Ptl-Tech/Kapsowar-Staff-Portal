<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Date of Admission</WTh>
            <WTh>Admission Reason</WTh>
            <WTh>Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&subPart2='+pageProps.name">{{$root.cplFnNavDateObjToString(record.DateOfAdmission)}}</WTd>
            <WTd>{{record.AdmissionReason}}</WTd>
            <WTd>{{record.AdmissionRemarks}}</WTd>
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
    import Actions from '@/modules/hmis/consultation/admission/CLT_AdmissionActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Admission List',
                    pageType:"list",
                    keys: { recKey: 'TreatmentNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Admissions',
                    formRoute: "/hmis/" + this.$route.params.section +"/clt-admission/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/clt-admission/list",
                    filter: this.filter,
                    name:'consultation-admissions',
                },
                actionsProps: { isNew: true,isNewCaption:"Add Admission", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
