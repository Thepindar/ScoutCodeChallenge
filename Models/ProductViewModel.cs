using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scout.Models
{
	public class ProductViewModel
	{
		public int ProductID { get; set; }
		[Required(ErrorMessage = "Please enter SKU.")]
		public string SKU { get; set; }
		[Required(ErrorMessage = "Please enter Product Desc.")]
		public string ProductDesc { get; set; }
	}
}