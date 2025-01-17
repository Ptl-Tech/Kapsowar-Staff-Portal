<template>
    <div :class="isLast?'':'border-b border-gray-500 mtx-1'">
        <router-link :to="to" :class="['py-1 my-0.5 text-xs leading-5 flex items-center gap-1 font-medium',active? 'bg-theme-1 hover:bg-theme-1 text-white rounded-md':'hover:bg-theme-1 hover:rounded-md bg-theme-2',sidebarDropdown? 'pl-1':'pl-2']">
            <slot></slot>
        </router-link>
    </div>
</template>
<script>
    export default{
        props: { to: '', active: { default: false }, sidebarDropdown: { default: false }, isLast: { default: false } },
    }
</script>