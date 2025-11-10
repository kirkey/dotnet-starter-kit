# 📱 Mobile Cycle Count - Visual UI Reference

## 🎨 Screen Layouts & Components

---

## Screen 1: Mobile Count List

```
┌─────────────────────────────────────┐
│  📱 My Cycle Counts    🖥️          │ ← Header with desktop switcher
│  Mobile View                        │
└─────────────────────────────────────┘
┌─────────────────────────────────────┐
│ 📅 Today's Counts (3)               │ ← Section header
├─────────────────────────────────────┤
│ ┌─────────────────────────────────┐ │
│ │ CC-2025-001      [⏳ InProgress]│ │ ← Status chip
│ │ 🏢 Main Warehouse               │ │
│ │ 📍 Zone A - Electronics         │ │
│ │ ⏰ Due: 2:00 PM                 │ │
│ │ ▓▓▓▓▓░░░░░░░░░░░ 35%           │ │ ← Progress bar
│ │ 📊 7 / 20 items ⚠️ 2 variances │ │
│ │                                 │ │
│ │ [   Continue Count →   ]        │ │ ← Action button
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ CC-2025-002      [📅 Scheduled] │ │
│ │ 🏢 Store #42                    │ │
│ │ 📍 Grocery Section              │ │
│ │ ⏰ Due: 4:00 PM                 │ │
│ │ ░░░░░░░░░░░░░░░░ 0%            │ │
│ │ 📊 0 / 15 items                 │ │
│ │                                 │ │
│ │ [    Start Count →    ]         │ │
│ └─────────────────────────────────┘ │
├─────────────────────────────────────┤
│ ▼ 📅 Upcoming Counts (5)           │ ← Expandable
├─────────────────────────────────────┤
│ ▼ ✅ Completed (8)                 │ ← Expandable
├─────────────────────────────────────┤
│ [        🔄 Refresh        ]        │
└─────────────────────────────────────┘
```

**Key Elements:**
- Large touch-friendly cards
- Color-coded status chips
- Visual progress indicators
- Clear action buttons
- Collapsible sections for secondary items

---

## Screen 2: Mobile Counting Interface

```
┌─────────────────────────────────────┐
│ ← CC-2025-001             [7/20]    │ ← Fixed header with progress
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ Progress                    35%     │
│ ▓▓▓▓▓▓▓░░░░░░░░░░░░░░░░░░░        │
│ ⚠️ 2 items with variances          │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ 📱 Scan Item                        │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │    📸 Camera Active             │ │
│ │                                 │ │
│ │    [Live Video Feed]            │ │ ← When scanning
│ │                                 │ │
│ │    Point at barcode...          │ │
│ │                                 │ │
│ │    [  Stop Scanner  ]           │ │
│ └─────────────────────────────────┘ │
│         - OR -                      │
│ ┌─────────────────────┐            │
│ │ 🔍 Item Barcode/SKU │ [🔍]       │ ← Manual entry
│ └─────────────────────┘            │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ 📦 Count This Item                  │
│                                     │
│ Sony PlayStation 5                  │
│ SKU: TECH-PS5-001                   │
│ 📍 Zone A - Shelf B2                │
│                                     │
│ ─────────────────────────────────  │
│                                     │
│ Expected Qty:  [  10  ]             │
│                                     │
│ Actual Quantity:                    │
│  ┌───┐   ┌─────────┐   ┌───┐      │
│  │ - │   │    8    │   │ + │      │ ← Large +/- buttons
│  └───┘   └─────────┘   └───┘      │
│                                     │
│ ⚠️ Variance: -2 (-20%)             │ ← Variance alert
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Notes (required)                │ │
│ │ 2 units damaged, removed from   │ │
│ │ shelf                           │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [  Skip  ]   [✓ Save Count  ]      │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ ✅ Recently Counted (5)             │
│                                     │
│ Sony PS5          10 → 8    [-2]   │
│ Xbox Series X     5 → 5     [✓]    │
│ Nintendo Switch   15 → 15   [✓]    │
│ ...                                 │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│                                     │
│    [  ✓ Complete Count  ]          │ ← Fixed bottom button
│                                     │
│    7/20 items - 13 remaining        │
└─────────────────────────────────────┘
```

**Key Elements:**
- Fixed header with count # and progress
- Prominent scanner section
- Large item card with clear layout
- Touch-optimized quantity controls
- Visual variance warnings
- Required notes field
- Fixed bottom completion button

---

## Component Details

### Status Chips

```
[📅 Scheduled]   - Gray background
[⏳ InProgress]  - Blue background
[✅ Completed]   - Green background
[❌ Cancelled]   - Red background
```

### Progress Bars

```
Empty:      ░░░░░░░░░░░░░░░░ 0%
In Progress: ▓▓▓▓▓░░░░░░░░░░░ 35%
Complete:    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ 100%
```

### Variance Indicators

```
No Variance:     ✅ [Icon]
Small (<5%):     📊 [+2]
Medium (5-10%):  ⚠️ [-3]
Large (>10%):    🚨 [+15]
```

### Action Buttons

```
Primary (Green):
┌─────────────────────┐
│  ✓ Save Count       │
└─────────────────────┘

Secondary (Blue):
┌─────────────────────┐
│  → Continue Count   │
└─────────────────────┘

Outlined (Default):
┌─────────────────────┐
│  Skip               │
└─────────────────────┘

Danger (Red):
┌─────────────────────┐
│  × Cancel           │
└─────────────────────┘
```

