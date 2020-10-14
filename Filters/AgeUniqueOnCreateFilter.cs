using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Filters
{
    public class AgeUniqueOnCreateFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var age = (AgeDto) context.ActionArguments["ageDto"];
            using IDbConnection database = new SqlConnection(MyConnectionString);

            const string sql =
                "SELECT * FROM SkiTickets.Age WHERE type = @type OR (@min >= minYears AND @min <= maxYears) OR (@max >= minYears AND @max <= maxYears)";
            var ageDao =
                database.QueryFirstOrDefault<AgeDao>(sql,
                    new {type = age.Type, min = age.MinYears, max = age.MaxYears});

            if (ageDao != null)
            {
                throw new AgeBadRequestException("Age already exists!");
            }

            base.OnActionExecuting(context);
        }
    }
}