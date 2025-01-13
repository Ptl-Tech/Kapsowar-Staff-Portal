<template>
    <div>
    <div class="modal modal-content relative" v-if="showModal">
        <div class="border-b text-center text-green-500">Confirmation</div>
        <div class="">
            <p>{{message}}</p>
        </div>
        <div class="flex justify-center absolute inset-x-0 bottom-0 pb-2 gap-2">
            <w-button @click="confirm" class="!bg-green-500">Confirm</w-button>
            <w-button @click="cancel" class="!bg-red-500">Cancel</w-button>
        </div>
    </div>
        </div>
</template>

<script>
    import WButton from '@/re-usables/components/WButton.vue';
    export default {
        components: { WButton },
        props: {
            message: String,
            showModal: Boolean,
        },
        methods: {
            confirm() {
                this.$emit('confirmed');
            },
            cancel() {
                this.$emit('canceled');
            },
        },
    };
</script>

<style scoped>
    /* Add your modal styling here */
    .modal {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 300px;
        height: 200px;
        background-color: white;
        padding: 20px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        border-radius: 8px;
    }
</style>
