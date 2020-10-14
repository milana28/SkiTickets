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
    public class SellingPointController : ControllerBase
    {
        private readonly ISellingPoint _sellingPoint;

        public SellingPointController(ISellingPoint sellingPoint)
        {
            _sellingPoint = sellingPoint;
        }
        
        [HttpPost]
        [SellingPointUniqueOnCreateFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Person> CreatePerson([FromBody] SellingPointDto sellingPointDto)
        {
            try
            {
                return Created("https://localhost:5001/SellingPoint", new OkResponse<Models.SellingPoint>(_sellingPoint.CreateSellingPoint(sellingPointDto)));
            }
            catch (SellingPointBadRequestException e)
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
        public ActionResult<List<Models.SellingPoint>> GetAll()
        {
            try
            {
                return Ok(new OkResponse<List<Models.SellingPoint>>(_sellingPoint.GetAll()));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{sellingPointId}")]
        [SellingPointWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Age> GetSellingPointById(int sellingPointId)
        {
            try
            {
                return Ok(new OkResponse<Models.SellingPoint>(_sellingPoint.GetSellingPointById(sellingPointId)));
            }
            catch (SellingPointNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{sellingPointId}")]
        [SellingPointWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.SellingPoint> DeleteSellingPoint(int sellingPointId)
        {
            try
            {
                return Ok(new OkResponse<Models.SellingPoint>(_sellingPoint.DeleteSellingPoint(sellingPointId)));
            }
            catch (SellingPointNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpPut("{sellingPointId}")]
        [SellingPointWithIdExistsFilter]
        [SellingPointUniqueOnUpdateFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.SellingPoint> UpdateSellingPoint(int sellingPointId, SellingPointDto sellingPointDto)
        {
            try
            {
                return Ok(new OkResponse<Models.SellingPoint>(_sellingPoint.UpdateSellingPoint(sellingPointId, sellingPointDto)));
            }
            catch (SellingPointNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (SellingPointBadRequestException e)
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