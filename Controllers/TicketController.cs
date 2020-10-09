using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SkiTickets.Domain;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;
using SkiTickets.Utils.Filters;
using SkiTickets.Utils.Responses;

namespace SkiTickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;
        private readonly IMemoryCache _cache;

        public TicketController(ITicket ticket, IMemoryCache cache)
        {
            _ticket = ticket;
            _cache = cache;
        }

        [HttpPost]
        [TicketTypeInTicketIsValidFilter]
        // [AgeValidFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.Ticket> CreateTicket(TicketDto ticketDto)
        {
            try
            {
                return Created("https://localhost:5001/Ticket", _ticket.CreateTicket(ticketDto));
            }
            catch (TicketTypeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaginationResponse<List<Models.Ticket>>> GetTickets([FromQuery(Name = "age")] string? age, 
            [FromQuery(Name = "from")] DateTime? fromDate, [FromQuery(Name = "to")] DateTime? toDate, [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")] int? pageSize)
        {
            try
            {
                if (!_cache.TryGetValue("Tickets", out List<Models.Ticket> tickets))
                {
                    _cache.Set("Tickets", tickets, TimeSpan.FromSeconds(600));
                }
                if (page != null || pageSize != null)
                {
                    return Ok(new PaginationResponse<Models.Ticket>((_ticket.GetTickets(age, fromDate, toDate)), page, pageSize));
                }
              
                return Ok(_ticket.GetTickets(age, fromDate, toDate));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{id}")]
        [TicketWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public ActionResult<List<Models.Ticket>> GetTicketById(int id)
        {
            try
            {
                return Ok(_ticket.GetTicketById(id));
            }
            catch (TicketNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{id}")]
        [TicketWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Ticket> DeleteTicket(int id)
        {
            try
            {
                return Ok(_ticket.DeleteTicket(id));
            }
            catch (TicketNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        [TicketWithIdExistsFilter]
        [TicketTypeInTicketIsValidFilter]
        // [AgeValidFilter(info = typeof(PersonDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Ticket> UpdateTicket(int id, TicketDto ticketDto)
        {
            try
            {
                return Ok(_ticket.UpdateTicket(id, ticketDto));
            }
            catch (TicketNotFoundException e)
            {
                return NotFound(e.Message);
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
    }
}