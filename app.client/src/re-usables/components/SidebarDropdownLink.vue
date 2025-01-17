<template>
    <Link :href="href" class="pr-2 pl-1 py-2 text-sm leading-5 bgTheme1 border-b border-gray-500 theme1-link flex items-center gap-1" :class="active? 'theme1-link-active':''"><slot></slot></Link>
</template>
<script>
    import { Link } from '@inertiajs/inertia-vue3'
    export default{
        props:{href:'',active:{default:''}},
        components:{Link}
    }
</script>


