using System;
using System.Collections;
using System.Collections.Generic;
using QuestMaker.Code;
using QuestMaker.Data;

namespace Qutilities {
	public static class Helper {
		/// <summary> !condition </summary>
		public static bool Not(bool condition) {
			return !condition;
		}

		/// <summary> obj is IList </summary>
		public static bool isList(object obj) {
			return obj is IList;
		}

		/// <summary> typeof(T).IsAssignableFrom(obj.GetType()); </summary>
		public static bool isSubOf<T>(object obj) {
			bool returnValue = typeof(T).IsAssignableFrom(obj.GetType());
			return returnValue;
		}

		/// <summary> typeof(Entity).IsAssignableFrom((Type)obj); </summary>
		public static bool isSubOfType<T>(Type obj) {
			bool returnValue = typeof(Entity).IsAssignableFrom(obj);
			return returnValue;
		}

		/// <summary> obj.GetType().GetGenericArguments()[0]; </summary>
		public static Type getListType(object obj) {
			Type returnValue = null;

			if (isList(obj))
				returnValue = obj.GetType().GetGenericArguments()[0];

			return returnValue;
		}

		/// <summary> isSubOfType<T>(getListType(obj)); </summary>
		public static bool isListOf<T>(object obj) {
			var returnValue = isSubOfType<T>(getListType(obj));
			return returnValue;
		}
	}
}