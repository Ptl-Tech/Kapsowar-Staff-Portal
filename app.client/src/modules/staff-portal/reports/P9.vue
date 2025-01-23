<template>
    <div class="flex justify-center">
        <FormPageTemplate :title="'Get P9'" class="grow sm:max-w-2xl">
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Payroll Year" showMandatory="true" :valErrors="valErrors.year">
                        <WSelect required="true" v-model="form.year" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="years" v-for="year in years" :value="year">{{year}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <WButton @click="OnDownloadReport()" class="bg-green-500">Download</WButton>
            </template>
        </FormPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useRouter } from 'vue-router'
    export default {
        components: {...W},
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                pageProps: {
                    title: 'P9',
                    pageType: "form",
                    controller: 'Reports',
                },
                form: {
                    year: 0,
                },
                valErrors: [],
                periods: [],
                years: [],
                months: [],
                formMode: 'edit',
            }
        },
        created() {
            this.FnFetchSetups();
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/P9Form', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = this.appConfig.errors.dataFetchFailure;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        } else {
                            this.periods = data.response.periods;
                            if(this.periods != null){
                                for(let i =0;i <= this.periods.length-1;i++){
                                    if(this.years.indexOf(this.periods[i].Period_Year) == -1){
                                        this.years.push(this.periods[i].Period_Year);
                                    }
                                }
                            }
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
            OnDownloadReport() {
                var url = this.appConfig.baseApiRoute + this.pageProps.controller+"/P9Report";
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.form)
                };
                fetch(url, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.valErrors) {
                            this.valErrors = data.valErrors;
                        }
                        else if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {

                            var a = document.createElement("a");
                            a.href = "data:pdf;base64,"+data.response;
                            a.download = "P9-"+this.form.year+".pdf";
                            a.click();
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