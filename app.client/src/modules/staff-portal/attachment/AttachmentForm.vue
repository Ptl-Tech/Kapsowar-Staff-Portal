<template>
    <div>
        <ModalPageTemplate :title="$route.params.action+' '+form.type +' Attachment'" @close="$emit('closeModal')">
            <grid class="sm:!grid-cols-2">
                <grid-col>
                    <field-group label="Attachment File" :valErrors="valErrors.attachmentFile">
                        <WInput type="file" accept="image/jpeg,image/png,application/pdf,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/msword" required="true" @input="onFileChange($event)" formMode="formMode" />
                    </field-group>
                </grid-col>
                <grid-col>
                    <field-group label="Description" showMandatory="true" :valErrors="valErrors.description">
                        <WTextarea required="true" v-model="form.description" :formMode="formMode"></WTextarea>
                    </field-group>
                </grid-col>
            </grid>
            <template #submission>
                <Actions :record="record" :pageProps="pageProps" :formData="form" @onValErrors="valErrors = $event"/>
            </template>
        </ModalPageTemplate>
    </div>
</template>
<script>
    import { W } from '@/re-usables/imports/FormPageComponents.js';
    import ModalPageTemplate from '@/re-usables/page-templates/ModalPageTemplate.vue';
    import Actions from '@/modules/ess/attachment/AttachmentActions.vue';
    import { useRouter } from 'vue-router'
    export default {
        components: { Actions, ...W,ModalPageTemplate},
        setup() {
            const router = useRouter()
            return { router };
        },
        data() {
            return {
                pageProps: {
                    pageType: "form",
                    controller: 'Attachment',
                },
                form: {
                    docNo: '',
                    lineNo: 0,
                    tableID: 0,
                    myAction: '',
                    type: '',
                    attachmentFile: '',
                    filename: '',
                    description: '',
                },
                valErrors: [],
                record: {},
                formMode: 'view',
            }
        },
        created() {
            this.form.myAction = this.$route.params.action;
            this.form.docNo = this.$route.params.docNo;
            this.form.lineNo = this.$route.params.lineNo;
            this.formMode = this.form.myAction;
            this.form.tableID = this.$route.params.tableID;
            this.form.type = this.$route.params.type;
        },
        methods: {
            onFileChange(event) {
                let files = event.target.files;
                var file = files[0];
                if (files.length) {
                    this.form.filename = file.name;
                    var self = this;
                    this.getBase64(file).then(data => {
                        self.form.attachmentFile = data;
                    });
                }
            },
           getBase64(file) {
                return new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = () => resolve(reader.result);
                    reader.onerror = error => reject(error);
                });
            }
        }
    }
</script>