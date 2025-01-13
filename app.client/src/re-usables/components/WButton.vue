<template>
    <button :type="type" class="bg-gray-500 inline-flex justify-center px-2 py-1 border border-transparent font-semibold text-xs text-white tracking-widest hover:bg-blue-700 active:bg-blue-900 focus:outline-none focus:border-blue-900 focus:shadow-outline-red disabled:opacity-25 transition ease-in-out duration-150 rounded-sm"><slot></slot></button>
</template>
<script>
    export default {
        props: {
            type: { default: 'button' },
        },
    }
</script>
