using System;
using System.Net;

namespace AbleToTrack.Model.Exceptions;

public class HttpResponseException : Exception
{
    public HttpStatusCode HttpStatusCode;

    public HttpResponseException(HttpStatusCode statusCode, string message) : base(message)
    {
        HttpStatusCode = statusCode;
    }
}