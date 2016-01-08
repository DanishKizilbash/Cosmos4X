using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class ThingDef : Def
	{
		public List<Drop> drops = new List<Drop> ();
		public override void init (string baseClass, string type, string category, string id, Dictionary<string, string> attributes)
		{
			base.init (baseClass, type, category, id, attributes);
			createDropList (GetAttribute ("Drops"));
		}
		public void createDropList (string input)
		{
			if (input != "") {
				int index = 0;
				string[] seperators = new string[] {":",","};
				string[] strings = input.Split (seperators, System.StringSplitOptions.None);
				while (index<strings.Length) {
					Drop curDrop = new Drop (strings [index], strings [index + 1], Parser.StringToInt (strings [index + 2]));
					drops.Add (curDrop);
					Debug.Log (curDrop.ToString ());
					index += 3;
				}
			}
		}
	}
}