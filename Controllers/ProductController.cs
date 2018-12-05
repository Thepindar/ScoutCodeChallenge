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
    public class ProductController : Controller
    {
		private SqlConnection con;

		//Connection activities
		private void connection()
		{
			string constr = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
			con = new SqlConnection(constr);
		}

		// GET: Product
		public ActionResult Index()
        {
            return View();
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

		// GET: Product/Create
		[HttpGet]
		public ActionResult AddProduct()
		{
			return View("~/Views/Product/AddProduct.cshtml");
		}

		// POST: Product/Create
		[HttpPost]
		public ActionResult AddProduct(Product ProductItem)
		{
			try
			{
				if (ModelState.IsValid)
				{
					ViewBag.Message = InsUpd_Product(ProductItem);
				}

				return View("~/Views/Product/AddProduct.cshtml");
			}
			catch
			{
				ViewBag.Message = "Error";
				return View("~/Views/Product/AddProduct.cshtml");
			}
		}

		//Calling InsUpd Stored procedure
		public string InsUpd_Product(Product obj)
		{
			try
			{
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_InsUpd_Product", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@ProductID", obj.ProductID);
				SQL.Parameters.AddWithValue("@SKU", obj.SKU.ToString());
				SQL.Parameters.AddWithValue("@ProductDesc", obj.ProductDesc.ToString());
				SqlDataReader reader = SQL.ExecuteReader();
				ProductViewModel product = new ProductViewModel();
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

		// GET: Bin/Edit/5
		[HttpGet]
		public ActionResult EditProduct(int id)
		{
			connection();
			con.Open();
			SqlCommand SQL = new SqlCommand("usp_Get_Product", con);
			SQL.CommandType = CommandType.StoredProcedure;
			SQL.Parameters.AddWithValue("@ProductID", id);
			SqlDataReader reader = SQL.ExecuteReader();
			ProductViewModel product = new ProductViewModel();
			if (reader.Read())
			{
				product.ProductID = id;
				product.SKU = reader["SKU"].ToString();
				product.ProductDesc = reader["ProductDesc"].ToString();
			}
			reader.Close();
			con.Close();

			return View("~/Views/Product/EditProduct.cshtml", product);

		}

		// POST: Bin/Edit/5
		[HttpPost]
		public ActionResult EditProduct(Product ProductItem)
		{
			try
			{
				if (ModelState.IsValid)
				{
					ViewBag.Message = InsUpd_Product(ProductItem);
				}
				return View("~/Views/Product/EditProduct.cshtml");
			}
			catch
			{
				ViewBag.Message = "Error";
				return View("~/Views/Product/EditProduct.cshtml");
			}
		}

		// POST: Bin/Delete/5
		public ActionResult DeleteProduct(int id)
		{
			try
			{
				// TODO: Add delete logic here
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_Delete_Product", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@ProductID", id);
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

				ViewBag.Title = "Product";

				ScoutEntities db = new ScoutEntities();
				List<Product> productList = db.Products.ToList();

				ProductViewModel productVM = new ProductViewModel();

				List<ProductViewModel> productVMList = productList.Select(x => new ProductViewModel { ProductID = x.ProductID, SKU = x.SKU, ProductDesc = x.ProductDesc }).ToList();

				return View("~/Views/Product/Product.cshtml", productVMList);
			}
			catch
			{
				ViewBag.Title = "Product";

				ScoutEntities db = new ScoutEntities();
				List<Product> productList = db.Products.ToList();

				ProductViewModel productVM = new ProductViewModel();

				List<ProductViewModel> productVMList = productList.Select(x => new ProductViewModel { ProductID = x.ProductID, SKU = x.SKU, ProductDesc = x.ProductDesc }).ToList();

				return View("~/Views/Product/Product.cshtml", productVMList);
			}
		}
	}
}
