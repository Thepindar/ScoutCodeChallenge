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
    public class InventoryController : Controller
    {
		private SqlConnection con;

		//Connection activities
		private void connection()
		{
			string constr = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
			con = new SqlConnection(constr);
		}
		
		// GET: Inventory
		public ActionResult Index()
        {
            return View();
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

		// GET: Inventory/Create
		[HttpGet]
		public ActionResult AddInventory()
		{
			InventoryViewModel inventory = new InventoryViewModel();
			ScoutEntities db = new ScoutEntities();
			
			//Product Dropdown
			var getProductList = db.Products.ToList();
			inventory.ProductList = new SelectList(getProductList, "ProductID", "ProductDesc");

			//Bin Dropdown
			var getBinList = db.Bins.ToList();
			inventory.BinList = new SelectList(getBinList, "BinID", "BinName");

			return View("~/Views/Inventory/AddInventory.cshtml", inventory);
		}

		// POST: Inventory/Create
		[HttpPost]
		public ActionResult AddInventory(InventoryViewModel InventoryItem)
		{
			try
			{
				if (InventoryItem.SelectedProductID != 0 && InventoryItem.SelectedBinID != 0 && InventoryItem.QTY != 0)
				{
					ViewBag.Message = InsUpd_Inventory(InventoryItem);
				}

				return AddInventory();
			}
			catch
			{
				ViewBag.Message = "Error";
				return AddInventory();
			}
		}

		//Calling InsUpd Stored procedure
		public string InsUpd_Inventory(InventoryViewModel obj)
		{
			try
			{
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_InsUpd_Inventory", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@InventoryID", obj.InventoryID);
				SQL.Parameters.AddWithValue("@ProductID", obj.SelectedProductID);
				SQL.Parameters.AddWithValue("@BinID", obj.SelectedBinID);
				SQL.Parameters.AddWithValue("@QTY", obj.QTY);
				SqlDataReader reader = SQL.ExecuteReader();
				InventoryViewModel inventory = new InventoryViewModel();
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

		// GET: Inventory/Edit/5
		[HttpGet]
		public ActionResult EditInventory(int id)
		{
			connection();
			con.Open();
			SqlCommand SQL = new SqlCommand("usp_Get_Inventory", con);
			SQL.CommandType = CommandType.StoredProcedure;
			SQL.Parameters.AddWithValue("@InventoryID", id);
			SqlDataReader reader = SQL.ExecuteReader();
			InventoryViewModel inventory = new InventoryViewModel();
			int ProductID = 0;
			int BinID = 0;

			if (reader.Read())
			{
				inventory.InventoryID = id;
				ProductID = Int32.Parse(reader["ProductID"].ToString());
				BinID = Int32.Parse(reader["BinID"].ToString());
				inventory.QTY = Int32.Parse(reader["QTY"].ToString());
			}
			reader.Close();
			con.Close();

			ScoutEntities db = new ScoutEntities();

			//Product Dropdown
			var getProductList = db.Products.ToList();
			inventory.ProductList = new SelectList(getProductList, "ProductID", "ProductDesc");
			inventory.SelectedProductID = ProductID;

			//Bin Dropdown
			var getBinList = db.Bins.ToList();
			inventory.BinList = new SelectList(getBinList, "BinID", "BinName");
			inventory.SelectedBinID = BinID;

			return View("~/Views/Inventory/EditInventory.cshtml", inventory);

		}

		// POST: Bin/Edit/5
		[HttpPost]
		public ActionResult EditInventory(InventoryViewModel InventoryItem)
		{
			try
			{
				if (InventoryItem.SelectedProductID != 0 && InventoryItem.SelectedBinID != 0 && InventoryItem.QTY != 0)
				{
					ViewBag.Message = InsUpd_Inventory(InventoryItem);
				}
				return EditInventory(InventoryItem.InventoryID);
			}
			catch
			{
				ViewBag.Message = "Error";
				return EditInventory(InventoryItem.InventoryID);
			}
		}

		// POST: Bin/Delete/5
		public ActionResult DeleteInventory(int id)
		{
			try
			{
				// TODO: Add delete logic here
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_Delete_Inventory", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@InventoryID", id);
				int i = SQL.ExecuteNonQuery();
				if (i >= 1)
				{
					ViewBag.Message = "ID: " + id + " has been successfully deleted.";
				}
				else
				{
					ViewBag.Message = "ID: " + id + " has been not been deleted.";
				}
				con.Close();

				ViewBag.Title = "Inventory";

				ScoutEntities db = new ScoutEntities();
				List<Inventory> inventoryList = db.Inventories.ToList();

				InventoryViewModel inventoryVM = new InventoryViewModel();

				List<InventoryViewModel> inventoryVMList = inventoryList.Select(x => new InventoryViewModel { InventoryID = x.InventoryID, ProductDesc = x.Product.ProductDesc, BinName = x.Bin.BinName, QTY = x.QTY }).ToList();

				return View("~/Views/Inventory/Inventory.cshtml", inventoryVMList);
			}
			catch
			{
				ViewBag.Title = "Inventory";

				ScoutEntities db = new ScoutEntities();
				List<Inventory> inventoryList = db.Inventories.ToList();

				InventoryViewModel inventoryVM = new InventoryViewModel();

				List<InventoryViewModel> inventoryVMList = inventoryList.Select(x => new InventoryViewModel { InventoryID = x.InventoryID, ProductDesc = x.Product.ProductDesc, BinName = x.Bin.BinName, QTY = x.QTY }).ToList();

				return View("~/Views/Inventory/Inventory.cshtml", inventoryVMList);
			}
		}
	}
}
