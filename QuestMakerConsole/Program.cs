using QuestMaker.Data;
using QuestMaker.Console.Code.DataAccess;

namespace QuestMaker.Console {
	public class Program {
		public static IDataAccess dataAccess;

		public static void Main(params string[] args) {
			title("QuestMaker - v0.0.0.5");
			header("*********************************************");
			header("||          QuestMaker - v0.0.0.5          ||");
			header("*********************************************");
			error("hey");

			if (args.Length == 0) {
				dataAccess = new DefaultDataAccess();   //import & export
			} else if (args[0] == "json") {
				dataAccess = new JsonDataAccess();      //import & export
			} else {
				info("DataAccess is not defined, resorting to default data access.");
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

		static void WriteLine(string text, System.ConsoleColor ForegroundColor = System.ConsoleColor.Gray, System.ConsoleColor BackgroundColor = System.ConsoleColor.Black) {
			System.ConsoleColor startingForegroundColor = System.Console.ForegroundColor;
			System.ConsoleColor startingBackgroundColor = System.Console.BackgroundColor;

			System.Console.ForegroundColor = ForegroundColor;
			System.Console.BackgroundColor = BackgroundColor;
			System.Console.WriteLine(text);
			System.Console.ForegroundColor = startingForegroundColor;
			System.Console.BackgroundColor = startingBackgroundColor;
		}

		public static void title(string title) { System.Console.Title = title; }
		public static void header(string text) { WriteLine(text, System.ConsoleColor.Black, System.ConsoleColor.White); }
		public static void info(string text) { WriteLine(text, System.ConsoleColor.Blue); }
		public static void debug(string text) { WriteLine(text, System.ConsoleColor.Green); }
		public static void error(string text) { WriteLine(text, System.ConsoleColor.Red); }
	}
}
