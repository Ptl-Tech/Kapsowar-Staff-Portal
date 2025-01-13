<template>
    <div class="flex flex-col justify-center items-center h-24 text-white rounded-md p-2">
        <div class="pl-1"><slot name="icon"></slot></div>
         <div class="font-semibold pb-0.5 text-xs text-center"><slot name="label"></slot></div>
         <div class="flex text-lg grow justify-center">
             <span v-if="!showLoader"><slot name="value"></slot></span>
             <span v-else>
                 <Loader v-if="showLoader" class="!text-white" />
             </span>
         </div>
    </div>
</template>
<script>
    import Loader from '@/re-usables/components/Loader.vue';
    export default {
        components: { Loader },
        props: { showLoader: { default: false }, isStatus: { default: false } },
    }
</script>

