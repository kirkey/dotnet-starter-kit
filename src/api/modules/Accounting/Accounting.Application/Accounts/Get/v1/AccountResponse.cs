using Accounting.Domain.Enums;
using FSH.Framework.Core.Dto;

namespace Accounting.Application.Accounts.Get.v1;
public sealed record AccountResponse(Category Category, TransactionType TransactionType, string ParentCode, string Code, decimal Balance) : BaseDto;
