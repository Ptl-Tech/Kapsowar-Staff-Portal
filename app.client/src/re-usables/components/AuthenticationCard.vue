<script setup>
  import AuthenticationCardLogo from '@/re-usables/components/AuthenticationCardLogo.vue';
</script>
<template>
  <div class="">
    <div class="flex h-screen text-xs md:text-sm">
      <div class="m-auto">
        <div class="flex items-center justify-center">
          <slot name="logo">
            <authentication-card-logo />
          </slot>
        </div>
        <div class="flex items-center justify-center mt-2">
          <div class="min-h-0 flex flex-col sm:justify-center items-center pt-1 pb-3 pr-2 pl-2 rounded-lg">
            <div class="flex justify-center items-center text-white font-semibold sm:pt-1 bg-theme-1 w-full rounded-t-md py-1 sm:py-2 uppercase">
              <slot name="title"></slot>
            </div>
            <div class="md:w-screen roundedx-md max-w-xs sm:max-w-sm md:max-sm-md px-2 sm:px-6 pt-2 sm:pt-6 pb-4 shadow-md overflow-hidden bg-white">
              <slot name="body"></slot>
              <div class="italic pt-2 text-xs flex justify-center">
                &copy {{new Date().getFullYear()}} All rights reserved {{appConfig.client.name}}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

</template>
