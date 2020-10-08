using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class PersonFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var args = context.ActionArguments;
            using IDbConnection database = new SqlConnection(MyConnectionString);
            var person = (PersonDto) args["personDto"];
            const string ageSql = "SELECT * FROM SkiTickets.Age WHERE type = @type";
            var age = database.QueryFirstOrDefault<AgeDao>(ageSql, new {type = person.Age});

            if (args.Count == 2)
            {
                var id = (int) args["id"];
                const string sql = "SELECT * FROM SkiTickets.Person WHERE id != @id AND firstName = @firstName AND lastName = @lastName AND ageId = @ageId";
                var personDao = database.QueryFirstOrDefault<PersonDao>(sql, new {id = id, firstName = person.FirstName, lastName = person.LastName, ageId = age.Id});

                if (personDao != null)
                {
                    throw new PersonBadRequestException("Person already exists!");
                }
            }
            else
            {
                const string personSql = "SELECT * FROM SkiTickets.Person WHERE firstName = @firstName AND lastName = @lastName AND ageId = @ageId";
                var personDao = database.QueryFirstOrDefault<PersonDao>(personSql, new {firstName = person.FirstName, lastName = person.LastName, ageId = age.Id});

                if (personDao != null)
                {
                    throw new PersonBadRequestException("Person already exists!");
                }
            }
            
            
            base.OnActionExecuting(context);
        }
    }
}