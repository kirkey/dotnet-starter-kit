# 📋 BANK ACCOUNT DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 20 complete files (1 domain + 1 events + 18 application)

---

## 🚀 Quick Start

### Create Bank Account
```csharp
var command = new CreateBankAccountCommand(
    EmployeeId: employeeId,
    AccountNumber: "9876543210",
    RoutingNumber: "123456789",
    BankName: "First National Bank",
    AccountType: "Checking",
    AccountHolderName: "John Doe",
    SwiftCode: "FNBAUS33",  // For international
    Iban: "US89FNBA1234567890",  // For international
    CurrencyCode: "USD");

var result = await mediator.Send(command);
// Returns: CreateBankAccountResponse with Id
```

### Search Bank Accounts
```csharp
var request = new SearchBankAccountsRequest
{
    EmployeeId = employeeId,
    AccountType = "Checking",
    IsPrimary = true,
    IsActive = true,
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<BankAccountResponse>
```

### Get Single Account
```csharp
var request = new GetBankAccountRequest(accountId);
var result = await mediator.Send(request);
// Returns: BankAccountResponse (with Last4Digits only)
```

### Update Account
```csharp
var command = new UpdateBankAccountCommand(
    Id: accountId,
    BankName: "Updated Bank Name",
    AccountHolderName: "Updated Name",
    SetAsPrimary: true,
    MarkAsVerified: true);

var result = await mediator.Send(command);
// Returns: UpdateBankAccountResponse
```

### Delete Account
```csharp
var command = new DeleteBankAccountCommand(accountId);
var result = await mediator.Send(command);
// Returns: DeleteBankAccountResponse
```

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **EmployeeId** | DefaultIdType? | Employee identifier |
| **AccountType** | string? | "Checking", "Savings" |
| **BankName** | string? | "First National" |
| **IsPrimary** | bool? | true / false |
| **IsActive** | bool? | true / false |
| **IsVerified** | bool? | true / false |
| **PageNumber** | int | 1 |
| **PageSize** | int | 10 |

---

## ✅ Validations

### Create Bank Account
- ✅ EmployeeId required & must exist
- ✅ AccountNumber required, min 8 chars
- ✅ RoutingNumber required, exactly 9 digits
- ✅ BankName required, max 100 chars
- ✅ AccountType required, max 50 chars
- ✅ AccountHolderName required, max 100 chars
- ✅ SwiftCode max 11 chars (optional)
- ✅ IBAN max 34 chars (optional)
- ✅ CurrencyCode exactly 3 chars (optional)

### Update Bank Account
- ✅ Id required
- ✅ BankName max 100 chars (when provided)
- ✅ AccountHolderName max 100 chars (when provided)
- ✅ SwiftCode max 11 chars (when provided)
- ✅ IBAN max 34 chars (when provided)
- ✅ Notes max 500 chars (when provided)

---

## 🎯 BankAccountResponse Properties

```csharp
BankAccountResponse
├── Id: DefaultIdType
├── EmployeeId: DefaultIdType
├── Last4Digits: string?  // ← Secured! Full number not shown
├── BankName: string
├── AccountType: string
├── AccountHolderName: string
├── IsPrimary: bool
├── IsActive: bool
├── IsVerified: bool
├── VerificationDate: DateTime?
├── SwiftCode: string?    // International
├── Iban: string?         // International
├── CurrencyCode: string? // Default: USD
└── Notes: string?
```

---

## 📊 Account Types

| Type | Examples |
|------|----------|
| **Checking** | Standard checking account |
| **Savings** | Savings account |
| **MoneyMarket** | Money market account |
| **Other** | Other custom types |

---

## 🔧 Configuration

### Register Keyed Services
```csharp
services.AddKeyedScoped<IRepository<BankAccount>>("hr:bankaccounts");
services.AddKeyedScoped<IReadRepository<BankAccount>>("hr:bankaccounts");
```

### Register Handlers
```csharp
services.AddMediatR(typeof(CreateBankAccountHandler));
services.AddMediatR(typeof(SearchBankAccountsHandler));
services.AddMediatR(typeof(GetBankAccountHandler));
services.AddMediatR(typeof(UpdateBankAccountHandler));
services.AddMediatR(typeof(DeleteBankAccountHandler));
```

### Register Validators
```csharp
services.AddValidatorsFromAssembly(typeof(CreateBankAccountValidator).Assembly);
```

---

## 📁 Folder Structure

```
BankAccounts/
├── Create/v1/ → Create command, handler, validator, response
├── Get/v1/ → Get request, handler, response
├── Search/v1/ → Search request, handler
├── Update/v1/ → Update command, handler, validator, response
├── Delete/v1/ → Delete command, handler, response
└── Specifications/ → BankAccount specifications
```

---

## 📊 Domain Methods

```csharp
// Create
var bankAccount = BankAccount.Create(
    employeeId, accountNumber, routingNumber, bankName, 
    accountType, accountHolderName, swift, iban, currency);

// Set as primary
bankAccount.SetAsPrimary();

// Remove primary status
bankAccount.RemovePrimaryStatus();

// Mark verified
bankAccount.MarkAsVerified();

// Update
bankAccount.Update(
    bankName: "New Bank",
    accountHolderName: "New Name",
    swiftCode: "SWIFT123",
    iban: "IBAN123",
    notes: "Updated account");

// Activate/Deactivate
bankAccount.Activate();
bankAccount.Deactivate();
```

---

## 🔒 Security Features

✅ **Account Number** - Encrypted at rest  
✅ **Routing Number** - Encrypted at rest  
✅ **Only Last4Digits** - Shown in API responses  
✅ **Primary Account** - Tracked for direct deposit  
✅ **Verification** - Status tracking with date  

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** Payroll Integration & Direct Deposit  


