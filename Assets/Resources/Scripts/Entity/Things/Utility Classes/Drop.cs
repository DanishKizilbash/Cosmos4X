using UnityEngine;
using System.Collections;
using System;
namespace Cosmos
{
	public class Drop
	{
		public string type;
		public string id;
		public int qty;
		public Drop (string ID, string Type, int Qty)
		{
			id = ID;
			type = Type;
			qty = Qty;
		}
		public override string ToString ()
		{
			return  "ID: " + id + " , Type: " + type + " , Qty: " + qty;
		}
		public void CreateNew (Thing parent)
		{/*
			Type myType = Type.GetType ("Eniso." + type);
			if (myType != null) {
				Thing newDrop = (Thing)Activator.CreateInstance (myType);
				if (newDrop != null) {
					newDrop.Init ("Thing_Prop_" + id);
					newDrop.Position = parent.Position;
					try {
						((Resource)newDrop).Add (qty);
					} catch (InvalidCastException) { 
					}
				} else {
					Debug.Log ("Drop of type " + type + " could not be created");
				}
			} else {
				Debug.Log ("Type value is null for drop of id " + id + " could not create");
			}
			*/
		}
	}
}
