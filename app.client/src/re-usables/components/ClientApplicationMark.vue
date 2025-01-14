<template>
    <div>
      <router-link :to="$root.isAuthenticated? '/dashboard':''" class="flex">
        <div class="flex items-center ml-1">
          <img :src="appConfig.client.logoPath" class="h-11 w-30" alt="client logo">
          <!--<div class="ml-2 font-bold uppercase text-xs sm:text-sm text-white hidden sm:flex"><span class="border-r border-r-gray-400 mr-1"></span>{{appConfig.solution.name}}</div>-->
          <!--<div v-if="$root.authUser != null" class="ml-2 font-bold uppercase text-xs sm:text-sm text-white hidden sm:flex"><span class="border-r border-r-gray-400 mr-1"></span>{{$root.authUser.branchCode}} Branch</div>-->
        </div>

      </router-link>
    </div>
</template>
