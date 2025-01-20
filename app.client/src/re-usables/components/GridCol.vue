<template>
    <div :class="[isBorder? 'borderx-r borderx-b p-1':'']">
        <slot></slot>
    </div>
</template>
<script>
    export default {
        props: { isBorder: { default: true } },
    }
</script>
