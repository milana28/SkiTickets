using System;

namespace SkiTickets.Models
{
    public class TicketDto
    {
        public int Id { set; get; }
        public string TicketType { set; get; }
        public float Price { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
    }
}