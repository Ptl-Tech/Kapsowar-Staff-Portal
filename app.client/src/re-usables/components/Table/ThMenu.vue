<template>
    <dropdown>
        <template #caption>
            <span class="flex gap-1 items-center justify-between"><span><slot></slot></span><ArrowsUpDownIcon class="w-3 h-3" /></span>
        </template>
        <template #content>
            <MenuItem v-slot="{ active }">
                <button @click="onAction('orderBy','ASC')" class="flex items-center hover:bg-theme-1 w-full py-1 text-gray-600 font-normal border-b">
                    <BarsArrowDownIcon class="w-5 h-5 pr-1" />
                    Ascending order
                </button>
            </MenuItem>
            <MenuItem v-slot="{ active }" class="">
                <button @click="onAction('orderBy','DESC')" class="flex items-center hover:bg-theme-1 w-full py-1 text-gray-600 font-normal border-b">
                    <BarsArrowUpIcon class="w-5 h-5 pr-1" />
                    Descending order
                </button>
            </MenuItem>
        </template>
    </dropdown>
</template>
<script>
    import { ArrowsUpDownIcon, BarsArrowDownIcon, BarsArrowUpIcon } from '@heroicons/vue/24/outline'
    import Dropdown from '@/re-usables/components/Dropdown.vue'
    import { MenuItem } from '@headlessui/vue'
    export default {
        components: { ArrowsUpDownIcon, Dropdown, MenuItem, BarsArrowDownIcon, BarsArrowUpIcon },
        emits:{
            onThAction:null,
        },
        methods: {
            onAction(action,value) {
                this.$emit('onThAction', { actionName: action, actionValue: value });
            }
        },
    }
</script>
