using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scout.Models
{
	public class CustomerOrderViewModel
	{
		public int OrderID { get; set; }
		public System.DateTime DateOrdered { get; set; }
		public string ProductDesc { get; set; }
		[Required]
		public string CustomerName { get; set; }
		[Required]
		public string CustomerAddress { get; set; }

		public int QTY { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Please enter Product desc.")]
		public int SelectedProductID { get; set; }
		public SelectList ProductList { get; set; }
	}
}