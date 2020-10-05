using System;
using System.Collections.Generic;

namespace SkiTickets.Utils.Responses
{
    public class PaginationResponse<T>
    {
        public List<T> Data { set; get; }
        public Metadata Metadata { set; get; }
        private int PageNumber { set; get; }
        private int Limit { set; get; }
        private int Offset { set; get; }
        private int PreviousOffset { set; get; }
        private int NextOffset { set; get; }
        private int PageCount { set; get; }
        
        public PaginationResponse(List<T> response, int? pageNumber, int? pageSize)
        {
            Data = response;
            Metadata = new Metadata();
            PageNumber = pageNumber ?? 1;
            Limit =  pageSize ?? 25;
            Offset = (PageNumber - 1) * Limit;
            PreviousOffset = (Offset - Limit) < 0 ? 0 : (Offset - Limit);
            NextOffset = Offset + Limit;
            PageCount = (int) Math.Ceiling((double) Data.Count / (double) Limit);
            
            Metadata = new Metadata()
            {
                Pagination = new Pagination()
                {
                    Offset = Offset,
                    Limit = Limit,
                    PreviousOffset = PreviousOffset,
                    NextOffset = NextOffset,
                    CurrentPage = PageNumber,
                    PageCount = PageCount,
                    TotalCount = Data.Count
                }
            };
        }
    }
}