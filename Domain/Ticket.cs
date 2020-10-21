using System;
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
        List<Models.Ticket> GetTickets(string? age, DateTime? from, DateTime? to);
        Models.Ticket GetTicketById(int id);
        Models.Ticket DeleteTicket(int id);
        Models.Ticket UpdateTicket(int id, TicketDto ticketDto);
    }
    
    public class Ticket : ITicket
    {
        private readonly IDbConnection _database;
        private readonly ITicketType _ticketType;
        private readonly IAge _age;

        public Ticket(IDatabase database, ITicketType ticketType, IAge age)
        {
            _database = database.Get();
            _ticketType = ticketType;
            _age = age;
        }

        public Models.Ticket CreateTicket(TicketDto ticketDto)
        {
            var ticketType = _ticketType.GetTicketTypeByTypeAndAge(ticketDto.TicketType, ticketDto.Age);
            const string sql =
                "INSERT INTO SkiTickets.Ticket VALUES (@ticketTypeId, @price, @fromDate, @toDate)" + 
                " SELECT * FROM SkiTickets.Ticket WHERE id = SCOPE_IDENTITY()";

            return TransformDaoToBusinessLogicTicket(_database.QueryFirstOrDefault<TicketDao>(sql, new
            {
                ticketTypeId = ticketType.Id,
                price = ticketDto.Price,
                fromDate = ticketDto.FromDate,
                toDate = ticketDto.ToDate
            }));
        }
        public List<Models.Ticket> GetTickets(string? age, DateTime? from, DateTime? to)
        {
            if (from != null && to != null)
            {
                return GetTicketsWithinDate(from, to); 
            }

            return age == null ? GetAll() : GetTicketsByAge(age);
        }
        public Models.Ticket GetTicketById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.Ticket WHERE id = @ticketId";
            return TransformDaoToBusinessLogicTicket(
                _database.QueryFirstOrDefault<TicketDao>(sql, new {ticketId = id}));
        }
        public Models.Ticket DeleteTicket(int id)
        {
            var ticket = GetTicketById(id);
            const string sql = "DELETE FROM SkiTickets.Ticket WHERE id = @ticketId";
            _database.Execute(sql, new {ticketId = id});

            return ticket;
        }
        public Models.Ticket UpdateTicket(int id, TicketDto ticketDto)
        {
            var ticketTypeId = _ticketType.GetTicketTypeByTypeAndAge(ticketDto.TicketType, ticketDto.Age).Id;
            
            const string sql =
                "UPDATE SkiTickets.Ticket SET ticketTypeId = @ticketTypeId, price = @price, fromDate = @fromDate, toDate = @toDate WHERE id = @id";
            _database.Execute(sql, new
            {
                id = id,
                ticketTypeId = ticketTypeId,
                price = ticketDto.Price,
                fromDate = ticketDto.FromDate,
                toDate = ticketDto.ToDate
            });

            return GetTicketById(id);
        }
        private List<Models.Ticket> GetAll()
        {
            var ticketList = new List<Models.Ticket>();
            var ticketDaoList = _database.Query<TicketDao>("SELECT * FROM SkiTickets.Ticket").ToList();
            ticketDaoList.ForEach(t => ticketList.Add(TransformDaoToBusinessLogicTicket(t)));

            return ticketList;
        }
        private List<Models.Ticket> GetTicketsByAge(string? age)
        {
            var ageId = _age.GetAgeByType(age).Id;
            var ticketList = new List<Models.Ticket>();
            const string sql = 
                "SELECT t.* FROM SkiTickets.Ticket as t LEFT JOIN SkiTickets.TicketType as tt ON t.ticketTypeId = tt.id WHERE tt.ageId = @ageId";
            var ticketDaoList = _database.Query<TicketDao>(sql, new {ageId = ageId}).ToList();
            ticketDaoList.ForEach(t => ticketList.Add(TransformDaoToBusinessLogicTicket(t)));

            return ticketList;
        }
        private List<Models.Ticket> GetTicketsWithinDate(DateTime? fromDate, DateTime? toDate)
        {
            var ticketList = new List<Models.Ticket>();
            const string sql = 
                "SELECT * FROM SkiTickets.Ticket WHERE fromDate >= @fromDate AND toDate <= @toDate";
            var ticketDaoList = _database.Query<TicketDao>(sql, new {fromDate = fromDate, toDate = toDate}).ToList();
            ticketDaoList.ForEach(t => ticketList.Add(TransformDaoToBusinessLogicTicket(t)));

            return ticketList;
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