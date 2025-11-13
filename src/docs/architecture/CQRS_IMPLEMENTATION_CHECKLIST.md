# CQRS Implementation Checklist - Bills and BillLineItems

## ✅ Completed Tasks

### BillLineItems Application Layer

#### ✅ Create Operation (Add Line Item)
- [x] AddBillLineItemCommand.cs
- [x] AddBillLineItemHandler.cs  
- [x] AddBillLineItemCommandValidator.cs
- [x] AddBillLineItemResponse.cs

#### ✅ Update Operation
- [x] UpdateBillLineItemCommand.cs
- [x] UpdateBillLineItemHandler.cs
- [x] UpdateBillLineItemCommandValidator.cs
- [x] UpdateBillLineItemResponse.cs

#### ✅ Delete Operation
- [x] DeleteBillLineItemCommand.cs
- [x] DeleteBillLineItemHandler.cs
- [x] DeleteBillLineItemCommandValidator.cs
- [x] DeleteBillLineItemResponse.cs

#### ✅ Get Operation (Query)
- [x] GetBillLineItemRequest.cs
- [x] GetBillLineItemHandler.cs
- [x] BillLineItemResponse.cs

#### ✅ GetList Operation (Query)
- [x] GetBillLineItemsRequest.cs
- [x] GetBillLineItemsHandler.cs
- [x] GetBillLineItemsByBillIdSpec.cs

### BillLineItems Infrastructure Layer (Endpoints)

#### ✅ Individual Endpoint Files
- [x] AddBillLineItemEndpoint.cs
- [x] UpdateBillLineItemEndpoint.cs
- [x] DeleteBillLineItemEndpoint.cs
- [x] GetBillLineItemEndpoint.cs
- [x] GetBillLineItemsEndpoint.cs

#### ✅ Endpoint Configuration
- [x] BillsEndpoints.cs (Extension mapper)
- [x] Removed old BillLineItemsEndpoints.cs (legacy file)

### Domain Layer

#### ✅ Exception Classes Fixed
- [x] BillNotFoundException
- [x] BillCannotBeModifiedException
- [x] BillAlreadyPostedException
- [x] BillAlreadyApprovedException
- [x] BillNotApprovedException
- [x] BillNotPostedException
- [x] BillAlreadyPaidException
- [x] BillInvalidAmountException
- [x] BillLineItemNotFoundException
- [x] BillLineItemCannotBeAddedException (NEW)

All now properly inherit from BadRequestException or NotFoundException.

## 📋 Still TODO - Bill Main Entity Operations

### Bills Application Layer (Following Same Pattern)

#### ⬜ Create Operation
- [ ] Create/v1/CreateBillCommand.cs
- [ ] Create/v1/CreateBillHandler.cs
- [ ] Create/v1/CreateBillCommandValidator.cs
- [ ] Create/v1/CreateBillResponse.cs

#### ⬜ Update Operation
- [ ] Update/v1/UpdateBillCommand.cs
- [ ] Update/v1/UpdateBillHandler.cs
- [ ] Update/v1/UpdateBillCommandValidator.cs
- [ ] Update/v1/UpdateBillResponse.cs

#### ⬜ Delete Operation
- [ ] Delete/v1/DeleteBillCommand.cs
- [ ] Delete/v1/DeleteBillHandler.cs
- [ ] Delete/v1/DeleteBillCommandValidator.cs (optional)
- [ ] Delete/v1/DeleteBillResponse.cs (optional)

#### ⬜ Get Operation
- [ ] Get/v1/GetBillRequest.cs
- [ ] Get/v1/GetBillHandler.cs
- [ ] Get/v1/BillResponse.cs
- [ ] Get/v1/GetBillByIdSpec.cs

#### ⬜ Search Operation
- [ ] Search/v1/SearchBillsCommand.cs
- [ ] Search/v1/SearchBillsHandler.cs
- [ ] Search/v1/SearchBillsSpec.cs

#### ⬜ Approve Operation
- [ ] Approve/v1/ApproveBillCommand.cs
- [ ] Approve/v1/ApproveBillHandler.cs
- [ ] Approve/v1/ApproveBillCommandValidator.cs
- [ ] Approve/v1/ApproveBillResponse.cs

#### ⬜ Reject Operation
- [ ] Reject/v1/RejectBillCommand.cs
- [ ] Reject/v1/RejectBillHandler.cs
- [ ] Reject/v1/RejectBillCommandValidator.cs
- [ ] Reject/v1/RejectBillResponse.cs

#### ⬜ Post Operation
- [ ] Post/v1/PostBillCommand.cs
- [ ] Post/v1/PostBillHandler.cs
- [ ] Post/v1/PostBillResponse.cs

#### ⬜ Void Operation
- [ ] Void/v1/VoidBillCommand.cs
- [ ] Void/v1/VoidBillHandler.cs
- [ ] Void/v1/VoidBillCommandValidator.cs
- [ ] Void/v1/VoidBillResponse.cs

#### ⬜ MarkAsPaid Operation
- [ ] MarkAsPaid/v1/MarkBillAsPaidCommand.cs
- [ ] MarkAsPaid/v1/MarkBillAsPaidHandler.cs
- [ ] MarkAsPaid/v1/MarkBillAsPaidResponse.cs

### Bills Infrastructure Layer (Endpoints)

#### ⬜ Individual Endpoint Files
- [ ] v1/CreateBillEndpoint.cs
- [ ] v1/UpdateBillEndpoint.cs
- [ ] v1/DeleteBillEndpoint.cs
- [ ] v1/GetBillEndpoint.cs
- [ ] v1/SearchBillsEndpoint.cs
- [ ] v1/ApproveBillEndpoint.cs
- [ ] v1/RejectBillEndpoint.cs
- [ ] v1/PostBillEndpoint.cs
- [ ] v1/VoidBillEndpoint.cs
- [ ] v1/MarkBillAsPaidEndpoint.cs

#### ⬜ Update Endpoint Configuration
- [ ] Update BillsEndpoints.cs to map all bill endpoints

## 🔍 Code Review Notes

### Issues Found and Fixed:
1. ✅ Duplicate files in Create/Update folders (old vs v1)
2. ✅ Missing command definitions (they were only validated, not defined)
3. ✅ DomainException class didn't exist - replaced with BadRequestException
4. ✅ Messy BillsEndpoints.cs with duplicate code
5. ✅ No separation between Commands/Queries
6. ✅ Handlers combined in single files instead of separate files

### Patterns Now Following:
- ✅ Each class in separate file
- ✅ Proper v1 folder structure
- ✅ Command/Query separation (CQRS)
- ✅ Validator for each command
- ✅ Response DTOs for all operations
- ✅ Proper exception inheritance
- ✅ Documentation on all classes/methods

## 📚 References

See these files for proper CQRS patterns:
- `/api/modules/Catalog/Catalog.Application/Products/`
- `/api/modules/Todo/Features/`
- `/api/modules/Accounting/Accounting.Application/ChartOfAccounts/`

## ✅ Build Status

The project builds successfully:
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
dotnet build api/modules/Accounting/Accounting.Application/Accounting.Application.csproj
# Result: Build succeeded with warnings only (no errors)
```

---

**Next Action**: Implement the Bills main entity operations following the exact same pattern demonstrated for BillLineItems.

