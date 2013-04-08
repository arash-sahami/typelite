﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLite {
	/// <summary>
	/// Configures property to be ignored by the script generator.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class TsIgnoreAttribute : Attribute {
	}
}
