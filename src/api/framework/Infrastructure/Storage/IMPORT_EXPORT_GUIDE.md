# Generic Import/Export Infrastructure

## Overview

The generic import/export infrastructure provides a dynamic, reusable solution for importing and exporting data across all modules in the application. It's located in the `Framework/Infrastructure/Storage` directory and can be called from anywhere in the application.

## Architecture

### Core Interfaces

1. **IDataImportService** - Generic service for importing Excel files
2. **IDataExportService** - Generic service for exporting to Excel files

### Implementations

1. **GenericDataImportService** - Provides automatic property mapping, custom mappers, and validation
2. **GenericDataExportService** - Supports multiple export formats, custom columns, templates, and multi-sheet workbooks

### Base Handlers

1. **GenericImportHandlerBase<TEntity, TImportRow>** - Abstract base for import handlers
2. **GenericExportHandlerBase<TEntity, TExportDto, TFilter>** - Abstract base for export handlers
3. **GenericExportWithConfigurationHandlerBase<TEntity, TExportDto, TFilter>** - Abstract base for export with custom column configuration

## Usage

### 1. Simple Import Example

#### Step 1: Define Import Row DTO

```csharp
public class ProductImportRow
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? StockQuantity { get; set; }
    public string? Category { get; set; }
}
```

#### Step 2: Create Import Handler

```csharp
public class ImportProductsHandler : GenericImportHandlerBase<Product, ProductImportRow>
{
    private readonly IReadRepository<Product> _readRepository;

    public ImportProductsHandler(
        IDataImportService importService,
        IRepository<Product> repository,
        IReadRepository<Product> readRepository,
        ILogger<ImportProductsHandler> logger)
        : base(importService, repository, logger)
    {
        _readRepository = readRepository;
    }

    protected override async Task<IReadOnlyList<string>> ValidateRowAsync(
        ProductImportRow row, 
        int rowIndex, 
        CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(row.Name))
        {
            errors.Add($"Row {rowIndex}: Product name is required");
        }

        if (!row.Price.HasValue || row.Price.Value <= 0)
        {
            errors.Add($"Row {rowIndex}: Valid price is required");
        }

        // Check for duplicates
        if (!string.IsNullOrWhiteSpace(row.Name))
        {
            var exists = await _readRepository.AnyAsync(
                new ProductByNameSpec(row.Name), 
                cancellationToken);
                
            if (exists)
            {
                errors.Add($"Row {rowIndex}: Product '{row.Name}' already exists");
            }
        }

        return errors;
    }

    protected override async Task<Product> MapToEntityAsync(
        ProductImportRow row, 
        CancellationToken cancellationToken)
    {
        return await Task.FromResult(Product.Create(
            row.Name!,
            row.Description,
            row.Price!.Value,
            row.StockQuantity ?? 0,
            row.Category));
    }
}
```

#### Step 3: Create Command and Endpoint

```csharp
// Command
public record ImportProductsCommand(FileUploadCommand File) 
    : GenericImportCommand<ImportResponse>
{
    public FileUploadCommand File { get; init; } = File;
}

// Endpoint
public class ImportProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/import", async (
            [FromBody] ImportProductsCommand command,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.BadRequest(result);
        })
        .WithName("ImportProducts")
        .WithTags("Products")
        .Produces<ImportResponse>(200)
        .Produces<ImportResponse>(400);
    }
}
```

### 2. Simple Export Example

#### Step 1: Define Export DTO

```csharp
public class ProductExportDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
```

#### Step 2: Create Export Handler

```csharp
public class ExportProductsHandler : GenericExportHandlerBase<Product, ProductExportDto, ProductFilterDto>
{
    public ExportProductsHandler(
        IDataExportService exportService,
        IReadRepository<Product> repository,
        ILogger<ExportProductsHandler> logger)
        : base(exportService, repository, logger)
    {
    }

    protected override Specification<Product>? BuildSpecification(ProductFilterDto filter)
    {
        return new ExportProductsSpec(filter);
    }

    protected override ProductExportDto MapToExportDto(Product entity)
    {
        return new ProductExportDto
        {
            Name = entity.Name,
            Description = entity.Description ?? string.Empty,
            Price = entity.Price,
            StockQuantity = entity.StockQuantity,
            Category = entity.Category ?? string.Empty,
            CreatedDate = entity.CreatedOn
        };
    }

    protected override string GetExportFileName()
    {
        return $"Products_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
    }
}
```

#### Step 3: Create Query and Endpoint

