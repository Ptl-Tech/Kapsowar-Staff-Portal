<template>
  <div :class="['flex items-center justify-center gap-2 shadow-lg bg-white rounded-md p-2 text-xs max-w-80']">
    <div class="flex flex-col gap-1 items-center">
      <div :class="['text-white']"><slot name="icon"></slot></div>
      <div :class="['font-bold text-gray-600']"><slot name="label"></slot></div>
      <div class="text-gray-500">
        <div :class="['flex gap-1 items-center']">
          <span v-if="!showLoader"><slot name="value"></slot></span>
          <span v-else>
            <Loader v-if="showLoader" class="!text-white" />
          </span>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
  import Loader from '@/re-usables/components/Loader.vue';

  import { DocumentCheckIcon } from '@heroicons/vue/24/outline'
  export default {
    components: { Loader, DocumentCheckIcon },
    props: { showLoader: { default: false }, isStatus: { default: false },color: { default: '' } },
  }
</script>

