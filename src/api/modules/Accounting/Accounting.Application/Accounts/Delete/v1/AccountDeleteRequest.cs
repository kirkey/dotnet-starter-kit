using FSH.Framework.Infrastructure.Abstract.Delete;
using MediatR;

namespace Accounting.Application.Accounts.Delete.v1;

public class AccountDeleteRequest(DefaultIdType id) : DeleteRequest<DefaultIdType>(id);
