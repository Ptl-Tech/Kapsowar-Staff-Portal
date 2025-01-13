<template>
    <div class="mb-4 bg-gray-200 p-1 border border-redx-500 rounded-md">
        <div class="flex sm:gap-10 flex-col sm:flex-row text-sm">
            <div>
                <img :src="appConfig.solution.userAvatarGeneral" class="h-20 w-30" alt="client logo">
            </div>
            <!---->
            <div>
                <table class="table-auto">
                    <tr>
                        <td :class="tdhClass">Patient Name:</td>
                        <td :class="tdClass">{{record.Surname}} {{record.MiddleName}} {{record.LastName}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">Patient No.:</td>
                        <td :class="tdClass">{{record.PatientNo}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">ID/Passport No.:</td>
                        <td :class="tdClass">{{record.IDNumber}}</td>
                    </tr>
                </table>
            </div>
            <!---->
            <div>
                <table>
                    <tr>
                        <td :class="tdhClass">Gender:</td>
                        <td :class="tdClass">{{record.Gender}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">Date of Birth:</td>
                        <td v-if="record.DateOfBirth != undefined" :class="tdClass">{{$root.cplFnNavDateObjToString(record.DateOfBirth)}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">Age:</td>
                        <td :class="tdClass">{{record.AgeinYears}}</td>
                    </tr>
                </table>
            </div>
            <!---->
            <div>
                <table>
                    <tr>
                        <td :class="tdhClass">Phone Number:</td>
                        <td :class="tdClass">{{record.TelephoneNo1}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">Payment Type:</td>
                        <td :class="tdClass">{{record.PatientType}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">Bill Amount:</td>
                        <td :class="tdClass">{{parseFloat(record.Balance).toLocaleString()}}</td>
                    </tr>
                </table>
            </div>
            <!---->
            <div>
                <table>
                    <tr>
                        <td :class="tdhClass">Email:</td>
                        <td :class="tdClass">{{record.Email}}</td>
                    </tr>
                    <tr>
                        <td :class="tdhClass">Nationality:</td>
                        <td :class="tdClass">{{record.Nationality}}</td>
                    </tr>
                </table>
            </div>
        </div>

    </div>
    <hr />
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useRouter } from 'vue-router'
    export default {
        components: { ...W },
        props: { data: {default:null},patientNo: {default:""}, isLoading: false },
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                record: {},
                formMode: 'view',
                tdhClass: 'font-semibold pb-2 pr-1',
                tdClass: 'pb-2',
            }
        },
        created() {
            if(this.data == null){
                 this.FnFetchSetups();
            }else{
                this.record = this.data;
            }
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + 'GeneralHMIS/PatientCard' +'?patientNo=' + this.patientNo, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.record = data.response;
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            }
        }
        

    }
</script>