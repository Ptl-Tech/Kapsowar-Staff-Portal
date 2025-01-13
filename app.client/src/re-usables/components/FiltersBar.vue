<template>
    <!--Existing Filters-->
    <div v-if="fieldsProps != null" class="border pb-2 pt-0 py-1 mb-2 text-xs">
        <div v-if="filters.length > 0">
            <span class="mb-2 underline text-blue-500 font-semibold">List Filters</span>
            <span v-for="filter in filters" class="text-blue-500"><br />- {{filter.caption}} = "{{filter.value}}" <button class="mr-2 text-red-500" @click="FnRemoveFilter(filters.indexOf(filter))">x</button></span>
        </div>
        <!--New Filters-->
        <div class="mt-1">
            <WSelect class="max-w-40 !py-1" v-model="filterField" @change="FnOnFilterFieldChange()">
                <option value="">--Filter By--</option>
                <option v-for="field in fieldsProps" :value="field.Name">{{field.Caption != null? field.Caption:field.Name.replace("_"," ")}}</option>
            </WSelect>
            <span v-if="!isFetching && dataType != null">
                <WSelect v-if="dataType == 'TableRelation'" class="max-w-40 !py-1" v-model="filterValue">
                    <option value="">--Filter Value--</option>
                    <option v-for="data in dropdownValues" :value="FnOption(data,dropdownKeyCaption.key)">{{FnOption(data,dropdownKeyCaption.caption)}}</option>
                </WSelect>
                <WSelect v-else-if="dataType == 'Option'" class="max-w-40 !py-1" v-model="filterValue">
                    <option value="">--Filter Value--</option>
                    <option v-for="data in dropdownValues" :value="data.key">{{data.caption}}</option>
                </WSelect>
                <WInput v-else placeholder="filter value" v-model="filterValue" class="w-32 sm:w-64 !h-6 !max-w-28 !"/>
                <WButton @click="FnAddFilter()" class="!h-6 !font-normal !px-1 !py-1 !bg-gray-500">Add Filter</WButton>
            </span>
            <span v-if="isFetching" class="loader-sm"></span>
        </div>
        <button v-if="filters.length > 0" @click="FnResetFilters" class="text-red-500 border rounded-sm border-red-500 px-1 mt-1">Reset Filters</button>
        <button v-else @click="FnCloseFilterBar()" class="text-red-500 border rounded-sm border-red-500 px-1 mt-1">Close Filter</button>
    </div>
    <div v-if="isFetching" class="sm-loader"></div>
</template>
<script>
    import WSelect from '@/re-usables/components/WSelect.vue'
    import WInput from '@/re-usables/components/WInput.vue'
    import WButton from '@/re-usables/components/WButton.vue'
    export default {
        components: { WSelect, WInput, WButton },
        props: { pageProps: { default: {} } },
        emits: {
            FnOnChangeFilters: null,
            FnOnResetFilters: null,
            FnOnCloseFilterBar:null
        },
        data() {
            return {
                filterField: '',
                filterValue: '',
                filters:[],
                isFetching: false,
                dataType:null,
                dropdownValues:[],
                dropdownKeyCaption: [],
                fieldsProps:null
            }
        },
        created() {
            this.FnGetFieldProps();
        },
        methods: {
            FnGetFieldProps() {
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
                            this.fieldsProps = data;
                        }
                    }).catch((error) => {
                    });
            },
            FnOnFilterFieldChange(){
                this.dataType = null;
                 if(this.filterField != ""){
                     const result = this.fieldsProps.find(obj => obj.Name === this.filterField);
                     if(result.DataType == "Option"){
                         var options = result.OptionString.split(",");
                         for(let i =0;i < options.length; i++){
                             var option = { key:options[i],caption:options[i]};;
                             this.dropdownValues.push(option);
                         }
                     }
                     else if (result.DataType == "TableRelation") {
                         this.FnFetchData(result.TableRelationUrl);
                         this.dropdownKeyCaption = result.DropdownKeyCaption;
                     }
                     this.dataType = result.DataType;
                 }
            },
            FnFetchData(url) {
                this.isFetching = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.Error) {
                            this.isFetching = false;
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.Error; 
                        } else {
                            if (data.response != undefined) {
                                this.dropdownValues = data.response;
                            }
                        }
                        this.isFetching = false;
                    }).catch((error) => {
                        this.isFetching = false;
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;                        
                    });
            },
            FnOption(data,field){
                var vars = field.split("+");
                if (vars.length > 1) {
                    var response = "";
                    for(let i =0;i < vars.length;i++){
                        response= response+" "+data[vars[i]]
                    }
                    return response;
                }else{

                    return data[field];
                }

            },
            FnAddFilter() {
                const result = this.fieldsProps.find(obj => obj.Name === this.filterField);
                var filter = { field: this.filterField, value: this.filterValue, caption: result.Caption }
                this.filters.push(filter);
                this.filterField = "";
                this.filterValue = "";
                this.$emit('FnOnChangeFilters', this.filters);
            },
            FnRemoveFilter(index) {
                this.filters.splice(index, 1);
                this.$emit('FnOnChangeFilters', this.filters);
            },
            FnResetFilters() {
                this.filters = [];
                this.$emit('FnOnResetFilters');
            },
            FnCloseFilterBar() {
                this.filters = [];
                this.$emit('FnOnCloseFilterBar');
            },
        },
    }
</script>