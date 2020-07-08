using System;

namespace QuestMaker.Code.Attributes {
	/// <summary>
	/// Gives info about the class for the DataViewer
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class DataViewerAttribute : Attribute {
		public int order;
		public bool mock;

		/// <param name="order">The order of the types in the export </param>
		/// <param name="mock">Hidden in tree view </param>
		public DataViewerAttribute(int order = 0, bool mock = false) {
			this.order = order;
			this.mock = mock;
		}
	}
}