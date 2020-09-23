using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class PersonExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var personId = (int) context.ActionArguments["id"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            const string sql = "SELECT * FROM SkiTickets.Person WHERE id = @id";
            var person = database.QueryFirstOrDefault<PersonDao>(sql, new {id = personId});

            if (person == null)
            {
                throw new PersonNotFoundException("Person does not exists!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}