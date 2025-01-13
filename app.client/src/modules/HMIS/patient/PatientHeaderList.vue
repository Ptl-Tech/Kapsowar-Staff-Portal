<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
      <template #thead>
        <WTh>Patient No.</WTh>
        <WTh>Name</WTh>
        <WTh>ID Number</WTh>
        <WTh>Gender</WTh>
        <WTh>D.O.B</WTh>
        <WTh>Date Registered</WTh>
        <WTh>Patient Type</WTh>
        <WTh>Actions</WTh>
      </template>
      <template v-for="(record,index) in records" #[`tbody-${index}`]>
        <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
        <WTd>{{record.MiddleName}} {{record.LastName}} {{record.Surname}}</WTd>
        <WTd>{{record.IDNumber}}</WTd>
        <WTd>{{record.Gender}}</WTd>
        <WTd>{{record.DateOfBirth.Day}}-{{record.DateOfBirth.Month}}-{{record.DateOfBirth.Year}}</WTd>
        <WTd>{{record.DateRegistered.Day}}-{{record.DateRegistered.Month}}-{{record.DateRegistered.Year}}</WTd>
        <WTd>{{record.PatientType}}</WTd>
        <WTd>
          <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
            <Actions :record="record" :pageProps="pageProps" />
          </EllipsisMenu>
        </WTd>
      </template>
        <!--<template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Active'">Active</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Cleared'">Cleared</MenuLink>
            </MenuTabs>
        </template>-->
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/patient/PatientHeaderActions.vue';
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
                    title: 'Patients List',
                    pageType:"list",
                    keys: { recKey: 'PatientNo' },
                    controller:'PatientsHeader',
                    formRoute: "/hmis/"+this.$route.params.section+"/patient/form",
                    listRoute: "/hmis/"+this.$route.params.section+"/patient/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true,isNewCaption:"New Patient", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
