<template>
    <div class="!text-xs">
        <ListPartPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
            <template #thead>
                <WTh>Student No</WTh>
                <WTh>Full Names</WTh>
                <WTh v-if="!isTheoryPractical">CAT Marks</WTh>
                <WTh v-if="!isTheoryPractical">Exam Marks</WTh>
                <WTh v-if="isTheoryPractical">Theory Marks</WTh>
                <WTh v-if="isTheoryPractical">Weighted Theory</WTh>
                <WTh v-if="isTheoryPractical">Practical Marks</WTh>
                <WTh v-if="isTheoryPractical">Weighted Practical</WTh>
                <WTh>Total Marks</WTh>
            </template>
            <template v-for="(record,index) in records" #[`tbody-${index}`]>
                <WTd>{{record.Student_No}}</WTd>
                <WTd>{{record.Student_Name}}</WTd>
                <WTd v-if="!isTheoryPractical">
                    <WInput type="number" required="true" v-model="records[index].Q2" :formMode="formMode" class="max-w-20" @input="FnValidateMarks(index)" />
                </WTd>
                <WTd v-if="!isTheoryPractical">
                    <WInput type="number" required="true" v-model="records[index].Q1" :formMode="formMode" class="max-w-20" @input="FnValidateMarks(index)" />
                </WTd>
                <WTd v-if="isTheoryPractical">
                    <WInput type="number" required="true" v-model="records[index].Q1" :formMode="formMode" class="max-w-20" @input="FnValidateMarks(index)" />
                </WTd>
                <WTd v-if="isTheoryPractical">{{records[index].Q3}}</WTd>
                <WTd v-if="isTheoryPractical">
                    <WInput type="number" required="true" v-model="records[index].Q2" :formMode="formMode" class="max-w-20" @input="FnValidateMarks(index)" />
                </WTd>
                <WTd v-if="isTheoryPractical">{{records[index].Q4}}</WTd>
                <WTd>{{records[index].Initial_Score}}</WTd>
            </template>
            <template #listMoreActions>
                <Actions :records="records" :pageProps="pageProps" />
            </template>
        </ListPartPageTemplate>
        <Actions :records="records" :pageProps="pageProps"/>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import WInput from '@/re-usables/components/WInput.vue';
    import Actions from '@/modules/staff-portal/academics/student-unit/StudentUnitActions.vue';

    export default {
        props: { props: { default: {} }, filter: { default: "" } },
        components: { Actions, ...W, WInput },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Lecturer Units',
                    pageType: "list",
                    //keys: { recKey: 'Line_No', parentKey: 'No' },
                    controller: 'StudentUnits',
                    //formRoute: "/ess/imprest-request/line/form",
                    //listRoute: "/ess/imprest-request/line/list",
                    filter: this.$route.query,
                },
                actionsProps: { isNew: false, isNewCaption: "New Request", isEdit: true, isDelete: false, isFilter: true, isExport: true,moreActions:true },
                header: {},
                amountTotal: 0,
                isTheoryPractical: null,
                formMode: "edit",
                examsSetup: [],
            }
        },
        created() {
            if (this.$route.query.programme == 'CPOTT' || this.$route.$query.programme == 'DPOTT') {
                this.isTheoryPractical = true;
            } else {
                this.isTheoryPractical = false;
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
                this.examsSetup = response.examSetup;
            },
            FnValidateMarks(recordIndex) {
                if (this.records[recordIndex].Q1 > 100) {
                    var msg = "Invalid Marks";
                    this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                    this.records[recordIndex].Q1 = 0;
                }
                if (this.records[recordIndex].Q2 > 100) {
                    var msg = "Invalid Marks";
                    this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                    this.records[recordIndex].Q2 = 0;
                }
                if (this.isTheoryPractical) {
                    if (this.$route.query.programme == "CPOTT") {
                        this.records[recordIndex].Q3 = Math.round((this.records[recordIndex].Q1 * 0.4));
                        this.records[recordIndex].Q4 = Math.round((this.records[recordIndex].Q2 * 0.6))
                    }
                    else if (this.$route.query.programme == "DPOTT") {
                        this.records[recordIndex].Q3 = Math.round((this.records[recordIndex].Q1 * 0.5));
                        this.records[recordIndex].Q4 = Math.round((this.records[recordIndex].Q2 * 0.5));
                    }
                    //
                    if (this.$route.query.programme == "KRCHN") {
                        this.records[recordIndex].Q4 = Math.round((this.records[recordIndex].Q2 * 0.5))
                    }
                    this.records[recordIndex].Initial_Score = this.records[recordIndex].Q3 + this.records[recordIndex].Q4;
                }
            }
        },
    }
</script>