```csharp
// Query
public record ExportProductsQuery(ProductFilterDto? Filter) 
    : GenericExportQuery<ProductFilterDto, ExportResponse>
{
    public ProductFilterDto? Filter { get; init; } = Filter;
}

// Endpoint
public class ExportProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/export", async (
            [FromBody] ExportProductsQuery query,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            
            return Results.File(
                result.Data,
                result.ContentType,
                result.FileName);
        })
        .WithName("ExportProducts")
        .WithTags("Products")
        .Produces<FileResult>(200);
    }
}
```

### 3. Advanced Export with Custom Column Configuration

```csharp
public class ExportProductsWithConfigHandler : GenericExportWithConfigurationHandlerBase<Product, ProductExportDto, ProductFilterDto>
{
    public ExportProductsWithConfigHandler(
        IDataExportService exportService,
        IReadRepository<Product> repository,
        ILogger<ExportProductsWithConfigHandler> logger)
        : base(exportService, repository, logger)
    {
    }

    protected override Specification<Product>? BuildSpecification(ProductFilterDto filter)
    {
        return new ExportProductsSpec(filter);
    }

    protected override ProductExportDto MapToExportDto(Product entity)
    {
        return new ProductExportDto
        {
            Name = entity.Name,
            Description = entity.Description ?? string.Empty,
            Price = entity.Price,
            StockQuantity = entity.StockQuantity,
            Category = entity.Category ?? string.Empty,
            CreatedDate = entity.CreatedOn
        };
    }

    protected override IEnumerable<ExportColumnConfiguration<ProductExportDto>> GetColumnConfigurations()
    {
        return new[]
        {
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Product Name",
                ValueSelector = p => p.Name,
                Order = 1,
                ColumnWidth = 30
            },
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Description",
                ValueSelector = p => p.Description,
                Order = 2,
                ColumnWidth = 50
            },
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Price (USD)",
                ValueSelector = p => p.Price,
                Format = "${0:N2}",
                Order = 3,
                ColumnWidth = 15
            },
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Stock",
                ValueSelector = p => p.StockQuantity,
                Order = 4,
                ColumnWidth = 12
            },
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Category",
                ValueSelector = p => p.Category,
                Order = 5,
                ColumnWidth = 20
            },
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Created Date",
                ValueSelector = p => p.CreatedDate,
                Format = "{0:yyyy-MM-dd}",
                Order = 6,
                ColumnWidth = 15
            }
        };
    }
}
```

### 4. Direct Service Usage (Without Base Handlers)

#### Import

```csharp
public class CustomImportService
{
    private readonly IDataImportService _importService;

    public CustomImportService(IDataImportService importService)
    {
        _importService = importService;
    }

    public async Task<List<ProductImportRow>> ImportProductsAsync(FileUploadCommand file)
    {
        // Option 1: Automatic property mapping
        var products = await _importService.ParseAsync<ProductImportRow>(file);
        
        return products.ToList();
    }

    public async Task<List<Product>> ImportWithCustomMapperAsync(FileUploadCommand file)
    {
        // Option 2: Custom mapper
        var products = await _importService.ParseWithMapperAsync<Product>(
            file,
            rowData => new Product
            {
                Name = rowData["Name"]?.ToString() ?? string.Empty,
                Price = Convert.ToDecimal(rowData["Price"] ?? 0),
                // ... map other fields
            });
        
        return products.ToList();
    }

    public async Task<ImportValidationResult> ValidateFileAsync(FileUploadCommand file)
    {
        // Validate file structure
        var expectedColumns = new[] { "Name", "Description", "Price", "StockQuantity", "Category" };
        var validationResult = await _importService.ValidateFileStructureAsync(
            file, 
            expectedColumns);
        
        return validationResult;
    }
}
```

#### Export

```csharp
public class CustomExportService
{
    private readonly IDataExportService _exportService;

    public CustomExportService(IDataExportService exportService)
    {
        _exportService = exportService;
    }

    public byte[] ExportToBytes(List<ProductExportDto> products)
    {
        return _exportService.ExportToBytes(products, "Products");
    }

    public Stream ExportToStream(List<ProductExportDto> products)
    {
        return _exportService.ExportToStream(products, "Products");
    }

    public Stream ExportWithCustomColumns(List<ProductExportDto> products)
    {
        var columns = new[]
        {
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Product Name",
                ValueSelector = p => p.Name,
                Order = 1,
                ColumnWidth = 30
            },
            new ExportColumnConfiguration<ProductExportDto>
            {
                HeaderName = "Price",
                ValueSelector = p => p.Price,
                Format = "${0:N2}",
                Order = 2
            }
        };

        return _exportService.ExportWithConfigurationToStream(products, columns, "Products");
    }

    public Stream ExportMultipleSheets()
    {
        var sheets = new Dictionary<string, IEnumerable<object>>
        {
            ["Products"] = GetProducts(),
            ["Categories"] = GetCategories(),
            ["Orders"] = GetOrders()
        };

        return _exportService.ExportMultipleSheetsToStream(sheets);
    }
}
```

