using System;
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

    }
}