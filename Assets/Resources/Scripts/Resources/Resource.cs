using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class Resource
	{
		public string function;
		public float value;
		public string name;
		public Entity entity;
		public Resource (string Name, float Value=0, Entity ParentEntity = null)
		{
			name = Name;
			value = Value;
			entity = ParentEntity;
			Finder.resourceDatabase.Add (this);
		}
		public float Add (float qty)
		{
			value += qty;
			return value;
		}
		public float Remove (float qty)
		{
			float realQty = 0;
			if (value > qty) {
				value -= qty;
				realQty = qty;
			} else {
				realQty = value;
				value = 0;
			}
			if (value <= 0) {
				Destroy ();
			}
			return realQty;
		}
		public void Destroy ()
		{
			entity = null;
			value = 0;
			name = "";
			Finder.resourceDatabase.Remove (this);
		}
		public bool Merge (Resource resource)
		{
			if (resource.name == name) {
				value += resource.value;			
				resource.Destroy ();
				return true;
			}
			return false;
		}
	}
}