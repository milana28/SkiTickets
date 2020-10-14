using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiTickets.Domain;
using SkiTickets.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;
using SkiTickets.Utils.Responses;

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
        [PersonUniqueOnCreateFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> CreatePerson([FromBody] PersonDto personDto)
        {
            try
            {
                return Created("https://localhost:5001/Person", new OkResponse<Models.Person>(_person.CreatePerson(personDto)));
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
                return Ok(new OkResponse<List<Models.Person>>(_person.GetAll()));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{personId}")]
        [PersonWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> GetPersonById(int personId)
        {
            try
            {
                return Ok(new OkResponse<Models.Person>(_person.GetPersonById(personId)));
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

        [HttpDelete("{personId}")]
        [PersonWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> DeletePerson(int personId)
        {
            try
            {
                return Ok(new OkResponse<Models.Person>(_person.DeletePerson(personId)));
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
        
        [HttpPut("{personId}")]
        [PersonWithIdExistsFilter]
        [AgeValidFilter(Info = "personDto")]
        [PersonUniqueOnUpdateFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> UpdatePerson(int personId, PersonDto personDto)
        {
            try
            {
                return Ok(new OkResponse<Models.Person>(_person.UpdatePerson(personId, personDto)));
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (AgeNotFoundException e)
            {
                return NotFound(e.Message);
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
    }
}