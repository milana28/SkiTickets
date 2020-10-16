using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SkiTickets.Utils.Responses
{
    public class ErrorResponse
    {
        public string Type { get; }
        public string Title { get; }
        public HttpStatusCode Status { get; }
        public string TraceId { get; }
        public object Errors { get; }

        public ErrorResponse(string message, IEnumerable<string> attributes)
        {
            var httpContext = new ActionContext();
            var errors = CreateErrorList.GetErrors(attributes, message);

            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            Title = message;
            Status = HttpStatusCode.BadRequest;
            TraceId = Activity.Current?.Id ?? httpContext?.HttpContext.TraceIdentifier;
            Errors = CreateErrorList.MapErrorObject(errors);
        }
    }
}