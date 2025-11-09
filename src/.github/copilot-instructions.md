Implement CQRS and DRY principles.
Each class should have a separate file.
Implement stricter and tighter validations on each validator and handler.
Refer to the existing Catalog and Todo Projects for code structure, patterns and guidance as samples.
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

Refer or review the md files if there are any related to the feature or functionality being implemented.

Do not add builder.HasCheckConstraint on database configuration.