using UnityEngine;
using System.Collections;
namespace Eniso {
	public static class Defaults {
		public static float TileSize = 128f;
		//
		public static float Seed;
		public static Vector3 ChunkSize = new Vector3 (16, 10, 16);
		public static  float PerlHeight = 3;
		public static  float PerlScale = 3;
		public static float PerlHeightMax = 10;
		public static float PerlScaleMax = 10;
		public static  int ViewDistance = 4;
		public static int YRenderDistance = 4;
	}
}