using Microsoft.Extensions.Logging;

namespace Accounting.Application.RetainedEarnings.UpdateNetIncome.v1;

public sealed class UpdateNetIncomeHandler : IRequestHandler<UpdateNetIncomeCommand, DefaultIdType>
{
    private readonly IRepository<Domain.Entities.RetainedEarnings> _repository;
    private readonly ILogger<UpdateNetIncomeHandler> _logger;

    public UpdateNetIncomeHandler(IRepository<Domain.Entities.RetainedEarnings> repository, ILogger<UpdateNetIncomeHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DefaultIdType> Handle(UpdateNetIncomeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating net income for retained earnings {Id}: {NetIncome}", request.Id, request.NetIncome);

        var re = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (re == null) throw new NotFoundException($"Retained earnings with ID {request.Id} not found");

        re.UpdateNetIncome(request.NetIncome);
        await _repository.UpdateAsync(re, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Net income updated for FY{FiscalYear}", re.FiscalYear);
        return re.Id;
    }
}

