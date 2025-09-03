window.fshNetwork = {
  init: function (dotnetRef) {
    function update() {
      dotnetRef.invokeMethodAsync('SetOnlineStatus', navigator.onLine);
    }
    window.addEventListener('online', update);
    window.addEventListener('offline', update);
    // initial status
    return navigator.onLine;
  },
  dispose: function(dotnetRef){
    // Not strictly necessary to remove handlers per instance; kept simple.
  }
};

