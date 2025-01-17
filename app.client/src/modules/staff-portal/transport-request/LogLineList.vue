<template>
    <h4 class="font-semibold py-2">Vehicle Logs</h4>
    <div v-if="isNew">
        <WRouterLink :to="pageProps.formRoute+'/create/'+docNo+'/0'" class="flex items-center !bg-blue-500" title="Edit"><PlusIcon class="iconSmall" />New Vehicle Log</WRouterLink>
    </div>
    <ListPartPageTemplate ref="tpList" :urlQuery="`?docNo=${docNo}`" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Start Date</WTh>
            <WTh>Destination</WTh>
            <WTh>Trip Start Time</WTh>
            <WTh>Odometer Reading</WTh>
            <WTh>Trip End Time</WTh>
            <WTh>End Date</WTh>
            <WTh>Odometer Reading</WTh>
            <WTh>Mileage</WTh>
            <WTh>Notes</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd :linkTo="pageProps.formRoute+'/edit/'+record.Transport_Request_No+'/'+record.No">{{record.Starting_Date.split('T')[0]}}</WTd>
            <WTd>{{record.Destination_Route}}</WTd>
            <WTd>{{record.Start_Time}}</WTd>
            <WTd>{{record.Start_Time_Odometer_Reading}}</WTd>
            <WTd>{{record.End_Time}}</WTd>
            <WTd></WTd>
            <WTd>{{record.End_Time_Odometer_Reading}}</WTd>
            <WTd>{{record.Distance_Covered_in_KMs}}</WTd>
            <WTd>{{record.Notes}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.No">
                    <Actions :record="record" :header="header" :pageProps="pageProps" />
                </EllipsisMenu>
            </WTd>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/transport-request/LogLineActions.vue';
    export default {
        props: { docNo: { default: "" }, isNew: {default:true} },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: 'Vehicle Logs List',
                    pageType:"list",
                    pKey: 'No',
                    pKeyHeader: 'Transport_Request_No',
                    controller:'VehicleLogLine',
                    docNo: this.docNo,
                    formRoute: "/ess/transport-request/log/form",
                },
                lineAction:"",
                lineNo:0,
                actionsProps: { isNew: false,isNewCaption:"New Vehicle Log", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                isLineModal:false
            }
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
                this.header = response.header;
            }
        },
    }
</script>