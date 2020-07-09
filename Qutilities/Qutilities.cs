using System;
using System.Collections;
using System.Collections.Generic;

namespace Qutilities {
	public static class Qutilities {
		/// <summary>
		/// return obj is IList
		/// </summary>
		public static bool isList(object obj) {
			return obj is IList;
		}

		/// <summary>
		/// typeof(T).IsAssignableFrom(obj.GetType());
		/// </summary>
		public static bool isSubOf<T>(object obj) {
			var returnValue = typeof(T).IsAssignableFrom(obj.GetType());
			return returnValue;
		}

		/// <summary>
		/// obj.GetType().GetGenericArguments()[genericTypeIndex];
		/// </summary>
		public static Type getGenericType(object obj, int genericTypeIndex) {
			var returnValue = obj.GetType().GetGenericArguments()[genericTypeIndex];
			return returnValue;
		}

		/// <summary>
		/// isList(obj) AND isSubOf T (getGenericType(obj, 0));
		/// </summary>
		public static bool isListOf<T>(object obj) {
			var returnValue = isList(obj) && isSubOf<T>(getGenericType(obj, 0));
			return returnValue;
		}

		public static List<T> convertListType<T, U>(List<U> list) where U : T {
			List<T> returnList = new List<T>();

			for (int i = 0;i < list.Count;i++)
				Convert.ChangeType(list[i], typeof(U));

			return returnList;
		}
	}
}