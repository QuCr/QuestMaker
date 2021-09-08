using System;
using System.Collections.Generic;

namespace QuestMaker.Console.Code {
	/// <summary> 
	/// List with a cursor that can go to the next & previous items.
	/// Used for cases like history-management.
	/// Rewrites history of all next items when not last item is selected with the cursor.
	/// Doesn't add the item if it's the same as the last, but still calls the action.
	/// </summary>
	public class CursorList<T> {
		List<T> list = new List<T>();
		int cursor = -1;
		Action<T> action = null;

		public CursorList() { }

		/// <param name="action"> The action that will be executed when an item is handled</param>
		public CursorList(Action<T> action) {
			this.action = action;
		}

		/// <summary> Goes to the given item, removes next items in history</summary>
		public void go(T item) {
			//rewriting history
			list.RemoveRange(cursor + 1, list.Count - cursor - 1);

			//if last item is current item, don't add to history
			if (list.Count == 0 || list[list.Count - 1].GetHashCode() != item.GetHashCode()) {
				list.Add(item);
				cursor++;
			}

			if (action != null) {
				action(item);
			}

			Helper.outputList(list);
		}

		/// <summary> Goes to the previous item</summary>
		public void back() {
			cursor--;
			if (action != null) {
				action(list[cursor]);
			}
		}

		/// <summary> Goes to the next item</summary>
		public void forward() {
			cursor++;
			if (action != null) {
				action(list[cursor]);
			}
		}

		/// <summary> Goes to the current item</summary>
		public void refresh() {
			if (action != null) {
				action(list[cursor]);
			}
		}

		/// <summary> Checks whether you can go back </summary>
		public bool canBack() {
			return cursor > 0;
		}

		/// <summary> Checks whether you can go forward </summary>
		public bool canForward() {
			return cursor < list.Count - 1;
		}

		/// <summary> Gets the item that the cursor currently points to </summary>
		public T currentItem() {
			return list[cursor];
		}
	}
}
