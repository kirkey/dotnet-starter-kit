using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Store.Domain.Entities;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1.Services;

namespace Store.Infrastructure.Services;

/// <summary>
/// Service implementation for generating PDF reports for purchase orders using QuestPDF.
/// </summary>
public sealed class PurchaseOrderPdfService : IPurchaseOrderPdfService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseOrderPdfService"/> class.
    /// </summary>
    public PurchaseOrderPdfService()
    {
        // Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Generates a professional PDF document for the specified purchase order.
    /// </summary>
    /// <param name="purchaseOrder">The purchase order to generate PDF for.</param>
    /// <returns>PDF file as byte array.</returns>
    public byte[] GeneratePdf(PurchaseOrder purchaseOrder)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, purchaseOrder));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    /// <summary>
    /// Composes the header section of the PDF.
    /// </summary>
    private static void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("PURCHASE ORDER").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                column.Item().PaddingTop(5).Text("Your Company Name").FontSize(12).SemiBold();
                column.Item().Text("123 Business Street").FontSize(9);
                column.Item().Text("City, State 12345").FontSize(9);
                column.Item().Text("Phone: (555) 123-4567").FontSize(9);
                column.Item().Text("Email: orders@yourcompany.com").FontSize(9);
            });

            row.ConstantItem(150).AlignRight().Column(column =>
            {
                column.Item().Height(60).Placeholder();
            });
        });
    }

    /// <summary>
    /// Composes the main content section of the PDF.
    /// </summary>
    private static void ComposeContent(IContainer container, PurchaseOrder purchaseOrder)
    {
        container.PaddingVertical(20).Column(column =>
        {
            column.Spacing(15);

            // Order Information Section
            column.Item().Element(content => ComposeOrderInfo(content, purchaseOrder));

            // Supplier Information Section
            column.Item().Element(content => ComposeSupplierInfo(content, purchaseOrder));

            // Items Table
            column.Item().Element(content => ComposeItemsTable(content, purchaseOrder));

            // Totals Section
            column.Item().Element(content => ComposeTotals(content, purchaseOrder));

            // Approval Section
            column.Item().PaddingTop(30).Element(ComposeApprovalSection);

            // Notes Section
            if (!string.IsNullOrWhiteSpace(purchaseOrder.Notes))
            {
                column.Item().Element(content => ComposeNotes(content, purchaseOrder));
            }
        });
    }

    /// <summary>
    /// Composes the order information section.
    /// </summary>
    private static void ComposeOrderInfo(IContainer container, PurchaseOrder purchaseOrder)
    {
        container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("PO Number: ").SemiBold();
                    text.Span(purchaseOrder.OrderNumber);
                });

                row.RelativeItem().Text(text =>
                {
                    text.Span("Order Date: ").SemiBold();
                    text.Span(purchaseOrder.OrderDate.ToString("MM/dd/yyyy"));
                });
            });

            column.Item().Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("Status: ").SemiBold();
                    text.Span(purchaseOrder.Status);
                });

                row.RelativeItem().Text(text =>
                {
                    text.Span("Expected Delivery: ").SemiBold();
                    text.Span(purchaseOrder.ExpectedDeliveryDate?.ToString("MM/dd/yyyy") ?? "N/A");
                });
            });

            if (purchaseOrder.IsUrgent)
            {
                column.Item().Text("⚠ URGENT ORDER").FontColor(Colors.Red.Medium).Bold();
            }
        });
    }

    /// <summary>
    /// Composes the supplier information section.
    /// </summary>
    private static void ComposeSupplierInfo(IContainer container, PurchaseOrder purchaseOrder)
    {
        container.Column(column =>
        {
            column.Item().Text("SUPPLIER INFORMATION").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
            column.Item().PaddingTop(5).PaddingLeft(10).Column(supplierColumn =>
            {
                supplierColumn.Item().Text(text =>
                {
                    text.Span("Name: ").SemiBold();
                    text.Span(purchaseOrder.Supplier?.Name ?? "N/A");
                });

                if (!string.IsNullOrWhiteSpace(purchaseOrder.DeliveryAddress))
                {
                    supplierColumn.Item().Text(text =>
                    {
                        text.Span("Delivery Address: ").SemiBold();
                        text.Span(purchaseOrder.DeliveryAddress);
                    });
                }

                if (!string.IsNullOrWhiteSpace(purchaseOrder.ContactPerson))
                {
                    supplierColumn.Item().Text(text =>
                    {
                        text.Span("Contact Person: ").SemiBold();
                        text.Span(purchaseOrder.ContactPerson);
                    });
                }

                if (!string.IsNullOrWhiteSpace(purchaseOrder.ContactPhone))
                {
                    supplierColumn.Item().Text(text =>
                    {
                        text.Span("Contact Phone: ").SemiBold();
                        text.Span(purchaseOrder.ContactPhone);
                    });
                }
            });
        });
    }

    /// <summary>
    /// Composes the items table section.
    /// </summary>
    private static void ComposeItemsTable(IContainer container, PurchaseOrder purchaseOrder)
    {
        container.Column(column =>
        {
            column.Item().Text("ORDER ITEMS").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
            
            column.Item().PaddingTop(5).Table(table =>
            {
                // Define columns
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(40);  // #
                    columns.RelativeColumn(3);   // Item Name
                    columns.RelativeColumn(2);   // SKU
                    columns.RelativeColumn();   // Qty
                    columns.RelativeColumn(1.5f); // Unit Price
                    columns.RelativeColumn(1.5f); // Discount
                    columns.RelativeColumn(1.5f); // Total
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#").SemiBold();
                    header.Cell().Element(CellStyle).Text("Item Name").SemiBold();
                    header.Cell().Element(CellStyle).Text("SKU").SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Qty").SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit Price").SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Discount").SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Total").SemiBold();

                    static IContainer CellStyle(IContainer cellContainer)
                    {
                        return cellContainer.Background(Colors.Blue.Darken2)
                            .Padding(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten1);
                    }
                });

                // Rows
                int rowNumber = 1;
                foreach (var item in purchaseOrder.Items)
                {
                    var backgroundColor = rowNumber % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;
                    
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).Text(rowNumber.ToString());
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).Text(item.Item?.Name ?? "N/A");
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).Text(item.Item?.Sku ?? "N/A");
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).AlignRight().Text(item.Quantity.ToString());
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).AlignRight().Text($"${item.UnitPrice:N2}");
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).AlignRight().Text($"${item.DiscountAmount:N2}");
                    table.Cell().Element(c => RowCellStyle(c, backgroundColor)).AlignRight().Text($"${item.TotalPrice:N2}");

                    if (!string.IsNullOrWhiteSpace(item.Notes))
                    {
                        table.Cell().ColumnSpan(7).Element(c => RowCellStyle(c, backgroundColor))
                            .PaddingLeft(10).Text($"Note: {item.Notes}").FontSize(8).Italic();
                    }

                    rowNumber++;
                }

                static IContainer RowCellStyle(IContainer rowContainer, string backgroundColor)
                {
                    return rowContainer.Background(backgroundColor)
                        .Padding(5)
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2);
                }
            });
        });
    }

    /// <summary>
    /// Composes the totals section.
    /// </summary>
    private static void ComposeTotals(IContainer container, PurchaseOrder purchaseOrder)
    {
        container.AlignRight().Width(250).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Text("Subtotal:").SemiBold();
                row.ConstantItem(100).AlignRight().Text($"${purchaseOrder.TotalAmount:N2}");
            });

            if (purchaseOrder.DiscountAmount > 0)
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Discount:");
                    row.ConstantItem(100).AlignRight().Text($"-${purchaseOrder.DiscountAmount:N2}").FontColor(Colors.Red.Medium);
                });
            }

            if (purchaseOrder.TaxAmount > 0)
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Tax:");
                    row.ConstantItem(100).AlignRight().Text($"${purchaseOrder.TaxAmount:N2}");
                });
            }

            if (purchaseOrder.ShippingCost > 0)
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Shipping:");
                    row.ConstantItem(100).AlignRight().Text($"${purchaseOrder.ShippingCost:N2}");
                });
            }

            column.Item().PaddingTop(5).BorderTop(2).BorderColor(Colors.Blue.Darken2).PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Text("Net Amount:").FontSize(12).Bold();
                row.ConstantItem(100).AlignRight().Text($"${purchaseOrder.NetAmount:N2}").FontSize(12).Bold();
            });
        });
    }

    /// <summary>
    /// Composes the approval section with mock data.
    /// </summary>
    private static void ComposeApprovalSection(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("APPROVALS").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
            
            column.Item().PaddingTop(10).Row(row =>
            {
                row.Spacing(20);

                // Prepared By
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Prepared By:").FontSize(9).SemiBold();
                    col.Item().PaddingTop(20).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                    col.Item().PaddingTop(5).Text("John Smith").FontSize(9);
                    col.Item().Text("Purchasing Manager").FontSize(8).Italic();
                    col.Item().Text($"Date: {DateTime.Now:MM/dd/yyyy}").FontSize(8);
                });

                // Reviewed By
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Reviewed By:").FontSize(9).SemiBold();
                    col.Item().PaddingTop(20).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                    col.Item().PaddingTop(5).Text("Sarah Johnson").FontSize(9);
                    col.Item().Text("Supply Chain Director").FontSize(8).Italic();
                    col.Item().Text($"Date: {DateTime.Now:MM/dd/yyyy}").FontSize(8);
                });

                // Approved By
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Approved By:").FontSize(9).SemiBold();
                    col.Item().PaddingTop(20).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                    col.Item().PaddingTop(5).Text("Michael Chen").FontSize(9);
                    col.Item().Text("Finance Director").FontSize(8).Italic();
                    col.Item().Text($"Date: {DateTime.Now:MM/dd/yyyy}").FontSize(8);
                });
            });
        });
    }
    
    /// <summary>
    /// Composes the notes section.
    /// </summary>
    private static void ComposeNotes(IContainer container, PurchaseOrder purchaseOrder)
    {
        container.Column(column =>
        {
            column.Item().Text("NOTES").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
            column.Item().PaddingTop(5).PaddingLeft(10).Text(purchaseOrder.Notes).FontSize(9);
        });
    }

    /// <summary>
    /// Composes the footer section of the PDF.
    /// </summary>
    private static void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.Span("Page ").FontSize(8).FontColor(Colors.Grey.Darken1);
            text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Darken1);
            text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Darken1);
            text.TotalPages().FontSize(8).FontColor(Colors.Grey.Darken1);
            text.Span(" | Generated on ").FontSize(8).FontColor(Colors.Grey.Darken1);
            text.Span(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")).FontSize(8).FontColor(Colors.Grey.Darken1);
        });
    }
}
