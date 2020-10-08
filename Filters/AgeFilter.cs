using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class AgeFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var args = context.ActionArguments;
            var age = (AgeDto) args["ageDto"];
            using IDbConnection database = new SqlConnection(MyConnectionString);

            if (args.Count == 2)
            {
                var id = (int) args["id"];
                const string firstSql = "SELECT * FROM SkiTickets.Age WHERE id != @id  AND (type = @type OR (@min >= minYears AND @min <= maxYears) OR (@max >= minYears AND @max <= maxYears))";
                var firstAge = database.QueryFirstOrDefault<AgeDao>(firstSql, new {id = id, type = age.Type, min = age.MinYears, max = age.MaxYears});

                if (firstAge != null)
                {
                    throw new AgeBadRequestException("Age already exists!");
                }
            }
            else
            {
                const string secondSql = "SELECT * FROM SkiTickets.Age WHERE type = @type OR (@min >= minYears AND @min <= maxYears) OR (@max >= minYears AND @max <= maxYears)";
                var secondAge = database.QueryFirstOrDefault<AgeDao>(secondSql, new {type = age.Type, min = age.MinYears, max = age.MaxYears});

                if (age.MinYears >= age.MaxYears)
                {
                    throw new AgeBadRequestException("MinYears must be smaller than maxYears!");
                }
            
                if (secondAge != null)
                {
                    throw new AgeBadRequestException("Age already exists!");
                }
            }
            
            base.OnActionExecuting(context);
        }
    }
}