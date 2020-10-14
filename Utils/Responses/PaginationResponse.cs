using System.Collections.Generic;
using SkiTickets.Utils.Models;

namespace SkiTickets.Utils.Responses
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; }
        public Metadata Metadata { get; }
        
        public PaginationResponse(List<T> response, int? pageNumber, int? pageSize)
        {
            Data = response;
            Metadata = PaginationCalculation<T>.Calculate(response, pageNumber, pageSize);
        }
    }
}