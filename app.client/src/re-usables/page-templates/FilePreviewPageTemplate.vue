<template>
    <div id="fileDiv"></div>
</template>

<script>
    export default {
        data() {
            return {
                dataRoute: { default: "" },
            }
        },
        created() {
            this.GetReport();
        },
        methods: {
            GetReport() {
                var controller = this.$route.query.src;
                var body = JSON.stringify(this.$route.query);
                this.dataRoute = this.appConfig.baseApiRoute + controller;
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: body
                };
                fetch(this.dataRoute, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            var msg = data.errors;
                            this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        }
                        else {
                            var b64 = data.response;
                            var obj = document.createElement('object');
                            obj.style.width = '100%';
                            obj.style.height = '842pt';
                            obj.type = 'application/pdf';
                            obj.data = 'data:application/pdf;base64,' + b64;
                            var fileDiv = document.getElementById("fileDiv");
                            fileDiv.appendChild(obj);
                        }
                        this.$root.loader.isLoading = false;
                    }).catch((error) => {
                        var msg = error;
                        this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                        this.$root.loader.isLoading = false;
                    });
            }
        }
    };
</script>