import {defineAsyncComponent} from 'vue'
import WTd from '@/re-usables/components/Table/Td.vue';
import WTh from '@/re-usables/components/Table/Th.vue';
import WTr from '@/re-usables/components/Table/Tr.vue';
import WRouterLink from '@/re-usables/components/WRouterLink.vue';
const WCheckbox = defineAsyncComponent(()=> import('@/re-usables/components/WCheckbox.vue'));
import WButton from '@/re-usables/components/WButton.vue';
import ListPageTemplate from "@/re-usables/page-templates/ListPageTemplate.vue";
import ListPartPageTemplate from "@/re-usables/page-templates/ListPartPageTemplate.vue";
import { PlusIcon } from '@heroicons/vue/24/outline';
import EllipsisMenu from '@/re-usables/components/EllipsisMenu.vue';
import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
import MenuLink from '@/re-usables/components/MenuTabs/MenuLink.vue';
//
const W = {}
W.WTd = WTd;
W.WTh = WTh;
W.WTr = WTr;
W.WCheckbox = WCheckbox;
W.ListPageTemplate = ListPageTemplate;
W.ListPartPageTemplate = ListPartPageTemplate;
W.WButton = WButton;
W.WRouterLink = WRouterLink;
W.PlusIcon = PlusIcon;
W.EllipsisMenu = EllipsisMenu;
W.MenuTabs = MenuTabs;
W.MenuLink = MenuLink;
export { W }
/*
 import WTd from '@/re-usables/components/Table/Td.vue'
import WTh from '@/re-usables/components/Table/Th.vue'
import WTr from '@/re-usables/components/Table/Tr.vue'
import WRouterLink from '@/re-usables/components/WRouterLink.vue'
import WCheckbox from '@/re-usables/components/WCheckbox.vue'
import WButton from '@/re-usables/components/WButton.vue'
import ListPageTemplate from "@/re-usables/page-templates/ListPageTemplate.vue"
import ListPartPageTemplate from "@/re-usables/page-templates/ListPartPageTemplate.vue"
import { PlusIcon } from '@heroicons/vue/24/outline'
import EllipsisMenu from '@/re-usables/components/EllipsisMenu.vue'
import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue'
import MenuLink from '@/re-usables/components/MenuTabs/MenuLink.vue'
 */