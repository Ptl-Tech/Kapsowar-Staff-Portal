<template>
	<select placeholder="search" :id="tsId" @input="$emit('update:modelValue', $event.target.value)" :class="[formMode == 'view'? '!bg-gray-200':'']" :readonly="formMode == 'view'? true:false" :disabled="formMode == 'view'? true:false"></select>
</template>
<script>
	var MyTomSelect = [];
	import TomSelect from 'tom-select/src/tom-select.complete'
	import * as css from 'tom-select/dist/css/tom-select.bootstrap4.css'
	export default {
        props: { dimNo: { default: null }, tsId: { default: "dims" }, initValue: { default: "" }, loadAll: { default: false },formMode: { default: '' },record: { default: null } },
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
			if(this.initValue == "" && this.record != null){
				if (this.record["Shortcut_Dimension_" + this.dimNo + "_Code"]) {
					this.initValue2 = this.record["Shortcut_Dimension_"+this.dimNo+"_Code"];
				}
                else if (this.record["Global_Dimension_" + this.dimNo + "_Code"]) {
                    this.initValue2 = this.record["Global_Dimension_" + this.dimNo + "_Code"];
                }
			}
			else if(this.initValue != ""){
				this.initValue2 = this.initValue;
			}
            if (this.dimNo != null && this.dimNo != "null") {
				this.initialize(this.dimNo,this.initValue2);
                this.setValue(this.dimNo);
			}
		},
		methods:{
			initialize(dimNo,myValue) {
				self = this;
                MyTomSelect[dimNo] = new TomSelect('#' + self.tsId, {
					valueField: 'Code',
					labelField: 'Name',
					searchField: ['Code','Name'],
					loadThrottle:1000,
					load: function (query, callback) {
						query = query.toUpperCase();
						var url = '/api/Dimensions/querydims?dimNo='+dimNo+'&qString=' + encodeURIComponent(query);
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
							return `<div>${escape(item.Code)}-${escape(item.Name)}</div>`
						},
						item: function (item, escape) {
							return `<div>${escape(item.Code)}-${escape(item.Name)}</div>`
						}
					},
					onLoad() {
                       self.setValue2(dimNo,myValue);
					}
				});
			},
			setValue(dimNo) {
				var value = this.initValue2;
				if (value != "") {
					MyTomSelect[dimNo].addOption({
						Code: value,
						Name: "",
					});
					MyTomSelect[dimNo].setValue([value]);
					if(!this.loadAll){
						MyTomSelect[dimNo].load(value);
					}else{
						MyTomSelect[dimNo].load("");
					}
					
				} else {
                    MyTomSelect[dimNo].load("");
				}
                this.selectedValue = value;
			},
			setValue2(dimNo,value) {
                if ((this.initialized[dimNo] == undefined || (this.initialized[dimNo] != undefined && !this.initialized[dimNo])) && value != "") {
					if (this.loadedOptions != null) {
						var activeOption = this.loadedOptions.find(obj => obj.Code == value);
						MyTomSelect[dimNo].updateOption(value, { Code: value, Name: activeOption.Name });
					}
					MyTomSelect[dimNo].setValue([value]);
					this.initialized[dimNo] = true;
				}
			},
		}
	}

</script>