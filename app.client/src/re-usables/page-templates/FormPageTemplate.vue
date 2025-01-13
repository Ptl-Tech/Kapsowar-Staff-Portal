<template>
    <div>
        <panel v-if="hasPanel" :title="title">
            <slot></slot>
            <template #submission>
                <slot name="submission"></slot>
            </template>
        </panel>
        <div v-else>
            <slot></slot>
            <slot name="submission"></slot>
        </div>
    </div>
</template>
<script>
    import Panel from '@/re-usables/components/Panel.vue';
    export default {
        props: { title: { default: "" },hasPanel: { default: true } },
        components: { Panel },
    }
</script>