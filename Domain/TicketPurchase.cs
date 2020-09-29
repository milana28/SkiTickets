using System;
using System.Data;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface ITicketPurchase
    {
        Models.TicketPurchase CreateTicketPurchase(TicketPurchaseDto ticketPurchaseDto);
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