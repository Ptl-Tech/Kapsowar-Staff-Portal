<template>
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Name</WTh>
            <WTh>Relationship</WTh>
            <WTh>Occupation</WTh>
            <WTh>Date of Birth</WTh>
            <WTh>Phone No.</WTh>
            <WTh>Email</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd>{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.Relationship}}</WTd>
            <WTd>{{record.Occupation}}</WTd>
            <WTd>{{$root.xFnNavDateObjToString(record.Date_of_Birth)}}</WTd>
            <WTd>{{record.Tel_No}}</WTd>
            <WTd>{{record.E_Mail}}</WTd>
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
                    keys: { recKey: 'Names' },
                    controller:'StaffNextOfKin',
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
