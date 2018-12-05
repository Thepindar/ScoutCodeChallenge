using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scout.Models
{
	public class BinViewModel
	{
		public int BinID { get; set; }
		[Required(ErrorMessage = "Please enter Bin Name.")]
		public string BinName { get; set; }
	}
}