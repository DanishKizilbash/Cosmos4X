using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Cosmos.AI
{
	public class ModifierGetRandom:Modifier
	{
		public override void ApplyModification ()
		{
			object obj = input;
			object[] objectList = ((IEnumerable)obj).Cast<object> ()
			.Select (x => x == null ? x : x)
			.ToArray ();

			int random = (int)Mathf.Floor (Random.Range (0, objectList.Length - 1));
			output = objectList [random];
			if (output != null) {
				state = State.Success;
			} else {
				state = State.Failure;
			}
		}
	}
}