Implement CQRS and DRY principles.
Each class should have a separate file.
Implement stricter and tighter validations on each validator and handler.
Follow Catalog and Todo Projects for code structure, patterns to ensure code consistency.
Add documentation for each Entity, fields, methods, functions and classes.
Only use string as enums.

## 📋 Best Practices Rules

### ✅ Rules to Apply:
1. **Commands for Writes** - Create, Update, Delete, Workflow actions
2. **Requests for Reads** - Get, Search, List
3. **Response for Output** - API contract (not DTO externally)
4. **DTO Internal Only** - When Response is too heavy
5. **ID in URL** - Not in request body for single-resource operations
6. **Property-based** - Not positional parameters (for NSwag compatibility)

## 🎨 Code Patterns Applied
✅ **Keyed Services**: All handlers now use proper keyed services:
✅ **Primary Constructor Parameters**: Modern C# constructor patterns
✅ **No Field Assignments**: Using parameters directly
✅ **SaveChangesAsync**: Proper transaction handling
✅ **Specification Pattern**: For queries and business rules
✅ **Pagination**: Full support with filtering
✅ **CQRS**: Commands for writes, Requests for reads
✅ **Response Pattern**: Consistent API contracts
✅ **Domain Events**: Entities raise proper events
✅ **Validation**: FluentValidation on all commands
✅ **Versioning**: All in v1 folders
✅ **Error Handling**: Custom exceptions with proper messages
✅ **Best Practices:** CQRS, DRY, SOLID principles application

Setup optimal indexing on database for frequently queried fields.

Pagination is handled by the repository layer, not specifications.

Refer or review the md files if any related to the feature or functionality being implemented.

Do not add builder.HasCheckConstraint on database configuration.