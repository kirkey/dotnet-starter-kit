#!/bin/bash

# MD Files Organization Script
# This script organizes all documentation files into feature-specific folders

cd "$(dirname "$0")"

echo "=== Starting MD Files Organization ==="

# Create directory structure
mkdir -p docs/accounting
mkdir -p docs/store
mkdir -p docs/warehouse
mkdir -p docs/blazor-ui
mkdir -p docs/signalr
mkdir -p docs/architecture
mkdir -p docs/inventory-counting

echo "✓ Created directory structure"

# Delete minor update/fix files (small summaries that are no longer needed)
rm -f ACCOUNTING_DB_INITIALIZER_APPROVAL_UPDATES.md
rm -f ACCOUNTING_DB_INITIALIZER_FIXES.md
rm -f ACCOUNTING_DI_FIX_COMPLETE.md
rm -f ACCOUNTING_DOCS_CLEANUP_COMPLETE.md
rm -f ACCOUNTING_EXPLICIT_PAGINATION_COMPLETE.md
rm -f ACCOUNTING_PAGINATION_ALL_FIXED.md
rm -f ACCOUNTING_SEED_DATA_COMPLETE.md
rm -f API_MISSING_PROPERTIES_FIXED.md
rm -f BANK_SEARCH_PATTERN_FIX.md
rm -f COMPILATION_ERRORS_FIX_COMPLETE.md
rm -f CONNECTION_HUB_SEPARATION.md
rm -f METERS_CONSUMPTION_IMPLEMENTATION_PROGRESS.md
rm -f METERS_CONSUMPTION_STATUS.md
rm -f QUICK_FIX_SUMMARY.md
rm -f SEARCH_HANDLER_FIX_COMPLETE_SUMMARY.md
rm -f SEARCH_HANDLER_FIX_FINAL_REPORT.md
rm -f SEARCH_HANDLER_PATTERN_FIX_PROGRESS.md
rm -f STORE_WAREHOUSE_PAGINATION_COMPLETE.md
rm -f FILES_INVENTORY.md
rm -f DESCRIPTION_NOTES_AUDIT_COMPLETE.md
rm -f NSWAG_SCRIPTS_LOCATION.md

echo "✓ Deleted minor update files"

# Move ACCOUNTING files
[ -f "ACCOUNTING_BEST_PRACTICES_REVIEW.md" ] && mv ACCOUNTING_BEST_PRACTICES_REVIEW.md docs/accounting/
[ -f "ACCOUNTING_DOCS_CLEANUP_VISUAL_SUMMARY.md" ] && mv ACCOUNTING_DOCS_CLEANUP_VISUAL_SUMMARY.md docs/accounting/
[ -f "ACCOUNTING_DOCS_QUICK_REFERENCE.md" ] && mv ACCOUNTING_DOCS_QUICK_REFERENCE.md docs/accounting/
[ -f "ACCOUNTING_PERIODS_REVIEW_COMPLETE.md" ] && mv ACCOUNTING_PERIODS_REVIEW_COMPLETE.md docs/accounting/
[ -f "BANKS_CHECKS_RECONCILIATIONS_PAYMENTS_REVIEW_COMPLETE.md" ] && mv BANKS_CHECKS_RECONCILIATIONS_PAYMENTS_REVIEW_COMPLETE.md docs/accounting/
[ -f "BUDGETS_PROJECTS_ACCRUALS_TAXCODES_REVIEW_COMPLETE.md" ] && mv BUDGETS_PROJECTS_ACCRUALS_TAXCODES_REVIEW_COMPLETE.md docs/accounting/
[ -f "CHART_OF_ACCOUNTS_REVIEW_COMPLETE.md" ] && mv CHART_OF_ACCOUNTS_REVIEW_COMPLETE.md docs/accounting/
[ -f "COST_CENTERS_REVIEW_COMPLETE.md" ] && mv COST_CENTERS_REVIEW_COMPLETE.md docs/accounting/
[ -f "CUSTOMERS_INVOICES_CREDITMEMOS_AR_REVIEW_COMPLETE.md" ] && mv CUSTOMERS_INVOICES_CREDITMEMOS_AR_REVIEW_COMPLETE.md docs/accounting/
[ -f "DEFERRED_PREPAID_RECURRING_REVIEW_COMPLETE.md" ] && mv DEFERRED_PREPAID_RECURRING_REVIEW_COMPLETE.md docs/accounting/
[ -f "FISCAL_PERIOD_RETAINED_EARNINGS_FINANCIAL_STATEMENTS_REVIEW_COMPLETE.md" ] && mv FISCAL_PERIOD_RETAINED_EARNINGS_FINANCIAL_STATEMENTS_REVIEW_COMPLETE.md docs/accounting/
[ -f "GENERAL_LEDGER_TRIAL_BALANCE_REVIEW_COMPLETE.md" ] && mv GENERAL_LEDGER_TRIAL_BALANCE_REVIEW_COMPLETE.md docs/accounting/
[ -f "JOURNAL_ENTRIES_REVIEW_COMPLETE.md" ] && mv JOURNAL_ENTRIES_REVIEW_COMPLETE.md docs/accounting/
[ -f "MEMBERS_REVIEW_COMPLETE.md" ] && mv MEMBERS_REVIEW_COMPLETE.md docs/accounting/
[ -f "METERS_CONSUMPTION_COMPLETE.md" ] && mv METERS_CONSUMPTION_COMPLETE.md docs/accounting/
[ -f "METERS_MODULE_COMPLETE.md" ] && mv METERS_MODULE_COMPLETE.md docs/accounting/
[ -f "POSTING_BATCH_REVIEW_COMPLETE.md" ] && mv POSTING_BATCH_REVIEW_COMPLETE.md docs/accounting/
[ -f "VENDORS_BILLS_DEBITMEMOS_PAYEES_AP_REVIEW_COMPLETE.md" ] && mv VENDORS_BILLS_DEBITMEMOS_PAYEES_AP_REVIEW_COMPLETE.md docs/accounting/
[ -f "WRITEOFFS_FIXEDASSETS_DEPRECIATION_INVENTORY_REVIEW_COMPLETE.md" ] && mv WRITEOFFS_FIXEDASSETS_DEPRECIATION_INVENTORY_REVIEW_COMPLETE.md docs/accounting/

