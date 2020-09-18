using System;

namespace SkiTickets.Models
{
    public class PriceList
    {
        public int Id { set; get; }
        public int TicketId { set; get; }
        public float Price { set; get; }
        public DateTime From { set; get; }
        public DateTime To { set; get; }
    }
}