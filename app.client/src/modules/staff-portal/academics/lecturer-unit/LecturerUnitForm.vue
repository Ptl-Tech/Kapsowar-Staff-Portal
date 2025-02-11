<template>
    <div>
        <grid class="mb-2">
            <grid-col>
                <field-group label="Semester" :showMandatory="true">
                    <WSelect v-model="form.semester">
                        <option value="">--select--</option>
                        <option v-if="examSemesters" v-for="option in examSemesters" :value="option.Code">{{option.Description}}</option>
                    </WSelect>
                </field-group>
            </grid-col>
        </grid>
        <Lines v-if="form.semester != ''" :key="form.semester" :filter="{semester:form.semester}"/>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Lines from '@/modules/staff-portal/academics/lecturer-unit/LecturerUnitList.vue';
    export default {
        components: {...W, Lines },
        setup() {
            const { router} = useFormComposable();
            return { router};
        },
        data() {
            return {
                pageProps: {
                    title: 'Lecturer Unit Form',
                    caption: 'Lecturer Unit',
                    pageType: "form",
                    keys: { recKey: 'No' },
                    controller:'LecturerUnits',
                    formRoute: "/ess/purchase-request/header/form",
                    listRoute: "/ess/purchase-request/header/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                    parentId: "",
                    semester: "",
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {},
                formMode: 'view',
                activeSubpart: "units",
                examSemesters:[]
            }
        },
        created() {
            this.form.myAction = this.$route.query.action;
            //this.form.parentId = this.$route.query.parentId;
            //this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
            this.$root.title = this.pageProps.title;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata', requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "popup", theme: "red", message: msg });
                            this.router.go(-1);
                        } else {
                            this.examSemesters = data.response.examSemesters;
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = this.appConfig.errors.dataFetchFailure;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    }
</script>
