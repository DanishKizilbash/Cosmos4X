using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Limb
	{
		public enum Condition
		{
			Normal=1,
			Damaged=2,
			Missing=3}
		;
		public Condition Status;
		public float Integrity;
		public float ConstructionCost;
		public Def def;
		public Limb ()
		{
		
		}
		public Limb Init (string defID)
		{
			GetDef (defID);
			SetAttributes ();
			return this;
		}
		public virtual void SetAttributes ()
		{
			
			ConstructionCost = Parser.StringToFloat (def.GetAttribute ("ConstructionCost"));
			Status = (Condition)Parser.StringToInt (def.GetAttribute ("Condition"));
			Integrity = Parser.StringToFloat (def.GetAttribute ("Integrity"));
		}
		public void GetDef (string defID)
		{
			def = Finder.DefDatabase.Get (defID);
			if (def == null) {
				if (defID != "") {
					Debug.Log ("Invalid defID: " + defID + " provided, reverting to default");
				}
				defID = DefaultID ();
				def = Finder.DefDatabase.Get (defID);
				if (def == null) {
					Debug.Log ("Default def Invalid, could not create Entity: " + defID);
					
				}
			}
		}
		public virtual string DefaultID ()
		{
			return "Limb_Default_Default_Default";
		}
	}
}