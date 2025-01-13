<template>
  <div>
      <Sidebar>
          <!--<sidebar-link to="/dashboard" :active="this.$route.path == '/dashboard'?true:false" :isLast="true"><Squares2X2Icon class="iconSmall" /> Dashboard</sidebar-link>-->
          <!---->
          <SidebarDropdown :active="this.$route.path.includes('/hmis/security')? true:false" :isLast="false">
              <template #caption>
                  <ShieldCheckIcon class="iconSmall" /> Security
              </template>
              <template #content>
                  <sidebar-link to="/hmis/security/visitor/list?status=Active-Today" :active="this.$route.fullPath.includes('/hmis/security/visitor')?true:false" :isLast="true">Visitors</sidebar-link>
              </template>
          </SidebarDropdown>
          <!---->
          <SidebarDropdown :active="this.$route.path.includes('/hmis/reception')? true:false" :isLast="false">
              <template #caption>
                  <UserGroupIcon class="iconSmall" /> Reception
              </template>
              <template #content>
                  <sidebar-link to="/hmis/reception/visitor/list?status=Active-Today" :active="this.$route.fullPath.includes('/hmis/reception/visitor')?true:false">Visitors</sidebar-link>
                  <sidebar-link to="/hmis/reception/patient/list" :active="this.$route.fullPath.includes('/hmis/reception/patient')?true:false" :isLast="false">Patients</sidebar-link>
                  <sidebar-link to="/hmis/reception/visit/list?status=Active-Today" :active="this.$route.fullPath.includes('/hmis/reception/visit')  && !this.$route.fullPath.includes('/hmis/reception/visitor')" :isLast="true">Visits List</sidebar-link>
              </template>
          </SidebarDropdown>
          <sidebar-link to="/hmis/triage/triage-patient/list?status=Pending-Today" :active="this.$route.fullPath.includes('/hmis/triage/triage-patient')?true:false" :isLast="false"><CalculatorIcon class="iconSmall" /> Triage</sidebar-link>
          <sidebar-link to="/hmis/consultation/consultation/list?status=New-Today" :active="this.$route.fullPath.includes('/hmis/consultation')?true:false" :isLast="false"><UsersIcon class="iconSmall" /> Consultations</sidebar-link>
          <sidebar-link to="/hmis/laboratory/test-header/list?status=Pending-Today" :active="this.$route.fullPath.includes('/hmis/laboratory')?true:false" :isLast="true"><AdjustmentsVerticalIcon class="iconSmall" /> Laboratory</sidebar-link>
      </Sidebar>
  </div>
</template>
<script>
  import Sidebar from "@/re-usables/components/Sidebar.vue";
  import SidebarLink from "@/re-usables/components/SidebarLink.vue";
  import SidebarHeading from "@/re-usables/components/SidebarHeading.vue";
  import SidebarDropdown from "@/re-usables/components/SidebarDropdown.vue";
  import SidebarMenu from "@/re-usables/components/SidebarMenu.vue";
  import {
    UserGroupIcon,IdentificationIcon,UsersIcon,AdjustmentsVerticalIcon,Squares2X2Icon,ShieldCheckIcon,CalculatorIcon
  } from '@heroicons/vue/24/outline'
  export default {
    components: {
          Sidebar, SidebarMenu, SidebarLink, UsersIcon, ShieldCheckIcon, AdjustmentsVerticalIcon, CalculatorIcon, SidebarDropdown, UserGroupIcon, SidebarHeading,IdentificationIcon,Squares2X2Icon
    },
  }
</script>
