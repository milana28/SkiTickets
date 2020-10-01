using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class CheckCapacity : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            using IDbConnection database = new SqlConnection(MyConnectionString);
            var now = DateTime.Now;
            var twoHoursAgo = now.AddHours(-2);
            const string sql = "SELECT * FROM SkiTickets.TicketUsed WHERE time <= @now AND time >= @twoHoursAgo";
            var ticketsUsedDao = database.Query<TicketUsedDao>(sql, new {now = DateTime.Now, twoHoursAgo = twoHoursAgo }).ToList();

            if (ticketsUsedDao.Count >= 1000)
            {
                throw new NoCapacity("Capacity is full!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}