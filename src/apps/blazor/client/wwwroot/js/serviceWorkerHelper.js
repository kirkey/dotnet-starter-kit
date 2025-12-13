// Service Worker Helper
// This file helps manage service worker registration and updates

window.serviceWorkerHelper = {
    // Unregister all service workers
    unregisterAll: async function () {
        if ('serviceWorker' in navigator) {
            const registrations = await navigator.serviceWorker.getRegistrations();
            for (const registration of registrations) {
                await registration.unregister();
                console.log('Service worker unregistered:', registration);
            }
            console.log(`Unregistered ${registrations.length} service worker(s)`);
            return registrations.length;
        }
        return 0;
    },

    // Update the service worker
    update: async function () {
        if ('serviceWorker' in navigator) {
            const registration = await navigator.serviceWorker.getRegistration();
            if (registration) {
                await registration.update();
                console.log('Service worker update triggered');
                return true;
            }
        }
        return false;
    },

    // Check service worker status
    getStatus: async function () {
        if ('serviceWorker' in navigator) {
            const registration = await navigator.serviceWorker.getRegistration();
            if (registration) {
                return {
                    hasServiceWorker: true,
                    installing: !!registration.installing,
                    waiting: !!registration.waiting,
                    active: !!registration.active,
                    scope: registration.scope
                };
            }
        }
        return { hasServiceWorker: false };
    }
};

// Expose to console for debugging
if (typeof window !== 'undefined') {
    console.log('Service Worker Helper loaded. Use window.serviceWorkerHelper for debugging.');
}

