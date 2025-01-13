<template>
        <div :class="['notification fixed top-0 right-0 z-50 p-2 rounded-sm',bgClass]">
            <div class=" text-white flex justify-between items-center z-40 gap-2">
                <div>{{notification.message}}</div>
                <button title="Close" @click="FnClose()" class=""><XMarkIcon class="h-4 w-4" /></button>
            </div>
        </div>    
</template>

<script>
    import { XMarkIcon } from '@heroicons/vue/24/outline'

    export default {
        components: { XMarkIcon },
        props: {
            notification: { default: {} },
        },
        data() {
            return {
                textClass: '',
                bgClass: '',
                borderClass:'',
            }
        },
        mounted() {
            if (this.notification.theme == 'red') {
                this.textClass = "text-red-500";
                this.borderClass = "border-red-500";
                this.bgClass = "!bg-red-500";
            }
            else if (this.notification.theme == 'green') {
                this.textClass = "text-green-500";
                this.borderClass = "border-green-500";
                this.bgClass = "!bg-green-500";
            }
            else if (this.notification.theme == 'blue') {
                this.textClass = "text-blue-500";
                this.borderClass = "border-blue-500";
                this.bgClass = "!bg-blue-500";
            }
        },
        methods: {
            FnClose() {
                this.$root.notificationPopup.isShow = false;
            },
        },
    };
</script>
