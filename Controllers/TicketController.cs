using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using DinkToPdf;
using DinkToPdf.Contracts;
using SkiTickets.Domain;
using SkiTickets.Filters;
using SkiTickets.Models;
using SkiTickets.Pdf;
using SkiTickets.Utils.Exceptions;
using SkiTickets.Utils.Responses;
using ColorMode = DinkToPdf.ColorMode;

namespace SkiTickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;
        private readonly IMemoryCache _cache;
        private readonly IConverter _converter;
        private readonly TemplateGenerator _template;

        public TicketController(ITicket ticket, IMemoryCache cache, IConverter converter, TemplateGenerator template)
        {
            _ticket = ticket;
            _cache = cache;
            _converter = converter;
            _template = template;
        }

        [HttpPost]
        [TicketTypeInTicketIsValidFilter]
        [AgeValidFilter(Info = "ticketDto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Models.Ticket> CreateTicket(TicketDto ticketDto)
        {
            try
            {
                return Created("https://localhost:5001/Ticket",
                    new OkResponse<Models.Ticket>(_ticket.CreateTicket(ticketDto)));
            }
            catch (TicketTypeNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e.Message, new List<string>(){"ticket"}));
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaginationResponse<List<Models.Ticket>>> GetTickets([FromQuery(Name = "age")] string? age,
            [FromQuery(Name = "from")] DateTime? fromDate, [FromQuery(Name = "to")] DateTime? toDate,
            [FromQuery(Name = "page")] int? page,
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
                    return Ok(new PaginationResponse<Models.Ticket>((_ticket.GetTickets(age, fromDate, toDate)), page,
                        pageSize));
                }

                return Ok(new OkResponse<List<Models.Ticket>>(_ticket.GetTickets(age, fromDate, toDate)));
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e.Message, new List<string>(){"age","from","to","page","pageSize"}));
            }
        }

        [HttpGet("{ticketId}")]
        [TicketWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Models.Ticket> GetTicketById(int ticketId)
        {
            try
            {
                return Ok(new OkResponse<Models.Ticket>(_ticket.GetTicketById(ticketId)));
            }
            catch (TicketNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e.Message, new List<string>(){"ticketId"}));
            }
        }

        [HttpDelete("{ticketId}")]
        [TicketWithIdExistsFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Ticket> DeleteTicket(int ticketId)
        {
            try
            {
                return Ok(new OkResponse<Models.Ticket>(_ticket.DeleteTicket(ticketId)));
            }
            catch (TicketNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e.Message, new List<string>(){"ticketId"}));
            }
        }

        [HttpPut("{ticketId}")]
        [TicketWithIdExistsFilter]
        [TicketTypeInTicketIsValidFilter]
        [AgeValidFilter(Info = "ticketDto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Ticket> UpdateTicket(int ticketId, TicketDto ticketDto)
        {
            try
            {
                return Ok(_ticket.UpdateTicket(ticketId, ticketDto));
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
                return BadRequest(new ErrorResponse(e.Message, new List<string>(){"ticket","ticketId"}));
            }
        }

        [HttpGet("{ticketId}/pdf")]
        [TicketWithIdExistsFilter]
        public async Task<IActionResult> CreatePdf(int ticketId)
        {
            var ticket = _ticket.GetTicketById(ticketId);

            var globalSettings = new GlobalSettings()
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings {Top = 10, Bottom = 10, Left = 10, Right = 10},
                DocumentTitle = "Ticket",
            };

            var objectSetting = new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = await _template.GetHtmlString(ticket),
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Css/Style.css"),
                    LoadImages = true
                },
                HeaderSettings = {FontName = "Arial", FontSize = 7, Right = "[page]/[toPage]", Line = true},
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = {objectSetting}
            };

            var file = _converter.Convert(pdf);

            return File(file, "applicaation/pdf");
        }
    }
}

