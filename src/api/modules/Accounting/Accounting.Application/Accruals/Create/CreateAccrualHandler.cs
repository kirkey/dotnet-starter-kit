using Accounting.Domain.Entities;

namespace Accounting.Application.Accruals.Create;

public sealed class CreateAccrualHandler(
    IRepository<Accrual> repository,
    IReadRepository<Accrual> readRepository)
    : IRequestHandler<CreateAccrualCommand, CreateAccrualResponse>
{
    public async Task<CreateAccrualResponse> Handle(CreateAccrualCommand request, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Unique check for AccrualNumber
        var exists = await readRepository.AnyAsync(new Specs.AccrualByNumberSpec(request.AccrualNumber), ct);
        if (exists)
            throw new ConflictException($"accrual number {request.AccrualNumber} already exists");

        var entity = Accrual.Create(request.AccrualNumber, request.AccrualDate, request.Amount, request.Description ?? string.Empty);
        await repository.AddAsync(entity, ct);
        return new CreateAccrualResponse(entity.Id, entity.AccrualNumber);
    }
}

