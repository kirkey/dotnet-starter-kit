# Cycle Counts - User Guide

**Quick Reference for Using the Cycle Counts UI**

---

## Overview

The Cycle Counts module helps you manage inventory counts with workflow operations, variance tracking, and reconciliation features.

**Route**: `/store/cycle-counts`

---

## Quick Start

### Creating a New Cycle Count

1. Click the **"Add"** button
2. Fill in the form:
   - **Count Number**: Unique identifier (e.g., CC-2025-001)
   - **Warehouse**: Select from dropdown (required)
   - **Scheduled Date**: When the count should occur
   - **Count Type**: Choose one:
     - **Full**: Count all items
     - **Partial**: Count specific items
     - **ABC**: Count based on ABC classification
     - **Random**: Random spot check
   - **Name**: Optional descriptive name
   - **Counter Name**: Person who will perform the count
   - **Description**: Optional details
   - **Notes**: Any additional information
3. Click **"Save"**
4. Count is created with status: **Scheduled**

---

## Adding Items to a Count

1. Find your count in the list
2. Click the **⋮** menu icon → **View Details**
3. Click **"Add Item"** button
4. Select an **Item** from the dropdown
5. System displays current inventory quantity
6. Add optional **Notes**
7. Click **"Add Item"**
8. Repeat for all items to count

---

## Counting Workflow

### Step 1: Start the Count

1. Find your **Scheduled** count
2. Click the **⋮** menu icon → **Start Count**
3. Confirm the action
4. Status changes to **InProgress**

### Step 2: Record Counts

1. Open the count details (⋮ → View Details)
2. For each item, click the **Edit** icon (pencil)
3. In the Record Count dialog:
   - View the **System Quantity** (expected)
   - Enter the **Counted Quantity** (actual physical count)
   - System calculates **Variance** in real-time:
     - 🟢 Green = Perfect match (no variance)
     - 🔵 Blue = Small variance (< 5 units)
     - 🟠 Orange = Moderate variance (5-9 units)
     - 🔴 Red = Significant variance (≥ 10 units)
   - Enter **Counted By** (your name)
   - Add **Notes** to explain any variances
4. Click **"Save Count"**
5. Repeat for all items

### Step 3: Complete the Count

1. After recording all items
2. Click the **⋮** menu icon → **Complete Count**
3. Confirm the action
4. Status changes to **Completed**
5. System calculates all variances

### Step 4: Reconcile Variances (if any)

1. If there are variances, a **"Reconcile Variances"** option appears
2. Click the **⋮** menu icon → **Reconcile Variances**
3. Confirm the action
4. System adjusts inventory levels to match counted quantities
5. Creates audit trail transactions

---

## Searching and Filtering

### Basic Search
Use the search box to find counts by Count Number, Name, or Description.

### Advanced Search
Click **"Advanced Search"** to filter by:
- **Warehouse**: Filter by specific warehouse
- **Status**: Filter by count status
  - Scheduled
  - In Progress
  - Completed
  - Cancelled
- **Count Type**: Filter by count type
  - Full
  - Partial
  - ABC
  - Random
- **Count Date From**: Start date range
- **Count Date To**: End date range

---

## Understanding the Count List

### Columns Displayed

| Column | Description |
|--------|-------------|
| **Count #** | Unique count identifier |
| **Warehouse** | Where the count is performed |
| **Count Date** | Scheduled date |
| **Status** | Current status (color-coded) |
| **Type** | Full/Partial/ABC/Random |
| **Total Items** | Number of items in the count |
| **Counted** | Number of items counted so far |
| **Variances** | Number of items with variances |

### Status Colors

- ⚪ **Scheduled**: Gray - Count is planned
- 🔵 **InProgress**: Blue - Counting is ongoing
- 🟢 **Completed**: Green - Count finished
- 🔴 **Cancelled**: Red - Count was cancelled

---

## Context Menu Actions

### View Details
- **Available**: Always
- **Action**: Opens detailed view of the count

### Start Count
- **Available**: Scheduled status only
- **Action**: Changes status to InProgress

### Complete Count
- **Available**: InProgress status only
- **Action**: Finalizes the count, calculates variances

### Reconcile Variances
- **Available**: Completed status with variances > 0
- **Action**: Adjusts inventory to match counted quantities

### Cancel Count
- **Available**: Scheduled or InProgress status
- **Action**: Cancels the count (cannot be undone)

---

## Count Details View

### Header Information
- Count Number and Status
- Warehouse and Location (if applicable)
- Count Type and Scheduled Date
- Start Date and Completion Date (when available)
- Counter Name
- Progress: X / Y items counted (with progress bar)
- Variance count
- Description and Notes

### Items Table
Shows all items in the count with:
- **Item Name**: Product being counted
- **System Qty**: Expected quantity from inventory
- **Counted Qty**: Actual physical count
- **Variance**: Difference (System - Counted)
  - 🟢 Green = Match (0 variance)
  - 🔴 Red = Difference (shows amount)
