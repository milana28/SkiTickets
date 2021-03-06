using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiTickets.Domain;
using SkiTickets.Utils.Responses;

namespace SkiTickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatistic _statistic;

        public StatisticController(IStatistic statistic)
        {
            _statistic = statistic;
        }
        
        [HttpGet("hours/{hours}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.Statistic> GetStatistic(int hours)
        {
            try
            {
                return Ok(new OkResponse<Models.Statistic>(_statistic.GetStatisticForPeriod(hours)));
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e.Message, new List<string>(){"hours"}));
            }
        }
    }
}