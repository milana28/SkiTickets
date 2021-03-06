using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Filters
{
    public class PersonUniqueOnCreateFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var person = (PersonDto) context.ActionArguments["personDto"];
            using IDbConnection database = new SqlConnection(MyConnectionString);

            const string ageSql = "SELECT * FROM SkiTickets.Age WHERE type = @type";
            var age = database.QueryFirstOrDefault<AgeDao>(ageSql, new {type = person.Age});

            const string sql =
                "SELECT * FROM SkiTickets.Person WHERE firstName = @firstName AND lastName = @lastName AND ageId = @ageId";
            var personDao = database.QueryFirstOrDefault<PersonDao>(sql,
                new {firstName = person.FirstName, lastName = person.LastName, ageId = age.Id});

            if (personDao != null)
            {
                throw new PersonBadRequestException("Person already exists!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}