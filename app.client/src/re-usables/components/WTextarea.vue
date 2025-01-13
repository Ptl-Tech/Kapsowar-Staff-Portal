<template>
    <div>
        <textarea :value="modelValue" @input="$emit('update:modelValue', $event.target.value)" ref="textarea" :class="[formMode == 'view'? '!bg-gray-200':'',classx != ''?classx:'','shadow appearance-none border w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline disabled:opacity-40']" :readonly="formMode == 'view'? true:false">
            <slot></slot>
        </textarea>
        <span v-if="maxLength != null" class="!pt-0 !text-xs">{{maxLength-modelValue.length}} characters remaining</span>
    </div>
</template>
<script>
    import { defineComponent } from 'vue'

    export default defineComponent({
        props: {
            modelValue: { default: "" }, 
            formMode: { default: "" },
            required: { default: "" },
            classx: { default: "" },
            maxLength: { default: null },
        },
        emits: ['update:modelValue'],
        methods: {
            focus() {
                this.$refs.textarea.focus()
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
