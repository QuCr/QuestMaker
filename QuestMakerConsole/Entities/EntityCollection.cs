﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using QuestMaker.Code;
using QuestMaker.Code.Attributes;

namespace QuestMaker.Data {
	/// <summary>
	/// Used for storing all the data of the project.
	/// Structure is a dictionairy of dictionairies of each type.
	/// Each dictionairy for the types contains the instances of that type.
	/// </summary>
	public class EntityCollection : Dictionary<string, Dictionary<string, Entity>> {
		public static EntityCollection collection = new EntityCollection();
		public static List<Type> types {
			get => getEntityTypes();
		}

		//Settings used for im- and exporting the data
		public const Formatting formatting = Formatting.Indented;
		public const PreserveReferencesHandling preserveReferencesHandling = PreserveReferencesHandling.Objects;
		public const TypeNameHandling typeNameHandling = TypeNameHandling.All;

		public EntityCollection() {
			foreach (Type type in types) {
				Add(type.Name, new Dictionary<string, Entity>());
			}
		}

		/// <summary>
		/// Reads the file's content and adds the deserialized Entitys to the collection 
		/// </summary>
		public static bool import(string json) {
			EntityCollection e = JsonConvert.DeserializeObject<EntityCollection>(json, new JsonSerializerSettings {
				PreserveReferencesHandling = preserveReferencesHandling,
				TypeNameHandling = TypeNameHandling.All
			});

			if (e != null)
				collection = e;
			return e != null;
		}

		/// <summary>
		/// Serializes all Entitys in the collection and returns the data in JSON format. 
		/// </summary>
		/// <returns>All Entitys in JSON</returns>
		public static string export() {
			return JsonConvert.SerializeObject(collection, formatting, new JsonSerializerSettings {
				PreserveReferencesHandling = preserveReferencesHandling,
				TypeNameHandling = typeNameHandling
			});
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
			if (packet is PacketSingle)
				return new List<Entity>() {
					getSingle(packet.type, packet.entities[0].id)
				};
			if (packet is PacketArray)
				return getArray(packet.type, (from a in packet.entities select a.id).ToArray());
			if (packet is PacketType)
				return getTypeArray(packet.type);
			return null; //returns null because it's not a Entity, which is what was asked.
		}

		/// <summary>
		/// Gets a single Entity
		/// </summary>
		public static Entity getSingle(Type type, string id) {
			return (from item in collection[type.Name] where id == item.Key select item.Value).FirstOrDefault();
		}

		/// <summary>
		/// Gets a list with 0, 1 or more Entitys
		/// </summary>
		public static List<Entity> getArray(Type type, params string[] ids) {
			List<Entity> entities = new List<Entity>();
			var s = collection[type.Name];
			entities.AddRange(from item in s where ids.Contains(item.Key) select item.Value);
			return entities;
		}

		/// <summary>
		/// Gets a list with all Entitys of a certain type
		/// </summary>
		public static List<Entity> getTypeArray(Type type) {
			List<Entity> entities = new List<Entity>();
			entities.AddRange(from item in collection[type.Name] select item.Value);
			return entities;
		}

		/// <summary>
		/// Adds the Entity to the collection
		/// </summary>
		/// <exception cref="ArgumentException"/>
		public void activate(Entity entity) {
			if (!getActiveIDs().Contains(entity.id)) {
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
				throw new KeyNotFoundException("Could not remove the Entitys, probably because it wasn't in there");
		}

		/// <summary>
		/// Gets all IDs of the Entitys in the collection
		/// </summary>
		/// <remark>
		/// Called every time a user adds another Entity
		/// </remark>
		List<string> getActiveIDs() {
			List<string> allIDs = new List<string>();
			foreach (Type type in types) {
				foreach (var id in this[type.Name].Keys) {
					allIDs.Add(id);
				}
			}
			return allIDs;
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
				orderby ((DataViewerAttribute)Attribute.GetCustomAttribute(
					type, typeof(DataViewerAttribute)))?.order
				select type
			).ToList();
		}
	}
}