<template>
    <!--<li :class="[decodeURI(this.$route.fullPath).includes(linkTo)?'border-b border-b-2 border-b-blue-500 text-blue-600':'text-gray-600','px-1 sm:px-2 pb-1 pt-0.5 bg-grayx-300 bg-theme-2 rounded-t-md !font-bold text-xs']">-->
    <!--<li :class="[decodeURI(this.$route.fullPath) == linkTo?'border-x-2 border-t-2 border-blue-500 !text-blue-500 ':'border-b','px-1 sm:px-2 pb-1 pt-0.5 !font-semibold !text-xs text-gray-500']">
        <router-link :to="linkTo" class="flex !w-auto !items-center">
            <slot></slot>
        </router-link>
    </li>-->
    <li :class="[decodeURI(this.$route.fullPath) == linkTo?'!bg-theme-1':'','px-1 sm:px-2 pb-1 pt-0.5 bg-grayx-300 bg-theme-2 rounded-t-md !font-bold text-xs']">
        <router-link :to="linkTo" class="flex font-semibold !w-auto text-xs !items-center">
            <slot></slot>
        </router-link>
    </li>
</template>
<script>
    export default {
        props: { linkTo: {default:""}}
    }
</script>
