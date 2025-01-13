<template>
    <td class="px-1 py-1 border-r text-xs" :colspan="colspan != ''?colspan:''">
        <span v-if="linkTo == ''"><slot></slot></span>
        <span v-else>
            <router-link :to="linkTo" class="text-blue-500 underline"><slot></slot></router-link>
        </span>
    </td>
</template>
<script>
    export default {
        props: { colspan: { default: 0 }, isBorder: { default: true }, linkTo: { default: '' } }
    }
</script>
