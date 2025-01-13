<template>
    <div>
        <FormPageTemplate :title="$route.params.action+' '+pageProps.caption">
            <PatientCard v-if="patient != null && patient.PatientNo != undefined" :data="patient"></PatientCard>
            <grid v-if="record.Status != undefined">
                <grid-col>
                    <field-group label="Lab Test No." :isOneLine="true">
                        {{record[pageProps.keys.recKey]}}
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Test Date" :valErrors="valErrors.firstName" :isOneLine="true">
                        {{$root.cplFnNavDateObjToString(record.LaboratoryDate)}}
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Request Area" :valErrors="valErrors.middleName" :isOneLine="true">
                        {{record.Request_Area}}
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Link No" :valErrors="valErrors.lastName" :isOneLine="true">
                        {{record.LinkNo}}
                    </field-group>
                </grid-col>
            </grid>
            <div class="pt-4 pb-1">
                <MenuTabs>
                    <MenuButton name="test-lines" :activeTab="activeTab" @activeTab="activeTab = $event">Lab Test Lines</MenuButton>
                </MenuTabs>
            </div>
            <div v-if="record[pageProps.keys.recKey] != undefined">
                <TestLines v-if="activeTab == 'test-lines'" :filter="{parentId:record[pageProps.keys.recKey]}" :props="{isNew:false}" />
            </div>
            <div class="flex gap-1 justify-center py-2">
                <Actions ref="actions" :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event" />
            </div>
        </FormPageTemplate>
    </div>
</template>
<script>
    import { defineAsyncComponent } from 'vue';
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import { useFormComposable } from '@/re-usables/composables/FormPageComposable.js';
    import Actions from '@/modules/hmis/laboratory/test-header/Lab_TestHeaderActions.vue';
    import TestLines from '@/modules/hmis/laboratory/test-line/Lab_TestLineList.vue';
    import MenuTabs from '@/re-usables/components/MenuTabs/MenuTabs.vue';
    import MenuButton from '@/re-usables/components/MenuTabs/MenuButton.vue';
    import PatientCard from '@/modules/hmis/patient/PatientCard.vue';

    export default {
        components: { Actions, ...W, TestLines, MenuTabs, MenuButton,PatientCard },
        setup() {
            const { router, cplIsFormLoaded, cplOnAfterFormLoaded } = useFormComposable();
            return { router, cplIsFormLoaded, cplOnAfterFormLoaded };
        },
        data() {
            return {
                pageProps: {
                    title: 'Lab Test Form',
                    caption: 'Lab Test',
                    pageType: "form",
                    keys: { recKey: 'LaboratoryNo' },
                    controller: 'Lab_TestHeaders',
                    formRoute: "/hmis/laboratory/test-header/form",
                    listRoute: "/hmis/laboratory/test-header/list",
                },
                form: {
                    recId: '',
                    myAction: "",
                },
                record: {},
                valErrors: [],
                approvers: [],
                approvalEntry: {}, 
                formMode: 'view',
                activeTab: "test-lines",
                patient:{}
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.parentId = this.$route.query.parentId;
            this.form.recId = this.form.myAction != 'create' ? this.$route.query.recId : this.form.recId;
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
                fetch(this.appConfig.baseApiRoute + this.pageProps.controller + '/getformdata' + this.$root.fnGetUrlQueryFromObject(this.$route.query) + '&myAction=' + this.form.myAction, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            //this.$root.errorModal.isShow = true;
                            //this.$root.errorModal.message = data.errors;
                            this.$root.FnNotification(data.errors, 'bg-red-500', false);
                            this.router.go(-1);
                        } else {
                            this.header = data.response.header;
                            this.patient = data.response.patient;
                            if (this.form.myAction != "create") {
                                var formData = data.response.formData;
                                this.record = formData;
                                this.form.recId = formData[this.pageProps.keys.recKey];
                                if (this.record.Status != "") {
                                    this.formMode = "view";
                                }
                            }
                        }
                        this.cplOnAfterFormLoaded();
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = this.appConfig.errors.dataFetchFailure;
                        this.$root.loader.isLoading = false;
                    });
            }
        },
        watch:{
          "form.paymentMode":function(){
            if(this.form.paymentMode == 1){
               this.loadTomSelects("insurances");
            }
          },

           "form.dobStr":function(){
                 this.getAge(this.form.dobStr);
           }

        }
    }
</script>
