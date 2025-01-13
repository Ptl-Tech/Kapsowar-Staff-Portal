<template>
    <div>
        <div class="flex flex-row">
            <div class="flex-grow overflow-x-auto no-scrollbarx">
                <span v-if="!(actionsProps != undefined && actionsProps.isEdit != undefined && actionsProps.isEdit == false)">
                    <WButton @click="FnEmitAction('edit')" class="flex items-center" :disabled="selectedRows.length == 0 || (selectedRows != null && selectedRows.length > 1)? true:false"><PencilIcon class="iconSmall" /> Edit</WButton>
                </span>
                <span v-if="!(actionsProps != undefined && actionsProps.isDelete != undefined && actionsProps.isDelete == false)">
                    <WButton @click="FnEmitAction('delete')" class="flex items-center" :disabled="selectedRows.length == 0? true:false"><TrashIcon class="iconSmall" /> Delete</WButton>
                </span>
                <span v-if="(actionsProps != undefined && actionsProps.moreActions != undefined)">
                    <slot name="moreActions"></slot>
                </span>
            </div>
        </div>
    </div>
</template>
<script>
    import WRouterLink from '@/re-usables/components/WRouterLink.vue'
    import Dropdown from '@/re-usables/components/Dropdown.vue'
    import WInput from '@/re-usables/components/WInput.vue'
    import WCheckbox from '@/re-usables/components/WCheckbox.vue'
    import WButton from '@/re-usables/components/WButton.vue'
    import { FunnelIcon, PlusIcon, ChevronDoubleDownIcon, PencilIcon, TrashIcon, DocumentIcon, ArrowsPointingOutIcon,ChevronDownIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { WButton, PlusIcon, PencilIcon, TrashIcon },
        props: { pageProps: { default: {} }, actionsProps: { default: {} }, selectedRows: { default: [] } },
        emits: {delete:null,edit:null},
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