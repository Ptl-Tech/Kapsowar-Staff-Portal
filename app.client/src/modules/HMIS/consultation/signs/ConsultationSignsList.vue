<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>System</WTh>
            <WTh>Sign Code</WTh>
            <WTh>Sign Description</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&recKey2='+record[pageProps.keys.recKey2]+'&subPart2=consultation-signs'">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.SignCode}}</WTd>
            <WTd>{{record.SignDescription}}</WTd>
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
    import Actions from '@/modules/hmis/consultation/signs/ConsultationSignsActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Consultation Signs',
                    pageType:"list",
                    keys: { recKey: 'System', recKey2: 'SignCode',parentKey:'TreatmentNo'},
                    controller:'ConsultationSigns',
                    formRoute: "/hmis/" + this.$route.params.section +"/signs/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/signs/list",
                    filter:this.filter,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Signs", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
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
