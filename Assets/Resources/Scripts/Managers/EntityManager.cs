using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public static class EntityManager
	{

		public static ActorShip AddShip (int x=0, int z=0, int y = 0)
		{
			ActorShip act;
			act = new ActorShip ();
			act.Init ("");
			act.MoveTo (new Vector3 (x, y, z));

			return act;
		}
	}
}
