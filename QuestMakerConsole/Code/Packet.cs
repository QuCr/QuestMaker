using System;
using System.Collections.Generic;
using System.Linq;
using QuestMaker.Data;

namespace QuestMaker.Code {
	[Flags]
	public enum HandlerEnum {
		flagList = 1,
		flagEntity = 2,
		flagType = 4,
		flagEditor = 8,
		flagViewer = 16,
		flagTree = 32,

		Null = 0
		, Single =			flagViewer									|	flagEntity					//18
		, DummyArray =		flagViewer	|	flagList														//17
		, Array =			flagViewer	|	flagList	|					flagEntity					//19
		, Type =			flagViewer	|	flagEditor	|	flagList	|	flagType	|	flagEntity	//31
		, Update =			flagViewer	|	flagEditor	|					flagEntity					//26
		, Edit =			flagViewer									|   flagEntity					//18
		, EditUpdate =		flagViewer	|	flagEditor	|	flagTree										//56
		, SingleEditor =					flagEditor	|					flagEntity                  //10

		/*FOR SUDDEN UNINTENDED REFORMATS.
		TABS WILL NOT BE REMOVED WHEN AUTO REFORMATTING

		Null = 0
		, Single =			flagViewer									|	flagEntity					//18
		, DummyArray =		flagViewer	|	flagList														//17
		, Array =			flagViewer	|	flagList	|					flagEntity					//19
		, Type =			flagViewer	|	flagEditor	|	flagList	|	flagType	|	flagEntity	//31
		, Update =			flagViewer	|	flagEditor	|					flagEntity					//26
		, Edit =			flagViewer									|   flagEntity					//18
		, EditUpdate =		flagViewer	|	flagEditor	|	flagTree										//56
		, SingleEditor =					flagEditor	|					flagEntity                  //10

		*/
	}

	/// <summary>
	/// Packets are used for detemining how data should be presented.
	/// Packets say if it is a list or not; if it references 0, 1 or many Entities;
	/// if it references all instances of a Entity type; if it's just a string or integer.
	/// All these different cases must be handled differently by the UI.
	/// </summary>
	public abstract class Packet {
		public Type type;
		public List<Entity> entities = new List<Entity>();
		public HandlerEnum handlerEnum = HandlerEnum.Null;

		/// <summary> handlerEnum.HasFlag(HandlerEnum.flagEntity); </summary>
		public bool hasEntities => handlerEnum.HasFlag(HandlerEnum.flagEntity);
		/// <summary> handlerEnum.HasFlag(HandlerEnum.flagList); </summary>
		public bool isList => handlerEnum.HasFlag(HandlerEnum.flagList);

		public Entity getEntity() => entities[0];

		protected Packet() { }

		public override string ToString() => throw new NotImplementedException();

		/// <summary> Creates a packet (Single, Type or DummyArray) for the given values. </summary>
		public static Packet byString(Type type, params string[] values) {
			if (typeof(Entity).IsAssignableFrom(type)) {
				List<Entity> entities = EntityCollection.byIDs(type, values);

				if (entities.FirstOrDefault() != null)
					return new PacketSingle(entities[0]);
				if (values == null)
					return new PacketType(entities[0].GetType());
			}
			return new PacketDummyArray(values[0].GetType(), values);                                                          //return new PacketDummySingle( type, ids[0] ); //TODO: kijken of deze gebruikt wordt
		}

		/// <summary> Creates packet (Single) for the given ID. </summary>
		public static PacketSingle byString(Type type, string value) {
			Entity entity = EntityCollection.byID(type, value);

			if (entity != null)
				return new PacketSingle(
					EntityCollection.getSingle(entity.GetType(), value)
				);
			throw new Exception("You gave a single string and the type is not an entity! " +
				"This means you want to make an entity for a single string, int, etc." +
				"Best practices are to not use an entity or packets, and to directly use the string." +
				"	If this is entirely not possible, go ahead!");
		}

		/// <summary> Creates a packet (Array) for the given entities </summary>
		public static PacketArray byEntity(params Entity[] entities) {
			return new PacketArray(entities);
		}

		/// <summary> Creates a packet (Single) for the given entity </summary>
		public static PacketSingle byEntity(Entity entities) {
			return new PacketSingle(entities);
		}

		/// <summary> Creates a packet (Type) for the given type </summary>
		public static PacketType byType(Type type) {
			return new PacketType(type);
		}
	}

	/// <summary>
	/// Packet for a single entity
	/// </summary>
	public sealed class PacketSingle : Packet {
		public PacketSingle(Entity entity) {
			handlerEnum = HandlerEnum.Single;
			type = entity.GetType();
			entities.Add(entity);
		}

