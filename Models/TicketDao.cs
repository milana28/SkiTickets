using System;

namespace SkiTickets.Models
{
    public class TicketDao
    {
        public int Id { set; get; }
        public int TicketTypeId { set; get; }
        public DateTime From { set; get; }
        public DateTime To { set; get; }
    }
}