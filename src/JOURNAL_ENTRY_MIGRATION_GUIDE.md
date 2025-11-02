# Journal Entry Database Migration Guide

## Overview
This guide covers the database migration required after refactoring JournalEntry and JournalEntryLine to follow the Budget/BudgetDetail pattern.

## Migration Steps

### 1. Create Migration

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/migrations/migrations
dotnet ef migrations add RefactorJournalEntryLines --context AccountingDbContext --startup-project ../../server
```

### 2. Review Generated Migration

The migration should include:

#### Schema Changes to JournalEntry Table
```sql
-- Add new columns if not already present
ALTER TABLE Accounting.JournalEntries 
ADD ApprovalStatus NVARCHAR(16) NOT NULL DEFAULT 'Pending';

ALTER TABLE Accounting.JournalEntries 
ADD ApprovedBy NVARCHAR(256) NULL;

ALTER TABLE Accounting.JournalEntries 
ADD ApprovedDate DATETIME2 NULL;

-- Add indexes
CREATE INDEX IX_JournalEntries_ApprovalStatus 
ON Accounting.JournalEntries(ApprovalStatus);
```

#### Schema Changes to JournalEntryLines Table
```sql
-- The JournalEntryLines table structure should remain similar
-- But add new columns for Memo and Reference
ALTER TABLE Accounting.JournalEntryLines 
ADD Memo NVARCHAR(500) NULL;

ALTER TABLE Accounting.JournalEntryLines 
ADD Reference NVARCHAR(100) NULL;

-- Ensure proper foreign key with cascade delete exists
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_JournalEntryLines_JournalEntries_JournalEntryId')
BEGIN
    ALTER TABLE Accounting.JournalEntryLines
    ADD CONSTRAINT FK_JournalEntryLines_JournalEntries_JournalEntryId
    FOREIGN KEY (JournalEntryId) REFERENCES Accounting.JournalEntries(Id)
    ON DELETE CASCADE;
END

-- Add index on Reference column
CREATE INDEX IX_JournalEntryLines_Reference 
ON Accounting.JournalEntryLines(Reference);
```

### 3. Data Migration Considerations

#### Migrate Description to Memo
If your existing JournalEntryLines have a `Description` column that needs to be renamed to `Memo`:

```sql
-- If Description column exists and needs to be renamed
EXEC sp_rename 'Accounting.JournalEntryLines.Description', 'Memo', 'COLUMN';
```

OR if you need to keep both:

```sql
-- Copy Description to Memo
UPDATE Accounting.JournalEntryLines
SET Memo = Description
WHERE Description IS NOT NULL;
```

### 4. Apply Migration

#### Development
```bash
dotnet ef database update --context AccountingDbContext --startup-project ../../server
```

#### Production
Generate SQL script for review:
```bash
dotnet ef migrations script --context AccountingDbContext --startup-project ../../server --output migration.sql
```

Review the SQL script before applying to production.

### 5. Verify Migration

Run these queries to verify the migration was successful:

```sql
-- Check JournalEntry table structure
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'Accounting' AND TABLE_NAME = 'JournalEntries'
ORDER BY ORDINAL_POSITION;

-- Check JournalEntryLines table structure
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'Accounting' AND TABLE_NAME = 'JournalEntryLines'
ORDER BY ORDINAL_POSITION;

-- Check foreign keys
SELECT fk.name AS FK_Name,
       OBJECT_NAME(fk.parent_object_id) AS Parent_Table,
       c1.name AS Parent_Column,
       OBJECT_NAME(fk.referenced_object_id) AS Referenced_Table,
       c2.name AS Referenced_Column,
       delete_referential_action_desc AS Delete_Action
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
INNER JOIN sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
WHERE OBJECT_NAME(fk.parent_object_id) = 'JournalEntryLines';

