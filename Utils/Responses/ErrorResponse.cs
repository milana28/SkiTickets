
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SkiTickets.Utils.Responses
{
    public class ErrorResponse
    {
        public Error Error { get; }

        public ErrorResponse(string message)
        {
            var httpContext = new ActionContext();
            var traceId = Activity.Current?.Id ?? httpContext?.HttpContext.TraceIdentifier;
            var error = new Error
            {
                Title = message,
                Status = HttpStatusCode.BadRequest,
                TraceId =  traceId,
            };
            
            Error = error;
        }
    }
}