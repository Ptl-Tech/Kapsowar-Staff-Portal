<template>
    <div :class="[pageProps.pageType == 'list'? 'flex-row':'flex-col sm:flex-row','flex justify-center gap-0.5']">
        <WButton class="!bg-green-500 rounded-sm" @click="OnSubmit('save')"><CheckIcon class="iconSmall" />Save Marks</WButton>
        <WButton class="!bg-red-500 rounded-sm" @click="OnSubmitMarks('save')"><CheckIcon class="iconSmall" />Submit Marks</WButton>
        <WButton class="!bg-yellow-500 rounded-sm" @click="xFnDownloadBCReport({route:'StudentUnits/ClassListReport',fileName:'Class list.pdf',body:pageProps.unitData,fileExtension:'PDF',root:$root,appConfig:appConfig})"><PrinterIcon class="iconSmall" />Class List</WButton>
        <WButton class="!bg-yellow-500 rounded-sm" @click="xFnDownloadBCReport({route:'StudentUnits/ClassAttendanceReport',fileName:'Class Attendance.pdf',body:pageProps.unitData,fileExtension:'PDF',root:$root,appConfig:appConfig})"><PrinterIcon class="iconSmall" />Class Attendance</WButton>
        <WButton class="!bg-yellow-500 rounded-sm" @click="xFnDownloadBCReport({route:'StudentUnits/MarkSheetReport',fileName:'Scoresheet.pdf',body:pageProps.unitData,fileExtension:'PDF',root:$root,appConfig:appConfig})"><PrinterIcon class="iconSmall" />Scoresheet</WButton>
        <!--<WButton v-if="record.Status != undefined && pageProps.pageType == 'form' && formData.myAction != 'view'" class="!bg-green-500 rounded-sm" @click="OnSubmit('submit')"><CheckIcon class="iconSmall" />Save & Send for Approval</WButton>-->
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/ActionsPartComponents.js'
    import { useFileManagementComposable } from '@/re-usables/composables/FileManagementComposable.js';
    import { useRouter } from 'vue-router'
    export default {
        props: { records: { default: null }, header: { default: null }, formData: { default: "" }, pageProps: { default: {} }, actionsProps: { default: {} } },
        components: { ...W },
        emits: ["onAction"],
        setup() {
            const router = useRouter();
            const { xFnDownloadBCReport } = useFileManagementComposable();
            return { router, xFnDownloadBCReport };
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
                url = this.appConfig.baseApiRoute + this.pageProps.controller + "/MarksEntry";
                //
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.records)
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
                            var msg = "Saved successfully";
                            res = data.response;
                            this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
            OnSubmitMarks(action, source = { GuiAllowed: true, next: null }) {
                if (source.GuiAllowed && (!confirm('Are you sure you want to submit? PLEASE NOTE YOU WILL NO LONGER BE ABLE TO EDIT THE MARKS.'))) { return; }
                var body;
                var url;
                var docNo = "";
                this.$emit('onValErrors', []);
                url = this.appConfig.baseApiRoute + this.pageProps.controller + "/SubmitMarks";
                //
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.records)
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
                            var msg = "Saved successfully";
                            res = data.response;
                            this.$root.FnNotification({ type: "popup", theme: "green", message: msg });
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>
