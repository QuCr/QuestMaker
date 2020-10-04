using System;
using QuestMaker.Data;
using QuestMaker.Console.Code.DataAccess;

namespace QuestMaker.Console {
	public class Program {
		public static IDataAccess dataAccess;

        public static void WriteLine(string line) { System.Console.WriteLine(line); }
        public static void Write(string line) { System.Console.Write(line); }

		public static void Main(params string[] args) {
			WriteLine("*********************************************");
			WriteLine("||          QuestMaker - v0.0.0.5          ||");
			WriteLine("*********************************************\n");

			if (args[0] == null || args[0] == "default") {
				dataAccess = new DefaultDataAccess();   //import & export
			} else if (args[0] == "json") {
				dataAccess = new JsonDataAccess();      //import & export
			} else {
				WriteLine("DataAcces is not defined, resorting to default data access.");
				dataAccess = new DefaultDataAccess();   //import & export
			}

			if (dataAccess.isImportable()) 
				dataAccess.import();
		}

		public static bool export() {
			if (dataAccess.isExportable())
				dataAccess.export(EntityCollection.entityCollection);
			return dataAccess.isExportable();
		}
	}
}
