using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Scout.Models;

namespace Scout.Controllers
{
    public class OrdersController : Controller
    {
		private SqlConnection con;

		//Connection activities
		private void connection()
		{
			string constr = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
			con = new SqlConnection(constr);
		}
		// GET: Orders
		public ActionResult Index()
        {
            return View();
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Orders/Create
		[HttpGet]
		public ActionResult AddOrders()
		{
			CustomerOrderViewModel Orders = new CustomerOrderViewModel();
			ScoutEntities db = new ScoutEntities();

			//Product Dropdown
			var getProductList = db.Products.ToList();
			Orders.ProductList = new SelectList(getProductList, "ProductID", "ProductDesc");
			
			return View("~/Views/Orders/AddOrders.cshtml", Orders);
		}

		// POST: Orders/Create
		[HttpPost]
		public ActionResult AddOrders(CustomerOrderViewModel OrdersItem)
		{
			try
			{
				if (OrdersItem.DateOrdered != null && OrdersItem.SelectedProductID != 0 && OrdersItem.QTY != 0 && OrdersItem.CustomerName != null && OrdersItem.CustomerAddress != null)
				{
					ViewBag.Message = InsUpd_Orders(OrdersItem);
				}

				return AddOrders();
			}
			catch
			{
				ViewBag.Message = "Error";
				return AddOrders();
			}
		}

		//Calling InsUpd Stored procedure
		public string InsUpd_Orders(CustomerOrderViewModel obj)
		{
			try
			{
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_InsUpd_Orders", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@OrdersID", obj.OrderID);
				SQL.Parameters.AddWithValue("@DateOrdered", obj.DateOrdered);
				SQL.Parameters.AddWithValue("@ProductID", obj.SelectedProductID);
				SQL.Parameters.AddWithValue("@QTY", obj.QTY);
				SQL.Parameters.AddWithValue("@CustomerName", obj.CustomerName);
				SQL.Parameters.AddWithValue("@CustomerAddress", obj.CustomerAddress);
				SqlDataReader reader = SQL.ExecuteReader();
				CustomerOrderViewModel Orders = new CustomerOrderViewModel();
				string msg = "";
				if (reader.Read())
				{
					msg = reader["msg"].ToString();
				}
				else
				{
					msg = "Error";
				}
				reader.Close();
				con.Close();
				return msg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// GET: Orders/Edit/5
		[HttpGet]
		public ActionResult EditOrders(int id)
		{
			connection();
			con.Open();
			SqlCommand SQL = new SqlCommand("usp_Get_Orders", con);
			SQL.CommandType = CommandType.StoredProcedure;
			SQL.Parameters.AddWithValue("@OrderID", id);
			SqlDataReader reader = SQL.ExecuteReader();
			CustomerOrderViewModel Orders = new CustomerOrderViewModel();
			int ProductID = 0;

			if (reader.Read())
			{
				Orders.OrderID = id;
				Orders.DateOrdered = Convert.ToDateTime(reader["DateOrdered"].ToString());
				ProductID = Int32.Parse(reader["ProductID"].ToString());
				Orders.QTY = Int32.Parse(reader["QTY"].ToString());
				Orders.CustomerName = reader["CustomerName"].ToString();
				Orders.CustomerAddress = reader["CustomerAddress"].ToString();
			}
			reader.Close();
			con.Close();

			ScoutEntities db = new ScoutEntities();

			//Product Dropdown
			var getProductList = db.Products.ToList();
			Orders.ProductList = new SelectList(getProductList, "ProductID", "ProductDesc");
			Orders.SelectedProductID = ProductID;

			return View("~/Views/Orders/EditOrders.cshtml", Orders);

		}

		// POST: Bin/Edit/5
		[HttpPost]
		public ActionResult EditOrders(CustomerOrderViewModel OrdersItem)
		{
			try
			{
				if (OrdersItem.DateOrdered != null && OrdersItem.SelectedProductID != 0 && OrdersItem.QTY != 0 && OrdersItem.CustomerName != null && OrdersItem.CustomerAddress != null)
				{
					ViewBag.Message = InsUpd_Orders(OrdersItem);
				}
				return EditOrders(OrdersItem.OrderID);
			}
			catch
			{
				ViewBag.Message = "Error";
				return EditOrders(OrdersItem.OrderID);
			}
		}

		// POST: Bin/Delete/5
		public ActionResult DeleteOrders(int id)
		{
			try
			{
				// TODO: Add delete logic here
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_Delete_Order", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@OrderID", id);
				int i = SQL.ExecuteNonQuery();
				if (i >= 1)
				{
					ViewBag.Message = "ID: " + id + " has been successfully deleted.";
				}
				else
				{
					ViewBag.Message = "ID: " + id + " has been not been deleted.";
				}

				ViewBag.Title = "Orders";

				List<CustomerOrderViewModel> customerOrderList = new List<CustomerOrderViewModel>();
				connection();
				con.Open();
				SqlCommand rebuildList = new SqlCommand("usp_View_Orders", con);
				rebuildList.CommandType = CommandType.StoredProcedure;
				SqlDataReader reader = rebuildList.ExecuteReader();
				while (reader.Read())
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
			catch
			{
				ViewBag.Title = "Orders";

				List<CustomerOrderViewModel> customerOrderList = new List<CustomerOrderViewModel>();
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_View_Orders", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SqlDataReader reader = SQL.ExecuteReader();
				while (reader.Read())
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
}