		public override string ToString() => $"Single<{type.Name}>({getEntity().id})";
	}

	/// <summary>
	/// Packet for an 0, 1 or many Dummy entities. Each dummy has one value.
	/// </summary>
	public sealed class PacketDummyArray : Packet {
		public PacketDummyArray(Type type, params string[] data) {
			handlerEnum = HandlerEnum.DummyArray;
			this.type = type;

			for (int i = 0;i < data.Length;i++) {
				entities.Add(new Dummy(i, data[i]));
			}
		}

		public override string ToString() => $"DummyArray<{type.Name}>[{entities.Count}]";
	}

	/// <summary>
	/// Packet for an 0, 1 or many entities.
	/// </summary>
	public sealed class PacketArray : Packet {
		public PacketArray(Entity[] entities) {
			handlerEnum = HandlerEnum.Array;
			type = entities.FirstOrDefault().GetType();
			base.entities = entities.ToList();
		}

		public override string ToString() => $"Array<{type.Name}>[{entities.Count}]";
    }

    /// <summary>
    /// Packet for all entities of given type.
    /// </summary>
    public sealed class PacketType : Packet {
        /// <param name="type">Packeted type of the entities</param>
        public PacketType(Type type) {
            handlerEnum = HandlerEnum.Type;
            this.type = type;
            entities = EntityCollection.getTypeArray(type); ;
        }

        public override string ToString() => $"Type<{type.Name}>[{entities.Count}]";
    }

    /// <summary>
    /// Packet for updating the controls
    /// </summary>
    public sealed class PacketUpdate : Packet {
        public PacketUpdate() {
            handlerEnum = HandlerEnum.Update;
        }

        public override string ToString() => $"Type<{type.Name}>[{entities.Count}]";
    }


	/// <summary>
	/// Packet with a value of entities that is going to be edited, referencing a packet for the edited value.
	/// Used when editing the value by selecting the entities from the underlying request.
	/// </summary>
	public sealed class PacketEdit : Packet {
		public Packet packet = null;
		public string field = null;

		/// <param name="packet">Underlying packet</param>
		/// <param name="field">Name of the edited field</param>
		public PacketEdit(Packet packet, string field) {
			this.packet = packet;
			entities.AddRange(EntityCollection.get(packet));
			type = packet.type.IsGenericType ? packet.type.GetGenericArguments()[0] : packet.type;
			this.field = field;
			handlerEnum = HandlerEnum.Edit;
		}

		public override string ToString() => 
			$"Edit{packet.handlerEnum}<{packet.type?.Name}>[{packet.entities.Count}] from {packet}";
	}

	/// <summary>
	/// Packet with an update for the edit of the entities, referening the underlying packet for the edit.
	/// Used when (de)selecting an item when editing
	/// </summary>
	public sealed class PacketEditUpdate : Packet {
		public PacketEdit packetEdit;
		public Entity value;
		public bool? isSelected;

		public PacketEditUpdate(PacketEdit packetEdit, Entity value, bool isSelected) {
			type = packetEdit.getEntity().GetType();
			this.packetEdit = packetEdit;
			this.value = value;
			entities = packetEdit.entities;
			this.isSelected = isSelected;
			handlerEnum = HandlerEnum.EditUpdate;
		}

		public PacketEditUpdate(PacketEdit packetEdit, Entity value) {
			type = packetEdit.getEntity().GetType();
			this.packetEdit = packetEdit;
			this.value = value;
			entities = packetEdit.entities;
			handlerEnum = HandlerEnum.EditUpdate;
		}

		public override string ToString() {
			PacketEdit PE = packetEdit as PacketEdit;
			char isSelectedChar = ' ';
			
			if (isSelected.HasValue)
				isSelectedChar = isSelected.Value ? '+' : '-';

			return $"EditUpdate<{type.Name}>({PE.entities.First().id})<{PE.type.Name}>[{value.id}] {isSelectedChar}";
		}
	}

	/// <summary>
	/// Updates only the editor
	/// </summary>
	public sealed class PacketSingleEditor : Packet {
        public PacketSingleEditor(Packet packet) {
            handlerEnum = HandlerEnum.SingleEditor;
            entities = EntityCollection.get(packet);
            type = getEntity().GetType();
        }

        public PacketSingleEditor(Entity entity) {
            handlerEnum = HandlerEnum.SingleEditor;
            entities.Add(entity);
            type = entity.GetType();
        }

        public override string ToString() => $"SingleEditor<{type.Name}>({getEntity().id})";
	}
}
 