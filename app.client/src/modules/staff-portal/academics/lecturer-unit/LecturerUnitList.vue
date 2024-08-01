<template>
    <div class="!text-xs">
        <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
            <template #thead>
                <WTh>Programme</WTh>
                <WTh>Stage</WTh>
                <WTh>Semester</WTh>
                <WTh>Unit Code</WTh>
                <WTh>Unit Name</WTh>
                <WTh>Campus</WTh>
                <WTh>Student Type</WTh>
                <WTh>Actions</WTh>
            </template>
            <template v-for="(record,index) in records" #[`tbody-${index}`]>
                <WTd>{{record.Description}}</WTd>
                <WTd>{{record.Stage}}</WTd>
                <WTd>{{record.Semester}}</WTd>
                <WTd>{{record.Unit}}</WTd>
                <WTd>{{record.Unit_Name}}</WTd>
                <WTd>{{record.Unit_Name}}</WTd>
                <WTd>{{record.Unit_Name}}</WTd>
                <WTd>
                    <EllipsisMenu :docNo="record[pageProps.keys.parentKey]+'#'+record[pageProps.keys.recKey]+'#'+record.Semester+'#'+record.Programme">
                        <Actions :record="record" :header="header" :pageProps="pageProps"/>
                    </EllipsisMenu>
                </WTd>
            </template>
        </ListPartPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/academics/lecturer-unit/LecturerUnitActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Lecturer Units',
                    pageType: "list",
                    keys: { recKey: 'Line_No', parentKey: 'Unit_No' },
                    controller: 'LecturerUnits',
                    //formRoute: "/ess/imprest-request/line/form",
                    //listRoute: "/ess/imprest-request/line/list",
                    filter: this.filter,
                },
                actionsProps: { isNew: false, isNewCaption: "New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true },
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
