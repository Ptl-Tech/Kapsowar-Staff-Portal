<template>
    <div :class="['relative py-0.5',!isLast? 'border-b border-gray-500':'border-t border-gray-500']">
        <!--<div @click="open = ! open" :class="['flex items-center justify-between cursor-pointer pr-2 pl-1 py-1.5 text-xs leading-5 text-white gap-1 font-bold border-t',isLast? 'border-b':'']">-->
        <div @click="open = ! open" :class="['flex items-center justify-between cursor-pointer pr-2 pl-0 py-1 text-xs leading-5 text-white gap-1 font-medium hover:bg-theme-3 hover:rounded-md']">
            <span class="flex items-center gap-0.5"><slot name="caption" /></span>
            <ChevronDownIcon v-if="!open" class="h-3 w-3 font-bold" />
            <ChevronUpIcon v-if="open" class="h-3 w-3 font-bold" />
        </div>
        <transition
            enter-active-class="transition ease-out duration-200"
            enter-from-class="transform opacity-0 scale-95"
            enter-to-class="transform opacity-100 scale-100"
            leave-active-class="transition ease-in duration-75"
            leave-from-class="transform opacity-100 scale-100"
            leave-to-class="transform opacity-0 scale-95">
            <div v-show="open"
                    class="absolutex zx-50 mtx-2 roundedx-md shadow-lgx w-full "
                    :class="[widthClass, alignmentClasses]"
                    style="display: none; padding-left:11px !important; width:100% !important"
                    >
                <div class="roundedx-md ring-1x ring-blackx ring-opacity-5 border-l border-gray-500 pl-0.5" :class="contentClasses">
                    <slot name="content" />
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
    props: {
        align: {
            default: 'right'
        },
        width: {
            default: '48'
        },
        contentClasses: {
            default: () => ['pyx-1', 'bgx-white']
        },
        active: { default: true },
        isLast: { default: false }
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
