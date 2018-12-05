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
    public class BinsController : Controller
	{
		private SqlConnection con;

		//Connection activities
		private void connection()
		{
			string constr = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
			con = new SqlConnection(constr);
		}

		// GET: Bin/Details/5
		public ActionResult Index()
        {
			return View();
        }

        // GET: Bin/Create
		[HttpGet]
        public ActionResult AddBin()
        {
            return View("~/Views/Bins/AddBin.cshtml");
        }

        // POST: Bin/Create
        [HttpPost]
        public ActionResult AddBin(Bin BinItem)
        {
            try
            {
				if (ModelState.IsValid)
				{
					ViewBag.Message = InsUpd_Bins(BinItem);
				}

                return View("~/Views/Bins/AddBin.cshtml");
            }
            catch
            {
				ViewBag.Message = "Error";
				return View("~/Views/Bins/AddBin.cshtml");
            }
        }

		//Calling InsUpd Stored procedure
		public string InsUpd_Bins(Bin obj)
		{
			try
			{
				connection();
				SqlCommand SQL = new SqlCommand("usp_InsUpd_Bins", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@BinID", obj.BinID);
				SQL.Parameters.AddWithValue("@BinName", obj.BinName);
				con.Open();
				SqlDataReader reader = SQL.ExecuteReader();
				BinViewModel bin = new BinViewModel();
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
        public ActionResult EditBins(int id)
        {
			connection();
			con.Open();
			SqlCommand SQL = new SqlCommand("usp_Get_Bins", con);
			SQL.CommandType = CommandType.StoredProcedure;
			SQL.Parameters.AddWithValue("@BinID", id);
			SqlDataReader reader = SQL.ExecuteReader();
			BinViewModel bin = new BinViewModel();
			if (reader.Read())
			{
				bin.BinID = id;
				bin.BinName = reader["BinName"].ToString();
			}
			reader.Close();
			con.Close();

			return View("~/Views/Bins/EditBins.cshtml", bin);

		}

        // POST: Bin/Edit/5
        [HttpPost]
        public ActionResult EditBins(Bin BinItem)
        {
            try
            {
				if (ModelState.IsValid)
				{
					ViewBag.Message = InsUpd_Bins(BinItem);
				}
				return View("~/Views/Bins/EditBins.cshtml");
			}
            catch
			{
				ViewBag.Message = "Error";
				return View("~/Views/Bins/EditBins.cshtml");
			}
        }
				 
		// POST: Bin/Delete/5
        public ActionResult DeleteBins(int id)
        {
            try
            {
				// TODO: Add delete logic here
				connection();
				con.Open();
				SqlCommand SQL = new SqlCommand("usp_Delete_Bins", con);
				SQL.CommandType = CommandType.StoredProcedure;
				SQL.Parameters.AddWithValue("@BinID", id);
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

				ViewBag.Title = "Bins";

				ScoutEntities db = new ScoutEntities();
				List<Bin> binList = db.Bins.ToList();

				BinViewModel binVM = new BinViewModel();

				List<BinViewModel> binVMList = binList.Select(x => new BinViewModel { BinID = x.BinID, BinName = x.BinName }).ToList();

				return View("~/Views/Bins/Bins.cshtml", binVMList);
			}
            catch
            {
				ViewBag.Title = "Bins";

				ScoutEntities db = new ScoutEntities();
				List<Bin> binList = db.Bins.ToList();

				BinViewModel binVM = new BinViewModel();

				List<BinViewModel> binVMList = binList.Select(x => new BinViewModel { BinID = x.BinID, BinName = x.BinName }).ToList();

				return View("~/Views/Bins/Bins.cshtml", binVMList);
			}
        }
    }
}
