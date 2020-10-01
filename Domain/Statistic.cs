using System;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface IStatistic
    {
        Models.Statistic GetStatisticForPeriod(int hours);
    }
    
    public class Statistic : IStatistic
    {
        private readonly IDbConnection _database;

        public Statistic(IDatabase database)
        {
            _database = database.Get();
        }
        
        public Models.Statistic GetStatisticForPeriod(int hours)
        {
            var now = DateTime.Now;
            var period = now.AddHours(-hours);
            const string sql = "SELECT * FROM SkiTickets.TicketUsed WHERE time <= @now AND time >= @period";
            var ticketsUsedDao = _database.Query<TicketUsedDao>(sql, new {now = DateTime.Now, period = period }).ToList();

            return new Models.Statistic()
            {
                Period = hours + " hours",
                NumberOfSkiers = ticketsUsedDao.Count
            };
        }
    }
}