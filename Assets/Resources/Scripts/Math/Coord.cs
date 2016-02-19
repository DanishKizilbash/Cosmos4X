using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Coord
	{
		public Vector2 pos;//Light Days - 365Max
		public Vector2 sector;//Light Years - 100max

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
		public Vector3 vector {
			get {
				float x = pos.x + UnitConversion.Distance.LightYear.Value (sector.x, UnitConversion.Distance.Mm);
				float y = pos.y + UnitConversion.Distance.LightYear.Value (sector.y, UnitConversion.Distance.Mm);
				return new Vector3 (x, y, 0);
			}
		}
	}
}