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
        private PersonDao _person = new PersonDao();
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _person = (PersonDao) context.ActionArguments["personDao"];
            var ageId = _person.AgeId;
           
            using IDbConnection database = new SqlConnection(MyConnectionString);
            const string sql = "SELECT * FROM SkiTickets.Age WHERE id = @id";
            var age = database.QueryFirstOrDefault<PersonDao>(sql, new {id = ageId});

            if (age == null)
            {
                throw new AgeNotFoundException("Age does not exists!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}