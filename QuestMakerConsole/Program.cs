using QuestMaker.Console.Code.DataAccess;
using QuestMaker.Data;
using System;

namespace QuestMaker.Console {
	public class Program {
		static IDataAccess dataAccess;

		public static void Main(params string[] args) {
			string dateTime = $"{DateTime.Now:HH:mm:ss dd/MM/yy}";

			title("QuestMaker - v0.0.0.5");
			header("                                                                                                                   ");
			header("                                          ***************************                                              ");
			header("                                          || QuestMaker - v0.0.0.5 ||                                              ");
			header("                                          ***************************                                              ");
			header($" Started at {dateTime}                                                          By QuCr (github.com/QuCr)");
			header("                                                                                                                      ");

			if (args.Length == 0) {
				dataAccess = new DefaultDataAccess();   //import & export
			} else if (args[0] == "default") {
				dataAccess = new DefaultDataAccess();    //import & export
			} else if (args[0] == "json") {
				dataAccess = new JsonDataAccess();      //import & export
			} else {
				error("DataAccess is not defined, resorting to default data access.");
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

		static void Write(string text, char letter = '?', ConsoleColor ForegroundColor = ConsoleColor.Gray, ConsoleColor BackgroundColor = System.ConsoleColor.Black) {
			ConsoleColor startingForegroundColor = System.Console.ForegroundColor;
			ConsoleColor startingBackgroundColor = System.Console.BackgroundColor;

			System.Console.ForegroundColor = ConsoleColor.Black;
			System.Console.BackgroundColor = ConsoleColor.White;
			System.Console.Write(" " + letter + " ");
			System.Console.ForegroundColor = ForegroundColor;
			System.Console.BackgroundColor = BackgroundColor;
			System.Console.Write("  ");
			System.Console.Write(text);
			System.Console.ForegroundColor = startingForegroundColor;
			System.Console.BackgroundColor = startingBackgroundColor;
		}

		public static void title(string title) { System.Console.Title = title; }
		public static void header(string text) { Write(text, ' ', ConsoleColor.Black, ConsoleColor.White); }
		public static void info(string text) { Write(text + "\n", 'I', ConsoleColor.Blue); }
		public static void debug(object text) { Write(text.ToString() + "\n", 'D', ConsoleColor.Green); }
		public static void error(string text) { Write(text + "\n", 'E', ConsoleColor.Red); }
	}
}
