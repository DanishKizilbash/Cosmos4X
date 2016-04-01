using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI.Pathfinding;
using Vectrosity;
namespace Cosmos
{
	public static class InputManager
	{
		public static float baseCameraSpeed = 0.5f ;
		public static float CameraSpeed {
			get {
				return baseCameraSpeed * (float)Math.Sqrt (Camera.main.orthographicSize);
			}
		}
		public static float CameraEase = 0.1f;
		public static Dictionary<string,bool> KeyIsDown = new Dictionary<string, bool> ();
		private static Entity lastSelectedEntity;
		public static Camera mainCam;
		public static bool selectMode = false;
		private static Vector3 selectOrig;
		private static Rect selectRect;
		private static VectorLine selectLine;
		public static bool camLockMode = false;
		public static bool mouseMoveMode = false;
		public static Vector3 mouseMoveDistance;
		public static Vector3 previousMousePos;
		public static Vector3 worldMousePos {
			get {
				return mainCam.ScreenToWorldPoint (Input.mousePosition);
			}
		}
		public static void Init ()
		{
			mainCam = Camera.main;
			selectLine = new VectorLine ("SelectBox", new Vector3[8], null, 1.0f);
		}
	
		public static void Update ()
		{
			mouseMoveDistance = mainCam.ScreenToWorldPoint (previousMousePos) - worldMousePos;
			mainCam = Camera.main;
			CheckKeyDown ();
			CheckKeyUp ();
			ActOnKeys ();

			//Mouse
			if (!UIManager.hasMouseFocus) {
				if (Input.GetMouseButton (1)) {
					if (Math.Abs (mouseMoveDistance.magnitude) > 0.1f) {
						mouseMoveMode = true;
					}
					if (mouseMoveMode) {
						DrawManager.MoveCameraBy (mouseMoveDistance);
					}
				}
				if (Input.GetMouseButtonUp (1)) {
					if (!mouseMoveMode) {
						if (Finder.selectedFleet == null) {
							string posString = worldMousePos.ToString ();
							foreach (Entity entity in Finder.selectedEntities) {
								try {
									Actor actor = (Actor)entity;
									actor.brain.EndCurrentTasks ();
									actor.brain.AddCommand ("UserMoveTo|" + posString);
								} catch (InvalidCastException) {
								}
							}
						} else {
							Finder.selectedFleet.FleetMoveTo (worldMousePos);
						}
					} else {
						mouseMoveMode = false;
					}
				}
				if (Input.GetMouseButtonUp (0)) {
					if (Finder.selectedFleet != null) {
						Finder.selectedFleet.isSelected = false;
					}
					selectMode = false;
					if (selectRect.size.magnitude < 0.05f) {
						SelectAtPoint (worldMousePos);
					} else {
						SelectInRect (selectRect);
					}
				}
				if (Input.GetMouseButtonDown (0)) {
					selectOrig = worldMousePos;
					selectMode = true;
				}
			}
			if (selectMode) {
				selectRect = MathI.RectFromPoints (selectOrig, worldMousePos);
				DrawSelectRect ();

			} else {
				DrawSelectRect (false);
			}
			if (camLockMode) {
				if (Finder.selectedEntities.Count > 0) {
					DrawManager.MoveCameraTo (Finder.selectedEntities [0].Center);
				} else {
					camLockMode = false;
				}
			}
			float d = Input.GetAxis ("Mouse ScrollWheel");
			if (d > 0f) {
				GameManager.currentGame.zoom--;
				//GameManager.viewScale;
			} else if (d < 0f) {
				GameManager.currentGame.zoom++;
			}
			previousMousePos = Input.mousePosition;
		}
		public static void SelectInRect (Rect rect)
		{
			foreach (Entity entity in GameManager.currentGame.currentSystem.entities) {
				entity.isSelected = entity.InRect (rect);							
			}
		}
		public static void SelectAtPoint (Vector3 Point)
		{
			foreach (Entity entity in GameManager.currentGame.currentSystem.entities) {
				entity.isSelected = entity.PointInside (Point);				
			}
		}
		private static void DrawSelectRect (bool visible=true)
		{
			selectLine.MakeRect (selectRect);
			selectLine.Draw3DAuto ();
			selectLine.active = visible;
		}
		public static  void SelectEntities (List<string> SelectionFilters=null)
		{
			
		}
		private static  void MultiEntitySelect ()
		{
			
		}
		private static  void SingleEntitySelect ()
		{

		}
		public static  void SendSelectedActorsToMouse ()
		{

		}

