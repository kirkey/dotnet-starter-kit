namespace Accounting.Application.Accruals.Update;

/// <summary>
/// Handles updating mutable fields of an <see cref="Accrual"/> aggregate.
/// </summary>
public sealed class UpdateAccrualHandler(
    IRepository<Accrual> repository,
    IReadRepository<Accrual> readRepository)
    : IRequestHandler<UpdateAccrualCommand, DefaultIdType>
{
    /// <summary>
    /// Processes the update command, validating business rules and persisting changes.
    /// </summary>
    public async Task<DefaultIdType> Handle(UpdateAccrualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Basic validation
        if (request.Id == DefaultIdType.Empty)
            throw new BadRequestException("Id is required.");

        if (request.AccrualNumber is { Length: > 50 })
            throw new BadRequestException("AccrualNumber cannot exceed 50 characters.");

        if (request.Amount is <= 0)
            throw new BadRequestException("Amount must be greater than zero.");

        // Load existing entity
        var entity = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException($"Accrual with id {request.Id} was not found.");

        // If accrual number is being changed, ensure uniqueness
        if (!string.IsNullOrWhiteSpace(request.AccrualNumber) && request.AccrualNumber != entity.AccrualNumber)
        {
            var exists = await readRepository.AnyAsync(new Specs.AccrualByNumberSpec(request.AccrualNumber), cancellationToken);
            if (exists)
                throw new ConflictException($"Accrual number {request.AccrualNumber} already exists.");
        }

        // Domain update (will enforce domain rules and immutability when reversed)
        entity.Update(request.AccrualNumber, request.AccrualDate, request.Amount, request.Description);

        await repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }
}
