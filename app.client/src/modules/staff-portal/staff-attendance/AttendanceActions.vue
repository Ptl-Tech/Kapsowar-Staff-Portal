<template>
    <div :class="['flex-col sm:flex-row','flex gap-0.5']">
        <WButton class="!bg-blue-500 rounded-sm" @click="OnSubmit('checkin')"><CheckIcon class="iconSmall" />Checkin</WButton>
        <WButton class="!bg-green-500 rounded-sm" @click="OnSubmit('checkout')"><CheckIcon class="iconSmall" />Checkout</WButton>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/ActionsPartComponents.js'
    import { useRouter } from 'vue-router'
    export default {
        props: { record: { default: null }, header: { default: null }, formData: { default: "" }, pageProps: { default: {} }, actionsProps: { default: {} } },
        components: { ...W },
        emits: ["onAction"],
        setup() {
            const router = useRouter();
            return { router };
        },
        data() {
            return {
                isModal: false
            }
        },
        methods: {
            OnSubmit(action, source = { GuiAllowed: true, next: null }) {
                if (source.GuiAllowed && (!confirm('Are you sure you want to ' + action + '?'))) { return; }
                var body;
                var url;
                var docNo = "";
                this.$emit('onValErrors', []);
                url = this.appConfig.baseApiRoute + this.pageProps.controller + "/Store";
                body = this.formData;
                body.type = action;
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(body)
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.valErrors) {
                            this.$emit('onValErrors', data.valErrors);
                            var msg = this.appConfig.errors.validationErrors;
                            this.$root.FnNotification({ type: "popup", theme: "red", message: msg });
                        }
                        else if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {
                            var res;
                            var msg = "";
                            res = data.response;
                            this.$router.go();
                            this.$root.FnNotification({ type: "popup", theme: "green", message: res });
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
        }
    }
</script>
