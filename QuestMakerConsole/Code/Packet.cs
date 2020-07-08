using System;
using System.Collections.Generic;
using System.Linq;
using QuestMaker.Data;

namespace QuestMaker.Code { 
	[Flags]
	public enum HandlerEnum {
		flagList = 1,
		flagDataObject = 2,
		flagViewer = 4,
		flagEditor = 8,
		flagTree = 16,
		
		Null = 0
		, Single =			flagViewer |								flagDataObject
		, DummyArray =		flagViewer |					flagList
		, Array =			flagViewer |					flagList |	flagDataObject
		, Update =							flagEditor |				flagDataObject
		, SingleEditor =					flagEditor |				flagDataObject
		, EditUpdate =		flagViewer |	flagEditor
	}

	/// <summary>
	/// Packets are used for detemining how data should be presented.
	/// Packets say if it is a list or not; if it refernces 0, 1 or many DataObjects;
	/// if it references all instances of a DataObject type; if it's just a string or integer.
	/// All these different cases must be handled differently by the UI.
	/// </summary>
	public abstract class Packet {
		public static Packet previousPacket = null;

		public Type type;
		public List<Entity> entities = new List<Entity>();
		public HandlerEnum handlerEnum = HandlerEnum.Null;

		protected Packet() { }

		public bool hasFlag(HandlerEnum flag) {
			return handlerEnum.HasFlag(flag);
		}

		public override string ToString() {
			try {
				if (handlerEnum == HandlerEnum.Array)
					return $"{this.handlerEnum}<{type.Name}>[{entities.Count}]";
				else
					return $"{this.handlerEnum}<{type?.Name}>({entities?.FirstOrDefault().id})";
			} catch (Exception) {
				if (this is PacketEditUpdate) {
					PacketEditUpdate REU = this as PacketEditUpdate;
					PacketEdit RE = (this as PacketEditUpdate).packetEdit as PacketEdit;

					return $"{this.handlerEnum}<{REU.type.Name}>({RE.entities.First().id})<{RE.type.Name}>[{REU.value.id}] {(REU.isSelected ? "+" : "-")}";
				}

				return "error";
			}
		}

		/// <summary>
		/// Returns DataObject(s) with corresponding IDs
		/// </summary>
		public static Packet createPacketByID(Type type, bool isArray, params string[] ids) {
			if (!isArray && ids.Count() != 1) {
				throw new Exception("The packet hasn't exactly 1 dataObject, so the packet type must be that of an array");
			}

			if (typeof(Entity).IsAssignableFrom(type)) {
				List<Entity> dataObjects = new List<Entity>();
				for (int i = 0;i < ids.Length;i++) {
					dataObjects.Add(EntityCollection.getSingle(type, ids[i]));
				}
				if (dataObjects.FirstOrDefault() != null)
					if (isArray)
						return new PacketArray(type, dataObjects);
					else
						return new PacketSingle(dataObjects[0]);
				if (ids == null)
					return new PacketType(type);
			}
			if (isArray)
				return new PacketDummyArray(type, ids);

			throw new Exception("DEBUG, zet hier een vinkje als deze ooit optreed"); //-> 
																					 //return new PacketDummySingle( type, ids[0] ); //TODO: kijken of deze gebruikt wordt
		}

		/// <summary>
		/// Creates a packet to use for later
		/// </summary>
		public static Packet getPacketWithDataObject(Type type, bool isArray, params Entity[] dataObjects) {
			if (!isArray && dataObjects.Count() != 1)
				throw new Exception("The packet hasn't exactly 1 dataObject, so the packet type must be that of an array");
			if (dataObjects == null)
				return new PacketType(type);
			if (isArray)
				return new PacketArray(type, dataObjects.ToList());
			return new PacketSingle(dataObjects[0]);
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
	}

	/// <summary>
	/// Packet for an 0, 1 or many Dummy DataObjects. Each dummy has one value.
	/// </summary>
	public sealed class PacketDummyArray : Packet {
		public PacketDummyArray(Type type, string[] data) {
			this.handlerEnum = HandlerEnum.DummyArray;
			this.type = type;

			for (int i = 0;i < data.Length;i++) {
				this.entities.Add(new Dummy(i, data[i]));
			}


		}

		public override string ToString() {
			return $"{this.handlerEnum}<{type.Name}>[{entities.Count}]";
		}
	}

	/// <summary>
	/// Packet for an 0, 1 or many DataObjects.
	/// </summary>
	public sealed class PacketArray : Packet {
		public PacketArray(Type type, List<Entity> dataObjects) {
			this.handlerEnum = HandlerEnum.Array;
			this.type = type;
			this.entities = dataObjects;
		}
	}

	/// <summary>
	/// Packet for all DataObjects of given type.
	/// </summary>
	public sealed class PacketType : Packet {
		/// <param name="type">Packeted type of the DataObjects</param>
		public PacketType(Type type) {
			this.handlerEnum = HandlerEnum.Array;
			this.type = type;
			this.entities = EntityCollection.getTypeArray(type); ;
		}
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
			this.handlerEnum = HandlerEnum.flagViewer;
		}

		public override string ToString() {
			return $"Edit{packet.handlerEnum}<{packet.type?.Name}>[{packet.entities.Count}] from {packet}";
		}
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
	}
}