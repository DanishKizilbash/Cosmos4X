using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class DrawManager
	{
		public static float processTime = 4f;
		public static bool IsInitiated = false;
		public static GameObject DrawGameObject;
		//Camera
		public static GameObject MainCamera;
		public static Vector2 CameraPos = new Vector2 (0, 0);
		public static Vector2 TargetCameraPos = new Vector2 (0, 0);
		public static float TargetSize = 1f;
		public static float CameraEase = 0.1f;
		public static Vector2 DisplaySize;

		public static void Init (GameObject parent = null)
		{
			MainCamera = Camera.main.gameObject;
			if (!IsInitiated) {
				IsInitiated = true;
				DrawGameObject = new GameObject ("DrawGameObject");
				if (parent != null) {
					DrawGameObject.transform.parent = parent.transform;
				}
			}
			New ();
		}

		public static void New ()
		{
			//Init Terrain Meshes
			MeshManager.Init ();
			MeshManager.Start ();
		}


		public static void Draw ()
		{

			UpdateCameraPos ();
			Profiler.Start ();

			float startTime = Time.realtimeSinceStartup;

			/*if (Time.realtimeSinceStartup - startTime > processTime) {
				break;
			}*/
				



			MeshManager.ApplyMeshUpdates ();

		}

		#region Camera
		private static void UpdateCameraPos ()
		{
			CameraPos += (TargetCameraPos - CameraPos) * CameraEase;
			//Debug.Log (CameraPos);
			Camera.main.transform.position = new Vector3 (CameraPos.x, CameraPos.y, -1);
			TargetSize = GameManager.currentGame.currentZoomScale;
			Camera.main.orthographicSize = Mathf.SmoothStep (Camera.main.orthographicSize, TargetSize, 0.25f);
		}
		public static void MoveCameraBy (Vector2 amount)
		{
			TargetCameraPos = TargetCameraPos + amount;
		}
		public static void MoveCameraTo (Vector2 pos)
		{
			TargetCameraPos = pos;
		}
		#endregion

	}
}