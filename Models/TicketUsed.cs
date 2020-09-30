using System;

namespace SkiTickets.Models
{
    public class TicketUsed
    {
        public int Id { set; get; }
        public Ticket Ticket { set; get; }
        public DateTime Time { set; get; }
    }
}