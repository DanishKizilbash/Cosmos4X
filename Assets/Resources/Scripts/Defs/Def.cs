using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cosmos
{
	public abstract class Def
	{
		public string BaseClass;
		public string Type;
		public string Category;
		public string ID;
		private string name = "";
		public virtual string Name {
			get {
				if (name == "") {
					string[] strings = new string[4];
					strings [0] = BaseClass;
					strings [1] = Type;
					strings [2] = Category;
					strings [3] = ID;
					name = String.Join ("_", strings);
				}
				return name;
			}
			set {
				name = value;
			}
		}
		public Dictionary<string,string> Attributes;

		public virtual void init (string baseClass, string type, string category, string id, Dictionary<string,string> attributes)
		{
			BaseClass = baseClass;
			Type = type;
			Category = category;
			ID = id;
			Attributes = attributes;
			AddToDatabase ();
		}
		public virtual void AddToDatabase ()
		{
			Finder.DefDatabase.Add (this);
		} 
		public virtual void Add (string attribute, string value)
		{
			Attributes.Add (attribute, value);
		}
		public virtual string GetAttribute (string key)
		{
			string value;
			if (!Attributes.TryGetValue (key, out value) || value == null) {
				value = "";
			}
			return value;
		}
		public virtual void Print ()
		{
			Debug.Log ("_________________________");
			Debug.Log ("Def of name : " + Name);
			Debug.Log ("Base Class : " + BaseClass);
			Debug.Log ("Type : " + Type);
			Debug.Log ("Category : " + Category);
			Debug.Log ("ID : " + ID);

			foreach (KeyValuePair<string,string> d in Attributes) {
				Debug.Log (d.Key + " : " + d.Value);
			}
		}
		public override string ToString ()
		{
			return Name;
		}

	}
}