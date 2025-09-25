# Grocery Items Import (Excel)

This feature imports new GroceryItems from an Excel (.xlsx) workbook and returns the total number of items imported.

- Endpoint: `POST /store/grocery-items/import?api-version=1`
- Request body: `{ "file": { "name": "items.xlsx", "extension": ".xlsx", "data": "<base64>" } }`
- Response: integer (total successfully imported)

## Excel Template Columns (Row 1 headers, data starts at Row 2)

| Col | Header              | Type     | Required | Notes |
|-----|---------------------|----------|----------|-------|
| A   | Name                | string   | yes      | <= 200 chars |
| B   | Description         | string   | no       | |
| C   | Sku                 | string   | yes      | <= 100 chars; unique |
| D   | Barcode             | string   | yes      | <= 100 chars; unique |
| E   | Price               | decimal  | yes      | >= 0 and >= Cost |
| F   | Cost                | decimal  | yes      | >= 0 |
| G   | MinimumStock        | int      | yes      | >= 0 |
| H   | MaximumStock        | int      | yes      | > 0, >= MinimumStock |
| I   | CurrentStock        | int      | yes      | 0..MaximumStock |
| J   | ReorderPoint        | int      | yes      | 0..MaximumStock |
| K   | IsPerishable        | bool     | no       | true/false/1/0 |
| L   | ExpiryDate          | date     | if K     | > today when IsPerishable=true |
| M   | Brand               | string   | no       | <= 200 chars |
| N   | Manufacturer        | string   | no       | <= 200 chars |
| O   | Weight              | decimal  | no       | >= 0 |
| P   | WeightUnit          | string   | if O>0   | <= 20 chars |
| Q   | CategoryId          | guid     | yes      | Existing Category Id |
| R   | SupplierId          | guid     | yes      | Existing Supplier Id |
| S   | WarehouseLocationId | guid     | no       | Optional |

Rows that fail validation or duplicate checks (SKU/Barcode) are skipped; the endpoint returns the count of successfully created items.

## Sample Request (JSON)

```
POST /store/grocery-items/import?api-version=1
Content-Type: application/json

{
  "file": {
    "name": "items.xlsx",
    "extension": ".xlsx",
    "data": "<base64-excel>"
  }
}
```

Max file size: 10 MB (base64 allowed). Only `.xlsx` is supported.

