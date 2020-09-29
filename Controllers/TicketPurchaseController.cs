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
    public class TicketPurchaseController : ControllerBase
    {
        private readonly ITicketPurchase _ticketPurchase;

        public TicketPurchaseController(ITicketPurchase ticketPurchase)
        {
            _ticketPurchase = ticketPurchase;
        }
        
        [HttpPost]
        [AgesMatchingFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.TicketPurchase> CreateTicketPurchase(TicketPurchaseDto ticketPurchaseDto)
        {
            try
            {
                return Created("https://localhost:5001/TicketPurchase", _ticketPurchase.CreateTicketPurchase(ticketPurchaseDto));
            }
            catch (AgesNotMatchingException e)
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
        public ActionResult<List<Models.TicketPurchase>> GetAll()
        {
            try
            {
                return Ok(_ticketPurchase.GetAll());
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        [TicketPurchaseExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Models.TicketPurchase>> GetTicketPurchaseById(int id)
        {
            try
            {
                return Ok(_ticketPurchase.GetTicketPurchaseById(id));
            }
            catch(TicketPurchaseNotFound e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("ticketType/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Models.TicketPurchase>> GetTicketPurchasesByTicketType(string type)
        {
            try
            {
                return Ok(_ticketPurchase.GetTicketPurchasesByTicketType(type));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("sellingPoint/{sellingPointId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Models.TicketPurchase>> GetTicketPurchasesBySellingPoint(int sellingPointId)
        {
            try
            {
                return Ok(_ticketPurchase.GetTicketPurchasesBySellingPoint(sellingPointId));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}