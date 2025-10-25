# Purchase Order PDF Report Implementation

## Overview
Successfully implemented a professional PDF report generation feature for Purchase Orders using QuestPDF library. The implementation includes:

- Professional PDF document with company header
- Purchase order details (PO number, dates, status, etc.)
- Supplier information
- Detailed items table with quantities, pricing, and totals
- Summary section with subtotal, discounts, tax, and net amount
- Mock approval signatures (Prepared By, Reviewed By, Approved By)
- Notes section
- Page numbering and footer

## Files Created/Modified

### Backend (API)

#### 1. Package Dependencies
- **File**: `Directory.Packages.props`
- **Added**: QuestPDF version 2024.12.3

#### 2. Application Layer
- **File**: `Store.Application/PurchaseOrders/Report/v1/GeneratePurchaseOrderPdfCommand.cs`
  - Command to request PDF generation for a purchase order

- **File**: `Store.Application/PurchaseOrders/Report/v1/GeneratePurchaseOrderPdfCommandValidator.cs`
  - Validates the PDF generation command

- **File**: `Store.Application/PurchaseOrders/Report/v1/GeneratePurchaseOrderPdfHandler.cs`
  - Handles the PDF generation request
  - Fetches purchase order with items and related data
  - Calls the PDF service to generate the document

- **File**: `Store.Application/PurchaseOrders/Report/v1/Services/IPurchaseOrderPdfService.cs`
  - Interface for PDF generation service

- **File**: `Store.Application/PurchaseOrders/Specs/PurchaseOrderByIdWithItemsSpec.cs`
  - Specification to load purchase order with items, supplier, and item details

#### 3. Infrastructure Layer
- **File**: `Store.Infrastructure/Services/PurchaseOrderPdfService.cs`
  - Full implementation of PDF generation using QuestPDF
  - Creates professional multi-section document with:
    - Company header with logo placeholder
    - Order information panel (PO number, dates, status)
    - Supplier details
    - Items table with alternating row colors
    - Financial summary (subtotal, discounts, tax, shipping, net amount)
    - Approval section with mock signatures:
      - Prepared By: John Smith (Purchasing Manager)
      - Reviewed By: Sarah Johnson (Supply Chain Director)
      - Approved By: Michael Chen (Finance Director)
    - Notes section
    - Footer with page numbers and generation timestamp

- **File**: `Store.Infrastructure/Endpoints/PurchaseOrders/v1/GeneratePurchaseOrderPdfEndpoint.cs`
  - REST endpoint: GET /api/v1/store/purchase-orders/{id}/pdf
  - Returns PDF file as downloadable attachment

- **File**: `Store.Infrastructure/StoreModule.cs`
  - Registered PDF service in dependency injection container
  - Mapped PDF generation endpoint

- **File**: `Store.Infrastructure/Endpoints/PurchaseOrders/PurchaseOrdersEndpoints.cs`
  - Added PDF endpoint to the purchase orders group

#### 4. Project Files Updated
- **File**: `Store.Application/Store.Application.csproj`
  - Added QuestPDF package reference

- **File**: `Store.Infrastructure/Store.Infrastructure.csproj`
  - Added QuestPDF package reference

### Frontend (Blazor)

#### 1. UI Updates
- **File**: `apps/blazor/client/Pages/Store/PurchaseOrders.razor`
  - Added "Download PDF" menu item to the ExtraActions menu
  - Shows PDF icon with red color for visual distinction
  - Available for all purchase orders regardless of status

- **File**: `apps/blazor/client/Pages/Store/PurchaseOrders.razor.cs`
  - Added `DownloadPdf(DefaultIdType id)` method
  - Makes HTTP call to the PDF endpoint
  - Uses existing `fshDownload.saveFile` JavaScript function to trigger download
  - Provides user feedback via Snackbar notifications

## API Endpoint

**Endpoint**: `GET /api/v1/store/purchase-orders/{id}/pdf`

**Response**: 
- Content-Type: application/pdf
- File download with name: `PurchaseOrder_{id}_{timestamp}.pdf`

## TODO: API Client Update

The Blazor UI currently uses a direct HTTP call to download the PDF. You mentioned you'll update the API client yourself later. When you do, replace the direct HTTP call in `PurchaseOrders.razor.cs` with:

```csharp
var pdfBytes = await Client.GeneratePurchaseOrderPdfEndpointAsync("1", id);
var fileName = $"PurchaseOrder_{id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
var base64 = Convert.ToBase64String(pdfBytes);
await Js.InvokeVoidAsync("fshDownload.saveFile", fileName, base64);
```

## Testing the Feature

1. **Start the API server**:
   ```bash
   cd src/api/server
   dotnet run
   ```

2. **Navigate to Purchase Orders page**: `/store/purchase-orders`

3. **Click the three-dot menu** on any purchase order

4. **Select "Download PDF"** - the PDF will be generated and downloaded automatically

## Customization

### Company Information
To update company details in the PDF header, edit:
`Store.Infrastructure/Services/PurchaseOrderPdfService.cs` - `ComposeHeader` method

Current placeholder values:
- Company Name: "Your Company Name"
- Address: "123 Business Street, City, State 12345"
- Phone: "(555) 123-4567"
- Email: "orders@yourcompany.com"

### Approval Signatures
To update approval names and titles, edit:
`Store.Infrastructure/Services/PurchaseOrderPdfService.cs` - `ComposeApprovalSection` method

Current mock approvers:
- Prepared By: John Smith (Purchasing Manager)
- Reviewed By: Sarah Johnson (Supply Chain Director)
- Approved By: Michael Chen (Finance Director)

### Styling
The PDF uses QuestPDF's fluent API. You can customize:
- Colors (currently using Blue.Darken2 for headers)
- Fonts (currently Arial, 10pt base size)
- Layout and spacing
- Table column widths

## Build Status

✅ Store.Application builds successfully
✅ Store.Infrastructure builds successfully
✅ All dependencies resolved
✅ QuestPDF integrated correctly

## Notes

- QuestPDF Community license is configured (free for non-commercial use)
- PDF generation happens server-side for security and performance
- The implementation follows CQRS pattern with command, validator, and handler
- Follows DRY principles with reusable compose methods
- Comprehensive documentation added to all classes and methods
- Uses specifications for efficient data loading with eager loading of related entities

