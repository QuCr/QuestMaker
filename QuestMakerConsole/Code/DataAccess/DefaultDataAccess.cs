using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuestMaker.Code;
using QuestMaker.Data;

namespace QuestMaker.Console.Code.DataAccess {
	public class DefaultDataAccess : IDataAccess {
		public bool isImportable() => true;
		public bool isExportable() => true;

		//Settings used for im- and exporting the data
		public const Formatting formatting = Formatting.Indented;
		public const PreserveReferencesHandling preserveReferencesHandling = PreserveReferencesHandling.Objects;
		public const TypeNameHandling typeNameHandling = TypeNameHandling.All;

		public void import() {
			var bed_red = new Waypoint("bed_red", -22, 9) { displayName = "Red Bed" };
			var bed_green = new Waypoint("bed_green", -22, 7) { displayName = "Green Bed" };
			var bed_blue = new Waypoint("bed_blue", -22, 5) { displayName = "Blue Bed" };
			var bed_yellow = new Waypoint("bed_yellow", -22, 3) { displayName = "Yellow Bed" };
			var point1 = new Waypoint("point1", -20, 6) { displayName = "Waypoint 1" };
			var point2 = new Waypoint("point2", -18, 6) { displayName = "Waypoint 2" };
			var point3 = new Waypoint("point3", -14, 6) { displayName = "Waypoint 3" };
			var point4 = new Waypoint("point4", -9, 5) { displayName = "Waypoint 4" };
			var startA = new Waypoint("startA", -7, 5) { displayName = "Start A" };
			var startB = new Waypoint("startB", -4, 5) { displayName = "Start B" };
			var startC = new Waypoint("startC", -9, 1) { displayName = "Start C" };

			Person albert = new Person("albert") {
				start = startA,
				displayName = "Albert",
				entityName = "zombie",
				speed = 0.1,
				rotation = 90
			};
			Person bert = new Person("bert") {
				start = startB,
				displayName = "Bert",
				entityName = "skeleton",
				rotation = 190
			};
			Person cyk = new Person("cyk") {
				start = startC,
				displayName = "Cyk",
				entityName = "husk",
				speed = 0.15,
				rotation = 170
			};

			new Route("route_albert") {
				displayName = "Route Albert",
				person = albert,
				waypoints = new List<Waypoint>() { startA, point4, point3, point2, bed_red }
			};
			new Route("route_bert") {
				displayName = "Route Bert",
				person = bert,
				waypoints = new List<Waypoint>() { startB, point4, point3, point2, point1, bed_green }
			};
			new Route("route_cyk") {
				displayName = "Route Cyk",
				person = cyk,
				waypoints = new List<Waypoint>() { startC, point4, point3, point2, point1, bed_blue }
			};

			Sentence sentence = new Sentence("sentence") {
				time = 10,
				person = bert,
				text = new List<string>() {
					"I don't know what to say.",
					"Do I need to start over?",
					"I'm supposed to say this, right?"
				}
			};

			Sentence sentence2 = new Sentence("sentence_2") {
				time = 20,
				person = cyk,
				text = new List<string>() {
					"I wanna go home!",
					"How long till we're home?",
					"I need to poop!"
				}
			};

			new Dialog("first_dialog") {
				sentences = new List<Sentence>() { sentence }
			};

			new Dialog("second_dialog") {
				sentences = new List<Sentence>() { sentence2 }
			};

			new Dialog("third_dialog") {
				sentences = new List<Sentence>() { sentence, sentence2 }
			};

			new Variable("var_first") {
				displayName = "My variable",
				value = "69420",
				used = true
			};
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
