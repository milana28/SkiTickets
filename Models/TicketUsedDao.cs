using System;

namespace SkiTickets.Models
{
    public class TicketUsedDao
    {
        public int Id { set; get; }
        public int TicketPurchaseId { set; get; }
        public DateTime Time { set; get; }
    }
}