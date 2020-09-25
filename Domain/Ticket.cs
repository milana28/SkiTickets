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
        Models.Ticket UpdateTicket(int id, TicketDto ticketDto);
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
            var ticketType = _ticketType.GetTicketTypeByTypeAndAge(ticketDto.TicketType, ticketDto.Age);
            const string sql =
                "INSERT INTO SkiTickets.Ticket VALUES (@ticketTypeId, @price, @fromDate, @toDate) SELECT * FROM SkiTickets.Ticket WHERE id = SCOPE_IDENTITY()";

            return TransformDaoToBusinessLogicTicket(_database.QueryFirstOrDefault<TicketDao>(sql, new
            {
                ticketTypeId = ticketType.Id,
                price = ticketDto.Price,
                fromDate = ticketDto.FromDate,
                toDate = ticketDto.ToDate
            }));
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
        public Models.Ticket UpdateTicket(int id, TicketDto ticketDto)
        {
            var ticketTypeId = _ticketType.GetTicketTypeByTypeAndAge(ticketDto.TicketType, ticketDto.Age).Id;
            
            const string sql =
                "UPDATE SkiTickets.Ticket SET ticketTypeId = @ticketTypeId, price = @price, fromDate = @fromDate, toDate = @toDate WHERE id = @id";
            _database.Execute(sql, new
            {
                ticketTypeId = ticketTypeId,
                price = ticketDto.Price,
                fromDate = ticketDto.FromDate,
                toDate = ticketDto.ToDate
            });

            return GetTicketById(id);
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