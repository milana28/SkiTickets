using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface IPerson
    { 
        List<Models.Person> GetAll();
        Models.Person GetPersonById(int id);
        Models.Person CreatePerson(PersonDto personDto);
        Models.Person DeletePerson(int id);
        Models.Person TransformDaoToBusinessLogicPerson(PersonDao personDao);
        Models.Person UpdatePerson(int id, PersonDto personDto);
    }
    
    public class Person : IPerson
    {
        private readonly IDbConnection _database;

        public Person(IDatabase database)
        {
            _database = database.Get();
        }

        public Models.Person CreatePerson(PersonDto personDto)
        {
            var person = new PersonDao()
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                AgeId = personDto.AgeId
            };

            const string sql =
                "INSERT INTO SkiTickets.Person VALUES (@firstName, @lastName, @ageId) SELECT * FROM SkiTickets.Person WHERE id = SCOPE_IDENTITY()";

            return TransformDaoToBusinessLogicPerson(_database.QueryFirst<PersonDao>(sql, person));
        }
        public List<Models.Person> GetAll()
        {
            var personList = new List<Models.Person>();
            var personDaoList = _database.Query<PersonDao>("SELECT * FROM SkiTickets.Person").ToList();
            personDaoList.ForEach(p => personList.Add(TransformDaoToBusinessLogicPerson(p)));
            
            return personList;
        }
        public Models.Person GetPersonById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.Person WHERE id = @personId";
            return TransformDaoToBusinessLogicPerson(_database.QueryFirstOrDefault<PersonDao>(sql, new {personId = id}));
        }
        public Models.Person DeletePerson(int id)
        {
            var person = GetPersonById(id);
            const string sql = "DELETE FROM SkiTickets.Person WHERE id = @personId";
            _database.Execute(sql, new {personId = id});

            return person;
        }
        public Models.Person UpdatePerson(int id, PersonDto personDto)
        {
            var newPerson = new PersonDao()
            {
                Id = id,
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                AgeId = personDto.AgeId
            };

            const string sql =
                "UPDATE SkiTickets.Person SET firstName = @firstName, lastName = @lastName, ageId = @ageId WHERE id = @id";
            _database.Execute(sql, newPerson);

            return GetPersonById(id);
        }
        public Models.Person TransformDaoToBusinessLogicPerson(PersonDao personDao)
        {
            const string sql = "SELECT * FROM SkiTickets.Age WHERE id = @ageId";
            var age = _database.QuerySingle<Models.Age>(sql, new {ageId = personDao.AgeId});

            return new Models.Person()
            {
                Id = personDao.Id,
                FirstName = personDao.FirstName,
                LastName = personDao.LastName,
                Age = age.Type
            };
        }
    }
}