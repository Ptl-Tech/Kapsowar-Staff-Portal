<template>
    <div class="relative w-full h-96 overflow-hidden">
        <div class="flex transition-transform duration-500"
             :style="{ transform: `translateX(-${currentIndex * 100}%)` }">
            <div v-for="(item, index) in items"
                 :key="index"
                 class="flex-shrink-0 w-full h-full">
                <img :src="item.image" :alt="item.alt" class="w-full h-full object-cover" />
            </div>
        </div>
        <button @click="prev" class="absolute left-0 top-1/2 transform -translate-y-1/2 shadow-lgx text-gray-50 font-semibold text-xl sm:text-2xl">❮</button>
        <button @click="next" class="absolute right-0 top-1/2 transform -translate-y-1/2 p-2 shadow-lgx text-gray-50 font-semibold text-xl sm:text-2xl">❯</button>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                currentIndex: 0,
                items: [
                    { image: 'https://textbookcentre.com/media/images/new_arrivals_Desktop_Ba.width-1600.format-webp-lossless.webp', alt: 'Slide 1' },
                    { image: 'https://textbookcentre.com/media/images/new_arrivals_Desktop_Ba.width-1600.format-webp-lossless.webp', alt: 'Slide 2' },
                    { image: 'https://textbookcentre.com/media/images/new_arrivals_Desktop_Ba.width-1600.format-webp-lossless.webp', alt: 'Slide 3' },
                ],
            };
        },
        methods: {
            next() {
                this.currentIndex = (this.currentIndex + 1) % this.items.length;
            },
            prev() {
                this.currentIndex = (this.currentIndex - 1 + this.items.length) % this.items.length;
            },
        },
    };
</script>

<style scoped>
    /* Optional: Add some styles to enhance the carousel */
</style>
