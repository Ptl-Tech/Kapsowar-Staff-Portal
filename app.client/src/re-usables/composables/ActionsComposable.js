export function useActionsComposable() {
    function mimeType(fileExtension) {
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
    function fileUrl(fileExtension, base64,filename) {
        if (!base64) return null;
        const byteCharacters = atob(base64);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: mimeType(fileExtension) });
        return URL.createObjectURL(blob);
    }
    function viewAttachment(fileExtension, base64) {
        var url = fileUrl(fileExtension, base64);
        if (fileExtension == 'DOCX' || fileExtension == 'XLSX') {
            window.location.href = url;
        } else {
            window.open(url, '_blank');
        }
    }
    //
    return { viewAttachment };
}