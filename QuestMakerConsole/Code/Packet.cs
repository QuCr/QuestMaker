using System;
using System.Collections.Generic;
using System.Linq;
using QuestMaker.Data;

namespace QuestMaker.Code {
	[Flags]
	public enum HandlerEnum {
		flagList = 1,
		flagDataObject = 2,
		flagType = 4,
		flagEditor = 8,
		flagViewer = 16,
		flagTree = 32,

		Null = 0
		, Single = flagViewer | flagDataObject
		, DummyArray = flagViewer | flagList
		, Array = flagViewer | flagList | flagDataObject
		, Type = flagViewer | flagList | flagType | flagDataObject
		, Update = flagEditor | flagDataObject
		, Edit = flagViewer
		, EditUpdate = flagViewer | flagEditor | flagTree
		, SingleEditor = flagEditor | flagDataObject
	}

	/// <summary>
	/// Packets are used for detemining how data should be presented.
	/// Packets say if it is a list or not; if it refernces 0, 1 or many DataObjects;
	/// if it references all instances of a DataObject type; if it's just a string or integer.
	/// All these different cases must be handled differently by the UI.
	/// </summary>
	public abstract class Packet {
		public Type type;
		public List<Entity> entities = new List<Entity>();
		public HandlerEnum handlerEnum = HandlerEnum.Null;

		/// <summary> handlerEnum.HasFlag(HandlerEnum.flagDataObject); </summary>
		public bool hasDataObjects => handlerEnum.HasFlag(HandlerEnum.flagDataObject);
		/// <summary> handlerEnum.HasFlag(HandlerEnum.flagList); </summary>
		public bool isList => handlerEnum.HasFlag(HandlerEnum.flagList);

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
			throw new Exception("You gave a single string and the type is not an DataObject! " +
				"This means you want to make an entity for a single string, int, etc." +
				"Best practices are to not use an entity or packets, and to directly use the string." +
				"	If this is entirely not possible, go ahead!");
		}

		/// <summary> Creates a packet (Array) for the given entities </summary>
		public static PacketArray byEntity(params Entity[] dataObjects) {
			return new PacketArray(dataObjects);
		}

		/// <summary> Creates a packet (Single) for the given entity </summary>
		public static PacketSingle byEntity(Entity dataObject) {
			return new PacketSingle(dataObject);
		}

		/// <summary> Creates a packet (Type) for the given type </summary>
		public static PacketType byType(Type type) {
			return new PacketType(type);
		}
	}

	/// <summary>
	/// Packet for a single DataObject
	/// </summary>
	public sealed class PacketSingle : Packet {
		public PacketSingle(Entity dataObject) {
			this.handlerEnum = HandlerEnum.Single;
			this.type = dataObject.GetType();
			this.entities.Add(dataObject);
		}

		public override string ToString() => $"Single<{type.Name}>({entities[0].id})";
	}

	/// <summary>
	/// Packet for an 0, 1 or many Dummy DataObjects. Each dummy has one value.
	/// </summary>
	public sealed class PacketDummyArray : Packet {
		public PacketDummyArray(Type type, params string[] data) {
			this.handlerEnum = HandlerEnum.DummyArray;
			this.type = type;

			for (int i = 0;i < data.Length;i++) {
				this.entities.Add(new Dummy(i, data[i]));
			}
		}

		public override string ToString() => $"DummyArray<{type.Name}>[{entities.Count}]";
	}

	/// <summary>
	/// Packet for an 0, 1 or many DataObjects.
	/// </summary>
	public sealed class PacketArray : Packet {
		public PacketArray(Entity[] dataObjects) {
			this.handlerEnum = HandlerEnum.Array;
			this.type = dataObjects.FirstOrDefault().GetType();
			this.entities = dataObjects.ToList();
		}

		public override string ToString() => $"Array<{type.Name}>[{entities.Count}]";
	}

	/// <summary>
	/// Packet for all DataObjects of given type.
	/// </summary>
	public sealed class PacketType : Packet {
		/// <param name="type">Packeted type of the DataObjects</param>
		public PacketType(Type type) {
			this.handlerEnum = HandlerEnum.Type;
			this.type = type;
			this.entities = EntityCollection.getTypeArray(type); ;
		}

		public override string ToString() => $"Type<{type.Name}>[{entities.Count}]";
	}

	/// <summary>
	/// Packet a value of DataObject that is going to be edited, referencing a packet for the edited value.
	/// Used when editing the value by selecting the dataobjects from the underlying reuqest.
	/// </summary>
	public sealed class PacketEdit : Packet {
		public Packet packet = null;
		public string field = null;

		/// <param name="packet">Underlying packet</param>
		/// <param name="dataObject">DataObject that is being edited</param>
		/// <param name="type">Type of the edited field</param>
		/// <param name="field">Name of the edited field</param>
		public PacketEdit(Packet packet, Entity dataObject, Type type, string field) {
			this.packet = packet;
			this.entities.Add(dataObject);
			this.type = type.IsGenericType ? type.GetGenericArguments()[0] : type;
			this.field = field;
			this.handlerEnum = HandlerEnum.Edit;
		}

		public override string ToString() => 
			$"Edit{packet.handlerEnum}<{packet.type?.Name}>[{packet.entities.Count}] from {packet}";
	}

	/// <summary>
	/// Packet an update for the edit of the DataObject, referening the underlying packet for the edit.
	/// Used when (de)selecting an item when editing
	/// </summary>
	public sealed class PacketEditUpdate : Packet {
		public PacketEdit packetEdit;
		public Entity value;
		public bool isSelected;

		public PacketEditUpdate(PacketEdit packetEdit, Entity value, bool isSelected) {
			this.type = packetEdit.entities[0].GetType();
			this.packetEdit = packetEdit;
			this.value = value;
			this.entities = packetEdit.entities;
			this.isSelected = isSelected;
			this.handlerEnum = HandlerEnum.EditUpdate;
		}

		public override string ToString() {
			PacketEdit PE = packetEdit as PacketEdit;

			return $"EditUpdate<{type.Name}>({PE.entities.First().id})<{PE.type.Name}>[{value.id}] " +
				$"{(isSelected ? "+" : "-")}";
		}
	}

	/// <summary>
	/// Updates only the editor
	/// </summary>
	public sealed class PacketSingleEditor : Packet {
		public PacketSingleEditor(Entity dataObject) {
			this.handlerEnum = HandlerEnum.SingleEditor;
			this.type = dataObject.GetType();
			this.entities.Add(dataObject);
		}

		public override string ToString() => $"SingleEdit<{type.Name}>({entities[0].id})";
	}
}