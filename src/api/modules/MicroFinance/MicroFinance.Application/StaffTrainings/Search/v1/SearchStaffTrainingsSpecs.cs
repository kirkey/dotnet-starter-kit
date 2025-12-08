using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Search.v1;

public class SearchStaffTrainingsSpecs : EntitiesByPaginationFilterSpec<StaffTraining, StaffTrainingSummaryResponse>
{
    public SearchStaffTrainingsSpecs(SearchStaffTrainingsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.StartDate, !command.HasOrderBy())
            .Where(x => x.StaffId == command.StaffId!.Value, command.StaffId.HasValue)
            .Where(x => x.TrainingCode == command.TrainingCode, !string.IsNullOrWhiteSpace(command.TrainingCode))
            .Where(x => x.TrainingType == command.TrainingType, !string.IsNullOrWhiteSpace(command.TrainingType))
            .Where(x => x.DeliveryMethod == command.DeliveryMethod, !string.IsNullOrWhiteSpace(command.DeliveryMethod))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.Provider == command.Provider, !string.IsNullOrWhiteSpace(command.Provider))
            .Where(x => x.StartDate >= command.StartDateFrom!.Value, command.StartDateFrom.HasValue)
            .Where(x => x.StartDate <= command.StartDateTo!.Value, command.StartDateTo.HasValue)
            .Where(x => x.IsMandatory == command.IsMandatory!.Value, command.IsMandatory.HasValue)
            .Where(x => x.CertificateIssued == command.CertificateIssued!.Value, command.CertificateIssued.HasValue);
}
