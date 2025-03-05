using Accounting.Domain.Enums;
using FSH.Framework.Core.Dto;

namespace Accounting.Application.Accounts.Get.v1;
public sealed record AccountResponse(Category Type, string Code, decimal Balance) : BaseDto;
