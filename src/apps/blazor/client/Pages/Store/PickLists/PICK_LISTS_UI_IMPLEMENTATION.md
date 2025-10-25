# Pick Lists UI Implementation

## Overview
Complete Blazor UI implementation for the Pick Lists feature, enabling warehouse managers to create, assign, and track picking tasks for order fulfillment with optimized pick paths and workflow management.

## Files Created/Updated

### Main Page (2 files)
1. **PickLists.razor** - Main page with advanced search, create/edit form, and workflow actions
2. **PickLists.razor.cs** - Page logic with CRUD operations and workflow state management

### Dialog Components (4 files)
3. **PickListDetailsDialog.razor** - Dialog for viewing pick list details including items and progress
4. **PickListDetailsDialog.razor.cs** - Dialog logic for loading and displaying details
5. **AssignPickListDialog.razor** - Dialog for assigning pick lists to pickers
6. **AssignPickListDialog.razor.cs** - Dialog logic for the assignment operation

## Features Implemented

### 1. Main Pick Lists Page

#### Data Grid Columns
- **Pick List #** - Unique pick list identifier
- **Warehouse** - Warehouse ID where picking occurs
- **Status** - Current workflow status (Created, Assigned, InProgress, Completed, Cancelled)
- **Type** - Picking type (Order, Wave, Batch, Zone)
- **Priority** - Priority level (higher number = higher priority)
- **Assigned To** - Name/ID of assigned picker
- **Total Items** - Total number of items to pick
- **Picked** - Number of items already picked
- **Started** - Date/time when picking started

#### Advanced Search Filters (8 filters)
- **Warehouse** - Filter by warehouse (dropdown with all warehouses)
- **Status** - Filter by status (Created, Assigned, InProgress, Completed, Cancelled)
- **Picking Type** - Filter by type (Order, Wave, Batch, Zone)
- **Assigned To** - Filter by picker name/ID (text search)
- **Min Priority** - Minimum priority level
- **Max Priority** - Maximum priority level
- **Start Date From** - Filter by start date range (from)
- **Start Date To** - Filter by start date range (to)

#### Create/Edit Form Fields
- **Pick List Number** - Required, unique identifier
- **Warehouse** - Required, autocomplete component
- **Picking Type** - Required, dropdown (Order, Wave, Batch, Zone)
- **Priority** - Numeric field, min 0 (higher = higher priority)
- **Reference Number** - Optional, text field
- **Expected Completion** - Optional, date picker (future dates only)
- **Notes** - Optional, multiline text (max 500 characters)
- **Status** - Read-only when editing, displays current workflow status
- **Pick List Id** - Read-only when editing, displays entity ID

#### Row Actions (Workflow-Based)
1. **View Details** - Opens detailed view dialog (always available)
2. **Assign to Picker** - Opens assignment dialog (only for Status = "Created")
3. **Start Picking** - Starts picking workflow (only for Status = "Assigned")
4. **Complete Picking** - Completes picking workflow (only for Status = "InProgress")
5. **Delete** - Standard delete action (inherited from EntityTable)

### 2. Pick List Details Dialog

#### Information Sections

**General Information**
- Pick List Number
- Status (with color-coded chip)
- Warehouse Name
- Picking Type
- Priority
- Assigned To (if assigned)
- Reference Number (if provided)

**Progress Tracking**
- Total Items - Total line items in pick list
- Picked Items - Number of items picked
- Completion Percentage - Calculated percentage with progress bar
- Visual progress bar showing completion

**Dates & Timeline**
- Started - When picking started (if started)
- Completed - When picking completed (if completed)
- Expected Completion - Expected completion date (if set)

**Notes**
- Displays notes field if provided

**Pick List Items Table**
- Sequence Number - Pick path order
- Item Name - Item to be picked
- Bin Name - Location to pick from
- Quantity To Pick - Required quantity
- Quantity Picked - Actually picked quantity
- Status - Item status (Pending, Picked, Short, Substituted)

#### Status Color Coding
- **Created** - Default (Gray)
- **Assigned** - Info (Blue)
- **InProgress** - Warning (Orange)
- **Completed** - Success (Green)
- **Cancelled** - Error (Red)

#### Item Status Color Coding
- **Pending** - Default (Gray)
- **Picked** - Success (Green)
- **Short** - Warning (Orange)
- **Substituted** - Info (Blue)

#### Features
- Add Item button (only visible when Status = "Created" and no items exist)
- Items displayed in sortable, dense table
- Visual completion progress with percentage
- Responsive layout for all screen sizes

### 3. Assign Pick List Dialog

#### Features
- Displays pick list summary in read-only table
- Shows impact message with total items count
- Required **Assigned To** field (max 100 characters)
- Prevents assignment without providing picker name/ID
- Shows loading indicator during API call
- Displays error messages if assignment fails
- Closes and refreshes parent grid on success

#### Validation
- Assigned To is required and cannot be empty/whitespace
- Max length of 100 characters with counter
- Immediate validation feedback
- Disable button while submitting

