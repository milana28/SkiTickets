using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Filters
{
    public class TicketTypeInTicketIsValidFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
      
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ticket = (TicketDto) context.ActionArguments["ticketDto"];
            var type = ticket.TicketType;
           
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            const string sql = "SELECT * FROM SkiTickets.TicketType WHERE type = @type";
            var ticketType = database.QueryFirstOrDefault<AgeDao>(sql, new {type = type});

            if (ticketType == null)
            {
                throw new TicketTypeNotFoundException("TicketType does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}