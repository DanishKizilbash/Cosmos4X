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
		public static float CameraOrthoSize;
		public static Vector2 TargetCameraPos = new Vector2 (0, 0);
		public static float TargetSize = 1f;
		public static float CameraEase = 0.2f;
		public static Vector2 DisplaySize;
		private static bool VectorCanvasFixed;
		//
		public static bool ZoomToMouse = true;

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
			if (!VectorCanvasFixed) {
				GameObject.Find ("VectorCanvas3D").GetComponent<RectTransform> ().position += Vector3.back * 99;
				GameObject.Find ("VectorCanvas").GetComponent<RectTransform> ().position += Vector3.back * 99;
				VectorCanvasFixed = true;
			}
			UpdateCameraPos ();
			Profiler.Start ();

			float startTime = Time.realtimeSinceStartup;

			/*if (Time.realtimeSinceStartup - startTime > processTime) {
				break;
			}*/
				
			//Finder.SelectedEntities[0]


			MeshManager.ApplyMeshUpdates ();

		}

		#region Camera
		private static void UpdateCameraPos ()
		{
			TargetSize = GameManager.currentGame.currentZoomScale;
			CameraOrthoSize = Mathf.SmoothStep (Camera.main.orthographicSize, TargetSize, 0.25f);
			if (ZoomToMouse && CameraOrthoSize - TargetSize > 0.01f) {
				Vector3 MousePos3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 MousePos = new Vector2 (MousePos3.x, MousePos3.y);
				float OrthoMultiplier = (2f / Camera.main.orthographicSize * (Camera.main.orthographicSize - CameraOrthoSize));	
				Vector2 MouseOffset = (MousePos - TargetCameraPos) * OrthoMultiplier;
				TargetCameraPos += MouseOffset;
			} 
			CameraPos += (TargetCameraPos - CameraPos) * CameraEase;
			//
			Camera.main.transform.position = new Vector3 (CameraPos.x, CameraPos.y, -100);
			Camera.main.orthographicSize = CameraOrthoSize;

		}
		public static void MoveCameraBy (Vector2 amount)
		{
			TargetCameraPos = TargetCameraPos + amount;
		}
		public static void MoveCameraTo (Vector2 pos, float zoom = 0)
		{
			TargetCameraPos = pos;
			if (zoom != 0) {
				GameManager.currentGame.currentZoomScale = zoom;
			}
		}
		#endregion

	}
}