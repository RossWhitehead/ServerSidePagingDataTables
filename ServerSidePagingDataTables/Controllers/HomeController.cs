using DataTables.Mvc;
using ServerSidePagingDataTables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace ServerSidePagingDataTables.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DataTableGet([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var db = new ServerSidePagingDataTablesDbContext();

            IQueryable<Product> query = db.Products;

            var totalCount = query.Count();

            // Apply filters
            if (requestModel.Search.Value != String.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Name.Contains(value) || p.Description.Contains(value));
            }

            var filteredCount = query.Count();

            // Sort
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : "";
                orderByString += column.Data + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }

            query = query.OrderBy(orderByString == String.Empty ? "name asc" : orderByString);

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);

            var data = query.Select(p => new
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Category = p.ProductCategory.Name
            }).ToList();

            return Json(new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}