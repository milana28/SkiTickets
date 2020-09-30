using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface ITicketPurchase
    {
        Models.TicketPurchase CreateTicketPurchase(TicketPurchaseDto ticketPurchaseDto);
        List<Models.TicketPurchase> GetAll();
        Models.TicketPurchase GetTicketPurchaseById(int id);
        List<Models.TicketPurchase> GetTicketPurchasesByTicketType(string type);
        List<Models.TicketPurchase> GetTicketPurchasesBySellingPoint(int sellingPointId);
    }
    
    public class TicketPurchase : ITicketPurchase
    {
        private readonly IDbConnection _database;
        private readonly IPerson _person;
        private readonly ITicket _ticket;
        private readonly ISellingPoint _sellingPoint;

        public TicketPurchase(IDatabase database, IPerson person, ITicket ticket, ISellingPoint sellingPoint)
        {
            _database = database.Get();
            _person = person;
            _ticket = ticket;
            _sellingPoint = sellingPoint;
        }
        
        public Models.TicketPurchase CreateTicketPurchase(TicketPurchaseDto ticketPurchaseDto)
        {
            var person = _person.GetPersonByFirstNameLastNameAndAge(ticketPurchaseDto.FirstName,
                ticketPurchaseDto.LastName, ticketPurchaseDto.Age);
            var personId = new int();

            if (person == null)
            {
                var personDto = new PersonDto()
                {
                    FirstName = ticketPurchaseDto.FirstName,
                    LastName = ticketPurchaseDto.LastName,
                    Age = ticketPurchaseDto.Age
                };
                var newPerson = _person.CreatePerson(personDto);
                personId = newPerson.Id;
            }
            
            if (person != null)
            {
                personId = person.Id;
            }

            const string sql =
                "INSERT INTO SkiTickets.TicketPurchase VALUES (@ticketId, @personId, @sellingPointId, @date)" +
                 "SELECT * FROM SkiTickets.TicketPurchase WHERE id = SCOPE_IDENTITY()";

            return TransformDaoToBusinessLogicTicketPurchase(_database.QueryFirstOrDefault<TicketPurchaseDao>(sql, new
            {
                ticketId = ticketPurchaseDto.TicketId,
                personId = personId,
                sellingPointId = ticketPurchaseDto.SellingPointId,
                date = DateTime.Now
            }));
        }
        public List<Models.TicketPurchase> GetAll()
        {
            var ticketPurchaseList = new List<Models.TicketPurchase>();
            var ticketPurchaseDaoList = _database.Query<TicketPurchaseDao>("SELECT * FROM SkiTickets.TicketPurchase").ToList();
            ticketPurchaseDaoList.ForEach(t => ticketPurchaseList.Add(TransformDaoToBusinessLogicTicketPurchase(t)));

            return ticketPurchaseList;
        }
        public Models.TicketPurchase GetTicketPurchaseById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.TicketPurchase WHERE id = @ticketPurchaseId";
            return TransformDaoToBusinessLogicTicketPurchase(
                _database.QueryFirstOrDefault<TicketPurchaseDao>(sql, new {ticketPurchaseId = id}));
        }
        public List<Models.TicketPurchase> GetTicketPurchasesBySellingPoint(int sellingPointId)
        {
            var ticketPurchaseList = new List<Models.TicketPurchase>();
            const string sql = "SELECT * FROM SkiTickets.TicketPurchase WHERE sellingPointId = @id";
            var ticketPurchaseDaoList = _database.Query<TicketPurchaseDao>(sql, new {id = sellingPointId}).ToList();
            ticketPurchaseDaoList.ForEach(t => ticketPurchaseList.Add(TransformDaoToBusinessLogicTicketPurchase(t)));

            return ticketPurchaseList;
        }
        public List<Models.TicketPurchase> GetTicketPurchasesByTicketType(string type)
        {
            var ticketPurchaseList = new List<Models.TicketPurchase>();
            const string sql =
                "SELECT tp.* FROM SkiTickets.TicketPurchase as tp LEFT JOIN SkiTickets.Ticket as t ON tp.ticketId = t.Id LEFT JOIN SkiTickets.TicketType as tt ON t.ticketTypeId = tt.Id WHERE tt.[type] = @type";
            var ticketPurchaseDaoList = _database.Query<TicketPurchaseDao>(sql, new {type = type}).ToList();
            ticketPurchaseDaoList.ForEach(t => ticketPurchaseList.Add(TransformDaoToBusinessLogicTicketPurchase(t)));

            return ticketPurchaseList;
        }
        private Models.TicketPurchase TransformDaoToBusinessLogicTicketPurchase(TicketPurchaseDao ticketPurchaseDao)
        {
            var person = _person.GetPersonById(ticketPurchaseDao.PersonId);
            var ticket = _ticket.GetTicketById(ticketPurchaseDao.TicketId);
            var sellingPoint = _sellingPoint.GetSellingPointById(ticketPurchaseDao.SellingPointId);
            
            return new Models.TicketPurchase()
            {
                Id = ticketPurchaseDao.Id,
                Ticket = ticket,
                Person = person,
                SellingPoint = sellingPoint,
                Date = ticketPurchaseDao.Date
            };
        }
    }
}