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

