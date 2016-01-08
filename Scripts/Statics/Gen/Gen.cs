using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Eniso {
	public static class Gen {

		public static class WorldGen {
			public static float PerlHeight;
			public static float PerlScale;
			public static float Seed;
			public static Vector3 Dimensions;
			public static float MaxHeight {
				get {
					return Dimensions.y;
				}
			}
			public static void Initiate (Vector3 dimensions, float perlHeight, float perlScale, float seed) {
				Dimensions = dimensions;
				PerlHeight = perlHeight;
				PerlScale = perlScale;
				Seed = seed;
			}

			public static Chunk CreateChunk (Vector2 position) {
				Chunk chunk = GenerateChunk (position);
				return chunk;
			}
			#region TerrainGen
			private static Chunk GenerateChunk (Vector2 position) {
				Finder.TileDatabase.AddEmptyChunkToMap (position, Dimensions);
				Vector2 dim = new Vector2 (position.x * Dimensions.x, position.y * Dimensions.z);
				float[,] perlinMap = MakePerlinMap (dim);
				List<List<List<Tile>>> entityMap = MakeEmptyMap (dim);
				Dictionary<Vector3, Tile> entityDictionary = GenTerrain (dim, entityMap, perlinMap);
				return new Chunk ().Init (position, entityDictionary, entityMap);
			}
			private static float[,] MakePerlinMap (Vector2 position) {
				float[,] perlinMap = new float[(int)Dimensions.x, (int)Dimensions.z];				
				//make perlin map
				for (int x =0; x< Dimensions.x; x++) {
					for (int z =0; z< Dimensions.z; z++) {
						Vector3 newPos = ApplyNoise (new Vector3 (x + position.x, 0, z + position.y));
						perlinMap [x, z] = PerlHeight - newPos.y;
					}
				}
				return perlinMap;
			}
			private static List<List<List<Tile>>> MakeEmptyMap (Vector2 position) {
				List<List<List<Tile>>> entityMap = new List<List<List<Tile>>> ();
				for (int y =0; y< MaxHeight; y++) {
					entityMap.Add (new List<List<Tile>> ());
					for (int x =0; x< Dimensions.x; x++) {
						entityMap [y].Add (new List<Tile> ());
						for (int z =0; z< Dimensions.z; z++) {
							entityMap [y] [x].Add (null);
						}
					}
				}
				return entityMap;
			}
			private static Dictionary<Vector3, Tile> GenTerrain (Vector2 position, List<List<List<Tile>>> entityMap, float[,] perlinMap) {
				Dictionary<Vector3, Tile> entityDictionary = new Dictionary<Vector3, Tile> ();
				for (int x =0; x< Dimensions.x; x++) {
					for (int z =0; z< Dimensions.z; z++) {
						int perlinHeight = (int)perlinMap [x, z];
						for (int y = 0; y<Dimensions.y; y++) {													
							Vector2 posOffset = new Vector2 (position.x, position.y);
							Tile curTile = new Tile (new Vector3 (x + posOffset.x, y, z + posOffset.y));
							if (y >= perlinHeight) {
								List<Entity> entities = new List<Entity> ();	
								//
								Floor floor = new Floor ();
								floor.cachedDefID = "Thing_Prop_Floors_Grass";
								entities.Add (floor);
								if (y != perlinHeight) {
									Wall wall = new Wall ();
									wall.cachedDefID = "Thing_Prop_Walls_Dirt";
									entities.Add (wall);
								}
								//
								if (y == perlinHeight) {
									if (Random.Range (0, 100) < 5) {
										ActorHuman actor = new ActorHuman ();
										entities.Add (actor);
									}
									if (Random.Range (0, 100) > 80) {
										PropFlora propflora = new PropFlora ();
										if (Random.Range (0, 100) > 70) {
											if (Random.Range (0, 100) > 60) {
												propflora.cachedDefID = "Thing_Prop_Flora_AppleTree";
											} else {
												propflora.cachedDefID = "Thing_Prop_Flora_BerryBush";
											}
										} else {
											propflora.cachedDefID = "Thing_Prop_Flora_PineTree";
										}
										entities.Add (propflora);
									}
								}
								curTile.Children = entities;	
							}
							entityDictionary.Add (new Vector3 (x, y, z), curTile);
							entityMap [y] [x] [z] = curTile;
						}
					}
				}
				return entityDictionary;
			}
			private static Vector3 ApplyNoise (Vector3 origPos) {
				Vector3 curPos = origPos;
				float perlNoise = Mathf.PerlinNoise (curPos.x / Dimensions.x * PerlScale + Seed, curPos.z / Dimensions.z * PerlScale + Seed) * PerlHeight;
				perlNoise = Mathf.FloorToInt (perlNoise);
				return new Vector3 (curPos.x, perlNoise, curPos.z);	
			}
			#endregion


		}
	}
}