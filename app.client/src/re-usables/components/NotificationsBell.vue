<template>
    <div>
        <div class="sm:relative flex items-center group">
            <router-link to="/notification/list?status=not-viewed" class="relative p-1 sm:p-1.5 focus:outline-none focus:ring-2 focus:ring-blue-500 rounded-full bg-theme-2 shadow-md hover:bg-gray-400">
                <BellAlertIcon class="iconSmall" />
                <span v-if="$root.authUser.newNotifications != undefined && $root.authUser.newNotifications != 0" class="absolute right-0 top-0 bg-red-500 rounded-full !px-1 font-thin text-white text-xs hidden sm:flex hover:cursor-pointer text-red-500" style="font-size:9px">{{$root.authUser.newNotifications}}</span>
            </router-link>
        </div>
    </div>
</template>

<script>
    import { XMarkIcon,BellAlertIcon } from '@heroicons/vue/24/outline'

    export default {
        components: { XMarkIcon,BellAlertIcon },
    };

</script>
