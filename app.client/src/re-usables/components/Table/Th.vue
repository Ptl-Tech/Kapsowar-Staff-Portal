<template>
    <th class=" text-left px-1 py-1 sm:py-2 border-r bg-theme-1 hover:bg-theme-1">
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

