<template>
    <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Staff No.</WTh>
            <WTh>Staff Name</WTh>
            <WTh>Email</WTh>
            <WTh>Designation</WTh>
            <WTh>Gender</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit?parentId='+record[pageProps.keys.parentKey]+'&recId='+record[pageProps.keys.recKey]">{{record.Staff_No}}</WTd>
            <WTd>{{record.Staff_Name}}</WTd>
            <WTd>{{record.Company_E_Mail}}</WTd>
            <WTd>{{record.Designation}}</WTd>
            <WTd>{{record.Gender}}</WTd>
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
    import Actions from '@/modules/staff-portal/training-application/line/TrainingLineActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Training Participants',
                    pageType: "list",
                    keys: { recKey: 'Line_No', parentKey: 'Application_No' },
                    controller: 'TrainingLine',
                    formRoute: "/ess/training/line/form",
                    listRoute: "/ess/training/line/list",
                    filter: this.filter,
                },
                actionsProps: { isNew: true, isNewCaption: "New Line", isEdit: false, isDelete: false, isSearch: false, isFilter: false, isExport: false, isOrderBy: false, moreActions: true },
                header: {},
                amountTotal: 0,
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
                if (this.records != null) {
                    for (var i = 0; i < this.records.length; i++) {
                        this.amountTotal = this.amountTotal + parseFloat(this.records[i].Amount);
                    }
                }
            }
        },
    }
</script>
