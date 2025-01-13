<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Appointment No.</WTh>
            <WTh>Date</WTh>
            <WTh>Patient No.</WTh>
            <WTh>Patient Name</WTh>
            <WTh>Clinic</WTh>
            <WTh>Doctor</WTh>
            <WTh>Patient Type</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?recId='+record[pageProps.keys.recKey]">{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.AppointmentDate.Day}}-{{record.AppointmentDate.Month}}-{{record.AppointmentDate.Year}}</WTd>
            <WTd>{{record.PatientNo}}</WTd>
            <WTd>{{record.SearchNames}}</WTd>
            <WTd>{{record.SpecialClinics}}</WTd>
            <WTd>{{record.Doctor}}</WTd>
            <WTd>{{record.SettlementType}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record[pageProps.keys.recKey]">
                    <Actions :record="record" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Active-Today'">Active Visits</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Active-All'">All Visits</MenuLink>
                <!--<MenuLink :linkTo="pageProps.listRoute+'?status=Completed'">Completed</MenuLink>-->
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/hmis/visit/VisitHeaderActions.vue';
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
                    title: 'Visit List',
                    pageType:"list",
                    keys: { recKey: 'AppointmentNo' },
                    controller:'VisitsHeader',
                    formRoute: "/hmis/"+this.$route.params.section+"/visit/form",
                    listRoute: "/hmis/"+this.$route.params.section+"/visit/list",
                    isLinkTabs: true,
                },
                actionsProps: { isNew: true,isNewCaption:"New Visit", isEdit: true, isDelete: false, isFilter: true, isExport: true},
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
