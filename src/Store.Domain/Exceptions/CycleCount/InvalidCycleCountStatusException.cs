using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.CycleCount;

public sealed class InvalidCycleCountStatusException(string status)
    : ForbiddenException($"Invalid cycle count status: '{status}'. Valid statuses are: Scheduled, InProgress, Completed, Cancelled.") {}

