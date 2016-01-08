using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public class Floor : Terrain {
		public override Entity Init (string defID) {
			parentDrawSort = 100;
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