# Time & Attendance Module - Quick Usage Guide

## 📱 Attendance API

### Clock In (Create)
```csharp
var command = new CreateAttendanceCommand(
    EmployeeId: employeeId,
    ClockInTime: new TimeSpan(8, 0, 0),
    ClockInLocation: "Building A, Floor 2"
);
var response = await mediator.Send(command);
// Returns: AttendanceResponse with attendance Id
```

### Clock Out (Update)
```csharp
var command = new UpdateAttendanceCommand(
    Id: attendanceId,
    ClockOutTime: new TimeSpan(17, 0, 0),
    ClockOutLocation: "Building A, Floor 2",
    Status: "Present"
);
var response = await mediator.Send(command);
```

### Search Attendance
```csharp
var request = new SearchAttendanceRequest
{
    PageNumber = 1,
    PageSize = 50,
    EmployeeId = employeeId,           // Optional
    StartDate = new DateTime(2025, 11, 1),
    EndDate = new DateTime(2025, 11, 30),
    Status = "Late",                   // Optional: Present, Late, Absent
    IsApproved = true                  // Optional
};
var result = await mediator.Send(request);
// Returns: PagedList<AttendanceResponse>
```

---

## 📋 Timesheet API

### Create Timesheet
```csharp
var command = new CreateTimesheetCommand(
    EmployeeId: employeeId,
    StartDate: new DateTime(2025, 11, 10),
    EndDate: new DateTime(2025, 11, 16),
    PeriodType: "Weekly"              // Weekly, BiWeekly, Monthly
);
var response = await mediator.Send(command);
// Returns: TimesheetResponse with timesheet Id
```

### Update Timesheet Hours
```csharp
var command = new UpdateTimesheetCommand(
    Id: timesheetId,
    RegularHours: 40,
    OvertimeHours: 5,
    Status: "Submitted"               // Draft, Submitted, Approved, Rejected
);
var response = await mediator.Send(command);
```

### Search Timesheets
```csharp
var request = new SearchTimesheetsRequest
{
    PageNumber = 1,
    PageSize = 25,
    EmployeeId = employeeId,
    StartDate = new DateTime(2025, 11, 1),
    EndDate = new DateTime(2025, 11, 30),
    Status = "Approved",
    IsApproved = true
};
var result = await mediator.Send(request);
// Returns: PagedList<TimesheetResponse>
```

---

## ⏰ Shift API

### Create Shift
```csharp
var command = new CreateShiftCommand(
    ShiftName: "Morning Shift",
    StartTime: new TimeSpan(6, 0, 0),
    EndTime: new TimeSpan(14, 0, 0),
    IsOvernight: false,
    BreakDurationMinutes: 60,
    Description: "Early morning shift"
);
var response = await mediator.Send(command);
// Returns: CreateShiftResponse with shift Id
```

### Update Shift
```csharp
var command = new UpdateShiftCommand(
    Id: shiftId,
    ShiftName: "Morning Shift Updated",
    StartTime: new TimeSpan(6, 30, 0),
    EndTime: new TimeSpan(14, 30, 0),
    BreakDurationMinutes: 45,
    Description: "Updated morning shift"
);
var response = await mediator.Send(command);
```

### Search Shifts
```csharp
var request = new SearchShiftsRequest
{
    PageNumber = 1,
    PageSize = 100,
    SearchString: "Morning",           // Filter by name or description
    IsActive: true
};
var result = await mediator.Send(request);
// Returns: PagedList<ShiftResponse>
```

---

## 🗓️ Holiday API

### Create Holiday
```csharp
var command = new CreateHolidayCommand(
    HolidayName: "Thanksgiving",
    HolidayDate: new DateTime(2025, 11, 27),
    IsPaid: true,
    IsRecurringAnnually: true,
    Description: "National Thanksgiving Holiday"
);
var response = await mediator.Send(command);
```

### Update Holiday
```csharp
var command = new UpdateHolidayCommand(
    Id: holidayId,
    HolidayName: "Thanksgiving Day",
    HolidayDate: null,
    IsPaid: true,
    IsRecurringAnnually: true,
    Description: "National Thanksgiving Holiday"
);
var response = await mediator.Send(command);
```

### Search Holidays
```csharp
var request = new SearchHolidaysRequest
{
    PageNumber = 1,
    PageSize = 50,
    SearchString: "Thanksgiving",
    StartDate: new DateTime(2025, 11, 1),
    EndDate: new DateTime(2025, 12, 31),
    IsPaid: true,
    IsActive: true
};
var result = await mediator.Send(request);
// Returns: PagedList<HolidayResponse>
```

---

## 🔍 Common Queries

### Get Today's Attendance for All Employees
```csharp
var request = new SearchAttendanceRequest
{
    PageNumber = 1,
    PageSize = 1000,
    StartDate = DateTime.Today,
    EndDate = DateTime.Today,
    IsApproved = false                 // Pending approval
};
var todayAttendance = await mediator.Send(request);
```

### Get Late Employees This Week
```csharp
var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
var weekEnd = weekStart.AddDays(7);

var request = new SearchAttendanceRequest
{
    PageNumber = 1,
    PageSize = 100,
    StartDate = weekStart,
    EndDate = weekEnd,
    Status = "Late"
};
var lateEmployees = await mediator.Send(request);
```

