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
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;

        public TicketController(ITicket ticket)
        {
            _ticket = ticket;
        }

        [HttpPost]
        [TicketTypeValidFilter]
        [AgeValidFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.Ticket> CreateTicket(TicketDto ticketDto)
        {
            // try
            // {
            //     return Created("https://localhost:5001/Ticket", _ticket.CreateTicket(ticketDto));
            // }
            // catch (Exception e)
            // {
            //     return BadRequest();
            // }
            return _ticket.CreateTicket(ticketDto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Models.Ticket>> GetAll()
        {
            try
            {
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
    }
}