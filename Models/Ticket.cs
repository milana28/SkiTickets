using System;

namespace SkiTickets.Models
{
    public class Ticket
    {
        public int Id { set; get; }
        public TicketType TicketType { set; get; }
        public float Price { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
    }
}