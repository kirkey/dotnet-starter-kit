# 👥 HumanResources Module

**Created:** November 13, 2025  
**Status:** ✅ Module Setup Complete  
**Pattern:** Following Catalog module structure

---

## 📋 Module Overview

The HumanResources module provides complete employee, department, company management, attendance, and payroll functionality for the FSH Starter application. This module follows the same architectural patterns as the Catalog module with Domain, Application, and Infrastructure layers.

---

## 🏗️ Project Structure

```
HumanResources/
├── HumanResources.Domain/
│   ├── Entities/
│   │   ├── Company.cs              ✅ Created
│   │   └── GlobalUsings.cs         ✅ Created
│   ├── Events/
│   │   └── CompanyEvents.cs        ✅ Created
│   ├── Exceptions/
│   │   └── CompanyExceptions.cs    ✅ Created
│   └── Constants/
│       (ready for constants)
│
├── HumanResources.Application/
│   ├── Companies/
│   │   ├── Create/
│   │   │   ├── CreateCompanyCommand.cs     ✅ Created
│   │   │   ├── CreateCompanyResponse.cs    ✅ Created
│   │   │   ├── CreateCompanyValidator.cs   ✅ Created
│   │   │   └── CreateCompanyHandler.cs     ✅ Created
│   │   ├── Get/           (ready to add)
│   │   ├── Search/        (ready to add)
│   │   ├── Update/        (ready to add)
│   │   ├── Delete/        (ready to add)
│   │   ├── Activate/      (ready to add)
│   │   └── Deactivate/    (ready to add)
│   ├── Departments/       (ready to add)
│   ├── Employees/         (ready to add)
│   ├── Common/            (ready for shared components)
│   ├── GlobalUsings.cs    ✅ Created
│   └── HumanResourcesMetadata.cs  ✅ Created
│
└── HumanResources.Infrastructure/
    ├── Endpoints/
    │   └── CompanyCreateEndpoint.cs        ✅ Created
    ├── Persistence/
    │   ├── Configurations/
    │   │   └── CompanyConfiguration.cs     ✅ Created
    │   ├── HumanResourcesDbContext.cs      ✅ Created
    │   ├── HumanResourcesRepository.cs     ✅ Created
    │   └── HumanResourcesDbInitializer.cs  ✅ Created
    ├── GlobalUsings.cs                     ✅ Created
    └── HumanResourcesModule.cs             ✅ Created
```

---

## ✅ What's Been Created

### 1️⃣ Domain Layer (HumanResources.Domain)
**Status:** Foundation Complete

**Entities:**
- ✅ **Company** - Full entity with multi-entity support
  - Properties: CompanyCode, LegalName, TradeName, TaxId, BaseCurrency, FiscalYearEnd, Address, Contact, Logo
  - Methods: Create, Update, UpdateAddress, UpdateContact, Activate, Deactivate, SetParentCompany, UpdateLogo
  - Validation: Comprehensive business rules

**Events:**
- ✅ CompanyCreated
- ✅ CompanyUpdated
- ✅ CompanyActivated
- ✅ CompanyDeactivated

**Exceptions:**
- ✅ CompanyNotFoundException
- ✅ CompanyCodeAlreadyExistsException

---

### 2️⃣ Application Layer (HumanResources.Application)
**Status:** Create Company Feature Complete

**Commands:**
- ✅ CreateCompanyCommand (with 8 parameters)
- ✅ CreateCompanyResponse
- ✅ CreateCompanyValidator (FluentValidation with strict rules)
- ✅ CreateCompanyHandler (with logging and repository)

**Metadata:**
- ✅ HumanResourcesMetadata (module constants)

---

### 3️⃣ Infrastructure Layer (HumanResources.Infrastructure)
**Status:** Complete Setup

**Persistence:**
- ✅ HumanResourcesDbContext (with schema: "humanresources")
- ✅ HumanResourcesRepository<T> (generic repository)
- ✅ HumanResourcesDbInitializer (migrations and seeding)
- ✅ CompanyConfiguration (EF Core entity configuration)

**Endpoints:**
- ✅ CompanyCreateEndpoint (POST /api/v1/humanresources/companies)
  - Returns 201 Created
  - Requires permission: "Permissions.Companies.Create"
  - API Version 1

**Module Registration:**
- ✅ HumanResourcesModule.Endpoints (Carter module)
- ✅ RegisterHumanResourcesServices (DI registration)
- ✅ UseHumanResourcesModule (middleware)

---

## 🔧 Integration Points

### Server Project Integration
✅ **Project References:**
- Added HumanResources.Infrastructure reference to Server.csproj

✅ **Extensions.cs Updates:**
- Added HumanResourcesMetadata to assemblies
- Added RegisterHumanResourcesServices()
- Added HumanResourcesModule.Endpoints to Carter
- Added UseHumanResourcesModule()

✅ **GlobalUsings.cs Updates:**
- Added FSH.Starter.WebApi.HumanResources.Application
- Added FSH.Starter.WebApi.HumanResources.Infrastructure

✅ **Solution File:**
- Added all 3 projects to FSH.Starter.sln

