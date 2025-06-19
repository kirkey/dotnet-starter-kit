using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.ChartOfAccounts.Exceptions;

internal sealed class ChartOfAccountForbiddenException(string accountCode)
    : ForbiddenException($"account with account code/name {accountCode} already exists.");
