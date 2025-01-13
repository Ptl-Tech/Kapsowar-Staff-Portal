<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+pageProps.title" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="System" :showMandatory="false" :valErrors="valErrors.system">
                        <WSelect required="true" v-model="form.system" id="systems" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="systems" v-for="option in systems" :value="option.Code">{{option.Description}}</option>
                        </WSelect>
                    </field-group>
                </grid-col>
            </grid>
            <grid class="sm:!grid-cols-1">
                <grid-col>
                    <field-group label="Sign" :showMandatory="false" :valErrors="valErrors.signNo">
                        <WSelect required="true" v-model="form.signNo" id="signs" :isTomSelect="true" :formMode="formMode">
                            <option value="">search</option>
                            <option v-if="signs" v-for="option in signs" :value="option.SignCode">{{option.SignsName}}</option>
                        </WSelect>
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
                    title: 'Signs Form',
                    pageType: "form",
                    keys: { recKey: 'System', recKey2: 'SignCode', parentKey: 'TreatmentNo' },
                    controller: 'ConsultationSigns',
                    formRoute: "/hmis/" + this.$route.params.section + "/signs/form",
                    listRoute: "/hmis/" + this.$route.params.section + "/signs/list",
                },
                form: {
                    parentKey: '',
                    recKey: "",
                    recKey2: "",
                    myAction: '',
                    system: "",
                    signNo: "",
                    description: "",
                },
                valErrors: [],
                record: {},
                header: {},
                formMode: 'view',
                systems: [],
                signs: [],
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
                            this.signs = data.response.signs;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.parentId = formData[this.pageProps.keys.parentKey];
                                this.form.system = formData.System;
                                this.form.signNo = formData.SignCode;
                                this.form.description = formData.SignDescription;
                                if (this.header.Status == "Completed") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.loadTomSelects("systems,signs");
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