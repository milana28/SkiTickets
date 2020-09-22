using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Domain
{
    public interface IPerson
    { 
        List<Models.Person> GetAll();
        Models.Person GetPersonById(int id);
    }
    
    public class Person : IPerson
    {
        private readonly IDbConnection _database;

        public Person(IDatabase database)
        {
            _database = database.Get();
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
            var person = _database.QueryFirstOrDefault<PersonDao>(sql, new {personId = id});

            if (person == null)
            {
                throw new PersonNotFoundException("Person does not exist!");
            }

            return TransformDaoToBusinessLogicPerson(person);
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