<template>
    <div>
        <div class="flex flex-row">
            <div class="flex-grow overflow-x-auto no-scrollbarx">
                <div v-if="pageProps.pageType.toLowerCase() == 'list'" class="flex gap-1">
                    <span v-if="!(actionsProps != undefined && actionsProps.isEdit != undefined && actionsProps.isEdit == false)">
                        <WButton @click="FnEmitAction('edit')" class="flex items-center !bg-blue-500" title="Edit"><PencilIcon class="iconSmall" /> <span class="hidden md:flex">Edit</span></WButton>
                    </span>
                    <span v-if="!(actionsProps != undefined && actionsProps.isDelete != undefined && actionsProps.isDelete == false)">
                        <WButton @click="FnEmitAction('deleteRec')" class="flex items-center" title="Delete"><TrashIcon class="iconSmall" /> <span class="hidden md:flex">Delete</span></WButton>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.moreActions != undefined)">
                        <slot name="formMoreActions"></slot>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.reports != undefined)">
                        <slot name="formReports"></slot>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.isAttachments != undefined && actionsProps.isAttachments == true)">
                        <WButton @click="FnEmitAction('attachments')" class="flex items-center" :disabled="!actionsProps.isAttachments? true:false"><DocumentIcon class="iconSmall" /> Attachments</WButton>
                    </span>
                    <span v-if="(actionsProps != undefined && actionsProps.isReport != undefined && actionsProps.isReport == true)">
                        <WButton @click="FnEmitAction('exportList')" class="flex items-center" :disabled="!actionsProps.isExport? true:false"><DocumentIcon class="iconSmall" /> Export</WButton>
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    const WRouterLink = defineAsyncComponent(() => import('@/re-usables/components/WRouterLink.vue'))
    const Dropdown = defineAsyncComponent(() => import('@/re-usables/components/Dropdown.vue'))
    const WInput = defineAsyncComponent(() => import('@/re-usables/components/WInput.vue'))
    const WCheckbox = defineAsyncComponent(() => import('@/re-usables/components/WCheckbox.vue'))
    const WButton = defineAsyncComponent(() => import('@/re-usables/components/WButton.vue'))
    import { FunnelIcon, PlusIcon, ChevronDoubleDownIcon, PencilIcon, TrashIcon, DocumentIcon, ArrowsPointingOutIcon,ChevronDownIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { WInput, WCheckbox, Dropdown, WRouterLink, FunnelIcon, WButton, PlusIcon, ChevronDownIcon, ChevronDoubleDownIcon, PencilIcon, TrashIcon, DocumentIcon, ArrowsPointingOutIcon },
        props: { recordId: {default:null},pageProps: { default: {} }, actionsProps: { default: {} }, isTableRow: { default: {} } },
        emits: {deleteRec:null,edit:null,filter:null},
        data() {
            return {
                search: "",
                typingTimer: null,
                typingTimeout: 1000, //milliseconds
                actionButton: "group flex w-full items-center rounded-md px-2 py-2 text-sm",
            }
        },
        methods: {
            FnEmitAction(action) {
                this.$emit(action);
            },
        }
    }
</script>