<template>
  <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
      <template #thead>
          <WTh>Treatment No.</WTh>
          <WTh>Type</WTh>
          <WTh>Date</WTh>
          <WTh>Time</WTh>
          <WTh>Patient No.</WTh>
          <WTh>Patient Name</WTh>
          <WTh>Waiting At</WTh>
          <WTh>Status</WTh>
          <WTh>Actions</WTh>
      </template>
      <template v-for="(record,index) in records" #[`tbody-${index}`]>
          <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
          <WTd>{{record.TreatmentType}}</WTd>
          <WTd>{{record.TreatmentDate.Day}}-{{record.TreatmentDate.Month}}-{{record.TreatmentDate.Year}}</WTd>
          <WTd>{{record.TreatmentTime}}</WTd>
          <WTd>{{record.PatientNo}}</WTd>
          <WTd>{{record.PatientName}}</WTd>
          <WTd>{{record.WaitingAt}}</WTd>
          <WTd>{{record.Status}}</WTd>
          <WTd>
              <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                  <Actions :record="record" :pageProps="pageProps" />
              </EllipsisMenu>
          </WTd>
      </template>
    <template #navigationTabs>
        <MenuTabs>
            <MenuLink :linkTo="pageProps.listRoute+'?status=New-Today'">New Today</MenuLink>
            <MenuLink :linkTo="pageProps.listRoute+'?status=Completed-Today'">Completed Today</MenuLink>
            <MenuLink :linkTo="pageProps.listRoute+'?status=All'">All</MenuLink>
        </MenuTabs>
    </template>
  </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/consultation/treatment-header/TreatmentHeaderActions.vue';
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
                    title: 'Consulation List',
                    pageType:"list",
                    keys: {recKey:'TreatmentNo'},
                    controller:'TreatmentsHeader',
                    formRoute: "/hmis/" + this.$route.params.section +"/consultation/form",
                    listRoute: "/hmis/" + this.$route.params.section +"/consultation/list",
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
