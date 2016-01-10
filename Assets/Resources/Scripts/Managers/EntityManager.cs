using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class EntityManager
	{

		public static List<ActorShip> AddShip (int qty=1, float x=0f, float z=0f, float y = 0f)
		{
			List<ActorShip> ships = new List<ActorShip> ();
			for (int i =0; i<qty; i++) {
				ActorShip act;
				act = new ActorShip ();
				act.Init ("");
				act.MoveTo (new Vector3 (x, y, z));
				ships.Add (act);
			}
			return ships;
		}
	}
}