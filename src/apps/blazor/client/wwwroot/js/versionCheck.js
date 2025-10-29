/**
 * Version Check JavaScript Interop
 * Handles client-side version checking and notification for application updates
 */

window.versionCheck = {
    /**
     * Fetches the current version from the server's version.json file
     * @returns {Promise<string|null>} The version string or null if fetch fails
     */
    fetchServerVersion: async function () {
        try {
            // Add timestamp to prevent caching
            const timestamp = new Date().getTime();
            const response = await fetch(`/version.json?t=${timestamp}`, {
                cache: 'no-cache',
                headers: {
                    'Cache-Control': 'no-cache',
                    'Pragma': 'no-cache'
                }
            });

            if (!response.ok) {
                console.warn('Failed to fetch version.json:', response.status);
                return null;
            }

            const data = await response.json();
            return data.version || null;
        } catch (error) {
            console.error('Error fetching server version:', error);
            return null;
        }
    },

    /**
     * Gets the currently stored version from localStorage
     * @returns {string|null} The stored version or null
     */
    getCurrentVersion: function () {
        try {
            return localStorage.getItem('app-version');
        } catch (error) {
            console.error('Error reading version from localStorage:', error);
            return null;
        }
    },

    /**
     * Stores the version in localStorage
     * @param {string} version - The version to store
     * @returns {boolean} Success status
     */
    setCurrentVersion: function (version) {
        try {
            if (version) {
                localStorage.setItem('app-version', version);
                return true;
            }
            return false;
        } catch (error) {
            console.error('Error saving version to localStorage:', error);
            return false;
        }
    },

    /**
     * Clears the stored version from localStorage
     */
    clearVersion: function () {
        try {
            localStorage.removeItem('app-version');
        } catch (error) {
            console.error('Error clearing version from localStorage:', error);
        }
    },

    /**
     * Reloads the page and clears the cache
     * @param {boolean} hardReload - Whether to perform a hard reload (clear cache)
     */
    reloadPage: function (hardReload = true) {
        if (hardReload) {
            // Clear service worker cache if available
            if ('serviceWorker' in navigator && 'caches' in window) {
                caches.keys().then(function (cacheNames) {
                    return Promise.all(
                        cacheNames.map(function (cacheName) {
                            return caches.delete(cacheName);
                        })
                    );
                }).then(function () {
                    console.log('Cache cleared, reloading...');
                    window.location.reload(true);
                }).catch(function (error) {
                    console.error('Error clearing cache:', error);
                    window.location.reload(true);
                });
            } else {
                window.location.reload(true);
            }
        } else {
            window.location.reload();
        }
    },

    /**
     * Initializes version checking on page load
     * @returns {Promise<Object>} Object containing version check result
     */
    initializeVersionCheck: async function () {
        try {
            const serverVersion = await this.fetchServerVersion();
            const currentVersion = this.getCurrentVersion();

            if (!serverVersion) {
                return {
                    success: false,
                    message: 'Unable to fetch server version'
                };
            }

            // If no current version stored, this is first load
            if (!currentVersion) {
                this.setCurrentVersion(serverVersion);
                return {
                    success: true,
                    isNewVersion: false,
                    currentVersion: serverVersion,
                    serverVersion: serverVersion
                };
            }

            // Check if versions differ
            const isNewVersion = currentVersion !== serverVersion;

            return {
                success: true,
                isNewVersion: isNewVersion,
                currentVersion: currentVersion,
                serverVersion: serverVersion
            };
        } catch (error) {
            console.error('Error in version check initialization:', error);
            return {
                success: false,
                message: error.message
            };
        }
    }
};

