﻿using Newtonsoft.Json;
using QuestMaker.Code;
using QuestMaker.Data;
using System.IO;

namespace QuestMaker.Console.Code.DataAccess {
	public class JsonDataAccess : IDataAccess {
		public bool isImportable() => true;
		public bool isExportable() => true;

		//Settings used for im- and exporting the data
		public const Formatting formatting = Formatting.Indented;
		public const PreserveReferencesHandling preserveReferencesHandling = PreserveReferencesHandling.Objects;
		public const TypeNameHandling typeNameHandling = TypeNameHandling.All;

		public void import() {
			string json;
			using (StreamReader reader = new StreamReader(Project.Path + "/questmaker.json")) {
				json = reader.ReadToEnd();
			}

			EntityCollection collection = JsonConvert.DeserializeObject<EntityCollection>(json, new JsonSerializerSettings {
				PreserveReferencesHandling = preserveReferencesHandling,
				TypeNameHandling = TypeNameHandling.All
			});

			EntityCollection.entityCollection = collection;
		}

		public void export(EntityCollection collection) {
			string json;

			json = JsonConvert.SerializeObject(collection, formatting, new JsonSerializerSettings {
				PreserveReferencesHandling = preserveReferencesHandling,
				TypeNameHandling = typeNameHandling
			});

			using (StreamWriter writer = new StreamWriter(Project.Path + "/questmaker.json")) {
				writer.Write(json);
			}
		}
	}
}
