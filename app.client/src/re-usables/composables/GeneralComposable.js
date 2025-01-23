import { ref } from 'vue'
export function useGeneralComposable() {
    function cplFnNavDateObjToString(navDateObj) {
        return navDateObj.Day + "-" + navDateObj.Month + "-" + navDateObj.Year;
    }
    function cplFnNavDateObjToISODate(navDateObj) {
        var date = "";
        if (navDateObj.Day != undefined) {
            date = new Date(navDateObj.Year, navDateObj.Month, navDateObj.Day).toISOString().split("T")[0];
        }
        return date;
    }
    function xFnDownloadDocumentAttachment(entryNo,docNo) {
        this.$root.loader.isLoading = true;
        const requestOptions = {
            method: "GET",
        };
        fetch(this.appConfig.baseApiRoute + 'General/DownloadAttachment?entryNo=' + entryNo + "&docNo=" + docNo, requestOptions)
            .then(response => {
                return response.json();
            })
            .then(data => {
                if (data && data.errors) {
                    var msg = this.appConfig.errors.dataFetchFailure;
                    this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                } else {
                    var a = document.createElement("a");
                    var split = data.response.split("###");
                    a.href = split[1];
                    a.download = split[0];
                    a.click();
                }
                this.$root.loader.isLoading = false;
            }).catch((error) => {
                var msg = this.appConfig.errors.dataFetchFailure;
                this.$root.FnNotification({ type: "modal", theme: "red", message: msg });
                this.$root.loader.isLoading = false;
            });
    }
    return {
        cplFnNavDateObjToString, cplFnNavDateObjToISODate, xFnDownloadDocumentAttachment
    };
}
