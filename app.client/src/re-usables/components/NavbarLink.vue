<template>
    <router-link :to="to" :class="['text-gray-700 font-bold hover:border-b-2 hover:border-red-500',to == $route.path? 'border-b-2 border-red-500':'']">
        <slot></slot>
    </router-link>
</template>
<script>
    export default{
        props: { to: '', active: { default: false }, sidebarDropdown: { default: false }, isLast: { default: false } },
    }
</script>


