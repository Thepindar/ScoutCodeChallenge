using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scout.Models
{
	public class InventoryViewModel
	{
		public int InventoryID { get; set; }
		public int ProductID { get; set; }
		public string ProductDesc { get; set; }
		public int BinID { get; set; }
		public string BinName { get; set; }
		[Range(1, int.MaxValue, ErrorMessage = "Please enter a positive non-zero QTY.")]
		public int QTY { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Please enter Product desc.")]
		public int SelectedProductID { get; set; }
		public SelectList ProductList { get; set; }
		[Range(1, int.MaxValue, ErrorMessage = "Please enter Bin Name.")]
		public int SelectedBinID { get; set; }
		public SelectList BinList { get; set; }
	}
}