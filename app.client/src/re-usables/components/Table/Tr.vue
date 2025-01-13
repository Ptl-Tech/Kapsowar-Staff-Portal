<template>
    <tr class="border-b hover:bg-sky-200 focus:bg-sky-300 border-b cursor-pointer" :class="[index % 2 == 0? 'bg-white':'bg-gray-100',isSelected? '!bg-sky-200':'']" @click="FnOnRowClick($event)" @dblclick="FnOnRowDoubleClick($event)">
        <slot></slot>
    </tr>
</template>
<script>
    export default {
        props: { class: { default: '' }, index: { default: null }, isSelected: { default: false }, isDoubleClick: { default: true } },
        methods: {
            FnOnRowClick(event) {
                if (this.index != null && this.isDoubleClick) {
                    if (event.ctrlKey) {
                        this.$emit('OnRowClick', { clickType: 'single+control', index: this.index});
                    } else {
                        this.$emit('OnRowClick', { clickType: 'single', index: this.index});
                    }
                }
            },
            FnOnRowDoubleClick(event) {
                if (this.index != null && this.isDoubleClick) {
                    this.$emit('OnRowClick', { clickType: 'double', index: this.index});
                }
            },
        }
    }
</script>
