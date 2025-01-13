<template>
    <div>
        <div class="relative flex justify-between overflow-hidden">
            <button class="scroll-button left bg-white hover:bg-grayc-300 text-gray-700 font-bold z-10">
                <ChevronLeftIcon class="iconSmall"/>
            </button>
            <div class="flex gap-1 mlx-1 scroll-container">
                <slot></slot> 
            </div>
            <button class="scroll-button right bg-white hover:bg-grayc-300 text-gray-700 font-bold z-10">
                <ChevronRightIcon class="iconSmall font-bold" />
            </button>
        </div>
    </div>
</template>
<script>
    import { ChevronLeftIcon,ChevronRightIcon } from '@heroicons/vue/24/outline'
    export default {
        components: { ChevronLeftIcon, ChevronRightIcon },
        mounted() {
            const scrollContainer = document.querySelector(".scroll-container");
            const scrollLeftButton = document.querySelector(".scroll-button.left");
            const scrollRightButton = document.querySelector(".scroll-button.right");
            let scrollAmount = 0;
            const scrollStep = 200; //200 Adjust scroll distance as needed

            scrollLeftButton.addEventListener("click", function () {
                scrollAmount = Math.max(scrollAmount - scrollStep, 0);
                scrollContainer.style.transform = `translateX(-${scrollAmount}px)`;
            });

            scrollRightButton.addEventListener("click", function () {
                const containerWidth = scrollContainer.clientWidth;
                const contentWidth = scrollContainer.scrollWidth;
                const maxScroll = contentWidth - containerWidth;
                scrollAmount = Math.min(scrollAmount + scrollStep, maxScroll);
                scrollContainer.style.transform = `translateX(-${scrollAmount}px)`;
            });
        }
    }
</script>
<style>
    .scroll-container {
        width: calc(100% - 80px); /* 80px Adjust based on button widths and padding */
        transition: transform 0.3s ease;
    }
</style>