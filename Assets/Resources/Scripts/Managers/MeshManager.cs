using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class MeshManager
	{
		private static int dispIter;
		public static int minPooledDisplays = 500;
		public static float poolGenMSLimit = 2;
		public static Vector2 defaultMeshTileDim = new Vector2 (1, 1);
		public static List<MeshDisplay> meshDisplays = new List<MeshDisplay> ();
		private static List<MeshDisplay> pooledDisplays = new List<MeshDisplay> ();
		public static GameObject MeshesGameObject;

		public static void Init ()
		{
			MeshesGameObject = new GameObject ("Meshes");
			MeshesGameObject.transform.parent = DrawManager.DrawGameObject.transform;
			SetMeshDimensions ();
		}
		private static void SetMeshDimensions ()
		{
			defaultMeshTileDim = new Vector2 (1, 1);
		}
		public static void Start ()
		{	
			pooledDisplays = new List<MeshDisplay> ();
			meshDisplays = new List<MeshDisplay> ();
			ApplyMeshUpdates ();
		}
		private static void GenPooledDisplays ()
		{
			float startTime = Time.realtimeSinceStartup;
			while (pooledDisplays.Count<minPooledDisplays && Time.realtimeSinceStartup-startTime<poolGenMSLimit) {
				AddMeshDisplayToPool (TextureManager.DefaultAtlas);
			}
		}
		public static void AddMeshDisplayToPool (SpriteAtlas Atlas)
		{
			pooledDisplays.Add (new MeshDisplay (defaultMeshTileDim, Atlas));
		}
		public static void UpdateEntity (Entity entity, Vector2 overrideVector=default(Vector2))
		{
			Graphic graphic = entity.MainGraphic;			

			if (entity.meshDisplay == null) {
				AssignEntity (entity);
			} 
			MeshDisplay tMeshDisp = entity.meshDisplay;
			tMeshDisp.setVisibility (entity.isVisible);
			if (entity.isVisible) {
				tMeshDisp.UpdatePosition ();
				if (overrideVector == default(Vector2)) {
					tMeshDisp.UpdateUV (0, 0, graphic.atlasVector);
				} else {
					tMeshDisp.UpdateUV (0, 0, overrideVector);
				}
			} 
		}
		public static void AssignEntity (Entity entity)
		{
			UpdateDisplayList ();
			MeshDisplay meshDisplay = meshDisplays [dispIter];
			entity.meshDisplay = meshDisplay;
			entity.meshDisplay.UpdateSpriteAtlas (entity.MainGraphic.spriteAtlas);
			entity.meshDisplay.UpdateScale ();
			meshDisplay.entity = entity;
			dispIter++;
		}
		private static void UpdateDisplayList ()
		{
			if (dispIter >= meshDisplays.Count) {
				int transferCnt = 1000;
				transferCnt = transferCnt > pooledDisplays.Count ? pooledDisplays.Count : transferCnt;
				meshDisplays.AddRange (pooledDisplays.GetRange (0, transferCnt));
				pooledDisplays.RemoveRange (0, transferCnt);
			}
			GenPooledDisplays ();
		}
		private static void MoveExcessDisplaysToPool ()
		{
			int excessCnt = meshDisplays.Count - dispIter;
			for (int i = 0; i<excessCnt; i++) {
				MeshDisplay meshDisp = meshDisplays [meshDisplays.Count - 1];
				meshDisp.setVisibility (false);
				pooledDisplays.Add (meshDisp);
				meshDisplays.RemoveAt (meshDisplays.Count - 1);
			}
		}
		public static void ApplyMeshUpdates ()
		{
			for (int i = 0; i<meshDisplays.Count; i++) {
				meshDisplays [i].ApplyUpdate ();					
			}
			MoveExcessDisplaysToPool ();
			GenPooledDisplays ();
			//dispIter = 0;
		}
		public static void CleanMeshDisplay (MeshDisplay meshDisplay)
		{
			if (meshDisplay != null) {
				meshDisplay.entity = null;
				meshDisplay.setVisibility (false);
				pooledDisplays.Add (meshDisplay);
				meshDisplays.Remove (meshDisplay);
				//dispIter -= 1;
			}
		}
	}
}