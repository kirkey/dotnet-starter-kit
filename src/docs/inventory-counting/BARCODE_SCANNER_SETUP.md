# 🔌 Barcode Scanner Integration - Quick Setup Guide

## 📱 Add QuaggaJS to Your Blazor App

### Step 1: Add Script References

**Edit: `wwwroot/index.html`**

Add these script tags before the closing `</body>` tag:

```html
<!-- QuaggaJS Barcode Scanner Library -->
<script src="https://cdn.jsdelivr.net/npm/quagga@0.12.1/dist/quagga.min.js"></script>

<!-- Custom Barcode Scanner Integration -->
<script src="js/barcode-scanner.js"></script>
```

**Complete example:**
```html
<!DOCTYPE html>
<html>
<head>
    <!-- ...existing head content... -->
</head>
<body>
    <div id="app">Loading...</div>
    
    <!-- Blazor scripts -->
    <script src="_framework/blazor.webassembly.js"></script>
    
    <!-- ADD THESE: -->
    <script src="https://cdn.jsdelivr.net/npm/quagga@0.12.1/dist/quagga.min.js"></script>
    <script src="js/barcode-scanner.js"></script>
</body>
</html>
```

### Step 2: Verify File Structure

Ensure these files exist:
```
/wwwroot/
  ├── js/
  │   └── barcode-scanner.js  ✅ (already created)
  └── index.html             ⚠️ (needs script tags added)
```

### Step 3: Test Camera Access

**Create a simple test page:**

```razor
@page "/test-scanner"
@inject IJSRuntime JS

<h3>Scanner Test</h3>

<MudButton @onclick="TestScanner">Test Scanner</MudButton>

@code {
    private async Task TestScanner()
    {
        try
        {
            var hasCamera = await JS.InvokeAsync<bool>("cycleCounts.isCameraAvailable");
            Console.WriteLine($"Camera available: {hasCamera}");
            
            if (hasCamera)
            {
                var hasPermission = await JS.InvokeAsync<bool>("cycleCounts.requestCameraPermission");
                Console.WriteLine($"Camera permission: {hasPermission}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

---

## 🚀 Alternative: Use Device Hardware Scanner

If you have **dedicated barcode scanner hardware** (e.g., Zebra, Honeywell devices), you don't need QuaggaJS!

### Hardware Scanner Setup

The JavaScript already includes keyboard scanner support:

```javascript
// Automatically enabled for hardware scanners
// that emit keyboard events
window.cycleCounts.enableKeyboardScanner(dotnetHelper);
```

**How it works:**
1. Hardware scanner scans barcode
2. Device types barcode + Enter
3. JavaScript captures the fast keyboard events
4. Barcode sent to Blazor component
5. Item searched automatically

**No camera needed!** 📱❌  
**Hardware scanner only!** 📟✅

---

## 🌐 Browser Compatibility

### Supported Browsers

| Browser | Camera Scanning | Hardware Scanner |
|---------|----------------|------------------|
| Chrome (Mobile) | ✅ Yes | ✅ Yes |
| Safari (iOS) | ✅ Yes* | ✅ Yes |
| Firefox (Mobile) | ✅ Yes | ✅ Yes |
| Edge (Mobile) | ✅ Yes | ✅ Yes |
| Samsung Internet | ✅ Yes | ✅ Yes |

*iOS requires HTTPS

### Requirements

**For Camera Scanning:**
- ✅ HTTPS connection (required by browsers)
- ✅ Camera permission granted
- ✅ QuaggaJS library loaded
- ✅ Modern browser (last 2 years)

**For Hardware Scanner:**
- ✅ Any browser
- ✅ Barcode scanner in keyboard mode
- ✅ No special permissions needed

---

## 🧪 Testing Checklist

### Camera Scanner Test

1. **Open mobile cycle count page**
   ```
   https://yourdomain.com/store/cycle-counts/mobile
   ```

2. **Start a count**
   - Tap "Start Count" on any scheduled count

3. **Test camera scanner**
   - Tap "Start Camera Scanner" button
   - Allow camera permission if prompted
   - Point camera at any barcode
   - Should see:
     - ✅ Live camera feed
     - ✅ Green overlay when barcode detected
     - ✅ Item loads automatically
     - ✅ Scanner stops after successful scan

4. **Expected Behavior**
   ```
   User taps scanner button
   → Browser asks for camera permission
   → Camera activates
   → User points at barcode
   → Barcode detected (green flash)
   → Item details appear
   → Scanner stops
   → User can count item
   ```

### Hardware Scanner Test

1. **Connect hardware scanner**
   - Pair via Bluetooth or plug in USB

2. **Configure scanner mode**
   - Set to "Keyboard Wedge" mode
   - Set suffix to "Enter" or "Tab"

3. **Test on mobile count page**
   - Scan any barcode
   - Item should load automatically
   - No camera activation needed

4. **Expected Behavior**
   ```
   User scans barcode with device
   → Barcode sent as keyboard input
   → JavaScript captures fast typing
   → Item search triggered
   → Item details appear
   → User can count item
   ```

---

## 🐛 Troubleshooting

### Issue: "QuaggaJS not loaded" Error

**Cause:** Script not loaded or blocked

**Solutions:**
1. Check `index.html` has script tags
2. Verify CDN is accessible
3. Check browser console for errors
4. Try local copy of QuaggaJS:
   ```html
   <script src="js/quagga.min.js"></script>
   ```

### Issue: Camera Permission Denied

**Cause:** Browser blocked camera access

**Solutions:**
1. Check browser settings for camera permission
2. Ensure HTTPS connection
3. Try different browser
4. Use hardware scanner instead

### Issue: Scanner Opens But Can't Read Barcodes

**Cause:** Barcode format not supported or poor lighting

**Solutions:**
1. Improve lighting conditions
2. Hold device steady
3. Adjust distance from barcode
4. Check barcode format is supported:
   - ✅ EAN-13, EAN-8
   - ✅ UPC-A, UPC-E  
   - ✅ Code 128
   - ✅ Code 39
5. Use manual entry as fallback

### Issue: Scanner Doesn't Stop After Scan

**Cause:** JavaScript error or detection issue

**Solutions:**
1. Check browser console for errors
2. Manually stop scanner (button provided)
3. Refresh page
4. Report bug with details

---

## 📦 Offline Installation (Optional)

If you want to host QuaggaJS locally instead of CDN:

### Download QuaggaJS

```bash
cd wwwroot/js
wget https://cdn.jsdelivr.net/npm/quagga@0.12.1/dist/quagga.min.js
```

### Update index.html

```html
<!-- Change this: -->
<script src="https://cdn.jsdelivr.net/npm/quagga@0.12.1/dist/quagga.min.js"></script>

