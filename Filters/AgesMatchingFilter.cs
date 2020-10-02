using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class AgesMatchingFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        private TicketPurchaseDto _ticketPurchaseDto = new TicketPurchaseDto();
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _ticketPurchaseDto = (TicketPurchaseDto) context.ActionArguments["ticketPurchaseDto"];
            var age = _ticketPurchaseDto.Age;
            using IDbConnection database = new SqlConnection(MyConnectionString);
            const string ageSql = "SELECT * FROM SkiTickets.Age WHERE type = @ageType";
            var ageId =  database.QueryFirstOrDefault<AgeDao>(ageSql, new {ageType = age}).Id;
            const string ticketSql = "SELECT * FROM SkiTickets.Ticket WHERE id = @ticketId";
            var ticket =  database.QueryFirstOrDefault<TicketDao>(ticketSql, new {ticketId = _ticketPurchaseDto.TicketId});
            const string ticketTypeSql = "SELECT * FROM SkiTickets.TicketType WHERE id = @ticketTypeId";
            var ticketType = database.QueryFirstOrDefault<TicketTypeDao>(ticketTypeSql, new {ticketTypeId = ticket.TicketTypeId});

            if (ticketType.AgeId != ageId)
            {
                throw new AgesNotMatchingException("Ticket is not for that age!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}