		public static void ActOnKeys ()
		{
			if (IsKeyDown ("W")) {
				DrawManager.MoveCameraBy (new Vector2 (0.0f, 1.0f) * CameraSpeed);
			}
			if (IsKeyDown ("S")) {
				DrawManager.MoveCameraBy (new Vector2 (0.0f, -1.0f) * CameraSpeed);
			}
			if (IsKeyDown ("A")) {
				DrawManager.MoveCameraBy (new Vector2 (-1.0f, 0.0f) * CameraSpeed);
			}
			if (IsKeyDown ("D")) {
				DrawManager.MoveCameraBy (new Vector2 (1.0f, 0.0f) * CameraSpeed);
			}
			if (IsKeyDown ("V", true)) {
				if (camLockMode) {
					camLockMode = false;
				} else {
					camLockMode = true;
				}
			}
			if (IsKeyDown ("SPACE", true)) {
				TickManager.SwitchPauseState ();
			}
			if (IsKeyDown ("TAB", true)) {
				if (Finder.selectedEntities.Count > 1) {
					Entity firstEntity = Finder.selectedEntities [0];
					Finder.selectedEntities [0] = Finder.selectedEntities [Finder.selectedEntities.Count - 1];
					Finder.selectedEntities [Finder.selectedEntities.Count - 1] = firstEntity;
				}
			}
			if (IsKeyDown ("E")) {
				if (IsKeyDown ("LCONTROL")) {
					List<Entity> entitiesQuedForDestruction = new List<Entity> ();
					foreach (Entity entity in Finder.selectedEntities) {
						entitiesQuedForDestruction.Add (entity);
					}
					Finder.selectedEntities.Clear ();
					for (int i = 0; i<entitiesQuedForDestruction.Count; i++) {
						entitiesQuedForDestruction [i].Destroy ();
					}
				} else {
					foreach (Entity entity in Finder.selectedEntities) {
						if (entity != null && !entity.isDestroyed) {
							Thing thing;
							try {
								thing = (Thing)entity;
							} catch (InvalidCastException) {
								thing = null;
							}
							if (thing != null) {
								JobManager.AddJob (thing, "Work");
							}
						}
					}
				}
			}

			if (IsKeyDown ("C")) {
				foreach (Entity entity in Finder.selectedEntities) {
					CelestialBody cb;
					try {
						cb = (CelestialBody)entity;
					} catch (InvalidCastException) {
						cb = null;
					}
					if (cb != null) {
						if (cb.colony == null) {
							cb.colony = new Colony (cb);
						}
					}
				}
			}
			if (IsKeyDown ("T", true)) {
				List<ActorShip> ships = new List<ActorShip> ();
				foreach (Entity entity in Finder.selectedEntities) {
					ActorShip ship;
					try {
						ship = (ActorShip)entity;
					} catch (InvalidCastException) {
						ship = null;
					}
					if (ship != null) {
						ships.Add (ship);
					}
				}
				if (ships.Count > 0) {
					new Fleet (ships).isSelected = true;

				}
			}
			if (IsKeyDown ("F")) {
				EntityManager.AddShip (1, new Vector3 (worldMousePos.x, worldMousePos.y, 0), GameManager.currentGame.currentSystemID, null, true);
			}

			if (IsKeyDown ("PAGEUP", true)) {
				GameManager.currentGame.ChangeSystem (GameManager.currentGame.currentSystemID + 1);
				Debug.Log (GameManager.currentGame.currentSystemID);
			}
			if (IsKeyDown ("PAGEDOWN", true)) {
				GameManager.currentGame.ChangeSystem (GameManager.currentGame.currentSystemID - 1);
				Debug.Log (GameManager.currentGame.currentSystemID);
			}
			if (IsKeyDown (".", true)) {
				TickManager.tickSpeedMultiplier += 0.5f;
			}
			if (IsKeyDown (",", true)) {
				if (TickManager.tickSpeedMultiplier > 0.5f) {
					TickManager.tickSpeedMultiplier -= 0.5f;
				}
			}

		}
		public  static void CheckKeyDown ()
		{
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				AddKey ("LCONTROL");
			}	
			if (Input.GetKeyDown (KeyCode.W)) {
				AddKey ("W");
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				AddKey ("S");
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				AddKey ("A");
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				AddKey ("D");
			}
			if (Input.GetKeyDown (KeyCode.F)) {
				AddKey ("F");
			}
			if (Input.GetKeyDown (KeyCode.T)) {
				AddKey ("T");
			}
			if (Input.GetKeyDown (KeyCode.C)) {
				AddKey ("C");
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				AddKey ("SPACE");
			}	
			if (Input.GetKeyDown (KeyCode.Tab)) {
				AddKey ("TAB");
			}	
			if (Input.GetKeyDown (KeyCode.E)) {
				AddKey ("E");
			}
			if (Input.GetKeyDown (KeyCode.V)) {
				AddKey ("V");
			}
			if (Input.GetKeyDown (KeyCode.PageUp)) {
				AddKey ("PAGEUP");
			}
			if (Input.GetKeyDown (KeyCode.PageDown)) {
				AddKey ("PAGEDOWN");
			}
			if (Input.GetKeyDown (KeyCode.Period)) {
				AddKey (".");
			}
			if (Input.GetKeyDown (KeyCode.Comma)) {
				AddKey (",");
			}
		}
		public static void CheckKeyUp ()
		{
			
			if (Input.GetKeyUp (KeyCode.LeftControl)) {
				RemoveKey ("LCONTROL");
			}
			if (Input.GetKeyUp (KeyCode.W)) {
				RemoveKey ("W");
			}
			if (Input.GetKeyUp (KeyCode.S)) {
				RemoveKey ("S");
			}
			if (Input.GetKeyUp (KeyCode.A)) {
				RemoveKey ("A");
			}
			if (Input.GetKeyUp (KeyCode.D)) {
				RemoveKey ("D");
			}
			if (Input.GetKeyUp (KeyCode.F)) {
				RemoveKey ("F");
			}
			if (Input.GetKeyUp (KeyCode.T)) {
				RemoveKey ("T");
			}
			if (Input.GetKeyUp (KeyCode.C)) {
				RemoveKey ("C");
			}
			if (Input.GetKeyUp (KeyCode.Space)) {
				RemoveKey ("SPACE");
			}	
			if (Input.GetKeyUp (KeyCode.Tab)) {
				RemoveKey ("TAB");
			}
			if (Input.GetKeyUp (KeyCode.E)) {
				RemoveKey ("E");
			}
			if (Input.GetKeyUp (KeyCode.V)) {
				RemoveKey ("V");
			}
			if (Input.GetKeyUp (KeyCode.PageUp)) {
				RemoveKey ("PAGEUP");
			}
			if (Input.GetKeyUp (KeyCode.PageDown)) {
				RemoveKey ("PAGEDOWN");
			}
			if (Input.GetKeyUp (KeyCode.Period)) {
				RemoveKey (".");
			}
			if (Input.GetKeyUp (KeyCode.Comma)) {
				RemoveKey (",");
			}
		}
		public static void AddKey (string key)
		{
			if (KeyIsDown.ContainsKey (key)) {
				KeyIsDown [key] = true;
			} else {
				KeyIsDown.Add (key, true);
			}
		}
		public static void RemoveKey (string key)
		{
			if (KeyIsDown.ContainsKey (key)) {
				KeyIsDown [key] = false;
			} else {
				KeyIsDown.Add (key, false);
			}
		}
		public static bool IsKeyDown (string key, bool oneTimePress=false)
		{			
			bool keyCheck = false;
			KeyIsDown.TryGetValue (key, out keyCheck);
			if (oneTimePress) {				
				RemoveKey (key);
			}
			return keyCheck;
		}
	}
}