<template>
    <select :value="modelValue" @input="$emit('update:modelValue', $event.target.value);onCompanyChange($event)" ref="select" :class="[formMode == 'view'? '!bg-gray-200':'',isTomSelect?'':'border p-2','cursor-pointer shadow appearance-nonex w-full text-gray-700 leading-tight focus:outline-none focus:shadow-outline disabledx:opacity-40 disabled:bg-gray-200']" :readonly="formMode == 'view'? true:false" :disabled="formMode == 'view'? true:false">
        <option value="">--select--</option>
        <option v-if="companies" v-for="option in companies" :value="option.id">{{option.caption != undefined?option.caption:option.name }}</option>
    </select>
</template>
<script>
    import { defineComponent } from 'vue'
    export default defineComponent({
        props: { modelValue: { default: '' }, formMode: { default: '' }, isTomSelect: { default: false }},

        emits: ['update:modelValue'],
        data() {
            return {
                companies: [
                    { id: "AIC Kapsowar Mission Hospital", name: "Kapsowar Mission Hospital" },
                    { id: "KAPSOWAR MTC LIVE", name: "Kapsowar Medical Training College" },
                ]
            }
        },
        methods: {
            focus() {
                this.$refs.select.focus()
            },
            onCompanyChange(event) {
                //if (event.target.value != "KRCS Test Co.") {
                //    this.$root.errorModal.message = "Only the KRCS Test Co. Nav Company is currently allowed because the portal is still under testing & develoment.";
                //    this.$root.errorModal.isShow = true;
                //    event.target.value = "";
                //    this.$emit('update:modelValue', event.target.value)
                //}
                //
            }
        }
    })
</script>
