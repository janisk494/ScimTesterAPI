// site.js - tiny helpers for Blazor interop

window.downloadFile = (dataUrl, filename) => {
    // dataUrl should be "data:application/json;base64,...." or blob URL
    const a = document.createElement('a');
    a.href = dataUrl;
    a.download = filename || 'download.json';
    document.body.appendChild(a);
    a.click();
    a.remove();
};

// copy to clipboard helper
window.copyToClipboard = async (text) => {
    try {
        await navigator.clipboard.writeText(text);
        return true;
    } catch (e) {
        console.error('copy failed', e);
        return false;
    }
};
