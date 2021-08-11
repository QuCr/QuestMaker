using QuestMaker.Code.Attributes;
using QuestMaker.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QuestMaker.Code {
	public class Project {
		public static string Path = @"C:\Users\Quinten\AppData\Roaming\.minecraft\saves\TestWorld\datapacks\TestPack";
		public static string Name = "TestPack";

		public static void generateProject() {
			try {
				Directory.Delete(@"C:\Users\Quinten\AppData\Roaming\.minecraft\saves\TestWorld\datapacks\" + Name, true);
			} catch (Exception) { throw; }

			List<MethodInfo> methods = new List<MethodInfo>();
			foreach (var typeMethods in from type in Assembly.GetAssembly(typeof(Project)).GetTypes()
										select type.GetMethods()) {
				methods.AddRange(from method in typeMethods
								 where method.GetCustomAttribute<FileAttribute>() != null
								 select method);
			}

			foreach (MethodInfo method in methods) {
				FileAttribute attribute = method.GetCustomAttribute<FileAttribute>();
				Type type = method.DeclaringType;

				string filepath = attribute.fullPath.ToLower();

				if (File.Exists(filepath))
					throw new Exception("File already exists, the assigned path is already used");

				if (method.IsStatic) {
					using (StreamWriter file = new StreamWriter(attribute.fullPath)) {
						file.Write(method.Invoke(null, new object[0]));
					}
				} else {
					if (type.IsSubclassOf(typeof(Entity))) {
						foreach (Entity entity in EntityCollection.get(new PacketType(type))) {
							string currentFilepath = filepath
														.Replace("{id}", entity.id)
														.Replace("{ID}", entity.id);

							using (StreamWriter file = new StreamWriter(currentFilepath)) {
								file.Write(method.Invoke(entity, new object[0]));
							}
						}
					} else
						throw new Exception("Non-static functions with FileAttribute must be defined in a Entity");
				}
			}
		}

		[File("pack", "mcmeta")]
		public static string pack() {
			return "{\n\t\"pack\": {\n\t\t\"pack_format\":4,\n\t\t\"description\":\"Datapack for " + Name + "\"\n\t}\n}";
		}

		[File("data/questmaker/functions/load", "mcfunction")]
		public static string load() {
			string data = "";
			foreach (Variable variable in EntityCollection.get(new PacketType(typeof(Variable)))) {
				if (variable.global && variable.used)
					data += $"scoreboard players set {variable.displayName} Global {variable.value}\n";
				else
					data += $"scoreboard objectives add {variable.displayName} dummy\n";
			}

			return "say Reloading Datapack \"" + Name + "\"\n"; // +
																//"scoreboard objectives add Global dummy\n" +
																//"scoreboard objectives setdisplay sidebar Global\n" + data;
		}

		[File("data/minecraft/tags/functions/load", "json")]
		public static string loadMinecraft() {
			return "{ \"values\": [ \"" + "questmaker" + ":load\" ] }";
		}

		[File("data/questmaker/functions/tick", "mcfunction")]
		public static string tick() {
			return "scoreboard players add Tick Global 1\n" +
					"execute if score Tick Global matches 20 run scoreboard players set Tick Global 0";
		}

		//[File("data/questmaker/functions/unload", "mcfunction")]
		public static string unload() {
			return "";
		}

		[File("data/minecraft/tags/functions/tick", "json")]
		public static string tickMinecraft() {
			return "{ \"values\": [ \"" + "questmaker" + ":tick\" ] }";
		}

		[File("data/questmaker/functions/routes/start/all/hard", "mcfunction")]
		public static string start_hard() {
			string text = "";

			foreach (var item in EntityCollection.getTypeArray(typeof(Route))) {
				text += $"function questmaker:routes/start_hard/{item.id}\n";
			}

			return text;
		}

		[File("data/questmaker/functions/routes/start/all/soft", "mcfunction")]
		public static string start_soft() {
			string text = "";

			foreach (var item in EntityCollection.getTypeArray(typeof(Route))) {
				text += $"function questmaker:routes/start_soft/{item.id}\n";
			}
			return text;
		}
	}
}