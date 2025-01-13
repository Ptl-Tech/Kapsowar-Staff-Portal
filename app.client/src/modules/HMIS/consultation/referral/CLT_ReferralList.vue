<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Date Referred</WTh>
            <WTh>Hospital No.</WTh>
            <WTh>Hospital Name</WTh>
            <WTh>Contact Person</WTh>
            <WTh>Referral Reason</WTh>
            <WTh>Referral Remarks</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]+'&subPart2='+pageProps.name">{{$root.cplFnNavDateObjToString(record.DateReferred)}}</WTd>
            <WTd>{{record.HospitalNo}}</WTd>
            <WTd>{{record.HospitalName}}</WTd>
            <WTd>{{record.Contactperson}}</WTd>
            <WTd>{{record.ReferralReason}}</WTd>
            <WTd>{{record.ReferralRemarks}}</WTd>
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
    import Actions from '@/modules/hmis/consultation/referral/CLT_ReferralActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Referral List',
                    pageType:"list",
                    keys: { recKey: 'TreatmentNo',parentKey:'TreatmentNo'},
                    controller:'CLT_Referrals',
                    formRoute: "/hmis/" + this.$route.params.section +"/clt-referral/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/clt-referral/list",
                    filter: this.filter,
                    name:'consultation-referral',
                },
                actionsProps: { isNew: true, isNewCaption:"Add Referral", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false,moreActions:true},
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
