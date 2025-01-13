<template>
    <li :class="[activeTab == name?'border-b-2 border-blue-500 !text-blue-500':'border-b','px-1 sm:px-2 pb-1 pt-0.5 !font-bold !text-sm text-gray-500']">
        <button @click="$emit('activeTab',name)" class="flex font-semibold !w-auto !items-center">
            <slot></slot>
        </button>
    </li>
    <!--<li :class="[activeTab == name?'!bg-theme-2':'','px-1 sm:px-2 pb-1 pt-0.5 bg-grayx-300 bg-theme-1 rounded-t-md !font-bold text-xs']">
        <button @click="$emit('activeTab',name)" class="flex font-semibold !w-auto text-xs !items-center">
            <slot></slot>
        </button>
    </li>-->
</template>
<script>
    export default {
        props: { name: {default:""},activeTab: {default:""}},
        emits:["activeTab"],
    }
</script>
