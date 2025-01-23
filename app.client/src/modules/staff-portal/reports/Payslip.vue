<template>
    <div class="flex justify-center">
        <FormPageTemplate :title="'Download Payslip'" class="grow sm:max-w-2xl">
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Payroll Year" showMandatory="true" :valErrors="valErrors.year">
                        <WSelect required="true" v-model="form.year" :formMode="formMode" @change="onYearChange()">
                            <option value="0">--select--</option>
                            <option v-if="years" v-for="year in years" :value="year">{{year}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Payroll Month" showMandatory="true" :valErrors="valErrors.month">
                        <WSelect required="true" v-model="form.month" :formMode="formMode">
                            <option value="0">--select--</option>
                            <option v-if="months" v-for="month in months" :value="month">{{GetMonthName(month)}}</option>
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
                    title: 'Payslip',
                    pageType: "form",
                    controller: 'Reports',
                },
                form: {
                    year: 0,
                    month: 0,
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
                fetch(this.appConfig.baseApiRoute +this.pageProps.controller+ '/PayslipForm', requestOptions)
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
                var url = this.appConfig.baseApiRoute + this.pageProps.controller+"/PayslipReport";
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
                            a.href = "data:pdf;base64," + data.response;
                            a.download = "Payslip-"+this.form.year+"-"+this.form.month+"-"+this.$root.authUser.userNo+".pdf";
                            a.click();
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            },
            onYearChange(){
                this.form.month = "0";
                this.months = [];
                var monthPeriods = this.periods.filter(obj => obj.Period_Year == this.form.year);
                if(monthPeriods != null){
                    for(let i =0;i <= monthPeriods.length-1;i++){
                        if(this.months == null || this.months.indexOf(monthPeriods[i].Period_Month) == -1){
                            this.months.push(monthPeriods[i].Period_Month);
                        }
                    }
                }
            },
            GetMonthName(monthNo){
                switch(monthNo) {
                  case 1:
                    return "January"
                    break;
                  case 2:
                    return "February"
                    break;
                  case 3:
                    return "March"
                    break;
                  case 4:
                    return "April"
                    break;
                  case 5:
                    return "May"
                    break;
                  case 6:
                    return "June"
                    break;
                  case 7:
                    return "July"
                    break;
                  case 8:
                    return "August"
                    break;
                  case 9:
                    return "September"
                    break;
                  case 10:
                    return "October"
                    break;
                  case 11:
                    return "November"
                    break;
                  case 12:
                    return "December"
                    break;
                  default:
                    return "";
                }
            }
        
        }
    }
</script>