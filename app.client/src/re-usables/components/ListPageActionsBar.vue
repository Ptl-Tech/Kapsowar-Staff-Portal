<template>
    <div>
        <div class="flex flex-row pl-1">
            <div class="flex-grow overflow-x-auto no-scrollbarx !text-xs">
                <div v-if="pageProps.pageType.toLowerCase() == 'list'" class="flex flex-row divide-x divide-white mb-2 items-center bg-white px-0">
                    <span v-if="!(actionsProps != undefined && actionsProps.isNew != undefined && actionsProps.isNew == false) && !(pageProps.props != undefined && pageProps.props.isNew != undefined && pageProps.props.isNew == false)">
                        <WRouterLink :to="pageProps.formRoute+'/create'+$root.fnGetUrlQueryFromObject(pageProps.filter)" class="flex items-center !text-xs !py-1 h-6 !bg-green-600" title="New record"><PlusIcon class="iconSmall !text-xs" /> <span class="hidden sm:flex">{{actionsProps.isNewCaption != "undefined"? actionsProps.isNewCaption:'New'}}</span><span class="flex sm:hidden">New</span></WRouterLink>
                    </span>
                    <span v-if="!(actionsProps != undefined && actionsProps.isDelete != undefined && actionsProps.isDelete == false)">
                        <WButton @click="FnEmitAction('delete')" class="flex items-center" :disabled="selectedRows.length == 0? true:false"><TrashIcon class="iconSmall" /> <span class="hidden sm:flex">Delete</span></WButton>
                    </span>
                    <span v-if="!(actionsProps != undefined && actionsProps.isSearch != undefined && actionsProps.isSearch == false)">
                        <WInput placeholder="search" v-model="search" @input="FnOnSearch()" class="!w-16 sm:!w-24 md:!w-36 !py-1 h-6" />
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.moreActions != undefined)">
                        <slot name="listMoreActions"></slot>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.reports != undefined)">
                        <slot name="listReports"></slot>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.isAttachments != undefined && actionsProps.isAttachments == true)">
                        <WButton @click="FnEmitAction('attachments')" class="flex items-center" :disabled="selectedRows.length == 0 || (selectedRows != null && selectedRows.length > 1)? true:false"><DocumentIcon class="iconSmall" /> Attachments</WButton>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.isReport != undefined && actionsProps.isReport == true)">
                        <WButton @click="FnEmitAction('exportList')" class="flex items-center" :disabled="selectedRows.length == 0 || (selectedRows != null && selectedRows.length > 1)? true:false"><DocumentIcon class="iconSmall" /> Export List</WButton>
                    </span>
                    <span v-if="!(actionsProps != undefined && actionsProps.isOrderBy != undefined && actionsProps.isOrderBy == false)" class="flex">
                        <WSelect @click="FnGetSortFields()" @change="FnOrderList()" v-model="sort.field" class="!py-0.5 max-w-22 h-6">
                            <option value="">--Sort by--</option>
                            <option v-if="sort.isFetching"><span class="loader-sm"></span></option>
                            <option v-for="field in sortFields" :value="field.Name">{{field.Caption != null? field.Caption:field.Name.replace("_"," ")}}</option>
                        </WSelect>
                        <WSelect v-if="sort.field != ''" v-model="sort.value" @change="FnOrderList()" class="py-1 max-w-22 h-6">
                            <option value="">Ascending</option>
                            <option value="desc">Descending</option>
                        </WSelect>
                    </span>
                    <!--<span v-if="!(actionsProps != undefined && actionsProps.isFilter != undefined && actionsProps.isFilter == false)">
                        <WButton @click="FnEmitAction('filter')" class="flex items-center !py-1 !font-normal" title="Filter list"><FunnelIcon class="h-3 w-3" /> <span class="hidden sm:flex">Filter</span></WButton>
                    </span>-->
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import Dropdown from '@/re-usables/components/Dropdown.vue'
    import WInput from '@/re-usables/components/WInput.vue'
    import WButton from '@/re-usables/components/WButton.vue'
    import WSelect from '@/re-usables/components/WSelect.vue'
    import { FunnelIcon, PlusIcon, ChevronDoubleDownIcon, PencilIcon, TrashIcon, DocumentIcon, ArrowsPointingOutIcon,ChevronDownIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { WInput, Dropdown, WRouterLink,WSelect, FunnelIcon, WButton, PlusIcon, ChevronDownIcon, ChevronDoubleDownIcon, PencilIcon, TrashIcon, DocumentIcon, ArrowsPointingOutIcon },
        props: { pageProps: { default: {} }, actionsProps: { default: {} }, selectedRows: { default: [] } },
        emits: { delete: null, edit: null, filter: null, orderList:null,search:null},
        data() {
            return {
                search: "",
                filter: "",
                typingTimer: null,
                sortFields: [],
                typingTimeout: 1000, //milliseconds
                actionButton: "group flex w-full items-center rounded-md px-2 py-2 text-sm",
                sort: {field:'',value:'',isFetching:false},
            }
        },
        methods: {
            FnEmitAction(action) {
                this.$emit(action);
            },
            FnOnSearch() {
                clearTimeout(this.typingTimer);
                var self = this;
                this.typingTimer = setTimeout(function () { self.$emit("search", self.search)}, this.typingTimeout);
            },
            FnGetSortFields() {
                if (this.sort.field == "" && this.sortFields.length == 0) {
                    this.sort.isFetching = true;
                    const requestOptions = {
                        method: "GET",
                    };
                    fetch(this.appConfig.baseApiRoute + this.pageProps.controller + "/FieldsProps", requestOptions)
                        .then(response => {
                            return response.json();
                        })
                        .then(data => {
                            if (data && data.errors) {
                            } else {
                                this.sortFields = data;
                            }
                            this.sort.isFetching = false;
                        }).catch((error) => {
                        });
                }
            },
            FnOrderList() {
                var sortValue = this.sort.value == "" ? "asc" : "desc";
                this.$emit('orderList', { colName: this.sort.field, actionValue: sortValue });
            }
        }
    }
</script>
