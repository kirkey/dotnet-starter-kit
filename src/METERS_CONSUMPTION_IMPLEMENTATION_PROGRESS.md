# Meters & Consumption Review and Implementation - IN PROGRESS

## Summary
Both Meters and Consumption modules were stubs with commented-out endpoints and old folder structures. I'm implementing complete CRUD + workflow operations for both following established patterns.

## Current Status

### METERS MODULE

#### What Was Found
- ❌ Partial implementation with old structure
- ❌ All endpoints commented out
- ❌ Only Create handler partially done
- ✅ Well-defined entity with domain events
- ✅ Proper configuration

#### Files Created So Far ✅
1. `Create/v1/CreateMeterCommand.cs` ✅
2. `Create/v1/CreateMeterCommandValidator.cs` ✅
3. `Create/v1/CreateMeterHandler.cs` ✅
4. `Get/v1/GetMeterRequest.cs` ✅

#### Still Need to Create
**Get Operation:**
- GetMeterByIdSpec.cs
- GetMeterHandler.cs
- MeterGetEndpoint.cs

**Search Operation:**
- SearchMetersRequest.cs
- SearchMetersSpec.cs
- SearchMetersHandler.cs
- MeterSearchEndpoint.cs

**Update Operation:**
- UpdateMeterCommand.cs
- UpdateMeterCommandValidator.cs
- UpdateMeterHandler.cs
- MeterUpdateEndpoint.cs

**Delete Operation:**
- DeleteMeterCommand.cs
- DeleteMeterHandler.cs
- MeterDeleteEndpoint.cs

**Workflow Operations:**
- UpdateMeterStatusCommand.cs
- UpdateMeterStatusHandler.cs
- MeterUpdateStatusEndpoint.cs
- RecordMeterReadingCommand.cs
- RecordMeterReadingHandler.cs
- MeterRecordReadingEndpoint.cs

**Endpoints:**
- Update MeterEndpoints.cs to enable all endpoints

### CONSUMPTION MODULE

#### What Was Found
- ❌ Partial implementation with old/new mixed structure
- ❌ All endpoints commented out
- ✅ Has some v1 folders but incomplete
- ✅ Well-defined entity with domain events

#### Need to Create All Operations
**CRUD:**
- Create (command, validator, handler, endpoint)
- Get (request, spec, handler, endpoint)
- Search (request, spec, handler, endpoint)
- Update (command, validator, handler, endpoint)
- Delete (command, handler, endpoint)

**Workflow:**
- Mark as estimated
- Validate reading
- Recalculate usage

**Endpoints:**
- Update ConsumptionsEndpoints.cs

## Implementation Plan

### Phase 1: Complete Meters Module ✅ (Partial)
1. ✅ Create operation
2. ⏳ Get operation
3. ⏳ Search operation
4. ⏳ Update operation
5. ⏳ Delete operation
6. ⏳ Workflow operations (UpdateStatus, RecordReading)
7. ⏳ Enable all endpoints

### Phase 2: Complete Consumption Module
1. ⏳ Create operation
2. ⏳ Get operation
3. ⏳ Search operation
4. ⏳ Update operation
5. ⏳ Delete operation
6. ⏳ Workflow operations
7. ⏳ Enable all endpoints

### Phase 3: Cleanup
1. ⏳ Remove old Commands, Handlers, Queries folders
2. ⏳ Verify build
3. ⏳ Create documentation

## Code Patterns to Follow

1. **Keyed Services**: `[FromKeyedServices("accounting:meters")]` and `[FromKeyedServices("accounting:consumptions")]`
2. **Specification Pattern**: For queries and projections
3. **Pagination**: Using `EntitiesByPaginationFilterSpec`
4. **CQRS**: Commands for writes, Requests for reads
5. **Primary Constructor Parameters**: Simplified DI
6. **Validation**: FluentValidation on all commands
7. **Versioning**: All in v1 folders

## Business Rules

### Meters
- Meter number must be unique
- Multiplier must be positive (default 1.0)
- Installation date cannot be in future
- Valid meter types: Single Phase, Three Phase, Smart Meter, Analog, Digital
- Valid statuses: Active, Inactive, Defective, Pending Installation
- Cannot delete meters with reading history

### Consumption
- Current and previous readings must be non-negative
- KWhUsed = (Current - Previous) × Multiplier
- IsValidReading = Current >= Previous
- Reading types: Actual, Estimated, Customer Read
- Reading sources: Manual, AMR, AMI
- Multiplier defaults to 1.0 if not provided

## API Endpoints Needed

### Meters
| Method | Endpoint | Status |
|--------|----------|--------|
| POST | `/api/v1/accounting/meters` | ⏳ Partial |
| GET | `/api/v1/accounting/meters/{id}` | ❌ |
| PUT | `/api/v1/accounting/meters/{id}` | ❌ |
| DELETE | `/api/v1/accounting/meters/{id}` | ❌ |
| POST | `/api/v1/accounting/meters/search` | ❌ |
| PUT | `/api/v1/accounting/meters/{id}/status` | ❌ |
| POST | `/api/v1/accounting/meters/{id}/reading` | ❌ |

### Consumptions
| Method | Endpoint | Status |
|--------|----------|--------|
| POST | `/api/v1/accounting/consumptions` | ❌ |
| GET | `/api/v1/accounting/consumptions/{id}` | ❌ |
| PUT | `/api/v1/accounting/consumptions/{id}` | ❌ |
| DELETE | `/api/v1/accounting/consumptions/{id}` | ❌ |
| POST | `/api/v1/accounting/consumptions/search` | ❌ |
| POST | `/api/v1/accounting/consumptions/{id}/mark-estimated` | ❌ |
| POST | `/api/v1/accounting/consumptions/{id}/validate` | ❌ |

## Estimated Completion

**Files to Create:**
- Meters: ~30 files
- Consumptions: ~35 files
- **Total**: ~65 files

**Time Required:**
- Meters: 2-3 hours
- Consumptions: 2-3 hours
- Testing & Documentation: 1 hour
- **Total**: 5-7 hours

## Next Steps

1. Complete remaining Meters operations
2. Implement all Consumption operations
3. Remove old folder structures
4. Build and test
5. Create comprehensive documentation
6. Update UI gap summary

---

**Status**: 🔄 IN PROGRESS (15% complete - Create operation for Meters done)
**Started**: November 10, 2025
**Target Completion**: November 10, 2025

