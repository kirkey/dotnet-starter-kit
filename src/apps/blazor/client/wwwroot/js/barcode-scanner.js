// Barcode Scanner JavaScript Interop for Mobile Cycle Count
// This file provides camera-based barcode scanning functionality

window.cycleCounts = window.cycleCounts || {};

// Global scanner state
let scannerActive = false;
let dotnetHelper = null;

/**
 * Initializes the barcode scanner using QuaggaJS
 * @param {object} helper - DotNet object reference for callbacks
 */
window.cycleCounts.startBarcodeScanner = (helper) => {
    if (scannerActive) {
        console.warn('Scanner already active');
        return;
    }

    dotnetHelper = helper;

    // Check if Quagga is loaded
    if (typeof Quagga === 'undefined') {
        console.error('QuaggaJS not loaded. Please add the script to index.html');
        alert('Barcode scanner library not loaded. Please contact support.');
        return;
    }

    // Create scanner container if it doesn't exist
    let container = document.getElementById('scanner-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'scanner-container';
        container.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: black;
        `;
        document.body.appendChild(container);
    }

    // Initialize Quagga
    Quagga.init({
        inputStream: {
            name: "Live",
            type: "LiveStream",
            target: container,
            constraints: {
                width: { min: 640 },
                height: { min: 480 },
                facingMode: "environment", // Use rear camera
                aspectRatio: { min: 1, max: 2 }
            }
        },
        locator: {
            patchSize: "medium",
            halfSample: true
        },
        numOfWorkers: navigator.hardwareConcurrency || 4,
        decoder: {
            readers: [
                "ean_reader",
                "ean_8_reader",
                "code_128_reader",
                "code_39_reader",
                "upc_reader",
                "upc_e_reader"
            ]
        },
        locate: true
    }, function(err) {
        if (err) {
            console.error('Quagga initialization failed:', err);
            alert('Camera access failed. Please check permissions.');
            window.cycleCounts.stopBarcodeScanner();
            return;
        }
        
        console.log("Barcode scanner initialized");
        Quagga.start();
        scannerActive = true;

        // Add visual feedback for detected codes
        Quagga.onProcessed(function(result) {
            const drawingCtx = Quagga.canvas.ctx.overlay;
            const drawingCanvas = Quagga.canvas.dom.overlay;

            if (result) {
                if (result.boxes) {
                    drawingCtx.clearRect(0, 0, drawingCanvas.width, drawingCanvas.height);
                    result.boxes.filter(box => box !== result.box).forEach(box => {
                        Quagga.ImageDebug.drawPath(box, { x: 0, y: 1 }, drawingCtx, { color: "green", lineWidth: 2 });
                    });
                }

                if (result.box) {
                    Quagga.ImageDebug.drawPath(result.box, { x: 0, y: 1 }, drawingCtx, { color: "#00F", lineWidth: 2 });
                }

                if (result.codeResult && result.codeResult.code) {
                    Quagga.ImageDebug.drawPath(result.line, { x: 'x', y: 'y' }, drawingCtx, { color: 'red', lineWidth: 3 });
                }
            }
        });

        // Handle detected barcodes
        Quagga.onDetected(function(result) {
            const code = result.codeResult.code;
            console.log('Barcode detected:', code);
            
            // Visual feedback
            const canvas = Quagga.canvas.dom.overlay;
            const ctx = canvas.getContext('2d');
            ctx.fillStyle = 'rgba(0, 255, 0, 0.3)';
            ctx.fillRect(0, 0, canvas.width, canvas.height);
            
            // Vibrate if supported
            if (navigator.vibrate) {
                navigator.vibrate(200);
            }
            
            // Send to Blazor
            if (dotnetHelper) {
                dotnetHelper.invokeMethodAsync('OnBarcodeScanned', code);
            }
            
            // Auto-stop after successful scan (optional)
            // window.cycleCounts.stopBarcodeScanner();
        });
    });
};

/**
 * Stops the barcode scanner and releases camera
 */
window.cycleCounts.stopBarcodeScanner = () => {
    if (!scannerActive) {
        return;
    }

    try {
        Quagga.stop();
        
        // Remove scanner container
        const container = document.getElementById('scanner-container');
        if (container) {
            container.remove();
        }
        
        scannerActive = false;
        dotnetHelper = null;
        
        console.log('Barcode scanner stopped');
    } catch (err) {
        console.error('Error stopping scanner:', err);
    }
};

/**
 * Checks if camera is available
 * @returns {Promise<boolean>}
 */
window.cycleCounts.isCameraAvailable = async () => {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.some(device => device.kind === 'videoinput');
    } catch (err) {
        console.error('Error checking camera:', err);
        return false;
    }
};

/**
 * Requests camera permissions
 * @returns {Promise<boolean>}
 */
window.cycleCounts.requestCameraPermission = async () => {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        // Stop the stream immediately, we just wanted to trigger permission
        stream.getTracks().forEach(track => track.stop());
        return true;
    } catch (err) {
        console.error('Camera permission denied:', err);
        return false;
    }
};

/**
 * Simple manual barcode scanner using device keyboard
 * Useful for dedicated barcode scanner hardware
 */
window.cycleCounts.enableKeyboardScanner = (helper) => {
    let barcodeBuffer = '';
    let lastKeyTime = Date.now();

    const handleKeyPress = (e) => {
        const currentTime = Date.now();
        
        // Reset buffer if more than 100ms between keypresses
        // (human typing is slower than scanner)
        if (currentTime - lastKeyTime > 100) {
            barcodeBuffer = '';
        }
        
        lastKeyTime = currentTime;

        if (e.key === 'Enter' && barcodeBuffer.length > 0) {
            console.log('Barcode scanned (keyboard):', barcodeBuffer);
            
            if (helper) {
                helper.invokeMethodAsync('OnBarcodeScanned', barcodeBuffer);
            }
            
            barcodeBuffer = '';
        } else if (e.key.length === 1) {
            barcodeBuffer += e.key;
        }
    };

    document.addEventListener('keypress', handleKeyPress);
    
    // Return cleanup function
    return () => {
        document.removeEventListener('keypress', handleKeyPress);
    };
};

// Add CSS for scanner overlay
const style = document.createElement('style');
style.textContent = `
    #scanner-container video {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
    
    #scanner-container canvas {
        position: absolute;
        top: 0;
        left: 0;
    }
    
    .scanner-overlay {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 80%;
        height: 40%;
        border: 2px solid #00ff00;
        box-shadow: 0 0 0 9999px rgba(0, 0, 0, 0.5);
    }
`;
document.head.appendChild(style);

console.log('Cycle Count barcode scanner module loaded');

