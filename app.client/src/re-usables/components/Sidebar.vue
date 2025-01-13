<template>
    <div>
        <div class="min-h-screenx no-scrollbar w-full sm:w-full whitespace-nowrap shadow-t-xl">
            <!--<div class="sticky top-11 h-fullx">-->
            <div class="sticky top-11 h-fullx px-0.5 pt-0.5">
                <!--<span class="absolute right-0 bg-white text-gray-600 px-1 rounded-full text-xs" @click="closeSidebar()">x</span>-->
                <slot></slot>
            </div>
         </div>
    </div>
</template>
<script>
    export default {
        data(){
            return{
                authUser:null
            }
        },
        mounted(){
            this.authUser = JSON.parse(localStorage.getItem("authUser"));
        },
        methods:{
            closeSidebar(){
                this.$root.$refs.navbar.toggleSidebar();
            }

        }
    }
</script>
