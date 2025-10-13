using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.ChartOfAccounts.Import.v1;

/// <summary>
/// Command to import chart of accounts from an Excel file (.xlsx). 
/// Returns ImportResponse containing total imported count, failed count, and error details.
/// Validates and creates account entries following USOA compliance standards.
/// </summary>
public sealed record ImportChartOfAccountsCommand(FileUploadCommand File) : IRequest<ImportResponse>;
