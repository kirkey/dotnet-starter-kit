# Journal Entry Refactoring - Budget/BudgetDetail Pattern Implementation

## Summary

Successfully refactored the Journal Entry implementation to follow the Budget/BudgetDetail master-detail pattern. Journal entries and their lines are now managed as separate aggregate roots with their own complete CRUD operations.

## Changes Made

### 1. Domain Layer Changes

#### JournalEntry Entity (`Accounting.Domain/Entities/JournalEntry.cs`)
- **Removed** the `_lines` collection and `Lines` property (was owned entity collection)
- **Removed** the `AddLine()` method (lines now managed separately)
- **Removed** the `IsBalanced()` private method (validation moved to application layer)
- **Updated** the `Post()` method to accept external balance validation
- **Kept** all other properties and methods (Date, ReferenceNumber, Source, IsPosted, PeriodId, OriginalAmount, ApprovalStatus, Approve, Reject, Reverse, Update)

#### JournalEntryLine Entity (`Accounting.Domain/Entities/JournalEntryLine.cs`) - NEW
- **Created** as separate aggregate root implementing `IAggregateRoot`
- **Properties**:
  - `JournalEntryId` - Foreign key to parent journal entry
  - `AccountId` - Chart of account reference
  - `DebitAmount` - Debit amount (non-negative)
  - `CreditAmount` - Credit amount (non-negative)
  - `Memo` - Optional description (max 500 chars)
  - `Reference` - Optional reference number (max 100 chars)
- **Validation**:
  - Either debit or credit must be non-zero, but not both
  - Both amounts must be non-negative
  - Follows the double-entry bookkeeping rules
- **Methods**:
  - `Create()` - Factory method
  - `Update()` - Update amounts, memo, and reference with validation

### 2. Application Layer Changes

#### Updated Commands

**CreateJournalEntryCommand**
- **Removed** `Lines` parameter
- **Removed** `JournalEntryLineDto` record
- Journal entries are now created without lines; lines added separately

**CreateJournalEntryHandler**
- **Removed** line creation logic
- Simplified to only create the journal entry header

#### New Journal Entry Line Commands/Handlers

Created complete CRUD operations for journal entry lines:

1. **Create** (`JournalEntries/Lines/Create/`)
   - `CreateJournalEntryLineCommand` - Command with full line details
   - `CreateJournalEntryLineHandler` - Validates parent exists and is not posted
   - `CreateJournalEntryLineValidator` - FluentValidation rules

2. **Update** (`JournalEntries/Lines/Update/`)
   - `UpdateJournalEntryLineCommand` - Command with optional updates
   - `UpdateJournalEntryLineHandler` - Validates parent is not posted
   - `UpdateJournalEntryLineValidator` - Validation rules

3. **Delete** (`JournalEntries/Lines/Delete/`)
   - `DeleteJournalEntryLineCommand` - Delete by ID
   - `DeleteJournalEntryLineHandler` - Validates parent is not posted

4. **Get** (`JournalEntries/Lines/Get/`)
   - `GetJournalEntryLineQuery` - Get single line by ID
   - `GetJournalEntryLineHandler` - Returns line response

5. **Search** (`JournalEntries/Lines/Search/`)
   - `SearchJournalEntryLinesByJournalEntryIdQuery` - Get all lines for a journal entry
   - `SearchJournalEntryLinesByJournalEntryIdHandler` - Uses specification pattern

#### Supporting Files

- **JournalEntryLineResponse.cs** - Response DTO for API
- **JournalEntryLinesByJournalEntryIdSpec.cs** - Specification for querying lines

#### Updated Post Handler

**PostJournalEntryHandler**
- **Added** dependency on `IReadRepository<JournalEntryLine>`
- **Added** balance validation logic:
  - Queries all lines for the journal entry
  - Calculates total debits and credits
  - Throws `JournalEntryNotBalancedException` if not balanced (tolerance: 0.01)
