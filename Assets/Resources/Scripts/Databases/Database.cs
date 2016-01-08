using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cosmos
{
	public abstract class Database
	{
		public int Count {
			get {
				return dictionary.Count;
			}
		}

		public Dictionary<string,object> dictionary = new Dictionary<string,object> ();

		public virtual  void Add (object entry)
		{
			string name = entry.ToString ();
			if (!dictionary.ContainsKey (name)) {
				dictionary.Add (name, entry);
			}
		}
		public virtual  void Add (object entry, string key)
		{
			string name = key;
			if (!dictionary.ContainsKey (name)) {
				dictionary.Add (name, entry);
			}
		}

		public virtual  void Remove (object entry)
		{
			if (dictionary.ContainsKey (entry.ToString ())) {
				dictionary.Remove (entry.ToString ());
			}
		}

		public virtual  object Get (string name)
		{
			object value = null;
			dictionary.TryGetValue (name, out value);
			return value;
		}
		public virtual void Set (string name, object obj)
		{
			if (name != null) {
				if (dictionary.ContainsKey (name)) {
					dictionary [name] = obj;
				}
			}
		}

		public virtual  void UpdateKey (string originalName, string newName)
		{
			if (originalName != newName) {
				if (!dictionary.ContainsKey (newName)) {
					if (originalName != null) {
						object tempObject = Get (originalName);
						if (tempObject != null) {
							dictionary.Remove (originalName);
							dictionary.Add (newName, tempObject);

						}
					}
				}
			}
		}
		public virtual List<object> GetAll ()
		{
			List<object> objectList = new  List<object> ();
			foreach (object obj in dictionary.Values) {
				objectList.Add (obj);
			}
			return objectList;
		}
		public virtual  void Clear ()
		{
			dictionary.Clear ();
		}

	}
}