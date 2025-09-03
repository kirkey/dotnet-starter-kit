window.fshIdle = (function(){
  let timeoutHandle, warningHandle, endTime, warnTime, dotnetRef, timeoutSec, warningSec;
  function resetTimers(){
    clearTimeout(timeoutHandle); clearTimeout(warningHandle);
    const now = Date.now();
    endTime = now + timeoutSec*1000;
    warnTime = endTime - warningSec*1000;
    schedule();
  }
  function schedule(){
    const now = Date.now();
    const warnDelay = Math.max(0, warnTime - now);
    const timeoutDelay = Math.max(0, endTime - now);
    warningHandle = setTimeout(()=>{
      const remaining = Math.round((endTime - Date.now())/1000);
      if(dotnetRef) dotnetRef.invokeMethodAsync('OnWarn', remaining);
    }, warnDelay);
    timeoutHandle = setTimeout(()=>{ if(dotnetRef) dotnetRef.invokeMethodAsync('OnTimeout'); }, timeoutDelay);
  }
  function activity(){ resetTimers(); }
  const events = ['mousemove','keydown','click','scroll','touchstart'];
  return {
    start: function(ref, tSec, wSec){
      dotnetRef = ref; timeoutSec = tSec; warningSec = wSec;
      events.forEach(e=>window.addEventListener(e,activity,{passive:true}));
      resetTimers();
    },
    reset: function(){ resetTimers(); },
    dispose: function(){
      events.forEach(e=>window.removeEventListener(e,activity));
      clearTimeout(timeoutHandle); clearTimeout(warningHandle);
    }
  };
})();

