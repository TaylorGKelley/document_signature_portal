using System.Net;

namespace DocSign.Application.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode Status { get; set; }
    public string Title { get; set; }
    public IEnumerable<string>? Errors { get; set; } = null;

	protected ApiException(string message, string title, HttpStatusCode status, IEnumerable<string>? errors = null) : base(message) {
        Status = status;
        Title = title;
        Errors = errors;
	}
}