---

## 📊 Database Schema

**Schema Name:** `humanresources`

**Tables:**
- ✅ Companies (configured with indexes and constraints)

**Key Indexes:**
- IX_Companies_CompanyCode (Unique)
- IX_Companies_IsActive
- IX_Companies_ParentCompanyId

---

## 🚀 API Endpoints

### Companies

#### Create Company
```http
POST /api/v1/humanresources/companies
Content-Type: application/json

{
  "companyCode": "COMP-001",
  "legalName": "ABC Corporation Inc.",
  "tradeName": "ABC Corp",
  "taxId": "12-3456789",
  "baseCurrency": "USD",
  "fiscalYearEnd": 12,
  "description": "Main operating company",
  "notes": "Headquartered in New York"
}

Response: 201 Created
Location: /api/v1/humanresources/companies/{id}
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

## 🔐 Permissions

**Required Permissions:**
- `Permissions.Companies.Create` - Create new companies

*(More permissions to be added as features are implemented)*

---

## 📝 Next Steps

### Phase 1: Complete Company CRUD
- [ ] Add GetCompanyRequest
- [ ] Add SearchCompaniesRequest
- [ ] Add UpdateCompanyCommand
- [ ] Add DeleteCompanyCommand
- [ ] Add ActivateCompanyCommand
- [ ] Add DeactivateCompanyCommand

### Phase 2: Department Management
- [ ] Create Department entity
- [ ] Add department CRUD operations
- [ ] Add department hierarchy support
- [ ] Link departments to companies

### Phase 3: Employee Management
- [ ] Create Employee entity
- [ ] Add employee CRUD operations
- [ ] Add hire/terminate workflows
- [ ] Link employees to departments

### Phase 4: Additional Features
- [ ] Attendance tracking
- [ ] Timesheet management
- [ ] Leave management
- [ ] Payroll processing
- [ ] Benefits administration

---

## 🧪 Testing

**Unit Tests:** (To be added)
- Domain entity tests
- Command validator tests
- Handler tests

**Integration Tests:** (To be added)
- API endpoint tests
- Database integration tests

---

## 📚 Documentation References

**Implementation Plan:**
- `/docs/hr/HR_PAYROLL_MODULE_IMPLEMENTATION_PLAN.md` (100+ pages)
- `/docs/hr/HR_PAYROLL_QUICK_REFERENCE.md`
- `/docs/hr/HR_PAYROLL_ENTITY_RELATIONSHIPS.md`

**Architecture:**
- Follows Catalog module pattern
- CQRS pattern with MediatR
- Domain-Driven Design (DDD)
- Repository pattern with keyed services
- FluentValidation for input validation

---

## 🎯 Success Criteria

✅ **Module Setup Complete:**
- All 3 projects created and building
- Project references configured
- Solution file updated
- Server integration complete
- Database context configured
- First entity (Company) implemented
- First endpoint (Create Company) working

**Next Milestone:**
- Complete Company CRUD operations
- Add Department entity and operations
- Begin Employee management

---

## 💡 Usage Example

```csharp
// Creating a company via API
var client = new HttpClient { BaseAddress = new Uri("https://localhost:5001") };

var request = new
{
    companyCode = "ACME-001",
    legalName = "ACME Corporation Inc.",
    tradeName = "ACME Corp",
    taxId = "98-7654321",
    baseCurrency = "USD",
    fiscalYearEnd = 12,
    description = "Technology company",
    notes = "Founded in 2025"
};

var response = await client.PostAsJsonAsync("/api/v1/humanresources/companies", request);
var result = await response.Content.ReadFromJsonAsync<CreateCompanyResponse>();

Console.WriteLine($"Company created with ID: {result.Id}");
```

---

## 🏆 Module Metrics

| Metric | Value |
|--------|-------|
| **Projects** | 3 (Domain, Application, Infrastructure) |
| **Entities** | 1 (Company) |
| **Commands** | 1 (CreateCompany) |
| **Endpoints** | 1 (POST /companies) |
| **Validators** | 1 (CreateCompanyValidator) |
| **Configurations** | 1 (CompanyConfiguration) |
| **Domain Events** | 4 (Created, Updated, Activated, Deactivated) |
| **Lines of Code** | ~900 |
| **Build Status** | ✅ Passing |

---

## ✨ Key Features Implemented

1. **Multi-Tenant Ready** - Uses Finbuckle.MultiTenant
2. **Multi-Currency Support** - BaseCurrency per company
3. **Fiscal Year Configuration** - Customizable fiscal year end
4. **Audit Trail** - Full audit fields (CreatedBy, CreatedOn, etc.)
5. **Domain Events** - Event-driven architecture
6. **Validation** - Strict FluentValidation rules
7. **Soft Delete** - Can be implemented easily
8. **Hierarchical Structure** - Parent company support
9. **Address Management** - Complete address fields
10. **Contact Information** - Phone, email, website, logo

---

**Module created successfully! Ready for Phase 1 development.** 🚀

For questions or issues, refer to the implementation plan documents in `/docs/hr/`.

