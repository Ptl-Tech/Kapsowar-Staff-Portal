<template>
    <div class="fixed z-50 inset-0 flex items-center justify-center">
        <div :class="['fixedx topx-1/2 leftx-1/2 max-w-sm rounded-md shadow-md p-2',bgClass]">
            <div :class="['border-b border-b-1 text-center  font-semibold flex items-center justify-between',borderClass,textClass]">
                <span></span>
                <span class="flex items-center"><span>{{theTitle}}</span></span>
                <button class="text-right px-1 font-bold" @click="closeModal()" title="close"><XMarkIcon :class="['h-4 w-4 text-red-500',textClass]" /></button>
            </div>
            <div class="mt-2">
                <p v-html="notification.message"></p>
            </div>
        </div>
    </div>
</template>

<script>
    import WButton from '@/re-usables/components/WButton.vue';
    import { XMarkIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { WButton, XMarkIcon },
        props: {
            notification: { default: {}},
        },
        data() {
            return {
                textClass: '',
                bgClass: '',
                borderClass:'',
                theTitle:'',
            }
        },
        mounted() {
            if (this.notification.theme == 'red') {
                this.textClass = "text-red-500";
                this.borderClass = "border-red-500";
                this.bgClass = "!bg-red-100";
                this.theTitle = this.notification.title != undefined ?  this.notification.title:"Error";
            }
            else if (this.notification.theme == 'green') {
                //this.textClass = "text-green-500";
                this.borderClass = "border-green-500";
                this.bgClass = "!bg-green-100";
                 this.theTitle = this.notification.title != undefined ?  this.notification.title:"Message";
            }
            else if (this.notification.theme == 'blue') {
                this.textClass = "text-blue-500";
                this.borderClass = "border-blue-500";
                this.bgClass = "!bg-blue-100";
                 this.theTitle = this.notification.title != undefined ?  this.notification.title:"Info";
            }
        },
        methods: {
            closeModal() {
                this.$root.notificationModal.isShow = false;
            },
        },
    };
</script>

