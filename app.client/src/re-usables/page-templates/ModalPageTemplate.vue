<template>
    <div>
        <div class="overlay relative sm:fixed z-30 sm:z-40">
            <div class="modal w-full sm:w-3/4 h-auto topx-96 sm:topx-64 left-1/2 rounded-lg overflow-auto">
                <div class="text-center bg-theme-1 rounded-t-md capitalize flex items-center justify-between">
                    <span></span>
                    <span>{{title}}</span>
                    <button class="text-right px-1 text-white font-bold" @click="$router.go(-1)" title="Go back"><span class="flex items-center"><ArrowLeftIcon class="h-4 w-4" /><span class="hidden sm:flex ml-1 text-xs">Back</span></span></button>
                </div>
                <div class="">
                    <slot></slot>
                </div>
                <div class="flex justify-center py-1 gap-2">
                    <slot name="submission"></slot>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { defineAsyncComponent } from 'vue';
    const WButton = defineAsyncComponent(() => import('@/re-usables/components/WButton.vue'));
    import { XMarkIcon,ArrowLeftIcon} from '@heroicons/vue/24/outline'
    export default {
        components: { WButton, XMarkIcon, ArrowLeftIcon },
        emits:["close"],
        props: {
            title: {default:""},
        }
    };
</script>
<style scoped>
    /* Add your modal styling here */
    .modal {
        position: absolute;
        top: 10%;
        /*left: 20%;*/
        /*transform: translate(-50%, -50%);*/
        transform: translateX(-50%);
        /*width: 300px;
        height: 200px;*/
        background-color: white;
        /*padding: 20px;*/
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        /*border-radius: 8px;*/
    }
    .overlay {
        /*position: fixed;*/
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0,0,0,0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        /*z-index: 1000;*/ /* Adjust the z-index to make sure it's on top of other elements */
    }
</style>
