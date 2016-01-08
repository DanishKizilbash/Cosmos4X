using UnityEngine;
using System.Collections;
namespace Eniso {
	public static class EntityManager {

		public static ActorHuman AddActor (int x, int z, int y = -1) {
			Tile spawnTile;
			ActorHuman act;
			act = new ActorHuman ();
			act.Init ("");
			if (y == -1) {
				spawnTile = Finder.TileDatabase.GetHighestTileAt (x, z);
			} else {
				spawnTile = Finder.TileDatabase.GetTile (x, y, z);
				if (spawnTile == null) {
					spawnTile = Finder.TileDatabase.GetHighestTileAt (x, z);
				}
			}
			act.ParentTile = spawnTile;
			return act;
		}
	}
}
