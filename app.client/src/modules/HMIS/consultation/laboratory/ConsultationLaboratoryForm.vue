<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="Test Package" :showMandatory="false" :valErrors="valErrors.testPackageCode">
                        <WSelect required="true" v-model="form.testPackageCode" id="testPackages" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="testPackages" v-for="option in testPackages" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Due Date" :showMandatory="true" :valErrors="valErrors.dueDate">
                        <WInput type="date" required="true" v-model="form.dueDate" :formMode="formMode" />
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Description" :valErrors="valErrors.description" :showMandatory="true">
                        <WTextarea required="false" v-model="form.description" :formMode="formMode" :maxLength="250"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Results" :valErrors="valErrors.results" :showMandatory="true">
                        <WTextarea required="false" v-model="form.results" :formMode="formMode" :maxLength="250"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <Actions :record="record" :header="header" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </template>
        </ModalPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import ModalPageTemplate from '@/re-usables/page-templates/ModalPageTemplate.vue';
    import Actions from '@/modules/hmis/consultation/laboratory/ConsultationLaboratoryActions.vue';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    export default {
        components: { Actions, ...W, ModalPageTemplate },
        emits:["closeModal"],
        setup() {
            const { router, loadTomSelects } = useFormComposable();
            return { router, loadTomSelects };
        },
        data() {
            return {
                pageProps: {
                    title: 'Laboratory Form',
                    pageType: "form",
                    keys: { recKey: 'LineNo', parentKey: 'TreatmentNo' },
                    controller: 'ConsultationLaboratory',
                    formRoute: "/hmis/" + this.$route.params.section + "/laboratory/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/laboratory/list",
                },
                form: {
                    parentKey: '',
                    recKey: 0,
                    myAction: '',
                    testPackageCode: "",
                    description: "",
                    dueDate:new Date().toISOString().split("T")[0],
                },
                valErrors: [],
                record: {},
                header: {},
                patient: {},
                formMode: 'view',
                testPackages: [],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.FnFetchSetups();
            this.formMode = this.form.myAction;
        },
        methods: {
            FnFetchSetups() {
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "GET",
                };
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata' + this.$root.fnGetUrlQueryFromObject(this.$route.query) + '&myAction=' + this.form.myAction, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        } else {
                            this.header = data.response.header;
                            this.testPackages = data.response.testPackages;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                this.form.testPackageCode = formData.LaboratoryTestPackageCode;
                                this.form.description = formData.BriefHistory;
                                this.form.DueDate = new Date(formData.DateDue.Year,formData.DateDue.Month,formData.DateDue.Day).toISOString().split("T")[0];
                                this.form.results = formData.Results;
                                if (this.header.Status == "Completed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("testPackages");
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