#### Summary Information Displayed
- Pick List Number
- Warehouse
- Picking Type
- Priority
- Total Items
- Reference Number (if provided)

## Technical Implementation

### Backend Integration
- Uses generated `IClient` API client from NSwag
- API Version: "1"
- Endpoints used:
  - `SearchPickListsEndpointAsync` - Search with filters
  - `CreatePickListEndpointAsync` - Create new pick list
  - `GetPickListEndpointAsync` - Get details with items by ID
  - `AssignPickListEndpointAsync` - Assign to picker
  - `StartPickingEndpointAsync` - Start picking workflow
  - `CompletePickingEndpointAsync` - Complete picking workflow
  - `DeletePickListEndpointAsync` - Delete pick list

### Data Models
- **PickListResponse** - API search response type
  - Id, PickListNumber, WarehouseId, Status, PickingType
  - Priority, AssignedTo, TotalLines, CompletedLines
  - StartDate, CompletedDate, ExpectedCompletionDate, ReferenceNumber
- **GetPickListResponse** - API get details response (includes Items collection)
  - All PickListResponse properties plus Items array
  - Items: PickListItemDto with sequence, quantities, status
- **PickListViewModel** - Form model (extends CreatePickListCommand)
  - Includes additional Id, Status, and ExpectedCompletionDate for display
- **CreatePickListCommand** - Create operation
- **AssignPickListCommand** - Assignment operation with AssignedTo
- **StartPickingCommand** - Start picking operation (no properties needed)
- **CompletePickingCommand** - Complete picking operation (no properties needed)

### Component Architecture
- Follows CycleCounts and InventoryReservations patterns
- Uses `EntityTable` and `EntityServerTableContext` for grid functionality
- Leverages MudBlazor components throughout
- Injects dependencies via `_Imports.razor`: Client, Snackbar, DialogService
- Uses `IMudDialogInstance` for dialog management

### Search Implementation
Maps UI filter properties to `SearchPickListsCommand`:
```csharp
var command = new SearchPickListsCommand
{
    PageNumber, PageSize, Keyword, 
    OrderBy = ["Priority desc", "CreatedOn desc"],
    WarehouseId, Status, PickingType, AssignedTo,
    MinPriority, MaxPriority,
    StartDateFrom, StartDateTo
};
```

### Workflow State Machine Logic
```
Created → Assigned → InProgress → Completed
   ↓         ↓           ↓
   └─────────┴───────────┴──────→ Cancelled
```

**UI Actions by Status**:
- **Created**: Can assign, can delete
- **Assigned**: Can start picking, can delete
- **InProgress**: Can complete picking, can delete
- **Completed**: Read-only (terminal state)
- **Cancelled**: Read-only (terminal state)

## User Workflows

### Create New Pick List
1. Click "Add Pick List" button
2. Fill required fields: Pick List Number, Warehouse, Picking Type
3. Optionally set priority, reference, expected completion, notes
4. Click Save
5. Grid refreshes with new pick list in "Created" status

### View Pick List Details
1. Click "..." menu on any row
2. Select "View Details"
3. Dialog opens showing all pick list information and items
4. Review progress, timeline, and item details
5. Click "Close" to return to grid

### Assign Pick List Workflow
1. Click "..." menu on a row with Status = "Created"
2. Select "Assign to Picker"
3. Dialog shows pick list summary
4. Enter picker name/ID (required, max 100 chars)
5. Click "Assign Pick List"
6. API processes assignment, Status changes to "Assigned"
7. Grid refreshes showing updated status

### Start Picking Workflow
1. Click "..." menu on a row with Status = "Assigned"
2. Select "Start Picking"
3. Confirmation dialog appears
4. Click "Start Picking" to confirm
5. API processes start, Status changes to "InProgress", StartDate recorded
6. Grid refreshes showing in-progress status

### Complete Picking Workflow
1. Click "..." menu on a row with Status = "InProgress"
2. Select "Complete Picking"
3. Confirmation dialog appears
4. Click "Complete Picking" to confirm
5. API processes completion, Status changes to "Completed", CompletedDate recorded
6. Grid refreshes showing completed status

### Search & Filter
1. Click "Advanced Search" toggle
2. Select desired filters (Warehouse, Status, Type, Assigned To, Priority, Dates)
3. Grid automatically updates with filtered results
4. Clear filters to show all pick lists

## Business Rules

### Creation Rules
- Pick List Number must be unique
- Warehouse and Picking Type are required
- Priority must be non-negative integer (0 or higher)
- Expected Completion Date (if set) must be future date
- Picking Type must be one of: Order, Wave, Batch, Zone
- Status is automatically set to "Created"

### Assignment Rules
- Can only assign pick lists with Status = "Created"
- Assigned To field is required (max 100 characters)
- Assignment changes Status to "Assigned"
- Once assigned, cannot add more items

