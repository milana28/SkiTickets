using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface ITicket
    {
        Models.Ticket CreateTicket(TicketDto ticketDto);
        Models.Ticket GetTicketById(int id);
        List<Models.Ticket> GetAll();
    }
    
    public class Ticket : ITicket
    {
        private readonly IDbConnection _database;
        private readonly ITicketType _ticketType;

        public Ticket(IDatabase database, ITicketType ticketType)
        {
            _database = database.Get();
            _ticketType = ticketType;
        }

        public Models.Ticket CreateTicket(TicketDto ticketDto)
        {
            var ticketTypeId = _ticketType.GetTicketTypeByType(ticketDto.TicketType);
            const string sql =
                "INSERT INTO SkiTickets.Ticket VALUES (@ticketTypeId, @price, @fromDate, @toDate) SELECT * FROM SkiTickets.Ticket WHERE id = SCOPE_IDENTITY()";

            return TransformDaoToBusinessLogicTicket(_database.QueryFirstOrDefault<TicketDao>(sql, new {ticketTypeId = ticketTypeId}));
        }
        public List<Models.Ticket> GetAll()
        {
            var ticketList = new List<Models.Ticket>();
            var ticketDaoList = _database.Query<TicketDao>("SELECT * FROM SkiTickets.Ticket").ToList();
            ticketDaoList.ForEach(t => ticketList.Add(TransformDaoToBusinessLogicTicket(t)));

            return ticketList;
        }
        public Models.Ticket GetTicketById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.Ticket WHERE id = @ticketId";
            return TransformDaoToBusinessLogicTicket(
                _database.QueryFirstOrDefault<TicketDao>(sql, new {ticketId = id}));
        }
        private Models.Ticket TransformDaoToBusinessLogicTicket(TicketDao ticketDao)
        {
            var ticketType = _ticketType.GetTicketTypeById(ticketDao.TicketTypeId);
            
            return new Models.Ticket()
            {
                Id = ticketDao.Id,
                TicketType = ticketType,
                Price = ticketDao.Price,
                FromDate = ticketDao.FromDate,
                ToDate = ticketDao.ToDate
            };
        }
    }
}