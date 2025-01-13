<template>
    <router-link :to="to" :class="[active? 'bg-gray-500 text-white hover:bg-blue-700':'bg-gray-500 text-white hover:bg-blue-700','inline-flex justify-center px-1 py-1 bg-gray-500 border border-transparent font-semibold !text-xs text-white tracking-widest hover:bg-blue-700 active:bg-blue-900 focus:outline-none focus:border-blue-900 focus:shadow-outline-blue disabled:opacity-25 transition ease-in-out duration-150']">
        <slot></slot>
    </router-link>
</template>
<script>
    export default {
        props: { to: '', active: { default: false }},
    }
</script>


