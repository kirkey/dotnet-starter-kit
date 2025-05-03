using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;
public sealed class ChartOfAccountNotFoundException(DefaultIdType id) : NotFoundException($"account with id {id} not found");
