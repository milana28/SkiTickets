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
    public class AgeController : ControllerBase
    {
        private readonly IAge _age;

        public AgeController(IAge age)
        {
            _age = age;
        }
        
        [HttpPost]
        [MinYearsSmallerThanMaxYearsFilter]
        [AgeUniqueOnCreateFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.Age> CreateTicket(AgeDto ageDto)
        {
            try
            {
                return Created("https://localhost:5001/Age", new OkResponse<Models.Age>(_age.CreateAge(ageDto)));
            }
            catch (AgeBadRequestException e)
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
        public ActionResult<List<Models.Age>> GetAll()
        {
            try
            {
                return Ok(new OkResponse<List<Models.Age>>(_age.GetAll()));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{ageId}")]
        [AgeWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> GetAgeById(int ageId)
        {
            try
            {
                return Ok(new OkResponse<Models.Age>(_age.GetAgeById(ageId)));
            }
            catch (AgeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e.Message));
            }
        }

        [HttpDelete("{ageId}")]
        [AgeWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> DeleteAge(int ageId)
        {
            try
            {
                return Ok(new OkResponse<Models.Age>(_age.DeleteAge(ageId)));
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
        
        [HttpPut("{ageId}")]
        [AgeWithIdExistsFilter]
        [MinYearsSmallerThanMaxYearsFilter]
        [AgeUniqueOnUpdateFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> UpdateAge(int ageId, AgeDto ageDto)
        {
            try
            {
                return Ok(new OkResponse<Models.Age>(_age.UpdateAge(ageId, ageDto)));
            }
            catch (AgeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (AgeBadRequestException e)
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