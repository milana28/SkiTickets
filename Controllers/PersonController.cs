using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiTickets.Domain;
using SkiTickets.Models;
using SkiTickets.Utils;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> CreatePerson([FromBody] PersonDto personDto)
        {
            try
            {
                return Created("https://localhost:5001/Person", _person.CreatePerson(personDto));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
                return BadRequest(e.Message);
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
                return BadRequest(e.Message);
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
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        [PersonExistsFilter]
        [AgeValidFilter]
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
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}