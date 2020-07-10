using System;
using System.Collections;
using System.Collections.Generic;
using QuestMaker.Code;
using QuestMaker.Data;

namespace Qutilities {
	public static class Qutilities {
		/// <summary> !condition </summary>
		public static bool Not(bool condition) {
			return !condition;
		}

		/// <summary> obj is IList </summary>
		public static bool isList(object obj) {
			return obj is IList;
		}

		/// <summary> typeof(T).IsAssignableFrom(obj.GetType()); </summary>
		/// <remarks> 
		/// If the object is already a type, it will not convert it anymore. 
		/// Be awere that when using the type 'Type', this function will be useless
		/// Maybe, if it's needed, there will be a flag for that type
		/// </remarks>
		public static bool isSubOf<T>(object obj) {
			bool returnValue = typeof(Entity).IsAssignableFrom(obj.GetType());
			return returnValue;
		}

		public static bool isSubOfType<T>(Type obj) {
			bool returnValue = typeof(Entity).IsAssignableFrom(obj);
			return returnValue;
		}

		/// <summary>
		/// obj.GetType().GetGenericArguments()[genericTypeIndex];
		/// </summary>
		public static Type getListType(object obj) {
			Type returnValue = obj.GetType().GetGenericArguments()[0];
			return returnValue;
		}

		/// <summary>
		/// isList(obj) AND isSubOf T (getGenericType(obj, 0));
		/// </summary>
		public static bool isListOf<T>(object obj) {
			var returnValue = isSubOfType<T>(getListType(obj));
			return returnValue;
		}

		/*public static List<T> convertListType<T, U>(List<U> list) where U : T {
			List<T> returnList = new List<T>();

			for (int i = 0;i < list.Count;i++)
				Convert.ChangeType(list[i], typeof(U));

			return returnList;
		}*/
	}
}