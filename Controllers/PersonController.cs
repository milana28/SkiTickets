using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiTickets.Domain;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;
using SkiTickets.Utils.Filters;

namespace SkiTickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPerson _person;

        public PersonController(IPerson person)
        {
            _person = person;
        }

        [HttpPost]
        [AgeValidFilter]
        [PersonExistsFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> CreatePerson([FromBody] PersonDto personDto)
        {
            try
            {
                return Created("https://localhost:5001/Person", _person.CreatePerson(personDto));
            }
            catch (PersonBadRequestException e)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Models.Person>> GetAll()
        {
            try
            {
                return Ok(_person.GetAll());
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{id}")]
        [PersonExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> GetPersonById(int id)
        {
            try
            {
                return Ok(_person.GetPersonById(id));
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [PersonExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> DeletePerson(int id)
        {
            try
            {
                return Ok(_person.DeletePerson(id));
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpPut("{id}")]
        [PersonExistsFilter]
        [AgeValidFilter(info = typeof(PersonDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> UpdatePerson(int id, PersonDto personDto)
        {
            try
            {
                return Ok(_person.UpdatePerson(id, personDto));
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (AgeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}