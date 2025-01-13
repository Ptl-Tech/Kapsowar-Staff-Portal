<template>
    <div>
        <div class="mt-3 py-4 bg-theme-1 flex justify-center w-full">
            <div class="py-2 text-center">
                <div class="text-center m-0 mb-2"><h6>&copy {{new Date().getFullYear()}} All rights reserved {{appConfig.client.name}}</h6></div>
                <DeveloperMark/>
            </div>
        </div>
    </div>
</template>
<script>
    import DeveloperMark from '@/re-usables/components/DeveloperApplicationMark.vue';
    
    export default {
        components: { DeveloperMark },
    }
 </script>