# Application Layer Implementation Plan

## Overview
Complete CQRS implementation for 12 new entities following existing patterns.

## Implementation Layers

### 1. Database Configurations (EF Core)
**Location:** `/Accounting.Infrastructure/Persistence/Configurations/`
**Pattern:** EntityTypeConfiguration with table mapping, indexes, owned collections

### 2. Commands
**Location:** `/Accounting.Application/{EntityName}/Create|Update|Delete/`
**Pattern:** Record-based commands with FluentValidation

### 3. Queries  
**Location:** `/Accounting.Application/{EntityName}/Get|Search|List/`
**Pattern:** Record-based queries with specification pattern

### 4. Validators
**Location:** Inline with commands
**Pattern:** FluentValidation AbstractValidator

### 5. Handlers
**Location:** `/Accounting.Application/{EntityName}/Handlers/`
**Pattern:** IRequestHandler with repository pattern

### 6. API Endpoints
**Location:** `/Accounting.Infrastructure/Endpoints/`
**Pattern:** Minimal API with FastEndpoints

##Priority Implementation Order

### Tier 1 - Critical (Complete Implementation)
1. **Bill** - AP workflow essential
2. **FiscalPeriodClose** - Month-end required
3. **TrialBalance** - Financial reporting foundation

### Tier 2 - High Priority (Sample Implementation)  
4. **AccountsReceivableAccount** - AR aging
5. **AccountsPayableAccount** - AP aging
6. **Customer** - Credit management

### Tier 3 - Standard (Pattern Documentation)
7-12. Remaining entities follow same pattern

## Files to Create Per Entity

### Database Layer (1 file)
- `{Entity}Configuration.cs`

### Application Layer (15-20 files)
- Commands:
  - `Create{Entity}Command.cs` + Validator
  - `Update{Entity}Command.cs` + Validator
  - `Delete{Entity}Command.cs` + Validator
  - Status-specific commands (Approve, Void, etc.)
  
- Queries:
  - `Get{Entity}ByIdQuery.cs`
  - `Get{Entity}ByNumberQuery.cs`
  - `Search{Entity}Query.cs`
  - `List{Entity}Query.cs`
  - Report queries (aging, metrics, etc.)
  
- Handlers:
  - `{Entity}CommandHandlers.cs`
  - `{Entity}QueryHandlers.cs`
  
- DTOs:
  - `{Entity}Dto.cs`
  - `{Entity}DetailsDto.cs`
  - Various response DTOs

### Infrastructure Layer (1 file)
- `{Entity}Endpoints.cs`

## Implementation Strategy

I'll provide:
1. ✅ Complete DB configurations for all 12 entities
2. ✅ Complete application layer for Bill (Tier 1 example)
3. ✅ Complete application layer for FiscalPeriodClose (Tier 1 example)
4. ✅ Partial application layer for remaining entities (samples)
5. ✅ Documentation for completing the rest

This approach gives you production-ready code for the most critical entities plus clear patterns to follow for the others.

