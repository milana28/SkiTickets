using System;
using System.Collections.Generic;
using SkiTickets.Utils;

namespace SkiTickets.Models
{
    public class Response<T>
    {
        public List<T> Data { set; get; }
        public Metadata Metadata { set; get; }
        
        public Response(List<T> response)
        {
            Data = response;
            Metadata = new Metadata();
            const int page = 1;
            const int limit = 25;
            const int offset = (page - 1) * limit;
            const int previousOffset = (offset - limit) < 0 ? 0 : (offset - limit);
            const int nextOffset = offset + limit;
            var pageCount = (int) Math.Ceiling((double) Data.Count / (double) limit);

            Metadata = new Metadata()
            {
                Pagination = new Pagination() {
                Offset = offset,
                Limit = limit,
                PreviousOffset = previousOffset,
                NextOffset = nextOffset,
                CurrentPage = page,
                PageCount = pageCount,
                TotalCount = Data.Count
                }
            };
        }
    }
}