## Features

### Import Features

- ✅ Automatic property mapping from Excel columns to object properties
- ✅ Custom column mapper support
- ✅ File structure validation
- ✅ Support for multiple data types (string, int, decimal, DateTime, Guid, bool, enums)
- ✅ Nullable type support
- ✅ Error handling and validation
- ✅ Row-level error reporting
- ✅ Cancellation token support

### Export Features

- ✅ Automatic property mapping to Excel columns
- ✅ Custom column configuration (headers, formats, widths)
- ✅ Multiple sheet support in single workbook
- ✅ Template-based export
- ✅ Stream and byte array output
- ✅ Auto-fit column widths
- ✅ Header styling
- ✅ Custom formatting support

## Benefits

1. **Reusability** - Write once, use everywhere across all modules
2. **Type Safety** - Strongly-typed with generic constraints
3. **Flexibility** - Support for both automatic and custom mapping
4. **DRY Principle** - Eliminates code duplication
5. **CQRS Pattern** - Follows command/query separation
6. **Validation** - Built-in validation support
7. **Error Handling** - Comprehensive error handling and reporting
8. **Performance** - Efficient streaming and memory management
9. **Extensibility** - Easy to extend with custom logic
10. **Documentation** - Full XML documentation on all public APIs

## Excel File Format

### Import Format

Excel files must have:
- First row as header with column names matching property names (case-insensitive)
- Data rows starting from row 2
- Proper data types in cells

Example:
```
| Name        | Description      | Price | StockQuantity | Category    |
|-------------|------------------|-------|---------------|-------------|
| Product A   | Description A    | 29.99 | 100           | Electronics |
| Product B   | Description B    | 49.99 | 50            | Furniture   |
```

### Export Format

Generated Excel files include:
- Bold header row with gray background
- Auto-fitted or custom column widths
- Formatted values based on configuration
- Professional appearance

## Migration Guide

### Migrating Existing Import/Export Code

1. Replace custom parsers with `IDataImportService`
2. Replace custom export logic with `IDataExportService`
3. Use base handlers or inject services directly
4. Update endpoints to use standard commands/queries
5. Remove duplicate code

### Example Migration

**Before:**
```csharp
// Custom parser with lots of boilerplate
public class CustomItemParser
{
    public async Task<List<Item>> ParseAsync(FileUploadCommand file)
    {
        // 100+ lines of Excel parsing code
        // Manual cell reading
        // Type conversion
        // Error handling
        // ...
    }
}
```

**After:**
```csharp
// Use generic service
public class ItemImportHandler : GenericImportHandlerBase<Item, ItemImportRow>
{
    // Only implement business logic
    protected override Task<IReadOnlyList<string>> ValidateRowAsync(...) { }
    protected override Task<Item> MapToEntityAsync(...) { }
}
```

## Best Practices

1. **Always validate** import data before processing
2. **Use DTOs** for import/export instead of domain entities
3. **Implement proper error handling** in validators and mappers
4. **Log operations** for troubleshooting
5. **Use specifications** for complex filtering in exports
6. **Consider memory** for large datasets (use streaming)
7. **Test with real data** to ensure proper type conversions
8. **Document expected columns** in API documentation
9. **Provide sample files** for users
10. **Handle duplicates** appropriately in import logic

## Troubleshooting

### Common Issues

**Issue: Column not found**
- Ensure Excel headers match property names (case-insensitive)
- Use custom mapper if column names differ

**Issue: Type conversion error**
- Ensure Excel cells have correct data types
- Implement custom conversion logic if needed

**Issue: Memory issues with large files**
- Process in batches
- Use streaming where possible
- Consider background jobs for very large imports

**Issue: Date format issues**
- Ensure dates are in Excel date format (not text)
- Use DateTime cells, not formatted strings

## Support

For questions or issues with the import/export infrastructure, contact the framework team or create an issue in the repository.