- Ensures journal entry is balanced before posting to GL

### 3. Infrastructure Layer Changes

#### Endpoints

Created complete REST API endpoints for journal entry lines:

**JournalEntryLinesEndpoints.cs**
- Main endpoint configuration
- Groups all line endpoints under `/journal-entry-lines`
- Tagged as "Journal-Entry-Lines"

**Individual Endpoints** (`v1/` folder):
1. `JournalEntryLineCreateEndpoint` - POST / - Create line
2. `JournalEntryLineUpdateEndpoint` - PUT /{id} - Update line
3. `JournalEntryLineDeleteEndpoint` - DELETE /{id} - Delete line
4. `JournalEntryLineGetEndpoint` - GET /{id} - Get single line
5. `JournalEntryLineSearchEndpoint` - GET /by-journal-entry/{journalEntryId} - List lines for journal entry

#### Endpoint Registration

**AccountingModule.cs**
- **Added** `MapJournalEntryLinesEndpoints()` call (after journal entries)
- **Added** repository registrations:
  - `IRepository<JournalEntryLine>` with key "accounting:journal-lines"
  - `IReadRepository<JournalEntryLine>` with key "accounting:journal-lines"

#### Persistence Configuration

**JournalEntryConfiguration.cs**
- **Removed** owned entity configuration for Lines
- **Added** properties for ApprovalStatus, ApprovedBy, ApprovedDate
- **Updated** indexes

**JournalEntryLineConfiguration.cs** - NEW
- Full EF Core configuration for JournalEntryLine entity
- Table: `JournalEntryLines` in Accounting schema
- Foreign key to JournalEntry with cascade delete
- Precision (18,2) for decimal amounts
- Indexes on JournalEntryId, AccountId, and Reference

## API Endpoints Summary

### Journal Entry (Master)
- POST `/accounting/journal-entries` - Create journal entry
- GET `/accounting/journal-entries/{id}` - Get journal entry
- PUT `/accounting/journal-entries/{id}` - Update journal entry
- DELETE `/accounting/journal-entries/{id}` - Delete journal entry
- GET `/accounting/journal-entries/search` - Search journal entries
- POST `/accounting/journal-entries/{id}/post` - Post to GL
- POST `/accounting/journal-entries/{id}/approve` - Approve entry
- POST `/accounting/journal-entries/{id}/reject` - Reject entry
- POST `/accounting/journal-entries/{id}/reverse` - Reverse entry

### Journal Entry Lines (Detail) - NEW
- POST `/accounting/journal-entry-lines` - Create line
- GET `/accounting/journal-entry-lines/{id}` - Get line
- PUT `/accounting/journal-entry-lines/{id}` - Update line
- DELETE `/accounting/journal-entry-lines/{id}` - Delete line
- GET `/accounting/journal-entry-lines/by-journal-entry/{journalEntryId}` - List lines for journal entry

## Benefits of This Pattern

1. **Separation of Concerns**: Master and detail entities are separate aggregates
2. **Flexibility**: Lines can be managed independently of the header
3. **Better API Design**: Clear, RESTful endpoints for both master and detail
4. **Consistency**: Matches the Budget/BudgetDetail pattern already in the codebase
5. **Validation**: Balance checking happens at the right layer (application)
6. **Scalability**: Large journal entries with many lines perform better
7. **Auditability**: Lines have their own audit trails as separate entities

## Database Migration Required

A database migration will be needed to:
1. Alter the JournalEntryLines table structure (no longer owned entity)
2. Add foreign key constraints
3. Add new indexes
4. Add new columns to JournalEntry (ApprovalStatus fields if not already present)

## Testing Recommendations

1. Test creating journal entry without lines
2. Test adding lines to journal entry
3. Test posting with unbalanced lines (should fail)
4. Test posting with balanced lines (should succeed)
5. Test updating lines when journal entry is posted (should fail)
6. Test deleting lines when journal entry is posted (should fail)
7. Test cascade delete (deleting journal entry should delete lines)
8. Test balance tolerance (0.01 threshold)

