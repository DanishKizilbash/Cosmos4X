using UnityEngine;
using System.Collections;
namespace Eniso.AI {
	public abstract class Modifier:Task {
		public override void Execute () {
			ApplyModification ();
		}
		public abstract void ApplyModification ();
	}
}