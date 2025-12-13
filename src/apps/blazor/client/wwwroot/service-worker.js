// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).

console.info('Service worker: Development mode - no caching enabled');

self.addEventListener('install', event => {
    console.info('Service worker: Installing (development mode)');
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    console.info('Service worker: Activating (development mode)');
    event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', () => { });
