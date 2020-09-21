using System;

namespace SkiTickets.Models
{
    public class Ticket
    {
        public int Id { set; get; }
        public TicketType TicketType { set; get; }
        public DateTime From { set; get; }
        public DateTime To { set; get; }
    }
}