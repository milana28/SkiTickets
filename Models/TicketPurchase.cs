using System;

namespace SkiTickets.Models
{
    public class TicketPurchase
    {
        public int Id { set; get; }
        public Ticket Ticket { set; get; }
        public Person Person { set; get; }
        public SellingPoint SellingPoint { set; get; }
        public DateTime Date { set; get; }
    }
}