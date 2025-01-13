<template>
    <input :class="[formMode == 'view'? '!bg-gray-200':'','shadow appearance-none border w-full py-2 px-3 leading-tight focus:outline-none focus:shadow-outline disabled:opacity-40 text-gray-600']" :value="modelValue" @input="$emit('update:modelValue', $event.target.value)" ref="input" :readonly="formMode == 'view'? true:false">
    <!--<span v-if="maxLength != null" class="!pt-0 !text-xs">{{maxLength-modelValue.length}} characters remaining</span>-->
</template>

<script>
    import { defineComponent } from 'vue'

    export default defineComponent({
        props: ['modelValue','formMode','maxLength'],

        emits: ['update:modelValue'],

        methods: {
            focus() {
                this.$refs.input.focus()
            }
        },
        watch: {
            modelValue: function () {
                if (this.maxLength != null && this.modelValue.length > this.maxLength) {
                    var newValue = this.modelValue.substring(0, this.maxLength);
                    this.$emit('update:modelValue', newValue);
                }
            }
        }
    })
</script>
