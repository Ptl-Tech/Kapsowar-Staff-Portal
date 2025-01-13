<template>
    <div>
        <div class="flex items-center ml-1">
            <div class="flex flex-col items-center text-yellow-300 font-mono">
                <div class="ml-1 font-bold uppercase text-sm">{{appConfig.solution.name}} for {{appConfig.client.name}}</div>
                <div class="ml-1">A solution by:</div>
                <div><img :src="appConfig.developer.logoPath" class="h-10 w-10 rounded-full" alt="developer logo"></div>
                <div class=""><a :href="appConfig.developer.websiteUrl" class="underline" target="_blank">{{appConfig.developer.name}}</a></div>
                <div class="italic">** {{appConfig.developer.tagline}} **</div>
                <div class=""><a :href="appConfig.developer.websiteUrl" class="underline" target="_blank">Website: {{appConfig.developer.websiteUrl}}</a></div>
            </div>
        </div>
    </div>
</template>