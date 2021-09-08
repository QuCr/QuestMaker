using Newtonsoft.Json;
using QuestMaker.Code.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuestMaker.Data {
	public class Entity {
		public const int ORDER_ID = -200;
		public const int ORDER_NAME = -100;

		[JsonProperty(Order = ORDER_ID)] public string id;
		[JsonProperty(Order = ORDER_NAME)] public string displayName;

		public void activate() => EntityCollection.entityCollection.activate(this);
		public void deactivate(string oldID = "") => EntityCollection.entityCollection.deactivate(this, oldID);

		public Entity() {
			id = Guid.NewGuid().ToString();
			displayName = id;
		}

		public Entity(string id, bool activate = true) {
			if (id == null || id.Length < 1)
				throw new ArgumentNullException("id is null or empty");

			this.id = id;
			displayName = id;

			if (activate) //only false for dummy objects
				this.activate();
		}

		public string ToMasterString() => displayName;
		public string ToDetailedString() => displayName;

		public override string ToString() {
			return id;
		}

		public static Entity createType(Type type = null, bool activate = true) {
			if (type == null)
				type = MethodBase.GetCurrentMethod().DeclaringType;

			var entity = Activator.CreateInstance(type, false) as Entity;

			if (activate)
				entity.activate();

			return entity;
		}
	}

	/// <summary> Used for showing values in lists, cannot be real entities</summary>
	[DataViewer(mock = true)]
	public class Dummy : Entity {
		public object value;

		public Dummy() : base() { }
		public Dummy(int id, object value) : base(id.ToString(), false) {
			this.value = value;
		}
	}

	[DataViewer(order = 100)]
	public class Waypoint : Entity {
		[JsonProperty(Order = 1)] public double x = 0;
		[JsonProperty(Order = 2)] public double y = 70;
		[JsonProperty(Order = 3)] public double z = 0;

		public Waypoint() : base() { }
		public Waypoint(string id, bool activate = true) : base(id, activate) { }

		public Waypoint(string id, double x, double z, bool activate = true) : base(id, activate) {
			this.x = x;
			this.z = z;
		}

		public Waypoint(string id, double x, double y, double z, bool activate = true) : base(id, activate) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	[DataViewer(order = 100)]
	public class Variable : Entity {
		[JsonProperty(Order = 1)] public string value;
		[JsonProperty(Order = 1)] public bool global;
		[JsonProperty(Order = 1)] public bool used;

		public Variable() : base() { }
		public Variable(string id, bool activate = true) : base(id, activate) { }

		public Variable(string id, string value, bool global, bool used, bool activate = true) : base(id, activate) {
			this.value = value;
			this.global = global;
			this.used = used;
		}
	}

	[DataViewer(order = 200)]
	public class Person : Entity {
		[JsonProperty(Order = 1)] public string entityName = "armor_stand";
		[JsonProperty(Order = 101)] public double speed = 0.25;
		[JsonProperty(Order = 102)] public double rotation = 0;

		[JsonProperty(Order = 500)] public Waypoint start;

		public Person(string id, bool activate = true) : base(id, activate) { }
		public Person() : base() { }
	}
	[DataViewer(order = 300)]
	public class Route : Entity {
		//static bool killWhenStopped = false;
		static bool announceWhenStarted = true;
		static bool announceWhenExecuting = false;
		static bool announceWhenStopped = false;
		[JsonProperty(Order = 1)] public List<Waypoint> waypoints;
		[JsonProperty(Order = 2)] public Person person;
		[JsonProperty(Order = 2)] public int duration = 300;

		public Route(string id, bool activate = true) : base(id, activate) { }
		public Route() : base() { }

		[File("data/questmaker/functions/routes/start_hard/{id}.mcfunction")]
		public string start_hard() {
			string text = $"";
			if (announceWhenStarted)
				text += $"tellraw @a \"[Function Started - {displayName}]\"\n";

			text += $"function questmaker:routes/tick/{id}\n" +
					$"schedule function questmaker:routes/stop/{id} {duration}t\n" +
					$"kill @e[tag=ACTOR_{id}]\n" +
					$"summon " + person.entityName + " " + waypoints[0].x + " " + waypoints[0].y + " " + waypoints[0].z + " " +
					"{CustomName:\"{\\\"text\\\":\\\"" + person.displayName + "\\\",\\\"color\\\":\\\"gold\\\",\\\"bold\\\":\\\"true\\\",\\\"italic\\\":\\\"true\\\"}\"," +
					"CustomNameVisible: 1b," +
					$"Tags: [\"ACTOR_{id}\"]," +
					"ShowArms:1b,Invulnerable:1b,Silent:1b,NoBasePlate:1b,DisabledSlots:2039583,NoAI:1b," +
					"ArmorItems:[{id:\"golden_boots\",Count:1b},{id:\"golden_leggings\",Count:1b},{id:\"golden_chestplate\",Count:1b},{id:\"player_head\",Count:1b,tag:{SkullOwner:\"QuintenOne\"}}],HandItems:[{id:\"shield\",Count:1b},{id:\"golden_axe\",Count:1b}]}" +
					"\n\n";

			text += $"kill @e[tag=TARGET_{id}]\n";
			text += "\n";

			foreach (Waypoint waypoint in waypoints) {
				text += "summon armor_stand " + waypoint.x + " " + waypoint.y + " " + waypoint.z + " " +
						"{CustomName:\"{\\\"text\\\":\\\"" + waypoint.displayName + "\\\",\\\"color\\\":\\\"blue\\\",\\\"bold\\\":\\\"true\\\",\\\"italic\\\":\\\"true\\\"}\"," +
						 "CustomNameVisible: 1b," +
						 $"Tags: [\"TARGET_{id}\"]," +
						 "DisabledSlots:2039583,Invisible:0b,Invulnerable:0b,NoBasePlate:1b,NoGravity:1b,Small:1b," +
						 "}\n";
			}
			return text;
		}

		[File("data/questmaker/functions/routes/start_soft/{id}.mcfunction")]
		public string start_soft() {
			string text = $"";
			if (announceWhenStarted)
				text += $"tellraw @a \"[Function Started - {displayName}]\"\n";

			text += $"function questmaker:routes/tick/{id}\n" +
					$"schedule function questmaker:routes/stop/{id} {duration}t\n";

			text += $"kill @e[tag=TARGET_{id}]\n";
			text += "\n";

			foreach (Waypoint waypoint in waypoints) {
				text += "summon armor_stand " + waypoint.x + " " + waypoint.y + " " + waypoint.z + " " +
						"{CustomName:\"{\\\"text\\\":\\\"" + waypoint.displayName + "\\\",\\\"color\\\":\\\"blue\\\",\\\"bold\\\":\\\"true\\\",\\\"italic\\\":\\\"true\\\"}\"," +
						 "CustomNameVisible: 1b," +
						 $"Tags: [\"TARGET_{id}\"]," +
						 "DisabledSlots:2039583,Invisible:0b,Invulnerable:0b,NoBasePlate:1b,NoGravity:1b,Small:1b," +
						 "}\n";
			}
			return text;
		}

		[File("data/questmaker/functions/routes/tick/{id}.mcfunction")]
		public string tick() {
			string text = $"";
			if (announceWhenExecuting)
				text += $"tellraw @a \"[Function Executing - {displayName}]\"\n";

			text += $"execute as @e[tag=ACTOR_{id},limit=1] at @s facing entity @e[tag=TARGET_{id},limit=1] feet run tp ~ ~ ~\n" +
					$"execute if entity @e[tag=TARGET_{id},limit=1] as @e[tag=ACTOR_{id},limit=1] at @s run tp ^ ^ ^" + person.speed.ToString().Replace(',', '.') + "\n\n" +
					$"execute if entity @e[tag=TARGET_{id}] run schedule function questmaker:routes/{id}/tick 1t\n\n" +
					$"execute unless entity @e[tag=TARGET_{id},distance=..1] at @e[tag=ACTOR_{id},limit=1] run kill @e[tag=TARGET_{id},distance=..1]";
			text += $"\nschedule function questmaker:routes/tick/{id} 1t\n";
			return text;
		}

		[File("data/questmaker/functions/routes/stop/{id}.mcfunction")]
		public string stop() {
			//string text = $"tellraw @a \"[Function Stopped - {displayName}]\"\n";
			string text = $"";
			if (announceWhenStopped)
				text += $"tellraw @a \"[Function Stopped - {displayName}]\"\n";

			//Only works in/after 1.15, will not be cleared with prior MC versions 
			text += $"\nschedule clear questmaker:routes/tick/{id}\n";
			return text;
		}
	}

	[DataViewer(order = 400)]
	public class Dialog : Entity {
		[JsonProperty(Order = 1)] public List<Sentence> sentences = new List<Sentence>();

		public Dialog(string id, bool activate = true) : base(id, activate) { }
		public Dialog() : base() { }

		[File("data/questmaker/functions/dialogs/{id}.mcfunction")]
		public string sentence() {
			int currentTime = 0;
			string data = "";

			for (int i = 0; i < sentences.Count; i++) {
				currentTime += sentences[i].time;
				data += $"schedule function questmaker:sentences/{sentences[i].id} {currentTime}t\n";
			}

			return data;
		}
	}

	[DataViewer(order = 500)]
	public class Sentence : Entity {
		[JsonProperty(Order = 1)] public int time;
		[JsonProperty(Order = 2)] public Person person;
		[JsonProperty(Order = 3)] public List<string> text;

		public Sentence(string id, bool activate = true) : base(id, activate) { }
		public Sentence() : base() { }

		[File("data/questmaker/functions/sentences/{id}.mcfunction")]
		public string sentence() {
			string data = $"tellraw @a \"[{person.displayName}] {text[0]}\"\n";
			for (int i = 1; i < text.Count; i++) {
				data += $"tellraw @a \"{text[i]}\"\n";
			}
			return data;
		}
	}
}