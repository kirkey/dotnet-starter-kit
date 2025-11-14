# 🚀 Quick Start Guide - Database-Driven Payroll System

**For Developers** | November 14, 2025

---

## ⚡ TL;DR

The HR module now has **database-driven payroll configuration** where:
- All pay components (SSS, tax, overtime, etc.) are stored in database
- Rates and formulas are configurable without code changes
- Philippine labor law compliance built-in
- Employee-specific overrides supported

**Status:** Phase 1 Complete - Core CRUD operations working  
**Ready For:** Testing via API

---

## 🎯 What's Been Implemented

### ✅ Core Entities
1. **PayComponent** - Earnings, deductions, taxes configuration
2. **PayComponentRate** - Brackets for SSS, PhilHealth, Pag-IBIG, BIR tax
3. **EmployeePayComponent** - Per-employee overrides and loans

### ✅ API Endpoints (PayComponent)
- `POST /api/v1/humanresources/paycomponents` - Create
- `GET /api/v1/humanresources/paycomponents/{id}` - Get by ID
- `PUT /api/v1/humanresources/paycomponents/{id}` - Update
- `DELETE /api/v1/humanresources/paycomponents/{id}` - Delete

### ✅ Infrastructure
- EF Core configurations
- Repository registrations
- Minimal API endpoints
- Multi-tenant support

---

## 🏃 Quick Start

### 1. Run Migration (FIRST TIME ONLY)
```bash
# From project root
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit

# Create migration
dotnet ef migrations add AddDatabaseDrivenPayroll \
  --project src/api/modules/HumanResources/HumanResources.Infrastructure \
  --startup-project src/api/host

# Apply migration
dotnet ef database update \
  --project src/api/modules/HumanResources/HumanResources.Infrastructure \
  --startup-project src/api/host
```

### 2. Run The API
```bash
cd src/api/host
dotnet run
```

### 3. Test via Swagger
Open browser: `https://localhost:7001/swagger`

Navigate to: **HumanResources > PayComponents**

---

## 📝 Usage Examples

### Example 1: Create SSS Employee Component

```json
POST /api/v1/humanresources/paycomponents
{
  "code": "SSS_EE",
  "componentName": "SSS Employee Share",
  "componentType": "Deduction",
  "calculationMethod": "Bracket",
  "glAccountCode": "2120",
  "description": "SSS employee contribution per RA 11199",
  "isCalculated": true,
  "isMandatory": true,
  "isSubjectToTax": false,
  "laborLawReference": "SSS Law RA 11199",
  "displayOrder": 1,
  "affectsGrossPay": false,
  "affectsNetPay": true
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Example 2: Create Overtime Component

```json
POST /api/v1/humanresources/paycomponents
{
  "code": "OT_REGULAR",
  "componentName": "Overtime Pay (Regular Day)",
  "componentType": "Earnings",
  "calculationMethod": "Formula",
  "calculationFormula": "HourlyRate * OvertimeHours * 1.25",
  "rate": 1.25,
  "glAccountCode": "6110",
  "description": "Overtime at 125% per Labor Code Article 87",
  "isCalculated": true,
  "isMandatory": false,
  "isSubjectToTax": true,
  "laborLawReference": "Labor Code Article 87",
  "displayOrder": 5,
  "affectsGrossPay": true,
  "affectsNetPay": true
}
```

### Example 3: Create Fixed Allowance

```json
POST /api/v1/humanresources/paycomponents
{
  "code": "TRANS_ALLOW",
  "componentName": "Transportation Allowance",
  "componentType": "Earnings",
  "calculationMethod": "Fixed",
  "fixedAmount": 2000.00,
  "glAccountCode": "6130",
  "description": "Monthly transportation allowance",
  "isCalculated": false,
  "isMandatory": false,
  "isSubjectToTax": true,
  "displayOrder": 10,
  "affectsGrossPay": true,
  "affectsNetPay": true
}
```

### Example 4: Get a Component

```http
GET /api/v1/humanresources/paycomponents/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "code": "SSS_EE",
  "componentName": "SSS Employee Share",
  "componentType": "Deduction",
  "calculationMethod": "Bracket",
  "calculationFormula": null,
  "rate": null,
  "fixedAmount": null,
  "minValue": null,
  "maxValue": null,
  "glAccountCode": "2120",
  "isActive": true,
  "isCalculated": true,
  "isMandatory": true,
  "isSubjectToTax": false,
  "isTaxExempt": false,
  "laborLawReference": "SSS Law RA 11199",
  "description": "SSS employee contribution per RA 11199",
  "displayOrder": 1,
  "affectsGrossPay": false,
  "affectsNetPay": true
}
```

### Example 5: Update a Component

```http
PUT /api/v1/humanresources/paycomponents/3fa85f64-5717-4562-b3fc-2c963f66afa6
{
  "description": "Updated: SSS employee contribution at 4.5%",
  "displayOrder": 2
}
```

### Example 6: Delete a Component

```http
DELETE /api/v1/humanresources/paycomponents/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

