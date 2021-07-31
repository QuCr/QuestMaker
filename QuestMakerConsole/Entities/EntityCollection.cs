using QuestMaker.Code;
using QuestMaker.Code.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuestMaker.Data {
	/// <summary>
	/// Used for storing all the data of the project.
	/// Structure is a dictionairy of dictionairies of each type.
	/// Each dictionairy for the types contains the instances of that type.
	/// </summary>
	public class EntityCollection : Dictionary<string, Dictionary<string, Entity>> {
		public static EntityCollection entityCollection = new EntityCollection();
		public static List<Type> types {
			get => getEntityTypes();
		}
		public EntityCollection() {
			foreach (Type type in types) {
				Add(type.Name, new Dictionary<string, Entity>());
			}
		}

		/// <summary>
		/// Gets all Entitys of type T that have the given IDs
		/// </summary>
		/// <remark>
		/// Returns null when there was no item found.
		/// A possible case is trying passing an Dummy through the function.
		/// This could not be constrained, because the constraint will cause problems 
		/// higher up with convertions.
		/// </remark>
		public static List<Entity> get(Packet packet) {
			if (packet is PacketSingle || packet is PacketSingleEditor)
				return new List<Entity>() {
					getSingle(packet.type, packet.entities[0].id)
				};
			if (packet is PacketArray)
				return getArray(packet.type, ( from a in packet.entities select a.id ).ToArray());
			if (packet is PacketType)
				return getTypeArray(packet.type);
			return null; //returns null because it's not a Entity, which is what was asked.
		}

		/// <summary> Searches an entity by its ID. </summary>
		public static Entity byID(Type type, string ID) {
			Dictionary<string, Entity> types = entityCollection[type.Name];
			for (int i = 0; i < types.Count; i++) {
				KeyValuePair<string, Entity> entity = types.ElementAt(i);
				if (entity.Key == ID)
					return entity.Value;
			}
			return null;
		}

		/// <summary> Searches entities by their ID. </summary>
		public static List<Entity> byIDs(Type type, params string[] ids) {
			List<Entity> entities = new List<Entity>();

			foreach (string id in ids) {
				Entity entity = byID(type, id);
				entities.Add(entity);
			}

			return entities;
		}

		/// <summary>
		/// Gets a single Entity
		/// </summary>
		public static Entity getSingle(Type type, string id) {
			return ( from item in entityCollection[type.Name] where id == item.Key select item.Value ).FirstOrDefault();
		}

		/// <summary>
		/// Gets a list with 0, 1 or more Entitys
		/// </summary>
		public static List<Entity> getArray(Type type, params string[] ids) {
			List<Entity> entities = new List<Entity>();
			var s = entityCollection[type.Name];
			entities.AddRange(from item in s where ids.Contains(item.Key) select item.Value);
			return entities;
		}

		/// <summary>
		/// Gets a list with all Entitys of a certain type
		/// </summary>
		public static List<Entity> getTypeArray(Type type) {
			//TODO .toList kan toch gebruikt worden?
			List<Entity> entities = new List<Entity>();
			entities.AddRange(from item in entityCollection[type.Name] select item.Value);
			return entities;
		}

		/// <summary>
		/// Adds the Entity to the collection
		/// </summary>
		/// <exception cref="ArgumentException"/>
		public void activate(Entity entity) {
			if (!isExistingID(entity.id)) {
				this[entity.GetType().Name].Add(entity.id, entity);
			} else
				throw new ArgumentException("An item with the same ID is already activated.");
		}

		/// <summary>
		/// Removes the Entity from the collection
		/// </summary>
		/// <exception cref="KeyNotFoundException"/>
		public void deactivate(Entity entity, string oldID) {
			bool deactivated = TryGetValue(entity.GetType().Name, out Dictionary<string, Entity> data);

			if (!data.Remove(entity.id))
				data.Remove(oldID);

			if (!deactivated)
				throw new KeyNotFoundException("Could not remove the Entity, probably because it wasn't in there");
		}

		/// <summary>
		/// Gets all IDs of the Entitys in the collection
		/// </summary>
		/// <remark>
		/// Called every time a user adds another Entity
		/// </remark>
		public static bool isExistingID(string id) {
			List<string> allIDs = new List<string>();
			foreach (Type type in types) {
				foreach (string item in entityCollection[type.Name].Keys) {
					allIDs.Add(item);
				}
			}
			return allIDs.Contains(id);
		}

		/// <summary>
		/// Gets all base types of Entity
		/// </summary>
		/// <remark>
		/// Called when the user opens a project
		/// </remark>
		static List<Type> getEntityTypes() {
			return (
				from type in Assembly.GetAssembly(typeof(Entity)).GetTypes()
				where type.BaseType == typeof(Entity)
				orderby ( (DataViewerAttribute)Attribute.GetCustomAttribute(
					type, typeof(DataViewerAttribute))
				)?.order
				select type
			).ToList();
		}
	}
}