﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TypeLite.Demo.Models {
	[TsClass(Module = "Eshop")]
	public class Order {
		public Product[] Products { get; set; }
		public decimal TotalPrice { get; set; }
	}
}