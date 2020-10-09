using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class TicketPurchaseExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ticketPurchaseId = (int) context.ActionArguments["id"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            const string sql = "SELECT * FROM SkiTickets.TicketPurchase WHERE id = @id";
            var ticketPurchase = database.QueryFirstOrDefault<TicketPurchaseDao>(sql, new {id = ticketPurchaseId});

            if (ticketPurchase == null)
            {
                throw new TicketPurchaseNotFoundException("TicketPurchase does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}