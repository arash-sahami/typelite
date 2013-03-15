﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeLite.TsModels;

namespace TypeLite {
	/// <summary>
	/// Creates a script model from CLR classes.
	/// </summary>
	public class TsModelBuilder {
		/// <summary>
		/// Gets or sets collection of classes in the model being built.
		/// </summary>
		internal Dictionary<Type, TsClass> Classes { get; set; }

		/// <summary>
		/// Initializes a new instance of the TsModelBuilder class.
		/// </summary>
		public TsModelBuilder() {
			this.Classes = new Dictionary<Type, TsClass>();
		}

		/// <summary>
		/// Adds type with all referenced classes to the model.
		/// </summary>
		/// <typeparam name="T">The type to add to the model.</typeparam>
		public void Add<T>() {
			this.Add<T>(true);
		}

		/// <summary>
		/// Adds type and optianlly referenced classes to the model.
		/// </summary>
		/// <typeparam name="T">The type to add to the model.</typeparam>
		/// <param name="includeReferences">bool value indicating whether classes referenced by T should be added to the model.</param>
		public void Add<T>(bool includeReferences) {
			this.Add(typeof(T), includeReferences);
		}

		/// <summary>
		/// Adds type with all referenced classes to the model.
		/// </summary>
		/// <param name="clrType">The type to add to the model.</param>
		public void Add(Type clrType) {
			this.Add(clrType, true);
		}

		/// <summary>
		/// Adds type and optianlly referenced classes to the model.
		/// </summary>
		/// <param name="clrType">The type to add to the model.</param>
		/// <param name="includeReferences">bool value indicating whether classes referenced by T should be added to the model.</param>
		public void Add(Type clrType, bool includeReferences) {
			var typeFamily = TsType.GetTypeFamily(clrType);
			if (typeFamily != TsTypeFamily.Class) {
				throw new ArgumentException(string.Format("Type '{0}' isn't class. Only classes can be added to the model", clrType.FullName));
			}

			if (!this.Classes.ContainsKey(clrType)) {
				var added = new TsClass(clrType);
				if (added.BaseType != null) {
					this.Add(added.BaseType.ClrType);
				}
				if (includeReferences) {
					this.AddReferences(added);
				}
				this.Classes[clrType] = added;
			}
		}

		/// <summary>
		/// Adds all classes annotated with the TsClassAttribute from an assembly to the model.
		/// </summary>
		/// <param name="assembly">The assembly with classes to add</param>
		public void Add(Assembly assembly) {
			foreach (var type in assembly.GetTypes().Where(t => TsType.GetTypeFamily(t) == TsTypeFamily.Class)) {
				this.Add(type);
			}
		}

		/// <summary>
		/// Build the model.
		/// </summary>
		/// <returns>The script model with the classes.</returns>
		public TsModel Build() {
			var model = new TsModel(this.Classes.Values);
			model.RunVisitor(new TypeResolver(model));
			return model;
		}

		/// <summary>
		/// Adds classes referenced by the class to the model
		/// </summary>
		/// <param name="classModel"></param>
		private void AddReferences(TsClass classModel) {
			foreach (var property in classModel.Properties) {
				var propertyTypeFamily = TsType.GetTypeFamily(property.PropertyType.ClrType);
				if (propertyTypeFamily == TsTypeFamily.Collection) {
					var collectionItemType = TsType.GetEnumerableType(property.PropertyType.ClrType);
					if (collectionItemType != null && TsType.GetTypeFamily(collectionItemType) == TsTypeFamily.Class) {
						this.Add(collectionItemType);
					}
				} else if (propertyTypeFamily == TsTypeFamily.Class) {
					this.Add(property.PropertyType.ClrType);
				}
			}
		}
	}


}