<template>
    <div>
        <div class="relative overflow-hidden transition-transform duration-200 hover:scale-105 flex flex-col justify-center items-center rounded-md">
            <img :src="image" alt="Item" class="w-full object-cover flex items-center" style="/*width:170px;*/ height: 170px"/>
            <div class="absolute bottom-0 bg-black bg-opacity-50 flex items-center justify-center w-full rounded-b-md">
                <h3 class="font-medium text-center text-sm text-gray-200"><slot name="title"></slot></h3>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: { image: {default:""}},
    };
</script>

<style scoped>
    img {
        height: auto; /* Adjust as needed */
    }
</style>
