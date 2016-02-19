using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class EntityManager
	{
		public static List<ActorShip> AddShip (int qty)
		{
			return AddShip (qty, 0, 0, 0);
		}
		public static List<ActorShip> AddShip (int qty, Vector3 position, int systemID=-1)
		{
			return AddShip (qty, position.x, position.y, position.z, systemID);
		}
		public static List<ActorShip> AddShip (int qty, float x, float z, float y, int systemID=-1)
		{
			if (systemID == -1) {
				systemID = GameManager.currentGame.currentSystemID;
			}
			List<ActorShip> ships = new List<ActorShip> ();
			for (int i =0; i<qty; i++) {
				ActorShip act;
				act = new ActorShip ();
				act.Init ("");
				act.MoveTo (new Vector3 (x, y, z));
				act.system = GameManager.currentGame.planetarySystems [systemID];
				ships.Add (act);
			}
			return ships;
		}
		public static List<PlanetarySystem> AddPlanetarySystem (int qty=1, Coord coord=null)
		{
			List<PlanetarySystem> systems = new List<PlanetarySystem> ();
			for (int i =0; i<qty; i++) {
				PlanetarySystem sys;
				sys = new PlanetarySystem (coord);
				systems.Add (sys);
			}
			return systems;
		}
	}
}