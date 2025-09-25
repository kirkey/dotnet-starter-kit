window.fshDownload = {
  saveFile: function (filename, base64) {
    try {
      const bytes = atob(base64);
      const array = new Uint8Array(bytes.length);
      for (let i=0;i<bytes.length;i++) array[i] = bytes.charCodeAt(i);
      const blob = new Blob([array], {type:'text/csv'});
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url; a.download = filename; a.click();
      setTimeout(()=>URL.revokeObjectURL(url), 2000);
    } catch (e) { console.error('fshDownload.saveFile error', e); }
  }
};

async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}