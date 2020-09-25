using System.Data;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface ITicketType
    {
        Models.TicketType GetTicketTypeByAge(string age);
        Models.TicketType GetTicketTypeByType(string type);
        Models.TicketType GetTicketTypeById(int id);
        Models.TicketType GetTicketTypeByTypeAndAge(string type, string ageType);
    }
    
    public class TicketType : ITicketType
    {
        private readonly IDbConnection _database;
        private readonly IAge _age;

        public TicketType(IDatabase database, IAge age)
        {
            _database = database.Get();
            _age = age;
        }

        public Models.TicketType GetTicketTypeByAge(string age)
        {
            var ageId = _age.GetAgeByType(age);
            const string sql = "SELECT * FROM SkiTickets.TicketType WHERE ageId = @ageId";

            return TransformDaoToBusinessLogicTicketType(_database.QueryFirstOrDefault<TicketTypeDao>(sql, new {ageId = ageId}));
        }
        public Models.TicketType GetTicketTypeById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.TicketType WHERE id = @ticketTypeId";
            return TransformDaoToBusinessLogicTicketType(_database.QueryFirstOrDefault<TicketTypeDao>(sql, new {ticketTypeId = id}));
        }
        public Models.TicketType GetTicketTypeByType(string type)
        {
            const string sql = "SELECT * FROM SkiTickets.TicketType WHERE type = @ticketType";
            return TransformDaoToBusinessLogicTicketType(_database.QueryFirstOrDefault<TicketTypeDao>(sql, new {ticketType = type}));
        }
        public Models.TicketType GetTicketTypeByTypeAndAge(string type, string ageType)
        {
            var age = _age.GetAgeByType(ageType);
            const string sql = "SELECT * FROM SkiTickets.TicketType WHERE type = @ticketType AND ageId = @ageId";
            return TransformDaoToBusinessLogicTicketType(_database.QueryFirstOrDefault<TicketTypeDao>(sql, new {ticketType = type, ageId = age.Id}));
        }
        private Models.TicketType TransformDaoToBusinessLogicTicketType(TicketTypeDao ticketTypeDao)
        {
            var age = _age.GetAgeById(ticketTypeDao.AgeId);
            
            return new Models.TicketType()
            {
                Id = ticketTypeDao.Id,
                Name = ticketTypeDao.Name,
                Type = ticketTypeDao.Type,
                Days = ticketTypeDao.Days,
                Age = age.Type
            };
        }
        
    }
}