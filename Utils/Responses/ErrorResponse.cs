using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SkiTickets.Utils.Responses
{
    public class ErrorResponse
    {
        public Error Error { set; get; }
        private string Message { set; get; }

        public ErrorResponse(string message)
        {
            var error = new Error {Title = message, Status = 400};
            Error = error;
        }
    }
}