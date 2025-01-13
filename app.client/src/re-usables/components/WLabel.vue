<template>
    <label class="block font-medium text-sm text-gray-700 rounded-sm px-1">
        <span v-if="value" class="flex items-center">
            {{ value }}
            <span class="text-red-500" title="Required">{{showMandatory?'*':''}}</span>
            <Loader v-if="showLoader" class="ml-1" />
        </span>
        <span v-else><slot /></span>
    </label>
</template>

<script>
    import Loader from '@/re-usables/components/Loader.vue';
export default {
        props: ['value', 'showMandatory','showLoader'],
        components: { Loader },
}
</script>
