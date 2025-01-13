<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid>
                <grid-col>
                    <field-group label="System" :showMandatory="false" :valErrors="valErrors.system">
                        <WSelect required="true" v-model="form.system" id="systems" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="systems" v-for="option in systems" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Symptom" :showMandatory="false" :valErrors="valErrors.symptomCode">
                        <WSelect required="true" v-model="form.symptomCode" id="symptoms" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="symptoms" v-for="option in symptoms" :value="option.SymptomCode">{{option.SymptomName}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Duration" :showMandatory="true" :valErrors="valErrors.duration">
                        <WInput type="text" required="true" v-model="form.duration" :formMode="formMode" />
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
                    <field-group label="Characteristics" :valErrors="valErrors.characteristics" :showMandatory="false">
                        <WTextarea required="false" v-model="form.characteristics" :formMode="formMode" :maxLength="250"></WTextarea>
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
    import Actions from '@/modules/hmis/consultation/signs/ConsultationSignsActions.vue';
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
                    title: 'Symptoms Form',
                    pageType: "form",
                    keys: { recKey: 'System', recKey2: 'SymptomCode', parentKey: 'TreatmentNo' },
                    controller: 'ConsultationSymptoms',
                    formRoute: "/hmis/" + this.$route.params.section + "/symptoms/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/symptoms/list",
                },
                form: {
                    parentKey: '',
                    recId: '',
                    recKey2: '',
                    myAction: '',
                    system: "",
                    symptomCode: "",
                    duration: "",
                    description: "",
                    characteristics: "",
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                systems: [],
                symptoms: [],
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
            this.form.recKey2 = this.form.myAction != 'create' ? this.$route.query.recKey2 : this.form.recKey2;
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
                            this.systems = data.response.systems;
                            this.symptoms = data.response.symptoms;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.system = formData.System;
                                this.form.symptomCode = formData.SymptomCode;
                                this.form.duration = formData.Duration;
                                this.form.description = formData.Description;
                                this.form.characteristics = formData.Characteristics;
                                if (this.header.Status == "Completed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("systems,symptoms");
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