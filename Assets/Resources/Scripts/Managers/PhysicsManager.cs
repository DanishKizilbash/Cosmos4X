using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class PhysicsManager
	{
		public static float GValue = 0.5f;
		public static List<PhysicsObject> physicsList = new List<PhysicsObject> ();
		public static void Update ()
		{
			for (int i=0; i<physicsList.Count; i++) {
				physicsList [i].Update ();
			}
		}
		public static void Add (PhysicsObject pObj)
		{
			if (!physicsList.Contains (pObj)) {
				physicsList.Add (pObj);
			}
		}
		public static void Remove (PhysicsObject pObj)
		{
			if (!physicsList.Contains (pObj)) {
				physicsList.Remove (pObj);
			}
		}
	}
}