﻿using QuestMaker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuestMaker.Code {
	[Flags]
	public enum HandlerEnum {
		flagList = 1,
		flagEntity = 2,
		flagType = 4,
		flagEditor = 8,
		flagViewer = 16,
		flagTree = 32,
		flagRef = 64,

		Null = 0
		, Single =			flagViewer									|	flagEntity
		, DummyArray =		flagViewer	|					flagList
		, Array =			flagViewer	|					flagList	|	flagEntity
		, Type =			flagViewer	|	flagEditor	|	flagList	|	flagEntity | flagType
		, Update =			flagViewer	|	flagEditor	|	flagTree	|	flagEntity
		, Edit =			flagRef										|   flagEntity
		, SingleEditor =					flagEditor	|					flagEntity

		/*FOR SUDDEN UNINTENDED REFORMATS.
		THESE TABS WILL NOT BE REMOVED WHEN AUTO REFORMATTING

		Null = 0
		, Single =			flagViewer									|	flagEntity
		, DummyArray =		flagViewer	|					flagList
		, Array =			flagViewer	|					flagList	|	flagEntity
		, Type =			flagViewer	|	flagEditor	|	flagList	|	flagEntity | flagType
		, Update =			flagViewer	|	flagEditor	|	flagTree	|	flagEntity
		, Edit =			flagRef										|   flagEntity
		, SingleEditor =					flagEditor	|					flagEntity

		*/
	}

	/// <summary>
	/// Packets are used for detemining how data should be presented.
	/// Packets say if it is a list or not; if it references 0, 1 or many objects;
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
		public static PacketArray byEntity(Entity[] entities, Type type = null) {
			return new PacketArray(entities, type);
		}

		/// <summary> Creates a packet (Array) for the given entities </summary>
		public static PacketArray byEntity(params Entity[] entities) {
			return new PacketArray(entities, null);
		}

		/// <summary> Creates a packet (Single) for the given entity </summary>
		public static PacketSingle byEntity(Type type, Entity entity) {
			return new PacketSingle(entity);
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

			for (int i = 0; i < data.Length; i++) {
				entities.Add(new Dummy(i, data[i]));
			}
		}

		public override string ToString() => $"DummyArray<{type.Name}>[{entities.Count}]";
	}

	/// <summary>
	/// Packet for an 0, 1 or many entities.
	/// </summary>
	public sealed class PacketArray : Packet {
		//TODO: type kan niet worden gevonden als de lijst leeg is
		// -> Verwijder alle waypoints uit een route en probeer het project te updaten
		public PacketArray(Entity[] entities, Type type) {
			handlerEnum = HandlerEnum.Array;
			this.type = type ?? entities.FirstOrDefault().GetType();
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
			entities = EntityCollection.getTypeArray(type);
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

		public override string ToString() => $"Update";
	}

	/// <summary>
	/// Packet with a value of entities that is going to be edited, referencing a packet for the edited value.
	/// Used when editing the value by selecting the entities from the underlying request.
	/// </summary>
	public sealed class PacketEdit : Packet {
		public Packet packet;

		public Entity entity;
		public FieldInfo field;

		/// <param name="packet">Underlying packet</param>
		public PacketEdit(Packet packet, Entity entity, FieldInfo field) {
			this.packet = packet;
			this.field = field;
			this.entity = entity;

			entities = packet.entities;
			type = packet.type.IsGenericType ? packet.type.GetGenericArguments()[0] : packet.type;
			handlerEnum = HandlerEnum.Edit;
		}

		public override string ToString() =>
			$"Edit<{packet.type?.Name}>[{packet.entities.Count}] from {packet}";
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