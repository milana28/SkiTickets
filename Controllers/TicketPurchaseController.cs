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
        [AgeInTicketPurchaseMatchesAgeInTicketFilter]
        [SellingPointInTicketPurchaseIsValidFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.TicketPurchase> CreateTicketPurchase(TicketPurchaseDto ticketPurchaseDto)
        {
            try
            {
                return Created("https://localhost:5001/TicketPurchase", new OkResponse<Models.TicketPurchase>(_ticketPurchase.CreateTicketPurchase(ticketPurchaseDto)));
            }
            catch (NoCapacity e)
            {
                return BadRequest();
            }
            catch (AgesNotMatchingException e)
            {
                return BadRequest();
            }
            catch (SellingPointNotFoundException e)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpPost("{ticketPurchaseId}/check")]
        [TicketPurchaseWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.TicketUsed> CheckTicket(int ticketPurchaseId)
        {
            try
            {
                return Created("https://localhost:5001/TicketPurchase/{id}/check", new OkResponse<Models.TicketUsed>(_ticketUsed.CheckTicket(ticketPurchaseId)));
            }
            catch (TicketPurchaseNotFoundException e)
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
                return Ok(new OkResponse<List<Models.TicketPurchase>>(_ticketPurchase.GetAll()));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet("{ticketPurchaseId}")]
        [TicketPurchaseWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.TicketPurchase> GetTicketPurchaseById(int ticketPurchaseId)
        {
            try
            {
                return Ok(new OkResponse<Models.TicketPurchase>(_ticketPurchase.GetTicketPurchaseById(ticketPurchaseId)));
            }
            catch(TicketPurchaseNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("ticketType/{ticketType}")]
        [TicketTypeWithTypeExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Models.TicketPurchase>> GetTicketPurchasesByTicketType(string ticketType)
        {
            try
            {
                return Ok(new OkResponse<List<Models.TicketPurchase>>(_ticketPurchase.GetTicketPurchasesByTicketType(ticketType)));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("sellingPoint/{sellingPointId}")]
        [SellingPointWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Models.TicketPurchase>> GetTicketPurchasesBySellingPoint(int sellingPointId)
        {
            try
            {
                return Ok(new OkResponse<List<Models.TicketPurchase>>(_ticketPurchase.GetTicketPurchasesBySellingPoint(sellingPointId)));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}