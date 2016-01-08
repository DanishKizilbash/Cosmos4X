using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public class Wall : Terrain {
		public override Entity Init (string defID) {
			parentDrawSort = 50;
			return base.Init (defID);
		}
		public override void Tick () {
			base.Tick ();
		}
		public override string DefaultID () {
			return "Thing_Prop_Walls_Default";
		}
		public override string ToString () {
			return Name;
		}

	}
}