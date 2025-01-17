<template>
    <div class="overlay z-40">
        <div class="modal w-full sm:w-3/4 h-auto top-96 sm:top-64 left-1/2 rounded-lg ">
            <div class="text-center bgAppTheme2 rounded-t-md capitalize flex items-center justify-between">
                <span></span>
                <span>Rejection Comment</span>
                <button class="text-right px-1 text-white font-bold" @click="$emit('close')" title="Close"><span class="flex items-center"><XMarkIcon class="h-4 w-4" /><span class="hidden sm:flex ml-1 text-xs">Close</span></span></button>
            </div>
            <div class="px-2">
                <field-group label="Rejection comments" :showMandatory="true">
                    <WTextarea v-model="comments" :maxLength="250"></WTextarea>
                </field-group>
            </div>
            <div class="flex justify-center py-1 gap-2">
                <WButton @click="onSubmit()">Submit</WButton>
            </div>
        </div>
    </div>
</template>

<script>
    import WButton from '@/re-usables/components/WButton.vue';
    import WTextarea from '@/re-usables/components/WTextarea.vue';
    import fieldGroup from '@/re-usables/components/FieldGroup.vue';
    import { XMarkIcon,ArrowLeftIcon} from '@heroicons/vue/24/outline'
    export default {
        components: { WButton, XMarkIcon, ArrowLeftIcon,WTextarea,fieldGroup },
        emits:["comments","close"],
        props: {
            title: {default:""},
        },
        data() {
            return {
                comments: ""
            }
        },
        methods:{
            onSubmit(){
                this.$emit("onSubmitReject",this.comments);
            }
        }
    };
</script>
<style scoped>
    /* Add your modal styling here */
    .modal {
        position: absolute;
        /*top: 20%;
        left: 50%;*/
        transform: translate(-50%, -50%);
        /*width: 300px;
        height: 200px;*/
        background-color: white;
        /*padding: 20px;*/
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        /*border-radius: 8px;*/
    }
    .overlay {
        position: fixed;
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
