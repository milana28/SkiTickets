using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Filters
{
    public class SellingPointUniqueOnUpdateFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sellingPoint = (SellingPointDto) context.ActionArguments["sellingPointDto"];
            var sellingPointId = (int) context.ActionArguments["sellingPointId"];

            using IDbConnection database = new SqlConnection(MyConnectionString);

            const string sql =
                "SELECT * FROM SkiTickets.SellingPoint WHERE id != @id AND name = @name AND location = @location";
            var sellingPointDao = database.QueryFirstOrDefault<SellingPointDao>(sql,
                new {id = sellingPointId, name = sellingPoint.Name, location = sellingPoint.Location});

            if (sellingPointDao != null)
            {
                throw new SellingPointBadRequestException("SellingPoint already exists!");
            }

            base.OnActionExecuting(context);
        }
    }
}