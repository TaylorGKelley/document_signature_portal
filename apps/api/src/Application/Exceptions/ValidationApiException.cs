using System.Net;

namespace DocSign.Application.Exceptions;

public class ValidationApiException(string message, Dictionary<string, string[]> fieldErrors) : ApiException(message, "One or more validation errors occurred.", HttpStatusCode.BadRequest)
{
    public readonly Dictionary<string, string[]> FieldErrors = fieldErrors;
}
