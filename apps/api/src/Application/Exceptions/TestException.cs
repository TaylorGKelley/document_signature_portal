using System.Net;

namespace DocSign.Application.Exceptions;

public class TestException(string message)
    : ApiException(message, "An unknown test erorr occured", HttpStatusCode.BadRequest) { }
