using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using SkiTickets.Models;
using SkiTickets.Utils.Exceptions;

namespace SkiTickets.Utils.Filters
{
    public class SellingPointExistsFilter : ActionFilterAttribute
    {
        private const string MyConnectionString =
            "Server=localhost;Database=skitickets;User Id=sa;Password=yourStrong(!)Password;";
        private int _sellingPintId;
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _sellingPintId = (int) context.ActionArguments["id"];
            using IDbConnection database = new SqlConnection(MyConnectionString);
            const string sql = "SELECT * FROM SkiTickets.SellingPoint WHERE id = @id";
            var sellingPoint = database.QueryFirstOrDefault<SellingPointDao>(sql, new {id = _sellingPintId});

            if (sellingPoint == null)
            {
                throw new SellingPointNotFoundException("SellingPoint does not exist!");
            }
            
            base.OnActionExecuting(context);
        }
    }
}