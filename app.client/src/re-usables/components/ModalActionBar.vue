<template>
    <div>
        <div class=" shadow">
            <div class="w-full">
                <ul id="tabs" class="inline-flex pb-0 px-1 bg-gray-200 w-full">
                    <li @click="activeTab('home')" class="px-1 sm:px-4 font-semibold text-black cursor-pointer flex items-center gap-1 text-xs" :class="active == 'home'? 'bg-white border-l border-t border-r border-blue-500 text-blue-600 pt-1':'border-gray-300 text-gray-700 border-r'"><unicon name="estate" :fill="active == 'home'? 'blue':'black'" class="icon-xs"/> <span class="hidden sm:block">Home</span></li>
                    <li @click="activeTab('moreActions')" class="px-1 sm:px-4 font-semibold text-black cursor-pointer flex items-center gap-1 text-xs" :class="active == 'moreActions'? 'bg-white border-l border-t border-r border-blue-500 text-blue-600 pt-1':'border-gray-300 text-gray-700 border-r'"><unicon name="expand-from-corner" :fill="active == 'moreActions'? 'blue':'black'" class="icon-xs"/> <span class="hidden sm:block">More Actions</span></li>
                </ul>
                <div class="flexx flex-rowx pr-4 pl-1 pt-2 pb-1">
                    <div v-if="active == 'home'" class="">
                        <x-standard-actions v-bind="{setup,selected}"
                            @onMultipleSelect="$emit('onMultipleSelect')"
                            @onFilter="onFilter"
                            @onExport="onExport"
                            @onNew="$emit('onNew')"
                            @onView="onView"
                            @onEdit="onEdit"
                            @onList="onList"
                            @onDelete="onDelete"
                            @onAttachments="$emit('onAttachments')"
                        />
                    </div>
                    <div v-if="active == 'moreActions'" class="pb-1">
                        <em v-if="this.setup.settings.isMoreActions == false" class="text-blue-500 text-xs">No more actions found</em>
                        <component v-if="this.setup.settings.isMoreActions != false && this.setup.settings.xMoreActions != null" :is="dynamicComponent" :selected="selected"></component>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import xStandardActions from '@/Components/ModalStandardActions'
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
            onList(url){
                this.$emit('onList',url);
            },
            onView(url){
                this.$emit('onView',url);
            },
            onEdit(url){
                this.$emit('onEdit',url);
            },
            onDelete(){
                this.$emit('onDelete');
            },
            activeTab(action){
                this.active = action;
                this.dynamicComponent = this.setDynamicComponent();
            },
            setDynamicComponent () {
                if(this.active == "moreActions" && this.setup.settings.isMoreActions != false && this.setup.settings.xMoreActions != null){
                    return markRaw(defineAsyncComponent(() => import(`@/${this.setup.settings.xMoreActions}.vue`)))
                }
            }
        },
    }
</script>

