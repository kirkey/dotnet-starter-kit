window.fshShortcuts = (function(){
  let map = {}; let ref; let registered = false;
  function handler(e){
    if (e.target && (e.target.tagName === 'INPUT' || e.target.tagName === 'TEXTAREA' || e.target.isContentEditable)) return;
    const key = (e.ctrlKey? 'Ctrl+':'') + (e.shiftKey? 'Shift+':'') + (e.altKey? 'Alt+':'') + e.key.toLowerCase();
    if(map[key]){ e.preventDefault(); if(ref) ref.invokeMethodAsync('OnShortcut', key); }
  }
  return {
    init: function(dotnetRef, keys){
      ref = dotnetRef; map = {}; keys.forEach(k=> map[k]=true); if(!registered){ window.addEventListener('keydown', handler); registered = true; }
    },
    dispose: function(){ if(registered){ window.removeEventListener('keydown', handler); registered=false; } map={}; ref=null; }
  };
})();

