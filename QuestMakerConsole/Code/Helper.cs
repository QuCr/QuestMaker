using QuestMaker.Console;
using QuestMaker.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace QuestMakerConsole.Code {
	public static class Helper {
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
		public static bool isSubTypeOfType<T>(Type obj) {
			bool returnValue = typeof(T).IsAssignableFrom(obj);
			return returnValue;
		}

		/// <summary> typeof(Entity).IsAssignableFrom((Type)obj); </summary>
		public static bool isSubTypeOfEntity(Type obj) {
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
			var returnValue = isSubTypeOfType<T>(getListType(obj));
			return returnValue;
		}

		public static void outputList<T>(List<T> list, bool newLine = false, Func<T, object> function = null) {
			outputList(list.ToArray(), newLine, function);
		}

		public static void outputList<T>(T[] array, bool newLine = false, Func<T, object> function = null) {
			if (function == null)
				function = x => x.ToString();

			string text = "[";
			if (newLine)
				text = "\n[";
			if (newLine)
				text += "\n";
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetType() == typeof(string))
					text += "\"";

				text += $"{function(array[i])}";

				if (array[i].GetType() == typeof(string))
					text += "\"";
				if (i < array.Length - 1)
					text += ", ";
				if (newLine)
					text += "\n";
			}
			text += "]";
			Program.info(text);
		}
	}
}
