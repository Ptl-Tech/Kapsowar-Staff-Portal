import { ref, getCurrentInstance } from 'vue'
export function useFileManagementComposable() {
    function xMimeType(fileExtension) {
        const mimeTypes = {
            'PDF': 'application/pdf',
            'DOCX': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            'XLSX': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            'PNG': 'image/png',
            'JPG': 'image/jpeg',
            'JPEG': 'image/jpeg',
            'TXT': 'text/plain'
        };
        return mimeTypes[fileExtension] || 'application/octet-stream';
    }
    //for downloading attachment
    function xFileUrl(fileExtension, base64, filename) {
        if (!base64) return null;
        const byteCharacters = atob(base64);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: xMimeType(fileExtension) });
        return URL.createObjectURL(blob);
    }
    //for downloading attachment
    function xViewAttachment(fileExtension, base64, filename) {
        fileExtension = fileExtension.toUpperCase();
        var url = xFileUrl(fileExtension, base64, filename);
        if (fileExtension == 'DOCX' || fileExtension == 'XLSX') {
            //window.location.href = url;//old
            const anchor = document.createElement('a');
            anchor.href = url;
            anchor.download = filename; // Set the desired file name
            // Append to the DOM, click it, then remove it
            document.body.appendChild(anchor);
            anchor.click();
            document.body.removeChild(anchor);
            // Revoke the object URL after the download
            //URL.revokeObjectURL(blobUrl);
        } else {
            //if (navigator.userAgentData.platform.indexOf('Win') > -1) {
            //    window.open(url, '_blank');
            //} else {
            //    window.location.href = url;
            //}
            window.open(url, '_blank');
        }
    }
    //for downloading attachment
    function xFnDownloadDocumentAttachment(record, root, appConfig) {
        root.loader.isLoading = true;
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(record)
        };
        fetch(appConfig.baseApiRoute + 'BC_Attachment/Download', requestOptions)
            .then(response => {
                return response.json();
            })
            .then(data => {
                if (data && data.errors) {
                    var msg = data.errors;
                    root.FnNotification({ type: "modal", theme: "red", message: msg });
                } else {
                    xViewAttachment(record.FileExtension.toUpperCase(), data.response, record.FileName);
                }
                root.loader.isLoading = false;
            }).catch((error) => {
                var msg = error;
                root.FnNotification({ type: "modal", theme: "red", message: msg });
                root.loader.isLoading = false;
            });
    }
    //
    function xFnDeleteDocumentAttachment(record, root, appConfig) {
        root.loader.isLoading = true;
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(record)
        };
        fetch(appConfig.baseApiRoute + 'BC_Attachment/Delete', requestOptions)
            .then(response => {
                return response.json();
            })
            .then(data => {
                if (data && data.errors) {
                    var msg = data.errors;
                    root.FnNotification({ type: "modal", theme: "red", message: msg });
                } else {
                    var msg = "Deleted successfully";
                    root.FnNotification({ type: "popup", theme: "green", message: msg });
                    root.router.go();
                }
                root.loader.isLoading = false;
            }).catch((error) => {
                var msg = error;
                root.FnNotification({ type: "modal", theme: "red", message: msg });
                root.loader.isLoading = false;
            });
    }
    //for downloading attachment
    function xFnDownloadBCReport(obj) {
        var root = obj.root;
        var appConfig = obj.appConfig;
        var fileExtension = obj.fileExtension;
        var fileName = obj.fileName;
        var body = obj.body;
        var url = appConfig.baseApiRoute + obj.route;
        //
        root.loader.isLoading = true;
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };
        fetch(url, requestOptions)
            .then(response => {
                return response.json();
            })
            .then(data => {
                if (data && data.valErrors) {
                    var msg = appConfig.errors.validationErrors;
                    root.FnNotification({ type: "modal", theme: "red", message: msg });
                }
                else if (data && data.errors) {
                    var msg = data.errors;
                    root.FnNotification({ type: "modal", theme: "red", message: msg });
                } else {
                    xViewAttachment(fileExtension.toUpperCase(), data.response, fileName);
                }
                root.loader.isLoading = false;
            }).catch((error) => {
                var msg = error;
                root.FnNotification({ type: "modal", theme: "red", message: msg });
                root.loader.isLoading = false;
            });
    }
    //
    return {
        xFnDownloadDocumentAttachment, xFnDeleteDocumentAttachment, xViewAttachment, xFnDownloadBCReport
    };
}
