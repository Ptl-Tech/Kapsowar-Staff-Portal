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
        methods:{
            GetReport() {
                this.dataRoute = this.appConfig.baseApiRoute+this.$route.query.source.replaceAll("__", "/");
                this.$root.loader.isLoading = true;
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(this.$route.query)
                };
                fetch(this.dataRoute, requestOptions)
                    .then(response => {
                        return response.json();
                    })
                    .then(data => {
                        if (data && data.errors) {
                            this.$root.errorModal.isShow = true;
                            this.$root.errorModal.message = data.errors;
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
                        this.$root.errorModal.isShow = true;
                        this.$root.errorModal.message = error;
                        this.$root.loader.isLoading = false;
                    });
            },
            queryToJSON(query) {
                let pairs = query.slice(1).split('&');
                let result = {};
                pairs.forEach(pair => {
                    let [key, value] = pair.split('=');
                    result[decodeURIComponent(key)] = decodeURIComponent(value || '');
                });
                return result;
            }
        }
    };
</script>