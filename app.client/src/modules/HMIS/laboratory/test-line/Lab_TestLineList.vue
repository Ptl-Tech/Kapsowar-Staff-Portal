<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Lab Test Code</WTh>
            <WTh>Lab Test Name</WTh>
            <WTh>Specimen Code</WTh>
            <WTh>Specimen Name</WTh>
            <WTh>Collection Date</WTh>
            <WTh>Collection Time</WTh>
            <WTh>Unit of Measure</WTh>
            <WTh>Count Value</WTh>
            <WTh>Positive</WTh>
            <WTh>Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.LaboratoryTestCode}}</WTd>
            <WTd>{{record.LaboratoryTestName}}</WTd>
            <WTd>{{record.SpecimenCode}}</WTd>
            <WTd>{{record.SpecimenName}}</WTd>
            <WTd>{{$root.cplFnNavDateObjToString(record.CollectionDate)}}</WTd>
            <WTd>{{record.CollectionTime}}</WTd>
            <WTd>{{record.MeasuringUnitName}}</WTd>
            <WTd>{{record.CountValue}}</WTd>
            <WTd>{{record.Positive? 'Yes':'No'}}</WTd>
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
    import Actions from '@/modules/hmis/laboratory/test-line/Lab_TestLineActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Test Lines',
                    pageType:"list",
                    keys: { recKey: 'SystemId', parentKey: 'Laboratory_No' },
                    controller:'Lab_TestLines',
                    formRoute: "/hmis/laboratory/test-line/form",
                    listRoute: "/hmis/laboratory/test-line/list",
                    filter: this.filter,
                },
                actionsProps: { isNew: false,isNewCaption:"New Test", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
