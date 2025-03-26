<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>ID/Passport No.</WTh>
            <WTh>Surname</WTh>
            <WTh>Other Names</WTh>
            <WTh>Date of Birth</WTh>
            <WTh>Percentage</WTh>
            <WTh>Phone No.</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd>{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.SurName}}</WTd>
            <WTd>{{record.Other_Name}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Date)}}</WTd>
            <WTd>{{record.Percentage}}</WTd>
            <WTd>{{record.Home_Tel_No}}</WTd>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import { useRouter } from 'vue-router'
    export default {
        components: { ...W },
        setup() {
            const router = useRouter();
            return { router };
        },
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'List',
                    pageType:"list",
                    keys: { recKey: 'ID_No_Passport_No' },
                    controller:'StaffDependants',
                //    formRoute: "/ess/staff-claim/header/form",
                //    listRoute: "/ess/staff-claim/header/list",
                },
                actionsProps: { isNew: false,isNewCaption:"New Claim", isEdit: false, isDelete: false, isFilter: true, isExport: false},
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.$route.query.type+" "+this.pageProps.title;
            },
        }
    }
</script>
