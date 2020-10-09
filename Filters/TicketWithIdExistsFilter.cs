using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class TicketWithIdExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ticketId = (int) context.ActionArguments["id"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            const string sql = "SELECT * FROM SkiTickets.Ticket WHERE id = @id";
            var ticket = database.QueryFirstOrDefault<TicketDao>(sql, new {id = ticketId});

            if (ticket == null)
            {
                throw new TicketNotFoundException("Ticket does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}