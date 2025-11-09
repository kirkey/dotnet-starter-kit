Implement CQRS and DRY principles.
Each class should have a separate file.
Implement stricter and tighter validations on each validator and handler.
Refer to the existing Catalog and Todo Projects for code structure, patterns and guidance as samples.
Add documentation for each Entity, fields, methods, functions and classes.
Only use string as enums.

Use Command for write operations (Create, Update, Delete, Workflow actions)
Use Request for read operations (Get, Search, List)
Return Response from endpoints (API contract)
Use DTO internally when Response is too heavy or needs different structure
Keep Commands/Requests simple - just enough to identify the operation
Put ID in URL, not in request body (for single-resource operations)

Refer or review the md files if there are any related to the feature or functionality being implemented.

Do not add builder.HasCheckConstraint on database configuration.