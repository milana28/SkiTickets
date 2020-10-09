using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class SellingPointUniqueOnCreateFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sellingPoint = (SellingPointDto) context.ActionArguments["sellingPointDto"];
            using IDbConnection database = new SqlConnection(MyConnectionString);

            const string sql = "SELECT * FROM SkiTickets.SellingPoint WHERE name = @name AND location = @location";
            var sellingPointDao = database.QueryFirstOrDefault<SellingPointDao>(sql,
                new {name = sellingPoint.Name, location = sellingPoint.Location});

            if (sellingPointDao != null)
            {
                throw new SellingPointBadRequestException("SellingPoint already exists!");
            }

            base.OnActionExecuting(context);
        }
    }
}