using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SkiTickets.Models;
using SkiTickets.Utils;

namespace SkiTickets.Domain
{
    public interface ISellingPoint
    {
        List<Models.SellingPoint> GetAll();
        Models.SellingPoint GetSellingPointById(int id);
    }
    
    public class SellingPoint : ISellingPoint
    {
        private readonly IDbConnection _database;

        public SellingPoint(IDatabase database)
        {
            _database = database.Get();
        }
        
        public List<Models.SellingPoint> GetAll()
        {
            var sellingPointList = new List<Models.SellingPoint>();
            var sellingPointDaoList = _database.Query<SellingPointDao>("SELECT * FROM SkiTickets.SellingPoint").ToList();
            sellingPointDaoList.ForEach(s => sellingPointList.Add(TransformDaoToBusinessLogicSellingPoint(s)));

            return sellingPointList;
        }
        public Models.SellingPoint GetSellingPointById(int id)
        {
            const string sql = "SELECT * FROM SkiTickets.SellingPoint WHERE id = @sellingPointId";
            return TransformDaoToBusinessLogicSellingPoint(_database.QueryFirstOrDefault<SellingPointDao>(sql, new {sellingPointId = id}));
        }
        public Models.SellingPoint DeleteSellingPoint(int id)
        {
            var sellingPoint = GetSellingPointById(id);
            const string sql = "DELETE FROM SkiTickets.SellingPoint WHERE id = @sellingPointId";
            _database.Execute(sql, new {sellingPointId = id});

            return sellingPoint;
        }
        private static Models.SellingPoint TransformDaoToBusinessLogicSellingPoint(SellingPointDao sellingPointDao)
        {
            return new Models.SellingPoint()
            {
                Id = sellingPointDao.Id,
                Name = sellingPointDao.Name,
                Location = sellingPointDao.Location
            };
        }
    }
}