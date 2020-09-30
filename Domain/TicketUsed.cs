using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface ITicketUsed
    {
        Models.TicketUsed CheckTicket(int ticketPurchaseId);
    }
    
    public class TicketUsed : ITicketUsed
    {
        private readonly IDbConnection _database;
        private readonly ITicketPurchase _ticketPurchase;

        public TicketUsed(IDatabase database, ITicketPurchase ticketPurchase)
        {
            _database = database.Get();
            _ticketPurchase = ticketPurchase;
        }

        public Models.TicketUsed CheckTicket(int ticketPurchaseId)
        {
            const string sql = "INSERT INTO SkiTickets.TicketUsed VALUES (@ticketPurchaseId, @time) SELECT * FROM SkiTickets.TicketUsed WHERE id = SCOPE_IDENTITY()";
            return TransformDaoToBusinessLogicTicketUsed(_database.QueryFirstOrDefault<TicketUsedDao>(sql,
                new {ticketPurchaseId = ticketPurchaseId, time = DateTime.Now}));
        }
        private Models.TicketUsed TransformDaoToBusinessLogicTicketUsed(TicketUsedDao ticketUsed)
        {
            var ticketPurchase = _ticketPurchase.GetTicketPurchaseById(ticketUsed.TicketPurchaseId);
            var ticket = ticketPurchase.Ticket;

            return new Models.TicketUsed()
            {
                Id = ticketUsed.Id,
                Ticket = ticket,
                Time = ticketUsed.Time
            };
        }
    }
}