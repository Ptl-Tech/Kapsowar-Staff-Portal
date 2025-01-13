<template>
    <div class="">
        <Menu as="div" class="relative inline-blockx text-left">
            <div>
                <MenuButton class="w-full">
                    <slot name="caption"></slot>
                </MenuButton>
            </div>
            <transition enter-active-class="transition duration-100 ease-out"
                        enter-from-class="transform scale-95 opacity-0"
                        enter-to-class="transform scale-100 opacity-100"
                        leave-active-class="transition duration-75 ease-in"
                        leave-from-class="transform scale-100 opacity-100"
                        leave-to-class="transform scale-95 opacity-0">
                <MenuItems class="absolute z-50 right-0 mt-2 w-48 sm:w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 focus:outline-none">
                    <div class="px-1 py-1 h-auto">
                       <slot name="content"></slot>
                    </div>
                </MenuItems>
            </transition>
        </Menu>
    </div>
</template>

<script>
    import { Menu, MenuButton, MenuItems } from '@headlessui/vue'
    export default {
        components: { Menu, MenuButton, MenuItems },
        props: { caption: { default: 'Caption' } }
    }
</script>
