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
using Ticket = SkiTickets.Domain.Ticket;

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
        [TicketTypeValidFilter]
        [AgeValidFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.Ticket> CreateTicket(TicketDto ticketDto)
        {
            try
            {
                return Created("https://localhost:5001/Ticket", _ticket.CreateTicket(ticketDto));
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
                    if (age != null)
                    {
                        return Ok(new PaginationResponse<Models.Ticket>((_ticket.GetTicketsByAge(age)), page, pageSize));
                    }
                    if (fromDate != null && toDate != null)
                    {
                        return Ok(new PaginationResponse<Models.Ticket>((_ticket.GetTicketsWithinDate(fromDate, toDate)), page, pageSize));
                    }
                    return Ok(new PaginationResponse<Models.Ticket>((_ticket.GetAll()), page, pageSize));
                }
                if (age != null)
                {
                    return Ok(_ticket.GetTicketsByAge(age));
                }
                if (fromDate != null && toDate != null)
                {
                    return Ok(_ticket.GetTicketsWithinDate(fromDate, toDate));
                }
                return Ok(_ticket.GetAll());

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{id}")]
        [TicketExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Models.Ticket>> GetTicketById(int id)
        {
            try
            {
                return Ok(_ticket.GetTicketById(id));
            }
            catch (TicketNotFound e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{id}")]
        [TicketExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Ticket> DeletePerson(int id)
        {
            try
            {
                return Ok(_ticket.DeleteTicket(id));
            }
            catch (TicketNotFound e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        [TicketExistsFilter]
        [AgeValidFilter(info = typeof(PersonDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Ticket> UpdatePerson(int id, TicketDto ticketDto)
        {
            try
            {
                return Ok(_ticket.UpdateTicket(id, ticketDto));
            }
            catch (TicketNotFound e)
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