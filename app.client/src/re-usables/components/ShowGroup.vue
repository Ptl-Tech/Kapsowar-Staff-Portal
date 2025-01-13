<template>
    <div class="grid grid-cols-1 p-1 sm:p-2 items-center justify-center">
        <div class="pb-1 font-semibold text-sm items-center"><slot name="label"></slot>:</div>
        <div class="text-sm items-center break-words"><slot name="value"></slot></div>
    </div>
</template>
