using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class AgeWithIdExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ageId = (int) context.ActionArguments["id"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            const string sql = "SELECT * FROM SkiTickets.Age WHERE id = @id";
            var age = database.QueryFirstOrDefault<AgeDao>(sql, new {id = ageId});

            if (age == null)
            {
                throw new AgeNotFoundException("Age does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}
