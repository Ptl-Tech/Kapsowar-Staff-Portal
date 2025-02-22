<template>
    <div>
        <router-link :to="$root.isAuthenticated? '/dashboard':''" class="flex">
            <div v-if="$root.authUser.navCompany == 'AIC Kapsowar Mission Hospital'" class="flex items-center ml-1">
                <img :src="appConfig.client.logoPath" class="h-11 w-30 bg-white rounded-full" alt="client logo">
                <div class="ml-2 font-bold uppercase text-xs sm:text-sm text-white hidden sm:flex"><span class="border-r border-r-gray-50 mr-1"></span>{{appConfig.solution.name}}</div>
            </div>
            <div v-if="$root.authUser.navCompany == 'KAPSOWAR MTC LIVE'" class="flex items-center ml-1">
                <img :src="appConfig.client.logoPath2" class="h-11 w-30 bg-white rounded-full" alt="client logo">
                <div class="ml-2 font-bold uppercase text-xs sm:text-sm text-white hidden sm:flex"><span class="border-r border-r-gray-50 mr-1"></span>{{appConfig.solution.name}}</div>
            </div>
            <!----->
        </router-link>
    </div>
</template>
