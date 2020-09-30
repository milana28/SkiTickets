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
        private readonly ITicketUsed _ticketUsed;

        public TicketPurchaseController(ITicketPurchase ticketPurchase, ITicketUsed ticketUsed)
        {
            _ticketPurchase = ticketPurchase;
            _ticketUsed = ticketUsed;
        }
        
        [HttpPost]
        [CheckCapacity]
        [AgesMatchingFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.TicketPurchase> CreateTicketPurchase(TicketPurchaseDto ticketPurchaseDto)
        {
            try
            {
                return Created("https://localhost:5001/TicketPurchase", _ticketPurchase.CreateTicketPurchase(ticketPurchaseDto));
            }
            catch (NoCapacity e)
            {
                return BadRequest();
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
        
        [HttpPost("{id}/check")]
        [TicketPurchaseExistsFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.TicketUsed> CheckTicket(int id)
        {
            try
            {
                return Created("https://localhost:5001/TicketPurchase/{id}/check", _ticketUsed.CheckTicket(id));
            }
            catch (TicketPurchaseNotFound e)
            {
                return NotFound();
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