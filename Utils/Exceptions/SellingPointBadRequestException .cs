using System;

namespace SkiTickets.Utils.Exceptions
{
    public class SellingPointBadRequestException : Exception
    {
        public SellingPointBadRequestException(string message) 
            :base(message) { }
    }
}