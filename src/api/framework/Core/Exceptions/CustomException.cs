using System.Net;

namespace FSH.Framework.Core.Exceptions;

public class CustomException(
    string message,
    List<string>? errors = null,
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : Exception(message)
{
    public List<string>? ErrorMessages { get; } = errors;

    public HttpStatusCode StatusCode { get; } = statusCode;
}
