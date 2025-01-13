<template>
    <div class="rounded-md bg-white text-gray-600">
        <!--<span class="flex justify-center text-sm font-semibold">Categories</span>-->
        <!---->
        <h3 class="bg-theme-1 rounded-t-md px-2 font-medium py-0.5 text-sm">Shop By Category</h3>
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 12333</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 2</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 3</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
        <!---->
        <SidebarDropdown :active="this.$route.path.includes('/ess/approval-entry')? true:false">
            <template #caption>
                <span class="text-black">Category 4</span>
            </template>
            <template #content>
                <div>
                    <grid>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 1</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 2</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 3</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 4</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 5</router-link>
                        </grid-col>
                        <grid-col :isBorder="false" class="justify-center">
                            <router-link to="/">Sub-Category 6</router-link>
                        </grid-col>
                    </grid>
                </div>
            </template>
        </SidebarDropdown>
    </div>
</template>
<script>
    import Sidebar from "@/re-usables/components/Sidebar.vue";
    import SidebarLink from "@/re-usables/components/SidebarLink.vue";
    import SidebarHeading from "@/re-usables/components/SidebarHeading.vue";
    import SidebarDropdown from "@/re-usables/components/SidebarDropdown.vue";
    import SidebarMenu from "@/re-usables/components/SidebarMenu.vue";
    import Grid from "@/re-usables/components/Grid.vue";
    import GridCol from "@/re-usables/components/GridCol.vue";
    import { ChevronRightIcon,DocumentChartBarIcon } from '@heroicons/vue/24/outline';
    //
    export default {
        components: {
            Sidebar, SidebarLink, SidebarHeading, SidebarDropdown, SidebarMenu, Grid, GridCol
            , ChevronRightIcon, DocumentChartBarIcon
        },
    }
</script>