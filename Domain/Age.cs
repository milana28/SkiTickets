using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface IAge
    {
        List<Models.Age> GetAll();
        Models.Age GetAgeById(int id);
        Models.Age DeleteAge(int id);
        Models.Age UpdateAge(int id, AgeDto ageDto);
    }
    
    public class Age : IAge
    {
        private readonly IDbConnection _database;

        public Age(IDatabase database)
        {
            _database = database.Get();
        }
        
         public List<Models.Age> GetAll()
        {
            return _database.Query<Models.Age>("SELECT * FROM SkiTickets.Age").ToList();
        }
        public Models.Age GetAgeById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.Age WHERE id = @ageId";
            return _database.QueryFirstOrDefault<Models.Age>(sql, new {ageId = id});
        }
        public Models.Age DeleteAge(int id)
        {
            var age = GetAgeById(id);
            const string sql = "DELETE FROM SkiTickets.Age WHERE id = @ageId";
            _database.Execute(sql, new {ageId = id});

            return age;
        }
        public Models.Age UpdateAge(int id, AgeDto ageDto)
        {
            var newAge = new Models.Age()
            {
                Id = id,
                Type = ageDto.Type,
                MinYears = ageDto.MinYears,
                MaxYears = ageDto.MaxYears
            };

            const string sql =
                "UPDATE SkiTickets.Age SET type = @type, minYears = @minYears, maxYears = @maxYears WHERE id = @id";
            _database.Execute(sql, newAge);

            return GetAgeById(id);
        }
    }
}