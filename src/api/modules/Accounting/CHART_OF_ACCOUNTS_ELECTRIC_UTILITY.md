# Electric Utility Chart of Accounts - Comprehensive Seed Data

## Overview
A complete chart of accounts for an electric power utility company with 105 accounts covering all aspects of utility operations, from generation and transmission to customer service and financial management.

## Account Structure

### ASSETS (1000-1999) - 40 Accounts

#### Current Assets (1100-1499)
- **Cash & Equivalents (1100-1150):** 6 accounts
  - Petty Cash, Operating Account, Payroll Account, Money Market, Short-term Investments
  
- **Accounts Receivable (1200-1250):** 6 accounts
  - Residential, Commercial, Industrial receivables
  - Allowance for doubtful accounts
  - Unbilled revenue
  
- **Inventory (1300-1340):** 5 accounts
  - Materials & Supplies, Fuel (Coal, Natural Gas), Spare Parts
  
- **Prepaid Expenses (1400-1420):** 3 accounts
  - Insurance, Rent

#### Property, Plant & Equipment (1500-1799)
- **Electric Plant in Service (1500-1540):** 5 accounts
  - Generation Plant
  - Transmission Plant
  - Distribution Plant
  - General Plant
  
- **Accumulated Depreciation (1600-1640):** 5 accounts
  - Separate depreciation for each plant category
  
- **Construction Work in Progress (1700):** 1 account

#### Other Assets (1800-1830)
- Regulatory Assets
- Deferred Tax Assets
- Long-term Investments

### LIABILITIES (2000-2999) - 20 Accounts

#### Current Liabilities (2100-2400)
- **Accounts Payable (2100-2120):** 3 accounts
  - Trade payables, Fuel payables
  
- **Accrued Liabilities (2200-2230):** 4 accounts
  - Payroll, Taxes, Interest
  
- **Customer Deposits (2300-2320):** 3 accounts
  - Residential, Commercial deposits
  
- **Current Debt (2400):** 1 account

#### Long-term Liabilities (2500-2630)
- **Long-term Debt (2500-2530):** 4 accounts
  - Bonds, Notes, Capital Leases
  
- **Deferred Credits (2600-2630):** 4 accounts
  - Regulatory liabilities
  - Deferred tax liabilities
  - Asset retirement obligations

### EQUITY (3000-3999) - 5 Accounts
- Common Stock
- Patronage Capital (for cooperative utilities)
- Retained Earnings
- Current Year Earnings

### REVENUE (4000-4999) - 14 Accounts

#### Electric Operating Revenue (4100-4430)
- **Residential Sales (4100-4120):** 3 accounts
  - Energy charges, Customer charges
  
- **Commercial Sales (4200-4220):** 3 accounts
  - Energy charges, Demand charges
  
- **Industrial Sales (4300-4320):** 3 accounts
  - Energy charges, Demand charges
  
- **Other Electric Revenue (4400-4430):** 4 accounts
  - Late payment charges, Reconnection fees, Meter testing

#### Non-Operating Revenue (4500-4530)
- Investment income
- Gain on asset sales
- Miscellaneous revenue

### EXPENSES (5000-9999) - 26 Accounts

#### Power Production (5100-5220)
- **Fuel Expenses (5100-5130):** 4 accounts
  - Coal, Natural Gas, Oil
  
- **Production Operations (5200-5220):** 3 accounts
  - Labor, Maintenance

#### Power Purchased (5300-5320)
- Base load, Peak load purchases

#### Transmission (5400-5420)
- Labor, Maintenance

#### Distribution (5500-5530)
- Labor, Maintenance, Meter Reading

#### Customer Service (5700-5730)
- Labor, Billing, Uncollectible accounts

#### Administrative & General (6000-6600)
- Salaries, Office expenses, Professional services
- Insurance, Employee benefits, Regulatory expenses

#### Depreciation (7000-7400)
- Generation, Transmission, Distribution, General

#### Taxes (8000-8200)
- Property taxes, Payroll taxes

#### Interest Expense (9000-9200)
- Interest on bonds and notes

## Key Features

### Industry-Specific Design
- Follows FERC (Federal Energy Regulatory Commission) account structure
- Includes regulatory assets and liabilities unique to utilities
- Tracks generation, transmission, and distribution separately
- Supports various customer classes (Residential, Commercial, Industrial)

### Comprehensive Coverage
- **Asset Management:** Tracks utility plant assets worth millions
- **Revenue Tracking:** Separates energy charges from demand charges
- **Expense Allocation:** Details fuel costs, operations, and maintenance
- **Regulatory Compliance:** Includes regulatory asset/liability accounts

### Member/Cooperative Features
- Patronage capital accounts for cooperative utilities
- Customer deposit tracking
- Member equity management

### Operational Categories
1. **Generation:** Power production facilities and fuel
2. **Transmission:** High-voltage transmission lines and substations
3. **Distribution:** Local distribution system serving customers
4. **Customer Service:** Billing, collections, and member services
5. **Administrative:** Corporate and support functions

## Sample Account Balances (Typical)
- **Assets:** $50-100 million (mostly plant assets)
- **Liabilities:** $30-60 million (bonds, regulatory liabilities)
- **Equity:** $20-40 million (member equity, retained earnings)
- **Annual Revenue:** $10-20 million (electricity sales)
- **Annual Expenses:** $8-15 million (operations, fuel, labor)

## Updated Seed Data References
All seed data in `AccountingDbInitializer.cs` has been updated to reference the new account codes:

- **Budget Details:** Uses generation fuel (5100), distribution labor (5510), admin salaries (6100), residential sales (4100)
- **Journal Entries:** Uses operating account (1120) and residential energy revenue (4110)
- **Fixed Assets:** Uses operating account (1120) and depreciation expense (7400)
- **Vendors/Payees:** Distributed across appropriate operating and administrative expense accounts
- **Payments:** Post to operating account (1120)
- **Write-offs:** Uses residential receivables (1210) and uncollectible accounts expense (5730)
- **Bank Reconciliations:** Uses operating account (1120)

## Integration Points
- **Billing System:** Posts to revenue accounts 4100-4430
- **Payroll System:** Posts to labor expense accounts (5210, 5410, 5510, etc.)
- **Asset Management:** Posts to plant accounts (1510-1540) and depreciation (7100-7400)
- **Procurement:** Posts to fuel and materials expense accounts
- **Customer Service:** Posts to accounts receivable (1210-1230)

## Date Created
October 31, 2025

## Status
✅ **Complete and Production-Ready**
- 105 comprehensive accounts for electric utility operations
- All seed data updated with correct account references
- No compilation errors
- Ready for database seeding

