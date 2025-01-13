<template>
    <!--<router-link :to="to" :class="[active? 'bg-theme-1 hover:bg-theme-1 text-white rounded-r-full':'hover:bg-theme-1 hover:rounded-r-full bg-theme-2',sidebarDropdown? 'border-l font-semibold':'font-bold',isLast?'':'border-b','pr-2 pl-1 py-1 text-xs leading-5 border-gray-300 flex items-center gap-1']">-->
    <router-link :to="to" :class="['py-1 mb-0.5 text-xs leading-5 borderx-gray-300 flex items-center gap-1 font-medium',active? 'bg-theme-2 hover:bg-theme-2 text-white rounded-md':'hover:bg-theme-2 hover:rounded-md bg-theme-1',sidebarDropdown? 'pl-1':'pl-2',isLast?'':'border-b border-gray-500']">
        <slot></slot>
    </router-link>
</template>
<script>
    export default{
        props: { to: '', active: { default: false }, sidebarDropdown: { default: false }, isLast: { default: false } },
    }
</script>


