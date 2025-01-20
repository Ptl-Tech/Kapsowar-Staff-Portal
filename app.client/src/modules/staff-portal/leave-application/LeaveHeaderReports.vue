<template>
    <div class="flex flex-col divide-y">
        <MenuItem v-slot="{ active }">
            <button class="actionStyles">
                Report 1
            </button>
        </MenuItem>
        <MenuItem v-slot="{ active }">
            <router-link to="/ess/report2" class="actionStyles">Report 2</router-link>
        </MenuItem>
    </div>
</template>
<script>
    import { MenuItem } from '@headlessui/vue'
    export default {
        props: { pageProps: { default: {} } },
        components: { MenuItem },
    }
</script>
