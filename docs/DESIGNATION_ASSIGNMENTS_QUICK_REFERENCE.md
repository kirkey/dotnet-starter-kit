# Designation Assignments UI - Quick Reference Guide

## Access Point
Navigate to: **Human Resources → Designation Assignments**

## Tab 1: Current Assignments

### Create New Assignment

1. **Click "Create" button**
2. **Fill in Assignment Details**
   - **Employee ID**: Enter the employee's GUID or number (required)
     - Employee Name and Number will auto-populate
   - **Designation ID**: Enter the designation GUID (required)
     - Designation Title will auto-populate
   - **Assignment Type**: Displays as read-only (determined by IsPlantilla/IsActingAs)

3. **Set Date Range**
   - **Effective Date**: When the assignment starts (required)
   - **End Date**: When it ends (optional, leave blank for ongoing)
   - **Tenure**: Auto-calculated and displayed

4. **Salary Information**
   - **Adjusted Salary**: Optional, only for Acting As with different pay

5. **Add Reason**
   - Helps track WHY the assignment was made
   - Examples: "Promotion", "Acting Manager", "Transfer"

6. **Set Status**
   - Toggle **Active** to activate/deactivate

7. **Click "Save"**

### Edit Assignment
- Click edit icon on any row
- Typically used to add an End Date or update Reason
- Click "Save" to confirm

### View History
- Move to **"Assignment History"** tab (see below)

---

## Tab 2: Assignment History

### View Employee History

1. **Optional: Filter by Employee**
   - Enter Employee ID to filter to one employee
   - Leave blank to see all employees

2. **Optional: Filter by Date Range**
   - Set "From Date" to start of period
   - Set "To Date" to end of period
   - Leave blank for all dates

3. **Click "Load History"**
   - Table populates with employees matching filters
   - Shows current designation, start date, and change count

### View Detailed Timeline
- Click **"View Details"** button on any row
- Dialog shows complete career history:
  - Designation name
  - Assignment type (Plantilla/Acting)
  - Period dates
  - Tenure months
  - In chronological order (newest first)

---

## Assignment Types Explained

### Plantilla (Primary)
- Official/permanent position
- Only ONE active at a time per employee
- Usually no end date (ongoing)
- Main job title
- **Examples**: "Senior Engineer", "Manager", "Analyst"

### Acting As (Temporary)
- Temporary different responsibilities
- Multiple allowed simultaneously
- Should have end date
- Can have adjusted salary
- Does NOT replace Plantilla
- **Examples**: "Acting Manager" (3 months), "Interim Project Lead"

---

## Common Workflows

### Scenario 1: Promote an Employee
1. Go to Current Assignments tab
2. Create NEW assignment
3. Select same employee, NEW designation
4. Set Type to Plantilla
5. Set Effective Date to promotion date
6. Reason: "Promotion - Leadership Development"
7. Save
8. Previous assignment automatically ends

### Scenario 2: Temporary Acting Role
1. Go to Current Assignments tab
2. Create NEW assignment (same employee)
3. Select NEW acting designation
4. Set Type to Acting As
5. Set Effective Date (start)
6. Set End Date (3 months later, for example)
7. Add Adjusted Salary if different pay
8. Reason: "Acting Manager - Vacancy Coverage"
9. Save
10. After End Date, system returns to Plantilla

### Scenario 3: Transfer Employee
1. Go to Current Assignments tab
2. Create NEW assignment
3. Same employee, new designation (possibly same title, different location)
4. Set Type to Plantilla
5. Reason: "Transfer to London Office"
6. Save
7. Previous location's assignment ends

### Scenario 4: View Employee's Full History
1. Go to Assignment History tab
2. Enter Employee ID
3. Click "Load History"
4. Click "View Details"
5. See complete career timeline

---

## Tips & Best Practices

### Date Management
- ✅ Always set Effective Date (when does it start?)
- ✅ Set End Date for temporary roles
- ❌ Don't leave End Date for permanent roles
- ✅ Tenure auto-calculates (you don't enter it)

### Documentation
- ✅ Fill in Reason field with clear context
- ✅ Use consistent terminology:
  - "Promotion - [reason]"
  - "Acting [role] - [duration]"
  - "Transfer - [location/department]"
  - "Temporary Assignment - [project]"
- ❌ Don't leave Reason blank

### Salary Fields
- ✅ Use Adjusted Salary only for Acting As with different pay
- ✅ Format as currency (system handles conversion)
- ❌ Don't modify base salary here (use Payroll module)

### History Tab
- ✅ Use for audits and career tracking
- ✅ Filter by date to see periods (e.g., "all assignments in 2025")
- ❌ Don't use to create assignments (use Tab 1)

---

## Help & Support

Click the **Help** button (top right) for comprehensive documentation including:
- Detailed concept explanations
- Type comparisons (Plantilla vs Acting As)
- Step-by-step creation process
- Real-world use case examples
- Salary adjustment guidance

---

## Common Questions

**Q: Can an employee have multiple Plantilla assignments?**
A: No, only ONE Plantilla (primary) at a time.

**Q: Can an employee have multiple Acting As assignments?**
A: Yes, multiple Acting assignments can overlap.

**Q: What happens when an Acting As ends?**
A: The system returns to the Plantilla designation automatically.

**Q: Can I undo an assignment?**
A: Not undo, but you can set an End Date to conclude it.

**Q: Are Salary adjustments real?**
A: They're tracked here for acting roles but don't affect actual payroll (use Payroll module for that).

**Q: How do I know when an assignment is effective?**
A: The system checks Effective Date ≤ today ≤ End Date (if set).

---

## Fields Reference

| Field | Required | Type | Notes |
|-------|----------|------|-------|
| Employee ID | Yes | GUID | Auto-fills name & number |
| Designation ID | Yes | GUID | Auto-fills title |
| Effective Date | Yes | Date | Starts the assignment |
| End Date | No | Date | Ends the assignment (if temporary) |
| Assignment Type | No | Dropdown | Read-only, shows Plantilla/Acting |
| Adjusted Salary | No | Currency | For Acting As only |
| Reason | No | Text | Free-form explanation |
| Active | No | Toggle | Activates/deactivates |
| Tenure | No | Calculated | Auto-calculated from dates |

