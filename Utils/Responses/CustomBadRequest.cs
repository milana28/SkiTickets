using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            ConstructErrorMessages(context);
        }
        
        private void ConstructErrorMessages(ActionContext context)
        {
            foreach (var keyModelStatePair in context.ModelState)
            {
                const string key = "title";
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = GetErrorMessage(errors[0]);
                        Errors.Add(key, new[] { errorMessage });
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = GetErrorMessage(errors[i]);
                        }
                        Errors.Add(key, errorMessages);
                    }
                }
            }
        }

        private static string GetErrorMessage(ModelError error)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ? "error" :
            error.ErrorMessage;
        }
    }
}