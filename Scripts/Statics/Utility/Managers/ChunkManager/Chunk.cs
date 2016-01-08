using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public class Chunk {
		public bool isLoaded = false;
		public int loadedY;
		public int loadedX;
		public int loadedZ;
		public Vector3 Dimensions = Gen.WorldGen.Dimensions;
		public Vector2 ChunkOffset;
		public Dictionary<Vector3,Tile> TileDictionary;
		public List<List<List<Tile>>>  TileMap;
		public bool visible = false;

		public Chunk Init (Vector2 Position, Dictionary<Vector3, Tile> tileDictionary, List<List<List<Tile>>> tileMap) {
			TileMap = tileMap;
			TileDictionary = tileDictionary;
			ChunkOffset = Position;
			loadedY = 0;
			loadedX = 0;
			loadedZ = 0;
			return this;
		}

		public void Update () {
			//if (!isLoaded) {
			isLoaded = InitiateUnloadedEntities ();
			//}

		}
		public Tile getTile (int x, int y, int z) {
			try {
				return TileMap [y] [x] [z];
			} catch (Exception) {
				return null;
			}
		}
		public bool InitiateUnloadedEntities () {
			float startTime = Time.realtimeSinceStartup;

			while (loadedY < TileMap.Count) {
				for (int x=loadedX; x<TileMap[loadedY].Count; x++) {
					loadedX = x;
					for (int z=loadedZ; z<TileMap[loadedY][loadedX].Count; z++) {
						loadedZ = z;
						//
						InitiateEntities (loadedX, loadedY, loadedZ);
						//
						ChunkManager.elapsedTime += (Time.realtimeSinceStartup - startTime) * 1000;
						startTime = Time.realtimeSinceStartup;
						if (ChunkManager.elapsedTime > ChunkManager.timeInterval) {
							return false;
						}
					}
					loadedZ = 0;
					//Break if over time
				}
				loadedX = 0;
				loadedY++;				
				//Debug.Log (ChunkOffset + "  :  " +elapsedTime);
			}
			return true;		    
		}
		public void InitiateEntities (int x, int y, int z) {
			Tile tile = null;
			Vector3 vector = new Vector3 (x, y, z);
			TileDictionary.TryGetValue (vector, out tile);
			if (tile != null) {		
				int xOff = (int)(ChunkOffset.x * Dimensions.x);
				int zOff = (int)(ChunkOffset.y * Dimensions.z);
				tile.MoveTo (x + xOff, y, z + zOff);
				tile.chunk = this;
				List<Entity> eList = tile.Children;
				for (int e = 0; e<eList.Count; e++) {
					Entity childEntity = eList [e];
					if (!childEntity.isInitiated) {
						childEntity.ParentTile = tile;
						childEntity.Init (childEntity.cachedDefID);	
					}
				}
			}
		}

	}
}