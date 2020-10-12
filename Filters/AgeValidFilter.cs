using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public sealed class AgeValidFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        public string Info;
        public object TypeOfObject;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var model = TypeOfObject;
            Console.WriteLine(model);

            var myObject = (TicketDto) context.ActionArguments[Info];
            var ageType = myObject.Age;
           
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            const string sql = "SELECT * FROM SkiTickets.Age WHERE type = @type";
            var age = database.QueryFirstOrDefault<AgeDao>(sql, new {type = ageType});

            if (age == null)
            {
                throw new AgeNotFoundException("Age does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }

}