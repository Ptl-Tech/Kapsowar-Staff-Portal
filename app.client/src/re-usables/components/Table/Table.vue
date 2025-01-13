<template>
    <div class="flex flex-col">
        <div class="flex-grow overflow-auto">
            <table class="table-auto w-full relative rounded-t-lg">
                <thead class="bg-gray-100 text-gray-600 text-sm font-semibold sticky top-0 border rounded-t-lg">
                    <tr>
                        <slot name="thead"></slot>
                    </tr>
                </thead>
                <tbody class="text-xs sm:text-sm border">
                    <slot name="tbody"></slot>
                </tbody>
            </table>
        </div>
    </div>
</template>
<script>
    //import xCheckbox from '@/Components/Checkbox'
    //export default {
    //    components: { xCheckbox },
    //}
</script>
