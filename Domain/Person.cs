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
        Models.Person GetPersonByFirstNameLastNameAndAge(string firstName, string lastName, string age);
        Models.Person UpdatePerson(int id, PersonDto personDto);
    }
    
    public class Person : IPerson
    {
        private readonly IDbConnection _database;
        private readonly IAge _age;

        public Person(IDatabase database, IAge age)
        {
            _database = database.Get();
            _age = age;
        }

        public Models.Person CreatePerson(PersonDto personDto)
        {
            var ageId = _age.GetAgeByType(personDto.Age).Id;

            const string sql =
                "INSERT INTO SkiTickets.Person VALUES (@firstName, @lastName, @ageId) SELECT * FROM SkiTickets.Person WHERE id = SCOPE_IDENTITY()";

            return TransformDaoToBusinessLogicPerson(_database.QueryFirst<PersonDao>(sql, new
            {
                firstName = personDto.FirstName, 
                lastName = personDto.LastName, 
                ageId = ageId
            }));
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
        public Models.Person GetPersonByFirstNameLastNameAndAge(string firstName, string lastName, string age)
        {
            var ageId = _age.GetAgeByType(age).Id;
            const string sql = "SELECT * FROM SkiTickets.Person WHERE firstName = @firstName AND lastName = @lastName AND ageId = @ageId";
            var personDao = _database.QueryFirstOrDefault<PersonDao>(sql, new
            {
                firstName = firstName,
                lastName = lastName,
                ageId = ageId
            });

            return personDao == null ? null : TransformDaoToBusinessLogicPerson(personDao);
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
            var ageId = _age.GetAgeByType(personDto.Age).Id;
            
            const string sql =
                "UPDATE SkiTickets.Person SET firstName = @firstName, lastName = @lastName, ageId = @ageId WHERE id = @id";
            _database.Execute(sql, new
            {
                firstName = personDto.FirstName,
                lastName = personDto.LastName,
                ageId = ageId,
                id = id
            });

            return GetPersonById(id);
        }
        private Models.Person TransformDaoToBusinessLogicPerson(PersonDao personDao)
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