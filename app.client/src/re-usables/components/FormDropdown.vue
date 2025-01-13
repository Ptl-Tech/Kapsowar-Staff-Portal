<template>
    <div class="relative w-full border-x border-b mb-2">
        <div @click="open = ! open;$emit('onToggle',open)" :class="[(active && open) || open? 'bg-red-800 !text-white':'bg-theme-2',  'flex justify-between items-center cursor-pointer pr-2 pl-1 py-1 text-sm leading-5 gap-1 border-t']">
            <slot name="caption" />
            <ChevronDownIcon v-if="!open" class="h-4 w-4 font-semibold"/>
            <ChevronUpIcon v-if="open" class="h-4 w-4 font-semibold"/>
        </div>
        <transition
            enter-active-class="transition ease-out duration-200"
            enter-from-class="transform opacity-0 scale-95"
            enter-to-class="transform opacity-100 scale-100"
            leave-active-class="transition ease-in duration-75"
            leave-from-class="transform opacity-100 scale-100"
            leave-to-class="transform opacity-0 scale-95">
            <div v-show="open"
                    class="absolutex zx-50 mtx-2 roundedx-md shadow-lgx w-full"
                    :class="[widthClass, alignmentClasses]"
                    style="display: none; padding-left:10px !important; width:100% !important"
                    >
                <div class="roundedx-md ring-1x ring-blackx ring-opacity-5 py-1 pr-1" :class="contentClasses">
                    <slot name="content" />
                    <div class="flex justify-center py-1 gap-2">
                        <slot name="submission"></slot>
                    </div>
                </div>
            </div>
        </transition>
    </div>
</template>

<script>
    import { onMounted, onUnmounted, ref } from 'vue'
    import { ChevronDownIcon, ChevronUpIcon } from '@heroicons/vue/24/outline';

    export default {
        components: { ChevronDownIcon, ChevronUpIcon },
        emits: ["onToggle"],
    props: {
        align: {
            default: 'right'
        },
        width: {
            default: '48'
        },
        contentClasses: {
            default: () => ['pyx-1', 'bg-white']
        },
        active:{default:true}
    },

    setup() {
        let open = ref(false)

        //const closeOnEscape = (e) => {
        //    if (open.value && e.key === 'Escape') {
        //        open.value = false
        //    }
        //}

        //onMounted(() => document.addEventListener('keydown', closeOnEscape))
        //onUnmounted(() => document.removeEventListener('keydown', closeOnEscape))

        return {
            open,
        }
    },
    mounted() {
        this.open = false;
        if (this.active == true) {
            this.open = true;
        }
    },
    computed: {
        widthClass() {
            return {
                '48': 'w-48',
            }[this.width.toString()]
        },

        alignmentClasses() {
            if (this.align === 'left') {
                return 'origin-top-left left-0'
            } else if (this.align === 'right') {
                return 'origin-top-right right-0'
            } else {
                return 'origin-top'
            }
        },
    },
    //watch:{
    //    active: function(){
    //        if(this.active == true){
    //            this.open = true;
    //        }
    //    }
    //}
}
</script>
