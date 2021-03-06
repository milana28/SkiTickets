using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Filters
{
    public class SellingPointWithIdExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sellingPintId = (int) context.ActionArguments["sellingPointId"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            
            const string sql = "SELECT * FROM SkiTickets.SellingPoint WHERE id = @id";
            var sellingPoint = database.QueryFirstOrDefault<SellingPointDao>(sql, new {id = sellingPintId});

            if (sellingPoint == null)
            {
                throw new SellingPointNotFoundException("SellingPoint does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}