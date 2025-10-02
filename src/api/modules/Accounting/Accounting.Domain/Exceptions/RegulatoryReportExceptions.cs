// RegulatoryReportExceptions.cs
// Per-domain exceptions for RegulatoryReport aggregate

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class RegulatoryReportNotFoundException(DefaultIdType id) : NotFoundException($"regulatory report with id {id} not found");
public sealed class RegulatoryReportForbiddenException(string message) : ForbiddenException(message);
public sealed class RegulatoryReportInvalidException(string message) : ForbiddenException(message);
public sealed class RegulatoryReportAlreadySubmittedException(DefaultIdType id) : ForbiddenException($"regulatory report with id {id} is already submitted");
public sealed class RegulatoryReportNotInReviewException(DefaultIdType id) : ForbiddenException($"regulatory report with id {id} must be in review to perform this operation");
public sealed class RegulatoryReportCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"regulatory report with id {id} cannot be modified in its current state");
public sealed class RegulatoryReportSubmissionDeadlineExceededException(DefaultIdType id) : ForbiddenException($"cannot submit regulatory report with id {id} after the submission deadline");
public sealed class RegulatoryReportAlreadyApprovedException(DefaultIdType id) : ForbiddenException($"regulatory report with id {id} is already approved");
public sealed class RegulatoryReportNotSubmittedException(DefaultIdType id) : ForbiddenException($"regulatory report with id {id} has not been submitted");