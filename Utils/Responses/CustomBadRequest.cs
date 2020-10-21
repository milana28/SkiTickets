using Microsoft.AspNetCore.Mvc;

namespace SkiTickets.Utils.Responses
{
    public class CustomBadRequest : ValidationProblemDetails
    {
        public CustomBadRequest(ActionContext context)
        {
            Status = 400;
            Title = "Invalid arguments";
            Detail = "The inputs supplied to the API are invalid";
            Type = context.HttpContext.TraceIdentifier;
        }
    }
}