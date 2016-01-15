using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Coord
	{
		public Vector2 pos;
		public Vector2 sector;

		public Coord (float X, float Y, float SectorX, float SectorY)
		{
			pos.x = X;
			pos.y = Y;
			sector.x = SectorX;
			sector.y = SectorY;
		}
		public Coord (Vector2 Pos, Vector2 Sector)
		{
			pos.x = Pos.x;
			pos.y = Pos.y;
			sector.x = Sector.x;
			sector.y = Sector.y;
		}
	}
}