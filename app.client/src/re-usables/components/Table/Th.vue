<template>
    <th class=" text-left px-1 py-1 sm:py-2 border-r bg-theme-2 hover:bg-theme-2 text-xs">
        <slot></slot>
    </th>
</template>
<script>
    export default{
        methods: {
            onThAction(actionParams) {
                actionParams.colName = this.colName;
                this.$emit('onThAction', actionParams);
            }
        },
    }
</script>

