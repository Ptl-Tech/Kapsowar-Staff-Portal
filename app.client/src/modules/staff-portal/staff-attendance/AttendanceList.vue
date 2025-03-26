<template>
    <!--<Actions :pageProps="pageProps" />-->
    <ListPageTemplate ref="tpList" :pageProps="this.pageProps" :actionsProps="this.actionsProps" @OnFetchData="OnFetchData($event)">
        <template #thead>
            <WTh>Date</WTh>
            <WTh>Staff Name</WTh>
            <WTh>Time In</WTh>
            <WTh>Time Out</WTh>
            <WTh>Hours Worked</WTh>
            <WTh>Location</WTh>
            <WTh>Comments</WTh>
        </template>
        <template v-for="(record,index) in records" #[`tbody-${index}`]>
            <WTd>{{$root.xFnNavDateObjToString(record.Date)}}</WTd>
            <WTd>{{record.Full_Name}}</WTd>
            <WTd>{{record.Time_In}}</WTd>
            <WTd>{{record.Time_Out}}</WTd>
            <WTd>{{record.Hours_Worked}}</WTd>
            <WTd>
                - Checkin: {{record.Signin_Location}} ({{record.Signin_Location_Coordinates}}) <br />
                - Checkout: {{record.Signout_Location}} <span v-if="record.Signout_Location_Coordinates != ''">({{record.Signout_Location_Coordinates}})</span>
            </WTd>
            <WTd>
                - Checkin: {{record.Sign_in_Comments}} <br />
                - Checkout: {{record.Sign_out_Comments}}
            </WTd>
        </template>
        <template #listMoreActions>
            <Actions :pageProps="pageProps" :formData="location" />
        </template>
    </ListPageTemplate>
</template>
<script>
    import { W } from '@/re-usables/imports/ListPageComponents.js';
    import Actions from '@/modules/staff-portal/staff-attendance/AttendanceActions.vue';
    //import * as location from '@/modules/staff-portal/staff-attendance/location.js';
    import { useRouter } from 'vue-router'
    export default {
        components: { Actions, ...W },
        setup() {
            const router = useRouter();
            return { router };
        },
        data() {
            return {
                records: [],
                pageProps: {
                    title: 'Attendance List',
                    pageType: "list",
                    keys: { recKey: 'Date' },
                    controller: 'StaffAttendance',
                    //    formRoute: "/ess/imprest-request/header/form",
                    //    listRoute: "/ess/imprest-request/header/list",
                },
                actionsProps: { isNew: false, isNewCaption: "New Imprest", isEdit: false, isDelete: false, isFilter: true, isExport: false,moreActions:true },
                location: {
                    location: "",
                    coordinates: "",
                    type: ""
                }
            }
        },
        mounted() {
            // Dynamically load the jQuery script
            const script = document.createElement('script');
            script.src = "https://code.jquery.com/jquery-3.5.1.js";
            script.integrity = "sha256-QWo7LDvxbWT2tbbQ97B53yJnYU3WhH/C8ycbRAkjPDc=";
            script.crossOrigin = "anonymous";
            script.onload = this.onJQueryLoaded;
            document.head.appendChild(script);
            this.fnLocation();
        },
        methods: {
            fnLocation() {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(this.savePosition);
                } else {
                    alert("Geolocation is not supported by this browser.");
                }
            },
            OnFetchData(response) {
                this.records = response.records;
                this.$root.title = this.pageProps.title;
            },
            savePosition(position) {
                var latitude = position.coords.latitude;
                var longitude = position.coords.longitude;
                this.location.coordinates = latitude + " " + longitude;
                //city
                var requestOptions = {
                    method: 'GET',
                };

                var query = "https://api.geoapify.com/v1/geocode/reverse?lat=" + latitude + "&lon=" + longitude + "&apiKey=ce450451298c4f02af81de334f02b2c0";
                fetch(query, requestOptions)
                    .then(response => response.json())
                    .then(result => {
                        var location = "";
                        if (result.features[0].properties.district != undefined) {
                            location = result.features[0].properties.district + "," + result.features[0].properties.city;
                        }
                        else if (result.features[0].properties.quarter != undefined) {
                            location = result.features[0].properties.quarter + "," + result.features[0].properties.city;
                        }
                        else if (result.features[0].properties.name != undefined) {
                            location = result.features[0].properties.name + "," + result.features[0].properties.city;
                        }
                        else if (result.features[0].properties.address_line1 != undefined) {
                            location = result.features[0].properties.address_line1 + "," + result.features[0].properties.state;
                        }
                        this.location.location = location;
                    })
                    .catch(error => { console.log(error) });
            }
        }
    }
</script>
