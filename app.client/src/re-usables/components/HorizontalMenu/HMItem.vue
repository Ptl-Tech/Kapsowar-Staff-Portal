<template>
    <!--<div :class="[decodeURI(this.$route.fullPath).includes(linkTo)?'border-b border-b-2 border-b-blue-500 text-blue-600':'text-gray-600','flex-shrink-0 text-xs font-semibold']">-->
    <div :class="[decodeURI(this.$route.fullPath) == linkTo?'bg-theme-1':'bg-theme-2','rounded-t-md flex-shrink-0 text-xs font-semibold']">
        <router-link :to="linkTo" class="flex font-semibold !w-auto text-xs !items-center bgx-gray-300 px-1 py-0.5 ">
            <slot></slot> <label v-if="isTotal && total > 0" :class="['rounded-full !px-1 ml-0.5 !text-xs !font-normal',decodeURI(this.$route.fullPath) == linkTo? '!bg-white text-red-500':'bg-red-500 text-white']">{{total}}</label>
        </router-link>
    </div>
</template>
<script>
    export default {
        props: { linkTo: { default: "" }, isTotal: { default: false }, total: { default:0 } }
    }
</script>