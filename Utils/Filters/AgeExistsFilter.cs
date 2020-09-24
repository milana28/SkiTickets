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
    public sealed class AgeExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        private int _ageId;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _ageId = (int) context.ActionArguments["id"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            const string sql = "SELECT * FROM SkiTickets.Age WHERE id = @id";
            var age = database.QueryFirstOrDefault<AgeDao>(sql, new {id = _ageId});

            if (age == null)
            {
                throw new AgeNotFoundException("Age does not exists!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}
