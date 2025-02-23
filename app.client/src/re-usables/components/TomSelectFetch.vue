<template>
	<select placeholder="search" :id="tsId" @input="$emit('update:modelValue', $event.target.value)" :class="[formMode == 'view'? '!bg-gray-200':'']" :readonly="formMode == 'view'? true:false" :disabled="formMode == 'view'? true:false"></select>
</template>
<script>
	var MyTomSelect = [];
	import TomSelect from 'tom-select/src/tom-select.complete'
	import * as css from 'tom-select/dist/css/tom-select.bootstrap4.css'
	export default {
        props: { cProps: { default: {} }, tsId: { default: "" }, filter: { default: "" }, initValue: { default: "" }, loadAll: { default: true },formMode: { default: '' },record: { default: null } },
		emits: ['update:modelValue'],
		data(){
			return{
                elTomSelect: null,
				loadedOptions: null,
				isInitialized:false,
				initialized:{},
				initValue2:"",
			}
		},
		mounted() {
			if (this.initValue == "" && this.record[this.cProps.recordField] != undefined) {
				this.initValue2 = this.record[this.cProps.recordField];
			}
			else if (this.initValue != "") {
				this.initValue2 = this.initValue;
			}
			if (this.tsId != "") {
                this.initialize(this.tsId, this.initValue2, this.cProps, this.filter, this.loadAll);
				this.setValue(this.tsId);
			}
		},
		methods:{
            initialize(tsId, myValue, cProps, filter, loadAll) {
				var self = this;
                MyTomSelect[tsId] = new TomSelect('#' + tsId, {
                    valueField: cProps.valueField,
                    labelField: cProps.labelField,
                    searchField: cProps.searchField,
					loadThrottle:1000,
					load: function (query, callback) {
						query = query.toUpperCase();
                        var url = '/api/General/odatafilter?webservice=' + cProps.webservice + '&query=$filter=' + filter + '&isList=' + loadAll;
						fetch(url)
						.then(response => response.json())
							.then(json => {
                                self.loadedOptions = json.response;
							callback(json.response);
						}).catch(() => {
							callback();
						});
					},
					// custom rendering functions for options and items
					render: {
						option: function (item, escape) {
							return `<div>${escape(item[cProps.valueField])} - ${escape(item[cProps.labelField])}</div>`
                            //return `<div>${escape(item[cProps.labelField])}</div>`
						},
						item: function (item, escape) {
                            return `<div>${escape(item[cProps.valueField])} - ${escape(item[cProps.labelField])}</div>`
                            //return `<div>${escape(item[cProps.labelField])}</div>`
						}
					},
					onLoad() {
                        self.setValue2(tsId, myValue);
					}
				});
			},
            setValue(tsId) {
				var value = this.initValue2;
				if (value != "") {
					var option = {};
                    option[this.cProps.valueField] = value;
                    option[this.cProps.labelField] = value;
                    MyTomSelect[tsId].addOption(option);
                    MyTomSelect[tsId].setValue([value]);
					if(!this.loadAll){
                        MyTomSelect[tsId].load(value);
					}else{
                        MyTomSelect[tsId].load("");
					}
					
				} else {
                    MyTomSelect[tsId].load("");
				}
                this.selectedValue = value;
			},
            setValue2(tsId, value) {
                if ((this.initialized[tsId] == undefined || (this.initialized[tsId] != undefined && !this.initialized[tsId])) && value != "") {
					if (this.loadedOptions != null) {
                        var activeOption = this.loadedOptions.find(obj => obj[this.cProps.valueField] == value);
                        var option = {};
						option[this.cProps.valueField] = value;
						if (activeOption != undefined) {
                            option[this.cProps.labelField] = activeOption[this.cProps.labelField];
						}
                        MyTomSelect[tsId].updateOption(value, option);
					}
                    MyTomSelect[tsId].setValue([value]);
                    this.initialized[tsId] = true;
				}
			},
		}
	}

</script>