### Workflow Rules
- **Start Picking**: Requires Status = "Assigned" and AssignedTo must be set
- **Complete Picking**: Requires Status = "InProgress"
- Cannot start/complete in wrong sequence
- Once Completed or Cancelled, status cannot change (terminal states)

### Default Sorting
- Primary: Priority descending (highest priority first)
- Secondary: CreatedOn descending (newest first)
- Tertiary: PickListNumber ascending

## Integration Points

### Autocomplete Components Used
- **AutocompleteWarehouse** - For warehouse selection

### Dropdown Data Loading
- Warehouses loaded via `SearchWarehousesEndpointAsync` (500 max, ordered by Name)
- Loaded on page initialization

### Permission Requirements
Based on `FshResources.Store`:
- View/Search: `Permissions.Store.View`
- Create: `Permissions.Store.Create`
- Delete: `Permissions.Store.Delete`
- Assign/Start/Complete (Update): `Permissions.Store.Update`

## Styling & UX

### Responsive Layout
- Grid columns adapt to screen size
- Dialog content scrollable on small screens
- Form fields use responsive grid (xs/sm/md/lg breakpoints)

### Visual Indicators
- Color-coded status chips for pick lists
- Color-coded status chips for pick list items
- Progress bar showing completion percentage
- Loading spinners during async operations
- Error alerts for failed operations
- Success notifications for completed actions

### User Feedback
- Snackbar notifications for all operations
- Inline error messages in dialogs
- Disabled buttons during processing
- Required field validation
- Character counters for text fields
- Confirmation dialogs for workflow transitions

## Code Quality

### Documentation
- XML comments on all public methods
- Clear section headers in UI
- Descriptive property names
- Inline comments for complex logic

### CQRS Pattern
- Separate commands for Create, Assign, Start, Complete
- Read operations use Response DTOs
- Search uses dedicated SearchCommand
- No update operation (pick lists follow create-then-workflow pattern)

### DRY Principles
- Reusable status color mapping methods
- Shared dialog patterns
- Centralized error handling
- Common workflow confirmation logic

### Error Handling
- Try-catch blocks on all API calls
- User-friendly error messages
- Graceful degradation on failures
- Loading states during async operations

## Testing Scenarios

### Functional Testing
- [ ] Create pick list with all required fields
- [ ] Create pick list with optional fields
- [ ] Search by each filter individually
- [ ] Combine multiple search filters
- [ ] View details of different pick list statuses
- [ ] Assign a created pick list
- [ ] Start picking on an assigned pick list
- [ ] Complete picking on an in-progress pick list
- [ ] Delete a pick list
- [ ] Verify unique pick list number validation

### Workflow Testing
- [ ] Verify "Assign" button only shows for Created status
- [ ] Verify "Start Picking" button only shows for Assigned status
- [ ] Verify "Complete Picking" button only shows for InProgress status
- [ ] Verify cannot start picking without assignment
- [ ] Verify status changes correctly through workflow
- [ ] Verify terminal states (Completed, Cancelled) are read-only

### Edge Cases
- [ ] Create pick list with future expected completion date
- [ ] Assign without providing picker name (button disabled)
- [ ] View details of pick list with no items
- [ ] View details of pick list with multiple items
- [ ] Search with no results
- [ ] Load page with no warehouses available

### Integration Testing
- [ ] Verify API client methods called correctly
- [ ] Confirm status changes after each workflow action
- [ ] Test with different picking types
- [ ] Test priority-based sorting
- [ ] Verify start and completion dates recorded

## Future Enhancements

### Potential Features
1. **Add Items Dialog** - Add items to pick list with bin/lot/serial tracking
2. **Record Pick Dialog** - Record actual picked quantities per item
3. **Bulk Operations** - Assign or cancel multiple pick lists
4. **Pick Path Optimization** - Visual pick path with sequence optimization
5. **Quick Filters** - Preset filter buttons (My Picks, High Priority, etc.)
6. **Export** - Export pick list to Excel/PDF for printing
7. **Print Pick Ticket** - Generate printable pick ticket
8. **Barcode Scanning** - Mobile-friendly barcode scanning for picking
9. **Real-time Updates** - SignalR for live status updates
10. **Picker Assignment Dropdown** - Dropdown of available pickers instead of text field

## Compliance

### Coding Standards
✅ Follows CQRS principles
✅ Each class in separate file
✅ XML documentation on entities, methods, functions
✅ Uses string-based enums (PickingType, Status)
✅ Tighter validation on all inputs
✅ Consistent with CycleCounts and InventoryReservations patterns
✅ No database check constraints

---

## Related Documentation
- Backend: `/api/modules/Store/docs/Store_PickList_Implementation_Complete.md`
- API Endpoints: `/api/modules/Store/STORE_ENDPOINTS_COMPLETE.md`
- Pages Organization: `/apps/blazor/client/Pages/Store/Docs/PAGES_ORGANIZATION.md`

---
*Implementation Date: January 2025*
*Status: Complete and Ready for Testing ✅*

