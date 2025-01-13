<template>
    <dropdown>
        <template #caption>
            <!--<div class="flex items-center px-1 cursor-pointer">-->
            <div class="flex">
                Test
            </div>
            <!--</div>-->
        </template>
        <template #content>
            <MenuItem v-slot="{ active }" class="border-b border-b-2 divide-y divide-gray-100">
                yyuuu
            </MenuItem>
            <MenuItem v-slot="{ active }">
                <button @click="FnLogout" :class="[active ? 'bg-theme-1 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    Profile
                </button>
            </MenuItem>
            <MenuItem v-slot="{ active }">
                <button @click="FnLogout" :class="[active ? 'bg-theme-1 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    Change Password
                </button>
            </MenuItem>
            <MenuItem v-slot="{ active }">
                <button @click="FnLogout" :class="[active ? 'bg-theme-1 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    System Manual
                </button>
            </MenuItem>
            <MenuItem v-slot="{ active }">
                <button @click="FnLogout" :class="[active ? 'bg-theme-1 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    Logout
                </button>
            </MenuItem>

        </template>
    </dropdown>
</template>

<script>
    import Dropdown from '@/re-usables/components/Dropdown.vue'
    import { MenuItem } from '@headlessui/vue'
    export default {
        components: { Dropdown, MenuItem },
    }
</script>