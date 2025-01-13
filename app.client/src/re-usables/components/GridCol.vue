<template>
    <div :class="[isBorder? 'border-r border-b p-1':'']">
        <slot></slot>
    </div>
</template>
<script>
    export default {
        props: { isBorder: { default: true } },
    }
</script>
