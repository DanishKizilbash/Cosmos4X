using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Cosmos.AI
{
	public class ModifierGetNearest:Modifier
	{
		public override void ApplyModification ()
		{
			Vector3 curTile = brain.actor.Position;
			object obj = input;
			Vector3[] objectList = ((IEnumerable)obj).Cast<Vector3> ()
			.Select (x => x)
			.ToArray ();

			if (objectList.Length > 0) {
				int index = 0;
				float minDist = float.MaxValue;
				for (int i = 0; i<objectList.Length; i++) {
					Vector3 curVec = objectList [i];
					float newDist = (curTile - curVec).sqrMagnitude;
					if (minDist > newDist) {
						minDist = newDist;
						index = i;
					}
				}
				output = objectList [index];
			} else {
				output = null;
			}
			if (output != null) {
				state = State.Success;
			} else {
				state = State.Failure;
			}
		}
	}
}