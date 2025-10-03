namespace Store.Infrastructure.Import.GroceryItems;

using System.Globalization;
using ClosedXML.Excel;
using FSH.Framework.Core.Storage.File.Features;

/// <summary>
/// Excel (.xlsx) parser for Grocery Item import using ClosedXML.
/// </summary>
internal sealed class ClosedXmlGroceryItemImportParser : IGroceryItemImportParser
{
    public async Task<IReadOnlyList<GroceryItemImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        if (!file.IsValid()) return Array.Empty<GroceryItemImportRow>();

        // Decode base64 to bytes
        byte[] bytes = Convert.FromBase64String(file.Data);

        using var ms = new MemoryStream(bytes, writable: false);
        using var workbook = new XLWorkbook(ms);
        var ws = workbook.Worksheets.FirstOrDefault();
        if (ws is null) return Array.Empty<GroceryItemImportRow>();

        var rows = new List<GroceryItemImportRow>(capacity: 128);

        // Assume first row is header
        var firstDataRow = 2;
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
        for (int r = firstDataRow; r <= lastRow; r++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string? ReadStr(string col) => ws.Cell($"{col}{r}").GetString()?.Trim();
            decimal? ReadDec(string col)
            {
                var c = ws.Cell($"{col}{r}");
                if (c.TryGetValue<decimal>(out var d)) return d;
                var s = c.GetString();
                if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return d;
                return null;
            }
            int? ReadInt(string col)
            {
                var c = ws.Cell($"{col}{r}");
                if (c.TryGetValue<int>(out var i)) return i;
                var s = c.GetString();
                if (int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out i)) return i;
                return null;
            }
            bool? ReadBool(string col)
            {
                var c = ws.Cell($"{col}{r}");
                if (c.TryGetValue<bool>(out var b)) return b;
                var s = c.GetString();
                if (bool.TryParse(s, out b)) return b;
                if (string.Equals(s, "1", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(s, "0", StringComparison.OrdinalIgnoreCase)) return false;
                return null;
            }
            DateTime? ReadDate(string col)
            {
                var c = ws.Cell($"{col}{r}");
                if (c.TryGetValue<DateTime>(out var dt)) return dt;
                var s = c.GetString();
                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt)) return dt;
                return null;
            }
            DefaultIdType? ReadGuid(string col)
            {
                var s = ws.Cell($"{col}{r}").GetString();
                if (DefaultIdType.TryParse(s, out var g)) return g;
                return null;
            }

            // Columns by letter (documented in README):
            // A: Name, B: Description, C: Sku, D: Barcode, E: Price, F: Cost,
            // G: MinimumStock, H: MaximumStock, I: CurrentStock, J: ReorderPoint,
            // K: IsPerishable, L: ExpiryDate, M: Brand, N: Manufacturer,
            // O: Weight, P: WeightUnit, Q: CategoryId, R: SupplierId, S: WarehouseLocationId
            var row = new GroceryItemImportRow
            {
                Name = ReadStr("A"),
                Description = ReadStr("B"),
                Sku = ReadStr("C"),
                Barcode = ReadStr("D"),
                Price = ReadDec("E"),
                Cost = ReadDec("F"),
                MinimumStock = ReadInt("G"),
                MaximumStock = ReadInt("H"),
                CurrentStock = ReadInt("I"),
                ReorderPoint = ReadInt("J"),
                IsPerishable = ReadBool("K"),
                ExpiryDate = ReadDate("L"),
                Brand = ReadStr("M"),
                Manufacturer = ReadStr("N"),
                Weight = ReadDec("O"),
                WeightUnit = ReadStr("P"),
                CategoryId = ReadGuid("Q"),
                SupplierId = ReadGuid("R"),
                WarehouseLocationId = ReadGuid("S"),
            };

            // skip empty lines
            if (string.IsNullOrWhiteSpace(row.Name) && string.IsNullOrWhiteSpace(row.Sku) && string.IsNullOrWhiteSpace(row.Barcode))
                continue;

            rows.Add(row);
        }

        // Simulate async boundary
        await Task.CompletedTask.ConfigureAwait(false);
        return rows;
    }
}

