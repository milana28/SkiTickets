using System;

namespace SkiTickets.Models
{
    public class TicketDao
    {
        public int Id { set; get; }
        public int TicketTypeId { set; get; }
        public float Price { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
    }
}