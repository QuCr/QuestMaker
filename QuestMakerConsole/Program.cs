﻿using System;
using QuestMaker.Code;
using QuestMaker.Data;
using QuestMakerConsole.Code.DataAccess;

namespace QuestMakerConsole {
	public class Program {
		public static IDataAccess dataAccess;

		public static void Main(params string[] args) {
			Console.WriteLine("*********************************************");
			Console.WriteLine("*           QuestMaker - v0.0.0.5           *");
			Console.WriteLine("*********************************************\n");

			switch (args[0]) {
				case "json":
					dataAccess = new JsonDataAccess();		//import & export
					break;
				default:
					dataAccess = new DefaultDataAccess();	//import
					break;
			}

			if (dataAccess.isImportable()) 
				dataAccess.import();

			Console.WriteLine(dataAccess.GetType().Name);
		}

		public static void export() {
			if (dataAccess.isExportable())
				dataAccess.export(EntityCollection.collection);
		}
	}
}