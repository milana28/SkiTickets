using System;

namespace SkiTickets.Models
{
    public class TicketPurchaseDao
    {
        public int Id { set; get; }
        public int TicketId { set; get; }
        public int PersonId { set; get; }
        public int SellingPointId { set; get; }
        public DateTime Date { set; get; }
    }
}