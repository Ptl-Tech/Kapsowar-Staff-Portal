<template>
  <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
    <template #thead>
      <WTh>Observation No.</WTh>
      <WTh>Date</WTh>
      <WTh>Time</WTh>
      <WTh>Patient No.</WTh>
      <WTh>Patient Name</WTh>
      <WTh>Actions</WTh>
    </template>
    <template v-for="(record,index) in records" #[`tbody-${index}`]>
      <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
      <WTd>{{record.ObservationDate.Day}}-{{record.ObservationDate.Month}}-{{record.ObservationDate.Year}}</WTd>
      <WTd>{{record.ObservationTime}}</WTd>
      <WTd>{{record.PatientNo}}</WTd>
      <WTd>{{record.PatientName}}</WTd>
      <WTd>
        <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
          <Actions :record="record" :pageProps="pageProps" />
        </EllipsisMenu>
      </WTd>
    </template>
    <template #navigationTabs>
        <MenuTabs>
            <MenuLink :linkTo="pageProps.listRoute+'?status=Pending-Today'">Pending Today</MenuLink>
            <MenuLink :linkTo="pageProps.listRoute+'?status=Closed-Today'">Closed Today</MenuLink>
            <MenuLink :linkTo="pageProps.listRoute+'?status=All'">All</MenuLink>
        </MenuTabs>
    </template>
  </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/triage/TriagePatientHeaderActions.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { Actions, ...W },
        setup() {
            const router = useRouter();
            return { router };
        },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Triage List',
                    pageType:"list",
                    keys: {recKey:'ObservationNo'},
                    controller:'TriagePatientsHeader',
                    formRoute: "/hmis/"+this.$route.params.section+"/triage-patient/form",
                    listRoute: "/hmis/"+this.$route.params.section+"/triage-patient/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: false,isNewCaption:"New", isEdit: true, isDelete: false, isFilter: true, isExport: true},
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
            },
        }
    }
</script>