- **Recount**: ⚠️ Warning icon if significant variance
- **Actions**: Edit button to record/update count (InProgress only)

---

## Variance Tracking

### Variance Calculation
```
Variance = Counted Quantity - System Quantity

Examples:
System: 100, Counted: 100 → Variance: 0 (Perfect match) 🟢
System: 100, Counted: 105 → Variance: +5 (Overage) 🔴
System: 100, Counted: 95  → Variance: -5 (Shortage) 🔴
```

### Variance Alerts
- **Success** (Green): No variance - perfect match
- **Info** (Blue): Small variance (< 5 units)
- **Warning** (Orange): Moderate variance (5-9 units)
- **Error** (Red): Significant variance (≥ 10 units)

### Recount Recommendations
When recording a count with variance ≥ 10 units, the system will:
1. Display a warning message
2. Suggest performing a recount
3. Allow you to add notes explaining the variance

---

## Best Practices

### Planning
1. **Schedule counts** during low-activity periods
2. **Use ABC analysis** to prioritize high-value items
3. **Assign specific counters** for accountability
4. **Plan by location** for efficient physical counting

### Counting
1. **Count physically first** before looking at system quantity
2. **Record immediately** to avoid forgetting
3. **Add notes** for any variances
4. **Recount** items with significant variances
5. **Take photos** of discrepancies (if needed)

### Reconciliation
1. **Review all variances** before reconciling
2. **Investigate** significant differences
3. **Document reasons** for variances
4. **Train staff** on proper counting procedures
5. **Monitor trends** to identify recurring issues

---

## Common Scenarios

### Scenario 1: Full Warehouse Count
```
1. Create count (Type: Full)
2. Add all items in the warehouse
3. Assign to counter
4. Start count
5. Counter records all quantities
6. Complete count
7. Review variances
8. Reconcile to adjust inventory
```

### Scenario 2: ABC Cycle Count
```
1. Create count (Type: ABC)
2. Add high-value A items only
3. Schedule for weekly
4. Start count on schedule
5. Record counts carefully
6. Complete and reconcile
7. Repeat weekly
```

### Scenario 3: Random Spot Check
```
1. Create count (Type: Random)
2. Add 10-20 random items
3. Start count immediately
4. Quick physical count
5. Record findings
6. Complete count
7. Review variances
8. Investigate any issues
```

### Scenario 4: Investigating Variances
```
1. Complete count shows variances
2. View details to see which items
3. Items with large variances:
   - Don't reconcile yet
   - Cancel the count
   - Create new count for problem items
   - Recount carefully
   - Investigate causes (theft, damage, errors)
4. Once satisfied, reconcile
```

---

## Troubleshooting

### Can't Start Count
- **Cause**: Count must be in Scheduled status
- **Solution**: Check status, create new count if needed

### Can't Add Items
- **Cause**: Count must be Scheduled or InProgress
- **Solution**: Check status, start count if scheduled

### Can't Record Counts
- **Cause**: Count must be InProgress
- **Solution**: Start the count first

### Can't See Reconcile Option
- **Cause**: Count must be Completed with variances > 0
- **Solution**: Complete the count first, check if variances exist

### Item Not in Dropdown
- **Cause**: Item not active or not found
- **Solution**: Check Items page, ensure item is active

---

## Keyboard Shortcuts

While in dialogs:
- **Enter**: Submit form (when focus is on a button)
- **Escape**: Close dialog
- **Tab**: Navigate between fields

---

## Tips & Tricks

### Efficiency
- Use **Advanced Search** to quickly find counts by warehouse
- **Sort by Count Date** to see upcoming counts
- **Filter by Status** to focus on InProgress counts
- Keep the **Details dialog open** while counting to quickly update items

### Accuracy
- Always **record the physical count first** before looking at system quantity
- **Count twice** for high-value items
- **Take breaks** during long counts to maintain accuracy
- **Use Notes field** to document unusual situations

### Organization
- Use **consistent naming** for count numbers (e.g., CC-YYYY-NNN)
- **Document your process** in the Description field
- **Assign specific counters** for accountability
- **Track completion time** to estimate future counts

---

## Related Documentation

- [CYCLE_COUNTS_UI_IMPLEMENTATION.md](./CYCLE_COUNTS_UI_IMPLEMENTATION.md) - Technical implementation details
- [CYCLE_COUNTS_VERIFICATION.md](./CYCLE_COUNTS_VERIFICATION.md) - Complete verification report
- [PAGES_ORGANIZATION.md](./PAGES_ORGANIZATION.md) - Navigation and page structure

---

## Support

For technical issues or questions:
1. Check this user guide first
2. Review the implementation documentation
3. Check the Store module documentation
4. Contact your system administrator

---

*Last Updated: October 25, 2025*  
*Version: 1.0*

