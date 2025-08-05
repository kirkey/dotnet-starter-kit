using Accounting.Domain;
using Accounting.Application.DepreciationMethods.Exceptions;
using Accounting.Application.DepreciationMethods.Queries;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.DepreciationMethods.Update;

public sealed class UpdateDepreciationMethodHandler(
    [FromKeyedServices("accounting")] IRepository<DepreciationMethod> repository)
    : IRequestHandler<UpdateDepreciationMethodRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateDepreciationMethodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (method == null) throw new DepreciationMethodNotFoundException(request.Id);

        // Check for duplicate method code (excluding current method)
        if (!string.IsNullOrEmpty(request.MethodCode) && request.MethodCode != method.MethodCode)
        {
            var existingByCode = await repository.FirstOrDefaultAsync(
                new DepreciationMethodByCodeSpec(request.MethodCode), cancellationToken);
            if (existingByCode != null)
            {
                throw new DepreciationMethodCodeAlreadyExistsException(request.MethodCode);
            }
        }

        method.Update(request.MethodCode, request.MethodName, request.CalculationFormula, request.IsActive, request.Description, request.Notes);

        await repository.UpdateAsync(method, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return method.Id;
    }
}
