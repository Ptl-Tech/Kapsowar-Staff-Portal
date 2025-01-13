<template>
    <div>
        <div class="flex justify-left text-xs pb-0 mb-0 border-b fixed top-12 z-40 bg-white w-9/12x left-2 sm:left-52 shadow" style="right:4px; height: 3.7rem;">
            <div class="w-full">
                <ul id="tabs" class="inline-flex pb-0 px-1 bg-gray-200 w-full">
                    <!-- <li class="px-1 py-1 fontx-semibold text-black rounded-t" :class="active == 'default'? 'border-l border-t border-r border-gray-300':'border-b border-gray-300'"></li> -->
                    <li @click="activeTab('home')" class="px-1 sm:px-4 font-semibold text-black cursor-pointer flex items-center gap-1 text-xs" :class="active == 'home'? 'bg-white border-l border-t border-r border-blue-500 text-blue-600 pt-1':'border-gray-300 text-gray-700 border-r'"><unicon name="estate" :fill="active == 'home'? 'blue':'black'" class="icon-xs"/> <span class="hidden sm:block">Home</span></li>
                    <li @click="activeTab('moreActions')" class="px-1 sm:px-4 font-semibold text-black cursor-pointer flex items-center gap-1 text-xs" :class="active == 'moreActions'? 'bg-white border-l border-t border-r border-blue-500 text-blue-600 pt-1':'border-gray-300 text-gray-700 border-r'"><unicon name="expand-from-corner" :fill="active == 'moreActions'? 'blue':'black'" class="icon-xs"/> <span class="hidden sm:block">More Actions</span></li>
                    <li @click="activeTab('reports')" class="px-1 sm:px-4 font-semibold text-black cursor-pointer flex items-center gap-1 text-xs" :class="active == 'reports'? 'bg-white border-l border-t border-r border-blue-500 text-blue-600 pt-1':'border-gray-300 text-gray-700 border-r'"><unicon name="file-alt"  :fill="active == 'reports'? 'blue':'black'" class="icon-xs"/> <span class="hidden sm:block">Reports</span></li>
                    <li @click="activeTab('charts')" class="px-1 sm:px-4 font-semibold text-black cursor-pointer flex items-center gap-1 text-xs" :class="active == 'charts'? 'bg-white border-l border-t border-r border-blue-500 text-blue-600 pt-1':'border-gray-300 text-gray-700'"><unicon name="chart-pie-alt"  :fill="active == 'charts'? 'blue':'black'" class="icon-xs"/> <span class="hidden sm:block">Charts</span></li>
                    <!-- <li class="px-1 font-semibold text-black rounded-t" :class="active == 'default'? 'border-l border-t border-r border-gray-300':'border-b border-gray-300'"></li> -->
                </ul>
                <div class="flexx flex-rowx pr-4 pl-1 pt-2">
                    <div v-if="active == 'home'" class="">
                        <x-standard-actions v-bind="{setup,selected}"
                            @onMultipleSelect="this.$parent.$parent.isMultipleSelect = !this.$parent.$parent.isMultipleSelect"
                            @onAttachments="this.$parent.isAttachments = !this.$parent.isAttachments"
                            @onFilter="onFilter"
                            @onExport="onExport"
                        />
                    </div>
                    <div v-if="active == 'moreActions'">
                        <em v-if="this.setup.settings.isMoreActions == false" class="text-blue-500">No more actions found</em>
                        <component v-if="this.setup.settings.isMoreActions != false && this.setup.settings.xMoreActions != null" :is="dynamicComponent" :selected="selected"></component>
                    </div>
                    <div v-if="active == 'reports'">
                        <em v-if="this.setup.settings.isReports == false"  class="text-blue-500">No reports found</em>
                        <component v-if="this.setup.settings.isReports != false" :is="dynamicComponent" :indexRoute="this.setup.settings.indexRoute"  :selected="selected"></component>
                    </div>
                    <div v-if="active == 'charts'">
                        <em v-if="this.setup.settings.isCharts == false"  class="text-blue-500">No charts found</em>
                        <component v-if="this.setup.settings.isCharts != false" :is="dynamicComponent" :indexRoute="this.setup.settings.indexRoute" ></component>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import xStandardActions from '@/Components/StandardActions'
    import {defineAsyncComponent,markRaw} from "vue";

    export default{
        components:{xStandardActions},
        props:{
            setup:Object,
            selected:{default:[]},
        },
        data(){
            return{
                active:'home',
                dynamicComponent:{default:''},
            }
        },
        methods:{
            onFilter(isFilter){
                this.$parent.$refs.filter.isFilter = isFilter;
            },
            onExport(){
                this.$parent.$refs.exportData.submit();
            },
            activeTab(action){
                this.active = action;
                this.dynamicComponent = this.setDynamicComponent();
            },
            setDynamicComponent () {
                if(this.active == "moreActions" && this.setup.settings.isMoreActions != false && this.setup.settings.xMoreActions != null){
                    return markRaw(defineAsyncComponent(() => import(`@/${this.setup.settings.xMoreActions}.vue`)))
                }
                else if(this.active == "reports" && this.setup.settings.isReports != false){
                    return markRaw(defineAsyncComponent(() => import(`@/${this.setup.settings.xReports+'/ReportsTab'}.vue`)))
                }
                else if(this.active == "charts" && this.setup.settings.isCharts != false){
                    return markRaw(defineAsyncComponent(() => import(`@/Admin/Charts/${/[^/]*$/.exec(this.setup.settings.xFolder)[0]+'/ChartsTab'}.vue`)))
                }
            }
        },
    }
</script>

