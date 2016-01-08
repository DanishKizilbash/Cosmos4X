using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public static class DrawManager_old {
		/*
		public static bool IsInitiated = false;
		public static GameObject MainCamera;
		private static GameObject DrawGameObject;
		public static Rect CameraRect;
		public static Vector2 CameraPos = new Vector2 ();
		public static Vector2 TargetCameraPos = new Vector2 ();
		public static float CameraEase = 0.1f;
		public static MeshFilter meshFilter;
		public static MeshRenderer meshRenderer;
		//
		public static float gridPixelSize;
		//
		private static List<List<List<Tile>>> VisibleTiles;
		private static List<List<Tile>> RenderTiles;
		public static int renderWidth;
		public static int renderHeight;
		private static Texture2D cachedBlankTexture;
		//
		private static List<Mesh> layerMeshes;
		private static List<GameObject> layerObjects;
		public static bool built = false;

		public static void Initiate (GameObject mainCamera, GameObject parent = null, float GridPixelSize=128f) {
			gridPixelSize = GridPixelSize;
			MainCamera = mainCamera;
			Finder.TileDatabase.TileSize = new Vector2 (GridPixelSize, GridPixelSize);
			if (!IsInitiated) {
				IsInitiated = true;
				DrawGameObject = new GameObject ("DrawGameObject");
				meshFilter = DrawGameObject.AddComponent<MeshFilter> ();
				meshRenderer = DrawGameObject.AddComponent<MeshRenderer> ();
				if (parent != null) {
					DrawGameObject.transform.parent = parent.transform;
				}
			}
		}
		public static void New () {
			Rect rRect = GetCameraRect ();
			renderWidth = (int)((rRect.width) * (gridPixelSize));
			renderHeight = (int)((rRect.height) * (gridPixelSize));
			cachedBlankTexture = GenBlankTexture ();
			BuildMeshes ();
			Profiler.Start ();
			for (int y = 0; y<layerMeshes.Count; y++) {
				UpdateLayer (y);
				Profiler.AddLap ();
			}
			Profiler.ReportLaps ();
		}
		public static void UpdateLayer (int layer) {
			Texture2D tex = BuildTexture (layer);
			layerObjects [layer].GetComponent<MeshRenderer> ().material.mainTexture = tex;
		}
		private static List<List<Tile>> GenBlankPooledTileList () {
			List<List<Tile>> tempList = new List<List<Tile>> ();
			for (int y = 0; y<Defaults.YRenderDistance; y++) {
				tempList.Add (new List<Tile> ());
			}
			return tempList;
		}
		private static Texture2D GenBlankTexture () {
			Texture2D texture = new Texture2D (renderWidth, renderHeight, TextureFormat.ARGB32, false);			
			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;

			Color fillColor = Color.clear;
			Color[] alphaPixels = new Color[texture.width * texture.height];			
			for (int i = 0; i <alphaPixels.Length; i++) {
				alphaPixels [i] = fillColor;
			}			
			texture.SetPixels (alphaPixels);
			texture.Apply ();
			 
			return texture;
		}
		public static void Draw () {

			if (IsInitiated && GameManager.curGameState == GameManager.GameState.GameRunning && Loader.curLoadState == Loader.LoadState.Finished) {
				UpdateCameraPos ();
				//Profiler.Start ();
				//BuildTexture ();
				//Profiler.ReportAndReset ("Build Texture :");
				UpdateLayer (0);
			}
		}

		private static Texture2D BuildTexture (int layer) {	
			Texture2D layerTexture = (Texture2D)layerObjects [layer].GetComponent<MeshRenderer> ().material.mainTexture;
			List<Tile> layerTiles = Finder.TileDatabase.GetLayers (layer);
			int cnt0 = 0;
			int cnt1 = 0;
			int cnt2 = 0;
			Rect texRect = new Rect (0, 0, renderWidth, renderHeight);
			int tileSize = (int)(gridPixelSize);
			Color[] v = ((Texture2D)Finder.TextureDatabase.Get ("Textures/Things/Props/Floors/FloorGrass")).GetPixels ();
			/*for (int i=0; i<layerTiles.Count; i++) {
				Tile curTile = layerTiles [i];
				Vector3 texPos = MathI.IsoToScreen (curTile.Position);
				int x = (int)(texPos.x);
				int y = (int)(texPos.y);
				cnt0++;
				cnt2 += curTile.Children.Count;
				//if (MathI.PointInRectangle (texPos, texRect) && MathI.PointInRectangle (texPos + new Vector3 (tileSize, tileSize, 0), texRect)) {
				foreach (Entity child in curTile.Children) {
					cnt1++;
					//Color[] p = child.MainGraphic.TexturePixels;
					//Debug.Log (texPos);
					layerTexture.SetPixels (x, y, tileSize, tileSize, v);
					//v = p;
				}
				//}
			}
			int cnt = 0;
			for (int x =0; x<renderWidth/gridPixelSize; x++) {
				for (int z =0; z<renderHeight/gridPixelSize; z++) {
					cnt++;
					layerTexture.SetPixels (x * 128, z * 128, tileSize, tileSize, v);
				}
			}
			Debug.Log (cnt);
			//Debug.Log (cnt0 + " Checks, resulting in " + cnt1 + " out of potential " + cnt2);
			//layerTexture.SetPixels (UnityEngine.Random.Range (0, 1000), UnityEngine.Random.Range (0, 1000), tileSize, tileSize, v);
			//layerTexture.Apply ();
			return layerTexture;
		}
		//}
		private static void AddMeshObject (Mesh tMesh) {
			GameObject gO = new GameObject ((layerMeshes.Count - 1).ToString ());
			MeshFilter meshFilter = gO.AddComponent<MeshFilter> ();
			MeshRenderer meshRenderer = gO.AddComponent<MeshRenderer> ();
			gO.transform.parent = DrawGameObject.transform;
			meshFilter.mesh = tMesh;
			meshRenderer.material.mainTexture = DupeTexture (cachedBlankTexture);
			gO.transform.position += Vector3.forward * layerMeshes.Count;
			meshRenderer.material.shader = Shader.Find ("Transparent/Diffuse");
			layerObjects.Add (gO);
		}
		private static Texture2D DupeTexture (Texture2D iTex) {
			Texture2D oTex = new Texture2D (iTex.width, iTex.height, TextureFormat.ARGB32, false);	
			oTex.SetPixels (iTex.GetPixels (0, 0, iTex.width, iTex.height));
			oTex.Apply ();
			return oTex;
		}
		private static void BuildMeshes () {		
			layerMeshes = new List<Mesh> ();
			layerObjects = new List<GameObject> ();
			int numLayers = (int)(ChunkManager.Dimensions.y);
			numLayers = 1;
			for (int y = 0; y<numLayers; y++) {
				layerMeshes.Add (BuildMesh (renderWidth, renderHeight));
				AddMeshObject (layerMeshes [y]);
			}

		}
		private static Mesh BuildMesh (int width, int height) {
			int numTiles = width * height;
			int numTris = numTiles * 2;
			
			int halfWidth = width / 2;
			int halfHeight = height / 2;
			//int numVerts = vWidth * vHeight;
			
			// Generate the mesh data
			Vector3[] vertices = new Vector3[4];
			Vector3[] normals = new Vector3[4];
			Vector2[] uv = new Vector2[4]; 
			int[] triangles = new int[6];
			uv [0] = new Vector2 (0, 0);
			uv [1] = new Vector2 (0, 1);
			uv [2] = new Vector2 (1, 1);
			uv [3] = new Vector2 (1, 0);

			vertices [0] = new Vector3 (-halfWidth, -halfHeight, 0);
			vertices [1] = new Vector3 (-halfWidth, halfHeight, 0);
			vertices [2] = new Vector3 (halfWidth, halfHeight, 0);
			vertices [3] = new Vector3 (halfWidth, -halfHeight, 0);

			normals [0] = Vector3.back;
			normals [1] = Vector3.back;
			normals [2] = Vector3.back;
			normals [3] = Vector3.back;

			triangles [0] = 0;
			triangles [1] = 1;
			triangles [2] = 2;

			triangles [3] = 0;
			triangles [4] = 2;
			triangles [5] = 3;

			// Create a new Mesh and populate with the data
			Mesh mesh = new Mesh ();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv;

			return mesh;
		}

		#region Camera
		public static Rect GetCameraRect () {
			Rect cameraPixels = Camera.main.pixelRect;
			Vector3 IsoVec = MathI.WorldToIso (new Vector3 (cameraPixels.width / 100, cameraPixels.height / 100, 0));
			Vector2 ScaleVec = new Vector2 (IsoVec.x, IsoVec.y) * 2;
			Rect newRect = new Rect (new Vector2 (CameraPos.x - ScaleVec.x / 2, CameraPos.y - ScaleVec.y / 2), ScaleVec);
			return newRect;
		}
		
		private static void UpdateCameraPos () {
			CameraPos += (TargetCameraPos - CameraPos) * CameraEase;
			//Debug.Log (CameraPos);
			Camera.main.transform.position = new Vector3 (CameraPos.x, CameraPos.y, -1);
		}
		public static void MoveCameraBy (Vector2 amount) {
			TargetCameraPos = TargetCameraPos + amount;
		}
		public static void MoveCameraTo (Vector2 pos) {
			TargetCameraPos = pos;
		}
		
		#endregion
		public static bool CheckVisibility (Tile tile) {
			if (tile != null) {
				if (tile.Children.Count > 0) {
					bool isVisible = false;
					int y = (int)tile.Position.y;
					int x = (int)tile.Position.x;
					int z = (int)tile.Position.z;
					if (y >= DepthManager.CurrentDepth) {
						if (x == 0 || z == 0 || x == ChunkManager.Dimensions.x - 1 || z == ChunkManager.Dimensions.z - 1) {
							isVisible = true;
						} else {
							bool surrounded = true;
							for (int xi = -1; xi<=1; xi++) {
								for (int yi = -1; yi<=1; yi++) {
									for (int zi = -1; zi<=1; zi++) {
										Tile nTile = Finder.TileDatabase.GetTile (x + xi, y + yi, z + zi);
										if (nTile == null || nTile.Children.Count == 0) {
											surrounded = false;
											break;
										}
									}
								}
							}
							if (!surrounded) {
								isVisible = true;
							} 	
						}
					}
					return isVisible;	
				}
			}
			return false;
		}
	*/
	}
}
