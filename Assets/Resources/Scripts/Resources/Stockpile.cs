using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class Stockpile:Exposable
	{
		public Entity entity;
		public Dictionary<string,Resource> resList;
		public Stockpile (Entity parentEntity)
		{
			resList = new Dictionary<string, Resource> ();
			entity = parentEntity;
		}
		public float Add (Resource resource, float value=0)
		{
			if (!resList.ContainsKey (resource.name)) {
				resList.Add (resource.name, new Resource (resource.name));
			}
			resList [resource.name].Add (value);
			return resList [resource.name].value;
		}
		public float Remove (Resource resource, float value=0)
		{
			Resource res;
			if (resList.ContainsKey (resource.name)) {
				res = resList [resource.name];
				res.Remove (value);
				if (res.value <= 0) {
					resList.Remove (res.name);
				}
				return res.value;
			}
			return 0;
		}
		public void Print ()
		{
			foreach (Resource res in resList.Values) {
				Debug.Log (res.name + ": " + res.value);
			}
		}
		public void Destroy ()
		{
			entity = null;
			foreach (Resource resource in resList.Values) {
				resource.Destroy ();
			}
		}
		public override void OnSelected (bool value)
		{
		}
	}
}