---

## 🔑 Key Concepts

### Calculation Methods

| Method | Use Case | Required Fields |
|--------|----------|-----------------|
| **Manual** | User enters amount | None |
| **Formula** | Calculate using variables | `calculationFormula` |
| **Percentage** | Fixed % of base amount | `rate` |
| **Bracket** | Uses rate tables (SSS, tax) | PayComponentRate records |
| **Fixed** | Same amount every time | `fixedAmount` |

### Component Types

| Type | Purpose | Examples |
|------|---------|----------|
| **Earnings** | Increases gross pay | Basic Pay, Overtime, Allowances |
| **Deduction** | Reduces net pay | SSS, PhilHealth, Pag-IBIG, Loans |
| **Tax** | Income tax withholding | Withholding Tax |
| **EmployerContribution** | Not deducted from employee | Employer SSS, PhilHealth, Pag-IBIG |

### Flags Explained

| Flag | Meaning | Example |
|------|---------|---------|
| `isMandatory` | Required by Philippine law | SSS, PhilHealth, Pag-IBIG |
| `isCalculated` | Auto-calculated by system | Overtime, Tax |
| `isSubjectToTax` | Included in taxable income | Basic Pay, Allowances |
| `isTaxExempt` | Tax-exempt (de minimis) | Meal allowance ≤₱50 |
| `affectsGrossPay` | Included in gross pay | Earnings only |
| `affectsNetPay` | Affects final take-home | All except employer contributions |

---

## 📋 Permissions

PayComponent endpoints require these permissions:
- `Permissions.PayComponents.Create`
- `Permissions.PayComponents.Update`
- `Permissions.PayComponents.View`
- `Permissions.PayComponents.Delete`

**Note:** Configure these in your identity/authorization system.

---

## 🐛 Troubleshooting

### Error: "PayComponent with id 'xxx' not found"
**Solution:** Component was deleted or ID is invalid. Use GET to verify.

### Error: "Code 'BASIC_PAY' already exists"
**Solution:** Component codes must be unique. Use different code or update existing.

### Error: "GL account code is required"
**Solution:** Every component needs a GL account for accounting integration.

### Migration Fails
**Solution:** 
1. Check database connection string
2. Ensure no pending migrations
3. Drop database and re-create if in development

---

## 📚 Related Documentation

- **DATABASE_DRIVEN_PAYROLL_ARCHITECTURE.md** - Complete architecture
- **DATABASE_DRIVEN_IMPLEMENTATION_SUMMARY.md** - Implementation details
- **PHILIPPINE_LAW_COMPLIANCE_REVIEW_COMPLETE.md** - Labor law compliance
- **PHASE_1_COMPLETE.md** - Implementation status

---

## 🔜 What's Next

### Phase 2: PayComponentRate (Brackets)
- Create rate tables for SSS, PhilHealth, Pag-IBIG
- Create BIR tax brackets
- Link to PayComponents

### Phase 3: EmployeePayComponent (Overrides)
- Assign custom allowances to employees
- Track employee loans
- One-time bonuses/deductions

### Phase 4: Payroll Calculation
- Integrate calculators with components
- Generate payroll from timesheets
- Calculate net pay with all deductions

---

## 💡 Pro Tips

1. **Start with Basic Pay**
   - Create "BASIC_PAY" component first
   - Set as Manual calculation
   - This is the foundation for all other calculations

2. **Use Meaningful Codes**
   - Keep codes short and descriptive
   - Use underscores: `SSS_EE`, `OT_REGULAR`
   - Avoid spaces and special characters

3. **Set Display Order**
   - Controls order in payslip
   - Earnings first (1-100)
   - Deductions next (101-200)
   - Tax last (201-300)

4. **Test Calculation Methods**
   - Create one of each type
   - Verify formulas work correctly
   - Check bracket lookups

5. **Document Labor Law References**
   - Always fill in `laborLawReference`
   - Makes audits easier
   - Helps with compliance

---

## 🎯 Success Checklist

Before deploying to production:
- [ ] Migration run successfully
- [ ] Can create pay components via API
- [ ] Can retrieve pay components via API
- [ ] Can update pay components via API
- [ ] Can delete pay components via API
- [ ] Multi-tenant isolation verified
- [ ] Permissions configured correctly
- [ ] Standard components seeded
- [ ] GL accounts configured
- [ ] Calculations tested

---

## 🆘 Need Help?

1. Check existing documentation
2. Review Swagger API docs
3. Check compilation errors
4. Review logs for runtime errors
5. Ask the team

---

**Last Updated:** November 14, 2025  
**Version:** 1.0  
**Status:** Phase 1 Complete ✅

