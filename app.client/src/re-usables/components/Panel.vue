<template>
    <div class="flex justify-center">
        <div class="panel w-screen sm:w-full">
            <div class="panel-header capitalize bg-theme-2 flex justify-between py-1 text-white text-sm w-full rounded-t-xl">
                <span></span>
                <span class="font-semibold">{{title}}</span>
                <button class="text-right px-2 text-white font-semibold" @click="$router.go(-1)" title="Go back"><span class="flex items-center"><ArrowLeftIcon class="h-4 w-4" /><span class="hidden sm:flex ml-1 text-xs">Back</span></span></button>
            </div>
            <div class="panel-body p-2 w-full border">
                <slot></slot>
                <div class="flex gap-1 justify-center py-2">
                    <slot name="submission"></slot>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import {ArrowLeftIcon } from '@heroicons/vue/24/outline'

    export default {
        components: {ArrowLeftIcon },
        props: {
            title: {default:'',submit:false},
        },
    }
</script>
