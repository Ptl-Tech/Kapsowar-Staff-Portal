
<template>
    <div>
        <div v-if="!isOneLine">
            <w-label class="mb-1" :value="label" :showMandatory="showMandatory" :showLoader="showLoader"></w-label>
            <div>
                <slot></slot>
            </div>
        </div>
        <div v-else>
            <div class="flex gap-1">
                <div class="grid grid-cols-1 sm:grid-cols-2">
                    <w-label class="flex grow" :value="label+' : '" :showMandatory="showMandatory" :showLoader="showLoader"></w-label>
                    <slot></slot>
                </div>
            </div>
        </div>
        <div v-if="valErrors != null && valErrors[0] != undefined" class="mt-0.5 !text-left">
            <w-label class="!bg-white !text-red-500 !text-xs !font-normal !rounded-none">{{valErrors[0].split(',')[0]}}</w-label>
        </div>
        <!--<span v-if="maxLength != null" class="!pt-0 !text-xs">{{maxLength[0]-maxLength[1]}} characters remaining</span>-->
    </div>
</template>
<script>
    import WLabel from '@/re-usables/components/WLabel.vue';

    export default {
        components: {WLabel },
        props: {
            label: {default:''},
            valErrors: null,
            showMandatory: { default: false },
            showLoader: { default: false },
            isOneLine: { default: false },
            maxLength: { default: null },
        },
        data() {
            return {

            }
        },
    }
</script>
