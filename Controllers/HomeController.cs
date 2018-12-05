using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scout.Models;

namespace Scout.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Product()
        {
            ViewBag.Title = "Products";

			ScoutEntities db = new ScoutEntities();
			List<Product> productList = db.Products.ToList();
			
			List<ProductViewModel> productVMList = productList.Select(x => new ProductViewModel { ProductID = x.ProductID, ProductDesc = x.ProductDesc, SKU = x.SKU }).ToList();

			return View("~/Views/Product/Product.cshtml", productVMList);
        }
		public ActionResult Bins()
        {
            ViewBag.Title = "Bins";

			ScoutEntities db = new ScoutEntities();
			List<Bin> binList = db.Bins.ToList();

			List<BinViewModel> binVMList = binList.Select(x => new BinViewModel { BinID = x.BinID, BinName= x.BinName}).ToList();

			return View("~/Views/Bins/Bins.cshtml", binVMList);
		}
        public ActionResult Inventory()
        {
            ViewBag.Title = "Inventory Levels";

			ScoutEntities db = new ScoutEntities();
			List<Inventory> inventoryList = db.Inventories.ToList();

			List<InventoryViewModel> inventoryVMList = inventoryList.Select(x => new InventoryViewModel { InventoryID = x.InventoryID, ProductDesc = x.Product.ProductDesc, BinName = x.Bin.BinName, QTY = x.QTY}).ToList();

			return View("~/Views/Inventory/Inventory.cshtml", inventoryVMList);
        }
        public ActionResult Orders()
		{
			ViewBag.Title = "Orders";

			List<CustomerOrderViewModel> customerOrderList = new List<CustomerOrderViewModel>();
			SqlConnection con;
			string constr = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
			con = new SqlConnection(constr);
			con.Open();
			SqlCommand SQL = new SqlCommand("usp_View_Orders", con);
			SQL.CommandType = CommandType.StoredProcedure;
			SqlDataReader reader = SQL.ExecuteReader();
			while(reader.Read())
			{
				CustomerOrderViewModel customerOrder = new CustomerOrderViewModel();
				customerOrder.OrderID = Int32.Parse(reader["OrderID"].ToString());
				customerOrder.DateOrdered = Convert.ToDateTime(reader["DateOrdered"].ToString());
				customerOrder.ProductDesc = reader["ProductDesc"].ToString();
				customerOrder.QTY = Int32.Parse(reader["QTY"].ToString());
				customerOrder.CustomerName = reader["CustomerName"].ToString();
				customerOrder.CustomerAddress = reader["CustomerAddress"].ToString();
				customerOrderList.Add(customerOrder);
			}
			reader.Close();
			con.Close();

			return View("~/Views/Orders/Orders.cshtml", customerOrderList);
		}
    }
}
