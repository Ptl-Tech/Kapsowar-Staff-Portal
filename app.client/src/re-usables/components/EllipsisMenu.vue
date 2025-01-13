<template>
    <div class="relative">
        <button @click="$root.activeActionRef != docNo? $root.activeActionRef = docNo:$root.activeActionRef = ''" type="button" class="ellipsis-menu flex items-center text-gray-400 hover:text-gray-600 focus:outline-none h-full w-full" id="menu-button" aria-expanded="true" aria-haspopup="true">
            <span class="sr-only"></span>
            <EllipsisHorizontalIcon class="h-full w-6 text-blue-500 font-semibold ellipsis-menu" />
        </button>
        <div v-show="$root.activeActionRef == docNo" class="z-10 origin-top-right absolute right-12 md:right-20 mtx-2 !text-xs w-auto max-w-56 rounded-md shadow-lg bg-gray-100 ring-1 ring-black ring-opacity-5 divide-y divide-gray-100 focus:outline-none" role="menu" aria-orientation="vertical" aria-labelledby="menu-button" tabindex="-1">
            <slot></slot>
        </div>
    </div>
</template>

<script>
    import { EllipsisHorizontalIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { EllipsisHorizontalIcon },
        props: {
            docNo: null,
        }
    };
</script>
