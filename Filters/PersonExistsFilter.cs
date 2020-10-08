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
            var args = context.ActionArguments;
            using IDbConnection database = new SqlConnection(MyConnectionString);

            if (args.Count == 1)
            {
                var person = (PersonDto) args["personDto"];
                const string sql = "SELECT * FROM SkiTickets.Age WHERE type = @type";
                var age = database.QueryFirstOrDefault<AgeDao>(sql, new {type = person.Age});
                const string firstSql = "SELECT * FROM SkiTickets.Person WHERE firstName = @firstName AND lastName = @lastName AND ageId = @ageId";
                var firstPerson = database.QueryFirstOrDefault<PersonDao>(firstSql, new {firstName = person.FirstName, lastName = person.LastName, ageId = age.Id});

                if (firstPerson != null)
                {
                    throw new PersonBadRequestException("Person already exists!");
                }
            }
            else
            {
                var personId = (int) args["id"];
                const string secondSql = "SELECT * FROM SkiTickets.Person WHERE id = @id";
                var secondPerson = database.QueryFirstOrDefault<PersonDao>(secondSql, new {id = personId});

                if (secondPerson == null)
                {
                    throw new PersonNotFoundException("Person does not exist!");
                }
            }
            
            
            base.OnActionExecuting(context);
        }
    }
}