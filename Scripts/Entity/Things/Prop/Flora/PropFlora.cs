using UnityEngine;
using System.Collections;
namespace Eniso {
	public class PropFlora : Prop {
		public override Entity Init (string defID) {
			base.Init (defID);
			return this;
		}
		public override void SetAttributes () {
		}
		public override void Tick () {
			base.Tick ();
		}
		public override string DefaultID () {
			return "Thing_Prop_Flora_PineTree";
		}
	}
}