<!-- To this: -->
<script src="js/quagga.min.js"></script>
```

**Benefits:**
- ✅ Works without internet
- ✅ Faster loading
- ✅ No CDN dependency

**Drawbacks:**
- ❌ Manual updates needed
- ❌ Larger app size

---

## 🎛️ Configuration Options

### Adjust Scanner Settings

**Edit: `wwwroot/js/barcode-scanner.js`**

```javascript
// Line ~35: Scanner initialization
Quagga.init({
    inputStream: {
        constraints: {
            width: { min: 640 },      // ⬅️ Adjust resolution
            height: { min: 480 },     // ⬅️ Adjust resolution
            facingMode: "environment" // ⬅️ "user" for front camera
        }
    },
    decoder: {
        readers: [
            "ean_reader",         // ⬅️ Add/remove formats as needed
            "code_128_reader",
            "upc_reader"
        ]
    }
});
```

### Common Adjustments

**Use Front Camera:**
```javascript
facingMode: "user"  // Instead of "environment"
```

**Higher Resolution:**
```javascript
width: { min: 1280 },
height: { min: 720 },
```

**Add QR Code Support:**
```javascript
readers: [
    "ean_reader",
    "code_128_reader",
    "qr_reader"  // ⬅️ Add this
]
```

---

## 📊 Performance Tips

### Optimize Scanner Performance

1. **Good Lighting**
   - Use well-lit areas
   - Avoid glare and shadows
   - Natural light is best

2. **Steady Hands**
   - Hold device stable
   - Use both hands
   - Rest on surface if possible

3. **Optimal Distance**
   - 10-20 cm from barcode
   - Fill 60-80% of frame
   - Parallel to barcode

4. **Clean Barcodes**
   - Remove dirt/damage
   - Ensure good print quality
   - Replace if necessary

### Reduce Battery Drain

```javascript
// Stop scanner when not in use
window.cycleCounts.stopBarcodeScanner();

// Don't leave camera running
// Component already handles this automatically
```

---

## 🔐 Security & Privacy

### Camera Permissions

**What the app can do:**
- ✅ Access camera feed
- ✅ Detect barcodes in real-time
- ✅ Process video locally

**What the app CANNOT do:**
- ❌ Take photos
- ❌ Record video
- ❌ Upload camera feed to server
- ❌ Access camera when app is closed

### User Privacy

- 📷 Camera only used when scanner is active
- 🔒 All processing happens on device
- 🚫 No images sent to server
- ✅ Permission required before access

---

## ✅ Installation Complete!

### Verify Setup

Run through this checklist:

- [ ] QuaggaJS script tag added to index.html
- [ ] barcode-scanner.js script tag added
- [ ] Both files load without errors (check console)
- [ ] Camera permission can be requested
- [ ] Scanner opens and shows camera feed
- [ ] Barcodes are detected
- [ ] Items load after scan
- [ ] Scanner stops automatically

### If All Green ✅

**You're ready to go!** 🎉

Users can now:
- Open mobile cycle count page
- Start counting
- Scan barcodes with camera
- Or use hardware scanners
- Count inventory efficiently

---

## 🆘 Need Help?

1. **Check browser console** for errors
2. **Test on different device/browser**
3. **Try hardware scanner** as alternative
4. **Use manual entry** as fallback
5. **Contact support** with error details

---

**Setup Complete!** 🎊

Next: [Test the mobile interface](MOBILE_CYCLE_COUNT_IMPLEMENTATION.md#testing-checklist)

