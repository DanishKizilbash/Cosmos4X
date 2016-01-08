using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public abstract class Terrain : Prop {
		public bool Walkable;
		public override void SetAttributes () {
			string walkable = def.GetAttribute ("Walkable");
			if (walkable == "True") {
				Walkable = true;
			} else {
				Walkable = false;
			}
		}
		public override Entity Init (string defID) {
			forceChunkUpdate = true;
			parentDrawSort = Mathf.Infinity;
			return base.Init (defID);
		}
		public override void Tick () {
			base.Tick ();
		}
		public override string DefaultID () {
			return "Thing_Prop_Floors_Default";
		}
		public override string ToString () {
			return Name;
		}
		
	}
}