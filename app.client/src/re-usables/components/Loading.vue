<template>
    <div>
        <div v-if="isLoading" class="loading-overlay flex items-center z-40">
            <span class="loader-sm"></span> <span class="ml-1">{{message}}</span>
        </div>
    </div>
</template>
<script>
    export default {
        props: {
            message: { default: 'Loading...Please wait' },
            isLoading: { default: true }
        },
        data() {
            return {

            }
        }
    }
</script>