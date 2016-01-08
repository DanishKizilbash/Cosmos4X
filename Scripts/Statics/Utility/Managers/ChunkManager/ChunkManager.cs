using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Eniso {
	public static class ChunkManager {
		public static Vector3 Dimensions;
		public static float timeInterval = 5f;
		public static List<List<Chunk>> LoadedChunks;
		private static Dictionary<Vector2,Chunk> CachedChunks;
		private static GameObject MainCamera;
		public static Vector3 ChunkSize;
		public static float elapsedTime = 0;
		public static int ViewDistance;
		public static float tilesUpdated;
		private static Vector2 previousPosition;
		public static Vector2 CurrentPosition {
			get {
				Vector3 curPos = MathI.WorldToIso (MainCamera.transform.position);
				return new Vector2 (Mathf.FloorToInt (curPos.x / ChunkSize.x), Mathf.FloorToInt (curPos.z / ChunkSize.z));
			}
			set {
				MainCamera.transform.position = value;
			}
		}
		public static int minX = 0;
		public static int maxX = 0;
		public static int minZ = 0;
		public static int maxZ = 0;

		public static void Initiate (int viewDistance) {
			MainCamera = Camera.main.gameObject;
			ChunkSize = Gen.WorldGen.Dimensions;
			ViewDistance = viewDistance;
			LoadedChunks = new List<List<Chunk>> ();
			CachedChunks = new Dictionary<Vector2,Chunk> ();
			previousPosition = CurrentPosition;
			Dimensions = new Vector3 (Gen.WorldGen.Dimensions.x * viewDistance, Gen.WorldGen.Dimensions.y, Gen.WorldGen.Dimensions.z * viewDistance);
			LoadChunks ();
			minX = 0;
			minZ = 0;
			maxX = (int)((Dimensions.x - 1) / ChunkSize.x);
			maxZ = (int)((Dimensions.z - 1) / ChunkSize.z);
		}

		public static void Update () {
			Vector2 curPos = CurrentPosition;
			if (curPos != previousPosition) {
				//DepthManager.CheckAllLayers ();
				previousPosition = curPos;
			}
			UpdateLoadedChunks ();
		}
		public static Chunk CreateChunk (Vector2 position) {
			Chunk chunk = Gen.WorldGen.CreateChunk (position);
			if (CachedChunks.ContainsKey (position)) {
				CachedChunks [position] = chunk;
			} else {
				CachedChunks.Add (position, chunk);
			}
			return chunk;
		}

		public static Chunk GetChunk (Vector2 position) {
			Chunk chunk = null;
			CachedChunks.TryGetValue (position, out chunk);
			if (chunk == null) {
				chunk = CreateChunk (position);
			}
			return chunk;
		}

		public static void LoadChunks () {
			if (LoadedChunks.Count == 0 || CurrentPosition != previousPosition) {
				LoadedChunks = new List<List<Chunk>> ();
				for (int x = -ViewDistance; x<ViewDistance; x++) {
					LoadedChunks.Add (new List<Chunk> ());
					for (int z = -ViewDistance; z<ViewDistance; z++) {
						Vector2 newPos = new Vector2 (0, 0);
						newPos.x = CurrentPosition.x + x;
						newPos.y = CurrentPosition.y + z;
						if (newPos.x >= 0 && newPos.y >= 0) {
							Chunk curChunk = GetChunk (newPos);
							int xval = x + ViewDistance;
							LoadedChunks [xval].Add (curChunk);
						}
					}
				}
			}
			previousPosition = CurrentPosition;
		}

		public static void UpdateLoadedChunks () {
			if (tilesUpdated != 0) {
				Debug.Log ("Tiles Updated: " + tilesUpdated);
			}
			tilesUpdated = 0;
			if (LoadedChunks.Count > 0) {
				elapsedTime = 0;
				bool isFullyLoaded = false;
				Chunk leastLoadedChunk = null;
				for (int x = 0; x<LoadedChunks.Count; x++) {
					for (int z = 0; z<LoadedChunks[x].Count; z++) {
						if (LoadedChunks [x] [z] != null) {
							if (leastLoadedChunk == null || LoadedChunks [x] [z].loadedY < leastLoadedChunk.loadedY) {
								leastLoadedChunk = LoadedChunks [x] [z];
							}
						}
						isFullyLoaded = LoadedChunks [x] [z].isLoaded;
					}
				}

				if (isFullyLoaded) {
					for (int x = 0; x<LoadedChunks.Count; x++) {
						for (int z = 0; z<LoadedChunks[x].Count; z++) {
							if (LoadedChunks [x] [z] != null) {
								LoadedChunks [x] [z].Update ();
							}
						}
					}
				} else {
					if (leastLoadedChunk != null) {
						leastLoadedChunk.Update ();
					}
				}
			}

		}
		/*public static void UpdateTile (Vector3 Pos) {
			Vector2 targetChunkPos = new Vector2 (Mathf.FloorToInt (Pos.x / ChunkSize.x), Mathf.FloorToInt (Pos.y / ChunkSize.z));
			Chunk targetChunk = GetChunk (targetChunkPos);
			Vector3 clampedTilePos = new Vector3 (Pos.x - targetChunkPos.x * ChunkSize.x, Pos.y, Pos.z - targetChunkPos.y * ChunkSize.z);
			targetChunk.updateTile(clampedTilePos,newTile)
		}*/
	}
}