echo "✓ Moved ACCOUNTING files"

# Move STORE files
[ -f "PURCHASE_ORDER_AUTO_REORDER_FEATURE.md" ] && mv PURCHASE_ORDER_AUTO_REORDER_FEATURE.md docs/store/
[ -f "PURCHASE_ORDER_GOODS_RECEIPT_FLOW_SUMMARY.md" ] && mv PURCHASE_ORDER_GOODS_RECEIPT_FLOW_SUMMARY.md docs/store/
[ -f "PURCHASE_ORDER_REVIEW_SUMMARY.md" ] && mv PURCHASE_ORDER_REVIEW_SUMMARY.md docs/store/
[ -f "PURCHASE_ORDER_SUPPLIER_FILTERING_FEATURE.md" ] && mv PURCHASE_ORDER_SUPPLIER_FILTERING_FEATURE.md docs/store/
[ -f "STORE_DASHBOARD_ENHANCEMENTS.md" ] && mv STORE_DASHBOARD_ENHANCEMENTS.md docs/store/
[ -f "STORE_WAREHOUSE_BEST_PRACTICES_COMPLETE.md" ] && mv STORE_WAREHOUSE_BEST_PRACTICES_COMPLETE.md docs/store/
[ -f "STORE_WAREHOUSE_BEST_PRACTICES_REVIEW.md" ] && mv STORE_WAREHOUSE_BEST_PRACTICES_REVIEW.md docs/store/
[ -f "STORE_WAREHOUSE_QUICK_REFERENCE.md" ] && mv STORE_WAREHOUSE_QUICK_REFERENCE.md docs/store/
[ -f "STORE_WAREHOUSE_UI_GAP_SUMMARY.md" ] && mv STORE_WAREHOUSE_UI_GAP_SUMMARY.md docs/store/

echo "✓ Moved STORE files"

# Move WAREHOUSE files
[ -f "WAREHOUSES_LOCATIONS_BINS_REVIEW_COMPLETE.md" ] && mv WAREHOUSES_LOCATIONS_BINS_REVIEW_COMPLETE.md docs/warehouse/
[ -f "WAREHOUSES_UI_IMPLEMENTATION_COMPLETE.md" ] && mv WAREHOUSES_UI_IMPLEMENTATION_COMPLETE.md docs/warehouse/
[ -f "WAREHOUSE_LOCATIONS_UI_IMPLEMENTATION_COMPLETE.md" ] && mv WAREHOUSE_LOCATIONS_UI_IMPLEMENTATION_COMPLETE.md docs/warehouse/
[ -f "WAREHOUSE_OPERATIONS_MASTER_DATA_REVIEW_COMPLETE.md" ] && mv WAREHOUSE_OPERATIONS_MASTER_DATA_REVIEW_COMPLETE.md docs/warehouse/
[ -f "INVENTORY_MANAGEMENT_REVIEW_COMPLETE.md" ] && mv INVENTORY_MANAGEMENT_REVIEW_COMPLETE.md docs/warehouse/

echo "✓ Moved WAREHOUSE files"

