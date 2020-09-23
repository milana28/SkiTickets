using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using SkiTickets.Domain;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> CreatePerson([FromBody] PersonDao personDao)
        {
            try
            {
                return Created("https://localhost:5001/Person", _person.CreatePerson(personDao));
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> UpdatePerson(int id, PersonDao personDao)
        {
            try
            {
                return Ok(_person.UpdatePerson(id, personDao));
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
    }
}