<template>
    <div class="flex flex-col">
        <div class="bg-gray-200 w-full px-2 py-0.5 rounded-t-md">
            <h3 class="font-semibold"><slot name="sectionName"></slot></h3>
        </div>
        <router-link :to="linkTo">
            <div class="flex w-full relative items-center bg-gray-100 rounded-b-md">
                <div class="flex transition-transform duration-300 w-full sm:pl-4 overflow-x-scroll sm:overflow-x-visible scrollbar-hidden" :style="{ transform: `translateX(-${currentIndex * (100 / itemsPerSlide)}%)` }">
                    <slot></slot>
                </div>
                <button @click="prevSlide" class="absolute transform h-full bg-gray-200 px-1 shadow rounded-bl-md scrollbar-hidden hidden sm:block">
                    &#10094;
                </button>
                <button @click="nextSlide" class="absolute top-1/2 right-0 transform -translate-y-1/2 bg-gray-200 rounded-br-md px-1 h-full shadow hidden sm:block">
                    &#10095;
                </button>
            </div>
        </router-link>
        
    </div>
    
</template>

<script>
    
    export default {
        props:{items:{default:[]},linkTo:{default:""}},
        data() {
            return {
                currentIndex: 0,
            };
        },
        computed: {
            itemsPerSlide() {
                if (window.innerWidth >= 1024) return 3; // Desktop
                if (window.innerWidth >= 768) return 3;  // Tablet
                return 1;                                // Mobile
            },
            maxIndex() {
                return Math.ceil(this.items.length / this.itemsPerSlide) - 1;
            }
        },
        methods: {
            nextSlide() {
                this.currentIndex = (this.currentIndex + 1) % (this.maxIndex + 1);
            },
            prevSlide() {
                this.currentIndex = (this.currentIndex - 1 + this.maxIndex + 1) % (this.maxIndex + 1);
            },
            /*getItemWidthClass() {
                //const widthClasses = ['w-1/2', 'w-1/3', 'w-1/4'];
                const widthClasses = ['w-1/2', 'w-1/4', 'w-1/6'];
                //const widthClasses = ['w-36', 'w-36', 'w-36'];
                return widthClasses[Math.min(this.itemsPerSlide - 2, widthClasses.length - 1)];
            },*/
            updateItemsPerSlide() {
                this.currentIndex = Math.min(this.currentIndex, this.maxIndex); // Adjust current index if necessary
            }
        },
        mounted() {
            window.addEventListener('resize', this.updateItemsPerSlide);
        },
        beforeDestroy() {
            window.removeEventListener('resize', this.updateItemsPerSlide);
        }
    };
</script>

<style scoped>
    .scrollbar-hidden::-webkit-scrollbar {
        display: none; /* Hide scrollbar for Chrome, Safari, and Opera */
    }
    .scrollbar-hidden {
        -ms-overflow-style: none; /* Hide scrollbar for Internet Explorer and Edge */
        scrollbar-width: none; /* Hide scrollbar for Firefox */
    }
</style>