## Next Steps for Client Application

The Blazor client application should be updated to:
1. Use the new separate line endpoints
2. Remove inline line editing from the journal entry form
3. Create a master-detail UI similar to Budget/BudgetDetails pages
4. Update the JournalEntryViewModel to not include Lines collection
5. Create JournalEntryLineViewModel for the detail grid
6. Implement separate API calls for line CRUD operations

## Files Created

### Domain Layer (1 file)
- `Accounting.Domain/Entities/JournalEntryLine.cs`

### Application Layer (11 files)
- `Accounting.Application/JournalEntries/Lines/Create/CreateJournalEntryLineCommand.cs`
- `Accounting.Application/JournalEntries/Lines/Create/CreateJournalEntryLineHandler.cs`
- `Accounting.Application/JournalEntries/Lines/Create/CreateJournalEntryLineValidator.cs`
- `Accounting.Application/JournalEntries/Lines/Update/UpdateJournalEntryLineCommand.cs`
- `Accounting.Application/JournalEntries/Lines/Update/UpdateJournalEntryLineHandler.cs`
- `Accounting.Application/JournalEntries/Lines/Update/UpdateJournalEntryLineValidator.cs`
- `Accounting.Application/JournalEntries/Lines/Delete/DeleteJournalEntryLineCommand.cs`
- `Accounting.Application/JournalEntries/Lines/Delete/DeleteJournalEntryLineHandler.cs`
- `Accounting.Application/JournalEntries/Lines/Get/GetJournalEntryLineQuery.cs`
- `Accounting.Application/JournalEntries/Lines/Get/GetJournalEntryLineHandler.cs`
- `Accounting.Application/JournalEntries/Lines/Search/SearchJournalEntryLinesByJournalEntryIdQuery.cs`
- `Accounting.Application/JournalEntries/Lines/Search/SearchJournalEntryLinesByJournalEntryIdHandler.cs`
- `Accounting.Application/JournalEntries/Lines/Responses/JournalEntryLineResponse.cs`
- `Accounting.Application/JournalEntries/Lines/Specs/JournalEntryLinesByJournalEntryIdSpec.cs`

### Infrastructure Layer (7 files)
- `Accounting.Infrastructure/Endpoints/JournalEntryLines/JournalEntryLinesEndpoints.cs`
- `Accounting.Infrastructure/Endpoints/JournalEntryLines/v1/JournalEntryLineCreateEndpoint.cs`
- `Accounting.Infrastructure/Endpoints/JournalEntryLines/v1/JournalEntryLineUpdateEndpoint.cs`
- `Accounting.Infrastructure/Endpoints/JournalEntryLines/v1/JournalEntryLineDeleteEndpoint.cs`
- `Accounting.Infrastructure/Endpoints/JournalEntryLines/v1/JournalEntryLineGetEndpoint.cs`
- `Accounting.Infrastructure/Endpoints/JournalEntryLines/v1/JournalEntryLineSearchEndpoint.cs`
- `Accounting.Infrastructure/Persistence/Configurations/JournalEntryLineConfiguration.cs`

### Files Modified (5 files)
- `Accounting.Domain/Entities/JournalEntry.cs`
- `Accounting.Application/JournalEntries/Create/CreateJournalEntryCommand.cs`
- `Accounting.Application/JournalEntries/Create/CreateJournalEntryHandler.cs`
- `Accounting.Application/JournalEntries/Post/PostJournalEntryHandler.cs`
- `Accounting.Infrastructure/AccountingModule.cs`
- `Accounting.Infrastructure/Persistence/Configurations/JournalEntryConfiguration.cs`

## Total Impact
- **24 new files created**
- **6 files modified**
- **Pattern**: Matches Budget/BudgetDetail exactly
- **Status**: ✅ Ready for database migration and client updates

