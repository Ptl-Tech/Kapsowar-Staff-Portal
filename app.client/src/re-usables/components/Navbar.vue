<template>
  <header>
    <!--<nav class="bg-gray-300 fixed w-full z-50 top-0 shadow-lg h:10 flex sm:block items-center">-->
    <nav class="bg-gray-200 fixed w-full z-50 top-0 shadow-lg h:10 block items-center">
      <div class="flex justify-between items-center py-0.5 mt-0 mb-0 h-full px-2">
        <!--left side-->
        <div class="flex items-center">
          <button v-if="$root.authUser != null" @click="toggleSidebar()" id="btnSidebar" class="sm:mr-2 inline-flex items-center justify-center p-0 w-8 h-8 rounded-full text-white hover:text-gray-800 hover:bg-gray-300 focus:outline-none focus:bg-gray-100 focus:text-gray-500 transition duration-150 ease-in-out" title="Toggle sidebar">
            <Bars3Icon v-if="!isCloseSidebar" class="h-5 w-5" title="Toggle sidebar" />
            <XMarkIcon v-if="isCloseSidebar" class="h-5 w-5 text-theme-1" title="Close sidebar" />
          </button>
          <ClientApplicationMark />
        </div>
        <!--center-->
        <div class="text-xs font-semibold text-white pl-1 sm:pl-0 hidden sm:flex">
          <div class="flex gap-4">
            <NavbarLink to="/">HOME</NavbarLink>
            <NavbarLink to="/tenders">OPEN TENDERS</NavbarLink>
            <NavbarLink v-if="$root.authUser != null" to="/request-for-quotations">RFQs</NavbarLink>
            <NavbarLink v-if="$root.authUser != null" to="/request-for-proposals">RFPs</NavbarLink>
            <NavbarLink v-if="$root.authUser != null" to="/expression-of-interest">EOI</NavbarLink>
            <NavbarLink to="/auth/login">LOGIN</NavbarLink>         
            <NavbarLink to="/auth/register">REGISTER</NavbarLink>
          </div>
        </div>
        <div v-if="$root.authUser == null"></div>
        <!--right side-->
        <!--<div class="hidden sm:flex">-->
        <div v-if="$root.authUser != null" class="flex !items-center gap-1 sm:gap-4">
          <div>
            <NotificationsBell />
          </div>
          <div>
            <dropdown class="">
              <template #caption>
                <!--<div class="flex items-center px-1 cursor-pointer">-->
                <div class="flex items-center">
                  <span class="pr-1 font-semibold text-xs hidden md:flex text-white">{{$root.authUser.FirstName}}</span>
                  <span><img src="/images/profile-avatar.png" class="h-6 md:h-7 w-6 md:w-7 rounded-full border border-red-500 inline-block" alt="client logo"></span>
                </div>
                <!--</div>-->
              </template>
              <template #content>
                <MenuItem v-slot="{ active }" class="border-b border-b-2 divide-y divide-gray-100">
                  <span class="flex text-gray-600 group w-full px-2 py-2 text-sm">
                    Logged in as {{$root.authUser.FirstName}} {{$root.authUser.LastName}}<br />
                    Branch: {{$root.authUser.branchCode.substring(0,10)+'..'}}
                  </span>
                </MenuItem>
                <MenuItem v-slot="{ active }">
                  <router-link to="/profile" :class="[active ? 'bg-theme-2 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    <UserCircleIcon class="w-5 h-5 pr-1" />
                    Profile
                  </router-link>
                </MenuItem>
                <MenuItem v-slot="{ active }">
                  <router-link to="/change-password" :class="[active ? 'bg-theme-2 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    <LockClosedIcon class="w-5 h-5 pr-1" />
                    Change Password
                  </router-link>
                </MenuItem>
                <!--<MenuItem v-slot="{ active }">
                <button @click="FnLogout" :class="[active ? 'bg-theme-1 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    <InformationCircleIcon class="w-5 h-5 pr-1" />
                    System Manual
                </button>
            </MenuItem>-->
                <MenuItem v-slot="{ active }">
                  <button @click="FnLogout" :class="[active ? 'bg-theme-2 text-white' : 'text-gray-900','group flex w-full items-center rounded-md px-2 py-2 text-sm',]">
                    <ArrowLeftStartOnRectangleIcon class="w-5 h-5 pr-1" />
                    Logout
                  </button>
                </MenuItem>
              </template>
            </dropdown>
          </div>
        </div>
      </div>
    </nav>
  </header>
</template>
<script>
  import ClientApplicationMark from '@/re-usables/components/ClientApplicationMark.vue'
  import NotificationsBell from '@/re-usables/components/NotificationsBell.vue'
  import Dropdown from '@/re-usables/components/Dropdown.vue'
  import NavbarLink from '@/re-usables/components/NavbarLink.vue'
  import { BeakerIcon, ArrowLeftStartOnRectangleIcon, UserCircleIcon, Bars3Icon } from '@heroicons/vue/16/solid'
  import { InformationCircleIcon, LockClosedIcon, XMarkIcon } from '@heroicons/vue/24/outline'
  import { MenuItem } from '@headlessui/vue'
  import { useRouter } from 'vue-router'
  export default {
    setup() {
      const router = useRouter()
      return { router };
    },
    components: {
      ClientApplicationMark, Dropdown, MenuItem, BeakerIcon, Bars3Icon, LockClosedIcon, XMarkIcon,
      ArrowLeftStartOnRectangleIcon, UserCircleIcon, InformationCircleIcon, NotificationsBell, NavbarLink
    },
    data() {
      return {
        isCloseSidebar: false
      }
    },
    methods: {
      toggleSidebar() {
        var sidebar = document.getElementById('sidebar');
        var btnSidebar = document.getElementById('btnSidebar');
        var windowWidth = window.innerWidth;
        if (windowWidth < 640) {
          sidebar.classList.toggle("hidden");
          if (!sidebar.classList.contains("hidden")) {
            this.isCloseSidebar = true;
          } else {
            this.isCloseSidebar = false;
          }
        } else {
          sidebar.classList.toggle("sm:block");
          if (sidebar.classList.contains("sm:block") && !sidebar.classList.contains("hidden")) {
            sidebar.classList.add("hidden");
            btnSidebar.click();
          }
        }
      },
      FnLogout() {
        if (confirm('Are you sure you want to logout?')) {
          var obj = JSON.parse(localStorage.getItem("authUser"));
          const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(obj)
          };
          fetch('/api/authentication/LogoutUserSession', requestOptions)
            .then(response => {
              return response.json();
            })
            .then(data => {
              if (data.isLoggedOut) {
                this.$root.ClearSession();
                this.router.push("/auth/login");
                this.router.go();

              }
              else {
                this.$root.errorModal.message = this.appConfig.errors.generalFailure;
                this.$root.errorModal.isShow = true;
              }
            }).catch((error) => {
              this.$root.errorModal.message = this.appConfig.errors.generalFailure;
              this.$root.errorModal.isShow = true;
            });
        }
      }
    },
  }
</script>
