using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Filters
{
    public class MinYearsSmallerThanMaxYearsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var age = (AgeDto) context.ActionArguments["ageDto"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            if (age.MinYears >= age.MaxYears)
            {
                throw new AgeBadRequestException("MinYears must be smaller than maxYears!");
            }

            base.OnActionExecuting(context);
        }
    }
}