-- Check indexes
SELECT 
    i.name AS IndexName,
    COL_NAME(ic.object_id, ic.column_id) AS ColumnName,
    i.type_desc AS IndexType
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
WHERE OBJECT_NAME(i.object_id) = 'JournalEntryLines'
ORDER BY i.name, ic.index_column_id;
```

### 6. Test Data Integrity

```sql
-- Verify all lines have valid journal entry references
SELECT COUNT(*) AS OrphanedLines
FROM Accounting.JournalEntryLines jel
LEFT JOIN Accounting.JournalEntries je ON jel.JournalEntryId = je.Id
WHERE je.Id IS NULL;

-- Should return 0

-- Verify balance for posted entries
SELECT 
    je.Id,
    je.ReferenceNumber,
    SUM(jel.DebitAmount) AS TotalDebits,
    SUM(jel.CreditAmount) AS TotalCredits,
    ABS(SUM(jel.DebitAmount) - SUM(jel.CreditAmount)) AS Variance
FROM Accounting.JournalEntries je
INNER JOIN Accounting.JournalEntryLines jel ON je.Id = jel.JournalEntryId
WHERE je.IsPosted = 1
GROUP BY je.Id, je.ReferenceNumber
HAVING ABS(SUM(jel.DebitAmount) - SUM(jel.CreditAmount)) > 0.01;

-- Should return no rows (all posted entries should be balanced)
```

## Rollback Plan

If issues arise, you can rollback:

```bash
# Rollback to previous migration
dotnet ef database update <PreviousMigrationName> --context AccountingDbContext --startup-project ../../server

# Or remove the migration entirely (if not yet applied to production)
dotnet ef migrations remove --context AccountingDbContext --startup-project ../../server
```

## Post-Migration Tasks

1. **Update API Documentation**: Regenerate Swagger/OpenAPI documentation
2. **Update Client Applications**: Update Blazor client to use new endpoints
3. **Update Integration Tests**: Test all journal entry and line CRUD operations
4. **Monitor Performance**: Check query performance on new indexes
5. **Verify Cascade Deletes**: Test that deleting a journal entry properly deletes its lines

## Breaking Changes

### API Changes
- POST `/journal-entries` no longer accepts `lines` in the request body
- Lines must be created separately via POST `/journal-entry-lines`
- GET `/journal-entries/{id}` no longer includes lines in response
- Use GET `/journal-entry-lines/by-journal-entry/{id}` to fetch lines

### Client Application Changes Required
- Remove inline line editing from journal entry forms
- Implement separate line management (similar to Budget Details)
- Update ViewModels to remove Lines collection
- Call separate endpoints for line CRUD operations

## Common Issues and Solutions

### Issue: Foreign Key Constraint Violation
**Symptom**: Cannot create lines due to FK constraint
**Solution**: Ensure JournalEntry exists before creating lines

### Issue: Balance Check Fails
**Symptom**: Cannot post journal entry even when balanced
**Solution**: Check for rounding issues; tolerance is 0.01

### Issue: Cannot Update/Delete Lines
**Symptom**: Forbidden error when modifying lines
**Solution**: Ensure journal entry is not yet posted; only draft entries can be modified

### Issue: Cascade Delete Not Working
**Symptom**: Lines remain after deleting journal entry
**Solution**: Verify FK constraint has ON DELETE CASCADE

## Performance Considerations

### Indexes Created
- `IX_JournalEntryLines_JournalEntryId` - For fetching lines by journal entry
- `IX_JournalEntryLines_AccountId` - For account-level queries
- `IX_JournalEntryLines_Reference` - For reference number lookups
- `IX_JournalEntries_ApprovalStatus` - For approval workflow queries

### Expected Behavior
- Fetching lines for a journal entry: O(log n) due to index
- Posting validation (balance check): O(n) where n = number of lines
- Cascade delete: Handled by database, no application overhead

## Support

For issues or questions:
1. Check error logs in `logs/` directory
2. Review migration script for unexpected changes
3. Test in development environment first
4. Contact development team for production migration support

