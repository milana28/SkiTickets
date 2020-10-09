using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class TicketTypeWithTypeExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        private TicketDto _ticket = new TicketDto();
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ticketType = (string) context.ActionArguments["ticketType"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            const string sql = "SELECT * FROM SkiTickets.TicketType WHERE type = @type";
            var ticketTypeDao = database.QueryFirstOrDefault<AgeDao>(sql, new {type = ticketType});

            if (ticketType == null)
            {
                throw new TicketTypeNotFoundException("TicketType does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}