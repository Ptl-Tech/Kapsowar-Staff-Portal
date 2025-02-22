<template>
    <div>
        <h4 class="font-semibold py-1">Approvers List</h4>
        <div>
            <table class="!w-auto">
                <thead>
                    <tr >
                        <WTh>Approver Name</WTh>
                        <WTh>Order/Sequence</WTh>
                        <WTh>Approval Status</WTh>
                    </tr>
                </thead>
                <tbody>
                    <WTr v-for="(record,index) in records" :index="index" :isDoubleClick="false">
                        <WTd>{{record.Approver_ID}}</WTd>
                        <WTd>{{record.Sequence_No}}</WTd>
                        <WTd>{{record.Status}}</WTd>
                    </WTr>
                </tbody>
            </table>
        </div>
    </div>
    </template>
    <script>
        import WTable from "@/re-usables/components/Table/Table.vue";
        import WTd from "@/re-usables/components/Table/Td.vue";
        import WTh from "@/re-usables/components/Table/Th.vue";
        import WTr from "@/re-usables/components/Table/Tr.vue";
        export default {
            props: { records: { default: null } },
            components: { WTable, WTd, WTh, WTr }
        }
    </script>