---

## Responsive Breakpoints

### Mobile Portrait (320px - 480px)
- Single column layout
- Full-width cards
- Large touch targets (48px min)
- Stacked buttons

### Mobile Landscape (480px - 768px)
- Same as portrait
- Slightly wider cards
- Better use of horizontal space

### Tablet (768px+)
- Two-column card layout
- Side-by-side buttons
- More items visible
- Consider switching to desktop view

---

## Color Scheme

```css
/* Status Colors */
--status-scheduled: #9E9E9E    /* Gray */
--status-inprogress: #2196F3   /* Blue */
--status-completed: #4CAF50    /* Green */
--status-cancelled: #F44336    /* Red */

/* Variance Colors */
--variance-none: #4CAF50       /* Green */
--variance-small: #2196F3      /* Blue */
--variance-medium: #FF9800     /* Orange */
--variance-large: #F44336      /* Red */

/* UI Elements */
--primary: #594AE2             /* Purple */
--background: #F5F5F5          /* Light Gray */
--card: #FFFFFF                /* White */
--text: #212121                /* Dark Gray */
--text-secondary: #757575      /* Medium Gray */
```

---

## Touch Target Sizes

```
Minimum Touch Target: 48x48px

Small:      40x40px (icon buttons)
Medium:     48x48px (standard)
Large:      56x56px (primary actions)
Extra Large: 64x64px (quantity +/-)
```

---

## Typography

```
Page Title:     H6 (20px, Semi-bold)
Section Header: Subtitle1 (16px, Semi-bold)
Body Text:      Body1 (14px, Regular)
Caption:        Caption (12px, Regular)
Button:         Button (14px, Semi-bold)
```

---

## Spacing System

```
Tiny:    4px   (gaps between elements)
Small:   8px   (card padding)
Medium:  16px  (section spacing)
Large:   24px  (major sections)
XLarge:  32px  (top/bottom margins)
```

---

## Animation & Feedback

### Loading States
```
Spinner:        Indeterminate progress
Skeleton:       Card placeholder
Shimmer:        Loading content
```

### Success Feedback
```
✅ Green check icon
Success snackbar
Haptic vibration (200ms)
```

### Error Feedback
```
❌ Red alert
Error snackbar
Shake animation
```

### Scan Feedback
```
Green flash overlay
Vibration (200ms)
Success sound (optional)
Auto-close scanner
```

---

## Icon Usage

```
📱 = Mobile/Phone
🏢 = Warehouse/Building
📍 = Location/Pin
⏰ = Time/Clock
📊 = Statistics/Chart
📅 = Calendar/Schedule
⏳ = In Progress/Hourglass
✅ = Success/Complete
❌ = Error/Cancel
⚠️ = Warning/Alert
🔍 = Search
📸 = Camera
✓ = Checkmark
← = Back/Return
→ = Forward/Continue
🔄 = Refresh/Reload
🖥️ = Desktop/Computer
```

---

## Accessibility

### Touch Targets
- ✅ Minimum 48x48px
- ✅ Adequate spacing (8px min)
- ✅ No overlapping targets

### Color Contrast
- ✅ Text: 4.5:1 minimum
- ✅ Icons: 3:1 minimum
- ✅ Status indicators: Multiple cues (color + icon + text)

### Screen Readers
- ✅ Semantic HTML
- ✅ ARIA labels on icons
- ✅ Status announcements
- ✅ Error messages

---

## Example User Flow

```
1. User Opens Mobile App
   ↓
2. Sees Today's Counts
   ↓
3. Taps "Start Count"
   ↓
4. Counting Interface Opens
   ↓
5. Taps "Start Scanner"
   ↓
6. Camera Activates
   ↓
7. Points at Barcode
   ↓
8. Barcode Detected (Flash + Vibrate)
   ↓
9. Item Details Load
   ↓
10. User Sees Expected Qty
    ↓
11. User Adjusts Actual Qty (+/-)
    ↓
12. Variance Alert Shows (if >5%)
    ↓
13. User Adds Notes (if variance)
    ↓
14. User Taps "Save Count"
    ↓
15. Success Message
    ↓
16. Recent Items Updates
    ↓
17. Progress Updates
    ↓
18. Ready for Next Item (back to #5)
    ↓
19. When All Done: "Complete Count" Enabled
    ↓
20. User Taps "Complete Count"
    ↓
21. Confirmation Dialog
    ↓
22. Count Marked Complete
    ↓
23. Returns to Count List
```

---

## Best Practices Applied

✅ **Mobile-First Design**
- Touch-optimized controls
- Large, clear typography
- Thumb-friendly layout

✅ **Progressive Disclosure**
- Show only what's needed
- Expandable sections
- Step-by-step workflow

✅ **Immediate Feedback**
- Visual indicators
- Success/error messages
- Progress tracking

✅ **Error Prevention**
- Validation before save
- Confirmation dialogs
- Clear requirements

✅ **Accessibility**
- High contrast
- Large touch targets
- Screen reader support

---

**Visual Reference Complete!** 🎨

Use this guide to understand the mobile UI layout and design decisions.

Next: [Setup Barcode Scanner](BARCODE_SCANNER_SETUP.md)

