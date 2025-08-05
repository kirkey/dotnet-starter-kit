using Accounting.Domain;
using Accounting.Application.DepreciationMethods.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.DepreciationMethods.Delete;

public sealed class DeleteDepreciationMethodHandler(
    [FromKeyedServices("accounting")] IRepository<DepreciationMethod> repository)
    : IRequestHandler<DeleteDepreciationMethodRequest>
{
    public async Task Handle(DeleteDepreciationMethodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (method == null) throw new DepreciationMethodNotFoundException(request.Id);

        await repository.DeleteAsync(method, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
