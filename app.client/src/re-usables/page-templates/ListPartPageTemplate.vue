<template>
    <div class="text-xs">
        <!---->
        <div class="fixedx !topx-30 leftx-30 rightx-2 z-50">
            <ListActionsBar :pageProps="pageProps" :actionsProps="actionsProps" @filter="FnIsFilter()" @search="FnSearch($event)" :selectedRows="selectedRows" @delete="FnDeleteMultiple()" @orderList="FnOnOrderList($event)">
                <template #listMoreActions>
                    <slot name="listMoreActions"></slot>
                </template>
                <template #listReports>
                    <slot name="listReports"></slot>
                </template>
            </ListActionsBar>
        </div>
        <!---->
        <div class="!top-40">
            <FiltersBar v-if="filterProps.isFilter" :pageProps="pageProps" @FnOnChangeFilters="FnOnChangeFilters($event)" @FnOnResetFilters="FnOnResetFilters()" @FnOnCloseFilterBar="FnOnCloseFilterBar()" />
            <!---->
            <div class="flex flex-col">
                <div class="grow overflow-x-auto pb-10">
                    <table class="table-auto w-full relative rounded-t-lg">
                        <thead class="bg-gray-100 text-gray-600 text-sm font-semibold sticky top-0 border rounded-t-lg">
                            <tr>
                                <th v-if="selectedRows.length > 1 || isMultiSelect">
                                    <WCheckbox title="select all" @click="isSelectAll = !isSelectAll" :checked="isSelectAll" />
                                </th>
                                <slot name="thead"></slot>
                            </tr>
                        </thead>
                        <tbody class="text-xs sm:text-sm border">
                            <WTr v-if="!isFetchingData && records != null && records.length > 0" v-for="index in records.length" :index="index-1" @onRowClick="FnOnRowClick($event)" :isSelected="FnIsSelectedRow(index-1)">
                                <WTd v-if="selectedRows.length > 1 || isMultiSelect" class="flex justify-center" :id="'tdSelect'+index-1">
                                    <WCheckbox :checked="FnIsSelectedRow(index-1)" :id="'checkboxSelect'+index-1" />
                                </WTd>
                                <slot :name="`tbody-${index-1}`"></slot>
                            </WTr>
                             <slot name="summaryRow"></slot>
                        </tbody>
                    </table>
                    <!---->
                    <div class="flex justify-center gap-1 items-center">
                        <div v-if="isFetchingData"><span class="loader-sm"></span> fetching data ...</div>
                        <div v-else-if="!isFetchingData && (records == null || (records != null && records.length == 0))" class="italic">No records found</div>
                    </div>
                    <div v-if="!isFetchingData && records != null" class="flex justify-center pt-1">
                        <div v-if="!isLoadingMore && records.length >= newRecordsLimit" class="mt-2 flex justify-center">
                            <WButton class="flex items-center rounded-sm bg-blue-500" @click="FnLoadMoreRecords()"><ChevronDoubleDownIcon class="h-4 w-4" /> Load More</WButton>
                        </div>
                        <div v-else-if="isLoadingMore == false && records.length < newRecordsLimit" class="mt-2 flex justify-center">
                            <span class="italic">No more records</span>
                        </div>
                        <span v-if="isLoadingMore" class="loader-sm"></span>
                    </div>
                </div>
            </div>

        </div>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    const FiltersBar = defineAsyncComponent(() => import('@/re-usables/components/FiltersBar.vue'))
    const ListActionsBar = defineAsyncComponent(() => import('@/re-usables/components/ListPageActionsBar.vue'))
    const FormActionsBar = defineAsyncComponent(() => import('@/re-usables/components/FormPageActionsBar.vue'))
    const WButton = defineAsyncComponent(() => import('@/re-usables/components/WButton.vue'))
    const WCheckbox = defineAsyncComponent(() => import('@/re-usables/components/WCheckbox.vue'))
    const WTr = defineAsyncComponent(() => import('@/re-usables/components/Table/Tr.vue'))
    const WTd = defineAsyncComponent(() => import('@/re-usables/components/Table/Td.vue'))
    const WTh = defineAsyncComponent(() => import('@/re-usables/components/Table/Th.vue'))
    import { useRouter } from 'vue-router'
    import {EllipsisVerticalIcon, ChevronDoubleDownIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { WTd, WTr, WTh, FiltersBar, ListActionsBar, FormActionsBar, WButton, EllipsisVerticalIcon, ChevronDoubleDownIcon, WCheckbox },
        props: {
            pageProps: { default: {} },
            actionsProps: { default: {} },
            indexFn: { default: "Index" },
            isAutoFetchData: { default: true },
        },
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                records: [],
                header: null,
                filter: this.$root.fnGetUrlQueryFromObject(this.pageProps.filter),
                isFetchingData: false,//changed
                search: '',
                filterProps: { isFilter: false, filters: [] },
                selectedRows: [],
                isMultiSelect: false,
                isSelectAll: false,
                filterQuery: "",
                recordsLimit: 20,
                newRecordsLimit: 20,
                isLoadingMore: false,
            }
        },
        created() {
            if (this.isAutoFetchData) {
                this.FnFetchData();
            }
        },
        methods: {
            FnFetchData() {
                var url = this.appConfig.baseApiRoute + this.pageProps.controller + '/' + this.indexFn + this.filter;
                if (!this.isLoadingMore) {
                    this.isFetchingData = true;
                }
                const requestOptions = {
                    method: "GET",
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.isFetchingData = false;
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
                        } else {
                            this.records = data.records;
                            this.header = data.header;
                        }
                        this.isFetchingData = false;
                        this.isLoadingMore = false;
                    }).catch((error) => {
                        this.isFetchingData = false;
                        this.isLoadingMore = false;
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                    });
            },
            FnOnOrderList(actionParams) {
                if (this.filterQuery == "") {
                    if (this.filter == "") {
                        this.filterQuery = '?';
                    } else {
                        this.filterQuery = this.filter +'&';
                    }
                    this.filterQuery = this.filterQuery+"order_by=" + actionParams.colName + "___" + actionParams.actionValue + "__order_by";
                } else {
                    if (this.filterQuery.indexOf("order_by=") > 0) {
                        var mySubString = this.filterQuery.substring(this.filterQuery.indexOf("order_by="), this.filterQuery.lastIndexOf("__order_by"));
                        var newOrderBy = actionParams.colName + "___" + actionParams.actionValue;
                        this.filterQuery = this.filterQuery.replace(mySubString, "order_by=" + newOrderBy);
                    } else {
                        this.filterQuery = this.filterQuery + "&&order_by=" + actionParams.colName + "___" + actionParams.actionValue + "__order_by";
                    }
                }
                this.FnFetchData(this.appConfig.baseApiRoute + this.pageProps.controller + '/index' + this.filterQuery);
            },
            FnSearch(searchValue) {
                this.filterQuery = "";
                this.search = searchValue;
                if (this.search != '') {
                    if (this.filter == "") {
                        this.filterQuery = '?';
                    } else {
                        this.filterQuery = this.filter +'&';
                    }
                    this.filterQuery = this.filterQuery+'filter=any___' + this.search + "__filter";
                } else {
                    if (this.filter == "") {
                        this.filterQuery = '?';
                    } else {
                        this.filterQuery = this.filter;
                    }
                    this.filterQuery = this.filterQuery;
                }
                this.FnFetchData(this.appConfig.baseApiRoute + this.pageProps.controller + '/index'+ this.filterQuery);
            },
            FnFieldCellClass(fieldProps, value) {
                if (fieldProps.CellStyleIf != undefined && fieldProps.CellStyleIf[value] != null) {
                    var styling = "";
                    styling = fieldProps.CellStyleIf[value];
                    return styling;
                }
            },
            FnFieldValueClass(fieldProps, value) {
                if (fieldProps.ValueStyleIf != undefined && fieldProps.ValueStyleIf[value] != null) {
                    var styling = "";
                    styling = fieldProps.ValueStyleIf[value] + ' ;padding:0px 2px 0px 2px;border-radius: 0.125rem';
                    return styling;
                }
            },
            FnIsFilter() {
                this.filterProps.isFilter = !this.filterProps.isFilter;
            },
            FnUpdateFieldsProps(fieldsProps) {
                for (let i = 0; i < this.pageProps.fields.length; i++) {
                    const result = fieldsProps.find(obj => obj.Name === this.pageProps.fields[i].name);
                    this.pageProps.fields[i].props = result;
                }
            },
            FnOnChangeFilters(newFilters) {
                this.filterProps.filters = newFilters;
                if (this.filterProps.filters.length > 0) {
                    this.filterQuery = "";
                    if (this.search != '') {
                        if (this.filter == "") {
                            this.filterQuery = '?';
                        } else {
                            this.filterQuery = this.filter+'&';
                        }
                        this.filterQuery = this.filterQuery+'filter=any___' + this.search;
                    }
                    for (let i = 0; i < newFilters.length; i++) {
                        if (this.filterQuery == "") {
                            if (this.filter == "") {
                                this.filterQuery = '?';
                            } else {
                                this.filterQuery = this.filter+'&';
                            }
                            this.filterQuery = this.filterQuery+"filter=" + newFilters[i].field + "___" + newFilters[i].value;
                        } else {
                            this.filterQuery = this.filterQuery + "&&" + newFilters[i].field + "___" + newFilters[i].value;
                        }
                    }
                    this.FnFetchData(this.appConfig.baseApiRoute + this.pageProps.controller + '/index' + this.filterQuery + "__filter");
                } else {
                    this.search = "";
                    if (this.filter == "") {
                        this.filterQuery = '?';
                    } else {
                        this.filterQuery = this.filter;
                    }
                    this.FnFetchData(this.appConfig.baseApiRoute + this.pageProps.controller + '/index'+this.filterQuery);
                }
            },
            FnOnResetFilters() {
                this.filterProps.filters = [];
                this.filterProps.isFilter = true;
                this.FnFetchData(this.appConfig.baseApiRoute + this.pageProps.controller + '/index');
            },
            FnOnCloseFilterBar() {
                this.filterProps.isFilter = false;
            },
            FnOnRowClick(params) {
                var rowRecordIndex = params.index;
                var clickType = params.clickType;
                if (clickType == "single" || clickType == "single+control") {
                    if (this.FnIsSelectedRow(rowRecordIndex) == false) {
                        if (clickType == "single+control") {
                            this.isMultiSelect = true;
                        }
                        else if (clickType == "single") {
                            if (this.isMultiSelect == false) {
                                this.selectedRows = [];
                            }
                        }
                        this.selectedRows.push(rowRecordIndex);
                    }
                    else {
                        this.selectedRows.splice(this.selectedRows.indexOf(rowRecordIndex), 1);
                    }
                } else {
                    if (this.pageProps.formRoute != undefined && this.pageProps.formRoute != null) {
                        if (this.pageProps.formEditable != undefined && this.pageProps.formEditable == false) {
                            this.router.push(this.pageProps.formRoute + "/view?parentId=" + this.records[rowRecordIndex][this.pageProps.keys.parentKey] + "&recId=" + this.records[rowRecordIndex][this.pageProps.keys.recKey]);
                        } else {
                            this.router.push(this.pageProps.formRoute + "/edit?parentId=" + this.records[rowRecordIndex][this.pageProps.keys.parentKey] + "&recId=" + this.records[rowRecordIndex][this.pageProps.keys.recKey]);
                        }
                    }
                }
            },

            FnIsSelectedRow(rowIndex) {
                if (this.selectedRows.find(element => element === rowIndex)) {
                    return true;
                } else {
                    return false;
                }
            },
            FnGetPkeyValue(record) {
                var pKeys = this.pageProps.keys.recKey.split("+");
                var value;
                if (pKeys.length <= 1) {
                    value = record[this.pageProps.keys.recKey]
                } else {
                    for (let i = 0; i < pKeys.length; i++) {
                        value = value + "+" + record[pKeys[i]];
                    }
                }
                return value;
            },
            FnGetPkeyHeaderValue(record) {
                var value;
                /*var pKeys = this.pageProps.pKeyHeader.split("+");
                
                if (pKeys.length <= 1) {
                    value = record[this.pageProps.pKeyHeader]
                } else {
                    for (let i = 0; i < pKeys.length; i++) {
                        value = value + "+" + record[pKeys[i]];
                    }
                }*/
                value = record[this.pageProps.keys.parentKey];
                return value;
            },
            FnLoadMoreRecords() {
                this.isLoadingMore = true;
                this.newRecordsLimit = this.records.length + this.recordsLimit;
                if (this.filterQuery == "") {
                    if (this.filter == "") {
                        this.filterQuery = '?';
                    } else {

                        this.filterQuery = this.filter+'&';
                    }
                    this.filterQuery = this.filterQuery+"take_max=" + this.newRecordsLimit + "__take_max";
                } else {
                    if (this.filterQuery.indexOf("take_max=") > 0) {
                        var currentMax = this.filterQuery.substring(this.filterQuery.indexOf("take_max="), this.filterQuery.lastIndexOf("__take_max"));
                        this.filterQuery = this.filterQuery.replace(currentMax, "take_max=" + this.newRecordsLimit);
                    } else {
                        this.filterQuery = this.filterQuery + "&&take_max=" + this.newRecordsLimit + "__take_max";
                    }
                }
                this.FnFetchData(this.appConfig.baseApiRoute + this.pageProps.controller + '/index' + this.filterQuery);
            },
            FnDeleteMultiple() {
                this.$root.loader = { isLoading: true, message: '' };
                var deleteCount = this.selectedRows.length;
                for (let i = 0; i < this.selectedRows.length; i++) {
                    var obj = this.records[this.selectedRows[i]-1];
                    const requestOptions = {
                        method: "POST",
                        headers: { 'Content-Type': "application/json" },
                        body: JSON.stringify(obj)
                    };
                    fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/Delete', requestOptions)
                        .then(response => {
                            return response.json();
                        })
                        .then(data => {
                            if (data && data.Error) {
                                var msg = data.Error;
                                this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                            } else {
                                this.records.splice(this.selectedRows[i], 1);
                                this.$root.loader = { isLoading: false, message: '' };
                            }
                        }).catch((error) => {
                            this.$root.loader = { isLoading: false, message: '' };
                            var msg = this.appConfig.errors.dataDeletionFailure;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        });
                }
                this.selectedRows = [];
                this.$root.FnNotification(deleteCount + " record(s) deleted successfully.", "bg-green-600", false);
            },
            FnDeleteSingle(recIndex) {
                if(!confirm("Are you sure you want to delete this document?")){return}
                this.$root.loader = { isLoading: true, message: '' };
                var deleteCount = this.selectedRows.length;
                var obj = this.records[recIndex];
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(obj)
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/delete', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.Error) {
                            var msg = data.Error;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.records.splice(this.selectedRows[i], 1);
                            this.$root.loader = { isLoading: false, message: '' };
                        }
                    }).catch((error) => {
                        this.$root.loader = { isLoading: false, message: '' };
                        var msg = this.appConfig.errors.dataDeletionFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                    });
            this.selectedRows = [];
            this.$root.FnNotification(deleteCount + " record(s) deleted successfully.", "bg-green-600", false);
            },
            FnEdit(recIndex) {
                this.router.push(this.pageProps.formRoute + "/edit/" + this.records[recIndex][this.pageProps.keys.recKey]);
            },
        },
        watch: {
            isSelectAll: function () {
                if (this.isSelectAll == false) {
                    this.selectedRows = [];
                    this.isMultiSelect = false
                } else {
                    this.selectedRows = [];
                    for (let i = 0; i < this.records.length; i++) {
                        this.selectedRows.push(i);
                    }
                }
            },
            records: function () {
                var response = {};
                response.records = this.records;
                response.header = this.header;
                this.$emit('OnFetchData', response);
            },
            isAutoFetchData: function (newValue, oldValue) {
                if (oldValue == false && newValue == true) {
                    this.FnFetchData();
                }
            }
        }
    }
</script>