### Get Pending Timesheets for Approval
```csharp
var request = new SearchTimesheetsRequest
{
    PageNumber = 1,
    PageSize = 50,
    Status = "Submitted",
    IsApproved = false
};
var pendingTimesheets = await mediator.Send(request);
```

### Get All Paid Holidays for Current Year
```csharp
var request = new SearchHolidaysRequest
{
    PageNumber = 1,
    PageSize = 100,
    StartDate = new DateTime(2025, 1, 1),
    EndDate = new DateTime(2025, 12, 31),
    IsPaid: true,
    IsActive: true
};
var paidHolidays = await mediator.Send(request);
```

### Get Employee's Timesheets for Month
```csharp
var request = new SearchTimesheetsRequest
{
    PageNumber = 1,
    PageSize = 10,
    EmployeeId = employeeId,
    StartDate = new DateTime(2025, 11, 1),
    EndDate = new DateTime(2025, 11, 30)
};
var monthlyTimesheets = await mediator.Send(request);
```

---

## ✅ Validation Rules

### Attendance
- Employee ID: Required, must exist
- Clock In Time: Required, valid TimeSpan
- Clock Out Location: Max 250 characters
- Status: Present, Late, Absent, LeaveApproved
- Manager Comment: Max 500 characters

### Timesheet
- Employee ID: Required, must exist
- Period dates: Start < End
- Period Type: Weekly, BiWeekly, or Monthly
- Hours: Must be >= 0
- Status: Draft, Submitted, Approved, Rejected, Locked

### Shift
- Shift Name: Required, max 100 characters
- Start Time: Required
- End Time: Required, different from start (or overnight)
- Break Duration: >= 0 minutes
- Description: Max 500 characters

### Holiday
- Holiday Name: Required, max 100 characters
- Holiday Date: Required, not in past
- IsPaid: Boolean (true/false)
- IsRecurringAnnually: Boolean (true/false)
- Description: Max 500 characters

---

## 🚀 Best Practices

### 1. Always Paginate
```csharp
// Good
var request = new SearchAttendanceRequest
{
    PageNumber = 1,
    PageSize = 50,
    // ... filters
};

// Bad - No pagination
var request = new SearchAttendanceRequest
{
    // ... no PageNumber/PageSize
};
```

### 2. Use Date Filters Appropriately
```csharp
// Good
var request = new SearchAttendanceRequest
{
    StartDate = beginningOfMonth,
    EndDate = endOfMonth,
    EmployeeId = specificEmployee
};

// Bad - Too broad
var request = new SearchAttendanceRequest
{
    // No date filters = multiple years of data
};
```

### 3. Filter by Status First
```csharp
// Good - Reduces results immediately
var request = new SearchAttendanceRequest
{
    Status = "Late",                   // Narrow results first
    StartDate = today,
    EndDate = today
};
```

### 4. Handle Timesheet Workflows
```csharp
// Workflow: Draft → Submitted → Approved → Locked
var timesheet = await mediator.Send(new GetTimesheetRequest(id));

if (timesheet.Status == "Draft")
{
    // Allow editing
    await mediator.Send(new UpdateTimesheetCommand(...));
}

if (timesheet.Status == "Submitted")
{
    // Ready for approval
}

if (timesheet.IsLocked)
{
    // Cannot modify
}
```

---

## 📊 Response Examples

### AttendanceResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "employeeId": "550e8400-e29b-41d4-a716-446655440001",
  "attendanceDate": "2025-11-14",
  "clockInTime": "08:00:00",
  "clockOutTime": "17:00:00",
  "clockInLocation": "Building A, Floor 2",
  "clockOutLocation": "Building A, Floor 2",
  "hoursWorked": 9.0,
  "status": "Present",
  "minutesLate": null,
  "reason": null,
  "isApproved": true,
  "managerComment": "All good",
  "isActive": true
}
```

### TimesheetResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "employeeId": "550e8400-e29b-41d4-a716-446655440001",
  "startDate": "2025-11-10",
  "endDate": "2025-11-16",
  "periodType": "Weekly",
  "regularHours": 40.0,
  "overtimeHours": 2.0,
  "totalHours": 42.0,
  "status": "Approved",
  "approverManagerId": "550e8400-e29b-41d4-a716-446655440003",
  "submittedDate": "2025-11-17",
  "approvedDate": "2025-11-18",
  "isLocked": true,
  "isApproved": true
}
```

### ShiftResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440004",
  "shiftName": "Morning Shift",
  "startTime": "06:00:00",
  "endTime": "14:00:00",
  "isOvernight": false,
  "breakDurationMinutes": 60,
  "workingHours": 7.5,
  "description": "Early morning shift",
  "isActive": true
}
```

### HolidayResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440005",
  "holidayName": "Thanksgiving",
  "holidayDate": "2025-11-27",
  "isPaid": true,
  "isRecurringAnnually": true,
  "description": "National Thanksgiving Holiday",
  "isActive": true
}
```

---

**Last Updated:** November 14, 2025  
**Status:** ✅ Implementation Complete  
**Build Status:** ✅ SUCCESS