# Move INVENTORY COUNTING files
[ -f "CYCLE_COUNTS_MASTER_DETAIL_VISUAL.md" ] && mv CYCLE_COUNTS_MASTER_DETAIL_VISUAL.md docs/inventory-counting/
[ -f "CYCLE_COUNTS_UI_REVIEW_COMPLETE.md" ] && mv CYCLE_COUNTS_UI_REVIEW_COMPLETE.md docs/inventory-counting/
[ -f "INVENTORY_COUNTING_EXECUTIVE_SUMMARY.md" ] && mv INVENTORY_COUNTING_EXECUTIVE_SUMMARY.md docs/inventory-counting/
[ -f "INVENTORY_COUNTING_INDEX.md" ] && mv INVENTORY_COUNTING_INDEX.md docs/inventory-counting/
[ -f "INVENTORY_COUNTING_QUICK_REFERENCE.md" ] && mv INVENTORY_COUNTING_QUICK_REFERENCE.md docs/inventory-counting/
[ -f "INVENTORY_COUNTING_TECHNICAL_GUIDE.md" ] && mv INVENTORY_COUNTING_TECHNICAL_GUIDE.md docs/inventory-counting/
[ -f "INVENTORY_COUNTING_VISUAL_SUMMARY.md" ] && mv INVENTORY_COUNTING_VISUAL_SUMMARY.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_API_CHECKLIST.md" ] && mv MOBILE_CYCLE_COUNT_API_CHECKLIST.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_API_IMPLEMENTATION.md" ] && mv MOBILE_CYCLE_COUNT_API_IMPLEMENTATION.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_API_QUICK_REFERENCE.md" ] && mv MOBILE_CYCLE_COUNT_API_QUICK_REFERENCE.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_IMPLEMENTATION.md" ] && mv MOBILE_CYCLE_COUNT_IMPLEMENTATION.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_INDEX.md" ] && mv MOBILE_CYCLE_COUNT_INDEX.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_SUMMARY.md" ] && mv MOBILE_CYCLE_COUNT_SUMMARY.md docs/inventory-counting/
[ -f "MOBILE_CYCLE_COUNT_VISUAL_GUIDE.md" ] && mv MOBILE_CYCLE_COUNT_VISUAL_GUIDE.md docs/inventory-counting/
[ -f "MULTI_WAREHOUSE_INVENTORY_COUNTING_GUIDE.md" ] && mv MULTI_WAREHOUSE_INVENTORY_COUNTING_GUIDE.md docs/inventory-counting/
[ -f "BARCODE_SCANNER_SETUP.md" ] && mv BARCODE_SCANNER_SETUP.md docs/inventory-counting/

echo "✓ Moved INVENTORY COUNTING files"

# Move BLAZOR UI files
[ -f "BLAZOR_CLIENT_UI_BEST_PRACTICES.md" ] && mv BLAZOR_CLIENT_UI_BEST_PRACTICES.md docs/blazor-ui/
[ -f "BLAZOR_CLIENT_UI_QUICK_REFERENCE.md" ] && mv BLAZOR_CLIENT_UI_QUICK_REFERENCE.md docs/blazor-ui/

echo "✓ Moved BLAZOR UI files"

# Move SIGNALR files
[ -f "README_SIGNALR.md" ] && mv README_SIGNALR.md docs/signalr/
[ -f "SIGNALR_ARCHITECTURE_DIAGRAM.md" ] && mv SIGNALR_ARCHITECTURE_DIAGRAM.md docs/signalr/
[ -f "SIGNALR_IMPLEMENTATION_SUMMARY.md" ] && mv SIGNALR_IMPLEMENTATION_SUMMARY.md docs/signalr/
[ -f "SIGNALR_QUICKSTART.md" ] && mv SIGNALR_QUICKSTART.md docs/signalr/
[ -f "SIGNALR_TESTING_GUIDE.md" ] && mv SIGNALR_TESTING_GUIDE.md docs/signalr/

echo "✓ Moved SIGNALR files"

# Move ARCHITECTURE files
[ -f "CQRS_IMPLEMENTATION_CHECKLIST.md" ] && mv CQRS_IMPLEMENTATION_CHECKLIST.md docs/architecture/
[ -f "ENDPOINT_PATTERN_REVIEW.md" ] && mv ENDPOINT_PATTERN_REVIEW.md docs/architecture/

echo "✓ Moved ARCHITECTURE files"

echo ""
echo "=== Organization Complete! ==="
echo ""
echo "Documentation structure:"
echo "  docs/accounting: $(ls -1 docs/accounting/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo "  docs/store: $(ls -1 docs/store/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo "  docs/warehouse: $(ls -1 docs/warehouse/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo "  docs/inventory-counting: $(ls -1 docs/inventory-counting/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo "  docs/blazor-ui: $(ls -1 docs/blazor-ui/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo "  docs/signalr: $(ls -1 docs/signalr/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo "  docs/architecture: $(ls -1 docs/architecture/*.md 2>/dev/null | wc -l | tr -d ' ') files"
echo ""
echo "Navigate to docs/README.md for the complete documentation index"
