<template>
    <select :value="modelValue" @input="$emit('update:modelValue', $event.target.value)" ref="select" :class="[formMode == 'view'? '!bg-gray-200':'',isTomSelect?'':'border p-2','cursor-pointer shadow appearance-nonex w-full text-gray-700 leading-tight focus:outline-none focus:shadow-outline disabledx:opacityx-20']" :readonly="formMode == 'view'? true:false" :disabled="formMode == 'view'? true:false">
        <slot></slot>
    </select>
</template>
<script>
    import { defineComponent } from 'vue'
    export default defineComponent({
        props: { modelValue: { default: '' }, formMode: { default: '' }, isTomSelect: { default: false }},
        emits: ['update:modelValue'],
        methods: {
            focus() {
                this.$refs.select.focus()
            }
        }
    })
</script>

