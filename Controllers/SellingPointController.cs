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
    public class SellingPointController : ControllerBase
    {
        private readonly ISellingPoint _sellingPoint;

        public SellingPointController(ISellingPoint sellingPoint)
        {
            _sellingPoint = sellingPoint;
        }
        
        [HttpPost]
        [AgeValidFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> CreatePerson([FromBody] SellingPointDto sellingPointDto)
        {
            try
            {
                return Created("https://localhost:5001/Person", _sellingPoint.CreateSellingPoint(sellingPointDto));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Models.SellingPoint>> GetAll()
        {
            try
            {
                return Ok(_sellingPoint.GetAll());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("{id}")]
        [SellingPointExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> GetSellingPointById(int id)
        {
            try
            {
                return Ok(_sellingPoint.GetSellingPointById(id));
            }
            catch (SellingPointNotFoundException e)
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