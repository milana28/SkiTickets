using System;
using System.Collections.Generic;

namespace SkiTickets.Utils
{
    public static class PaginationCalculation<T>
    {
        private static List<T> Data { set; get; }
        private static int PageNumber { set; get; }
        private static int Limit { set; get; }
        private static int Offset { set; get; }
        private static int PreviousOffset { set; get; }
        private static int NextOffset { set; get; }
        private static int PageCount { set; get; }

        public static Metadata Calculate(List<T> response, int? pageNumber, int? pageSize)
        {
            if (pageNumber != null && pageSize != null)
            {
                var index = (int) ((pageNumber - 1) * pageSize);
                var count = (int) pageSize;
                Data = response.GetRange(index, count);
            }
            else if (pageNumber != null)
            {
                var index = (int) ((pageNumber - 1) * 25);
                Data = response.GetRange(index, 25);
            }
            else
            {
                Data = response;
            }

            PageNumber = pageNumber ?? 1;
            Limit = pageSize ?? 25;
            Offset = (PageNumber - 1) * Limit;
            PreviousOffset = (Offset - Limit) < 0 ? 0 : (Offset - Limit);
            NextOffset = Offset + Limit;
            PageCount = (int) Math.Ceiling((double) Data.Count / (double) Limit);

            return new Metadata()
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