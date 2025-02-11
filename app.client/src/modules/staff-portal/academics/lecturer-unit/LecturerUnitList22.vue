<template>
    <WSelect class="!w-auto">
        <option value="">Select Semester</option>
    </WSelect>
    <ListPageTemplate v-if="semester != ''" ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>No.</WTh>
            <WTh>Name</WTh>
            <WTh>Gender</WTh>
            <WTh>Campus</WTh>
            <WTh>Leave Balance</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd>{{record[pageProps.keys.recKey]}}</WTd>
            <WTd>{{record.Full_Name}}</WTd>
            <WTd>{{record.Gender}}</WTd>
            <WTd>{{record.Global_Dimension_1_Code}}</WTd>
            <WTd>{{record.Leave_Balance}}</WTd>
        </template>
        <template #navigationTabs>
            <MenuTabs>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Active'">Active</MenuLink>
                <MenuLink :linkTo="pageProps.listRoute+'?status=Inactive'">Inactive</MenuLink>
            </MenuTabs>
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import WSelect from '@/re-usables/components/WSelect.vue';
    import Actions from '@/modules/staff-portal/purchase-request/header/PurchaseRequestHeaderActions.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { Actions, ...W, WSelect },
        setup() {
            const router = useRouter();
            return { router };
        },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Lecturer List',
                    pageType: "list",
                    keys: { recKey: 'No' },
                    controller: 'Lecturers',
                    formRoute: "/ess/academics/lecturer-list/form",
                    listRoute: "/ess/academics/lecturer-list/list",
                },
                actionsProps: { isNew: false, isNewCaption: "New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true },
                semester: "",
                semesters: "",
            }
        },
        created() {
            this.FnFetchSetups();
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
            },
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + 'LecturerUnits/GetExamSemesters' , requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "popup", theme: "red", message: msg });
                            this.router.go(-1);
                        } else {
                            this.semesters = data.response.semesters;
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>
