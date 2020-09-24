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
            var ageList = new List<Models.Age>();
            var ageDaoList = _database.Query<AgeDao>("SELECT * FROM SkiTickets.Age").ToList();
            ageDaoList.ForEach(a => ageList.Add(TransformDaoToBusinessLogicAge(a)));

            return ageList;
        }
        public Models.Age GetAgeById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.Age WHERE id = @ageId";
            return TransformDaoToBusinessLogicAge(_database.QueryFirstOrDefault<AgeDao>(sql, new {ageId = id}));
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
        private static Models.Age TransformDaoToBusinessLogicAge(AgeDao ageDao)
        {
            return new Models.Age()
            {
                Id = ageDao.Id,
                Type = ageDao.Type,
                MinYears = ageDao.MinYears,
                MaxYears = ageDao.MaxYears
            };
        }
    }
}