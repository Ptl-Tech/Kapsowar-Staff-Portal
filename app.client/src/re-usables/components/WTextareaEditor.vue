<template>
    <div>
        <ckeditor :editor="editor" :disabled="formMode == 'edit' || formMode == 'create'?false:true" v-model="editorData" ref="textarea" :config="editorConfig"></ckeditor>
    </div>
</template>
<script>
  import { defineComponent } from 'vue';
  import { Ckeditor } from '@ckeditor/ckeditor5-vue';
    import {
        ClassicEditor,
        Bold,
        Essentials,
        Heading,
        Indent,
        IndentBlock,
        Italic,
        List,
        Paragraph,
        Undo
    } from 'ckeditor5';

    import 'ckeditor5/ckeditor5.css';

    export default defineComponent({
      components: { Ckeditor },
        props: {
            formMode: { default: "" },
            maxLength: { default: null },
        },
        emits: ['editorData'],
        data() {
            return {
                editor: ClassicEditor,
                editorData: '',
                editorConfig: {
                    toolbar: [
                        'undo', 'redo', '|',
                         'bold', 'italic', '|',
                        'bulletedList', 'numberedList', 'indent', 'outdent'
                    ],
                    plugins: [
                        Bold,
                        Essentials,
                        Heading,
                        Indent,
                        IndentBlock,
                        Italic,
                        List,
                        Paragraph,
                        Undo
                    ],
                },
            };
        },
        methods: {
            focus() {
                this.$refs.textarea.focus()
            }
        },
        watch: {
            editorData: function () {
                var serialiazed = "";
                if (this.editorData != "") {
                    var serialiazed = JSON.stringify(this.editorData);
                }
                this.$emit('editorData', serialiazed);
            }
        }
    })
</script>
<style scoped>
    .ck-content ul,
    .ck-content ol {
        padding-left: 20px !important; /* Adjust padding to ensure bullets/numbers are visible */
    }

    .ck-content {
        min-height: 150px !important;
    }
</style>
