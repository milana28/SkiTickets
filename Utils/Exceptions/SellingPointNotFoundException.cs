using System;

namespace SkiTickets.Utils.Exceptions
{
    public class SellingPointNotFoundException : Exception
    {
        public SellingPointNotFoundException(string message) 
            :base(message) { }
    }
}