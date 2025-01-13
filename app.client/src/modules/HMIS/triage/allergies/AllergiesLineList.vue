<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
      <template #thead>
        <WTh>Complaints</WTh>
        <WTh>Reason for Visit</WTh>
        <WTh>Food Allergy</WTh>
        <WTh>Drug Allergy</WTh>
        <WTh>Assessed By</WTh>
        <WTh>Actions</WTh>
      </template>
      <template v-for="(record,index) in records" #[`tbody-${index}`]>
          <WTd :linkTo="pageProps.formRoute+'/edit/?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.Complaints}}</WTd>
          <WTd>{{record.ReasonforRevisit}}</WTd>
          <WTd>{{record.FoodAllergy}}</WTd>
          <WTd>{{record.DrugAllergy}}</WTd>
          <WTd>{{record.AssessedBy}}</WTd>
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
    import Actions from '@/modules/hmis/triage/allergies/AllergiesLineActions.vue';

    export default {
        props: {props: {default:{}},filter:{default:""} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Patient Allergies',
                    pageType:"list",
                    keys: { recKey:'ObservationNo',parentKey:'ObservationNo'},
                    controller:'TriageAllergies',
                    formRoute: "/hmis/" + this.$route.params.section +"/allergies/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/allergies/list",
                    filter:this.filter,
                    props:this.props,
                },
                actionsProps: { isNew: true,isNewCaption:"Add Allergies", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
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
