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
    public class AgeController : ControllerBase
    {
        private readonly IAge _age;

        public AgeController(IAge age)
        {
            _age = age;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Models.Age>> GetAll()
        {
            try
            {
                return Ok(_age.GetAll());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("{id}")]
        [AgeExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> GetAgeById(int id)
        {
            try
            {
                return Ok(_age.GetAgeById(id));
            }
            catch (AgeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [AgeExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> DeleteAge(int id)
        {
            try
            {
                return Ok(_age.DeleteAge(id));
            }
            catch (AgeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        [AgeExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> UpdatePerson(int id, AgeDto ageDto)
        {
            try
            {
                return Ok(_age.UpdateAge(id, ageDto));
            }
            catch (AgeNotFoundException e)
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