<template>
    <h4 class="font-semibold py-2 capitalize">{{type}} Attachments List {{optional? '(Optional)':''}}</h4>
    <div v-if="editable">
        <WRouterLink :to="'/ess/attachment/form/create/'+docNo+'/'+tableID+'/'+type" class="flex items-center !bg-blue-500 capitalize" title="Edit"><PlusIcon class="iconSmall" />New Attachment</WRouterLink>
    </div>
    <ListPartPageTemplate ref="tpList" :urlQuery="`?docNo=${docNo}&tableID=${tableID}`" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <!--<WTh>No</WTh>-->
            <WTh>Description</WTh>
            <WTh>File Name</WTh>
            <WTh>Actions</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <!--<WTd>{{record.Entry_No}}</WTd>-->
            <WTd>{{record.Description}}</WTd>
            <WTd>{{record.Attachment_FileName}}</WTd>
            <WTd>
                <EllipsisMenu :docNo="record.Entry_No">
                    <Actions :record="record" :pageProps="pageProps"  :editable="editable"/>
                </EllipsisMenu>
            </WTd>
        </template>
    </ListPartPageTemplate>
    <router-view></router-view>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/ess/attachment/AttachmentActions.vue';
    export default {
        props: { docNo: { default: "" }, tableID: { default: 0 }, type: { default: 0 }, editable: { default: true },optional:{default:false } },
        components: { Actions, ...W},
        data(){
            return {
                records:[],
                pageProps: {
                    title: this.type+' '+'Attachments List',
                    pageType:"list",
                    pKey: 'No',
                    controller:'Attachment',
                    docNo:this.docNo
                },
                lineAction:"",
                lineNo:0,
                actionsProps: { isNew: false,isNewCaption:"New Attachment", isEdit: false, isDelete: false,isSearch:false, isFilter: false, isExport: false,isOrderBy:false},
                isLineModal:false
            }
        },
        created(){
            this.pageProps.title = this.title;
            this.$root.title = this.pageProps.title;
        },
        methods: {
            OnFetchData(response) {
                this.records = response.records;
            }
        },
    }
</script>