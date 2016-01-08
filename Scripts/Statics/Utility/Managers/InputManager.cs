using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Eniso;
using Eniso.AI.Pathfinding;
namespace Eniso {
	public static class InputManager {
		public static float CameraSpeed = 0.3f;
		public static float CameraEase = 0.1f;
		public static Dictionary<string,bool> KeyIsDown = new Dictionary<string, bool> ();
		public static Tile TileAtMouse;
		public static GUIEntity mouseIndicator;
		private static Entity lastSelectedEntity;
		public static bool selectMode = false;
		public static bool selectMulti = false;
		public static List<Tile> selectedTiles = new List<Tile> ();
		public static Tile PreviousTileAtMouse;
		private static int depthChangeDelay = 2;
		private static int depthChangeTime = 0;
		public static void Init () {
			mouseIndicator = new MouseIndicator ();
			mouseIndicator.Init ("GUI_Indicator_Mouse_MouseTile");
		}
	
		public static void Update () {
			mouseIndicator.Position = GetIsoPosAtMouse ();
			mouseIndicator.ParentTile = null;
			mouseIndicator.QueForDraw ();
			CheckKeyDown ();
			CheckKeyUp ();
			ActOnKeys ();

			//
			if (depthChangeTime > 0) {
				depthChangeTime --;
			} else {
				if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
					if (DepthManager.CurrentDepth > 0) {
						DepthManager.CurrentDepth--;
						depthChangeTime = depthChangeDelay;
					}
				} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
					if (DepthManager.CurrentDepth < Finder.TileDatabase.TileMap.Count - 1) {
						DepthManager.CurrentDepth++;
						depthChangeTime = depthChangeDelay;
					}
				}
			}
			if (Input.GetMouseButtonUp (1)) {
				SendSelectedActorsToMouse ();
			}
			if (Input.GetMouseButtonUp (0)) {
				if (selectMulti) {
					foreach (Tile tile in selectedTiles) {
						MultiEntitySelect (tile);
					}
					SelectEntities ();
				}
				selectMode = false;
				selectMulti = false;				

			}
			if (Input.GetMouseButtonDown (0)) {
				if (!selectMode) {
					PreviousTileAtMouse = GetTileAtMouse ();
					Finder.ClearSelectedEntities ();
				}
				selectMode = true;
			}
			if (selectMode) {
				if (PreviousTileAtMouse != TileAtMouse) {
					selectMulti = true;
				} 
				SelectEntities ();

			}
		}
		public static  void SelectEntities (List<string> SelectionFilters=null) {
			Tile tileAtMouse = GetTileAtMouse ();
			if (tileAtMouse != null) {
				if (!selectMulti) {
					SingleEntitySelect (tileAtMouse);
					if (Finder.SelectedEntities.Count > 0 && Finder.SelectedEntities [0] != null) {	
						GUIController.SetFocus (Finder.SelectedEntities [0]);
					}
				} else {
					float x1 = tileAtMouse.Position.x;
					float x2 = PreviousTileAtMouse.Position.x;
					float z1 = tileAtMouse.Position.z;
					float z2 = PreviousTileAtMouse.Position.z;
					float xOrig = MathI.Min (x1, x2);
					float zOrig = MathI.Min (z1, z2);
					float xTarget = MathI.Max (x1, x2);
					float zTarget = MathI.Max (z1, z2);
					float xLen = Mathf.Abs (xTarget - xOrig);
					float zLen = Mathf.Abs (zTarget - zOrig);
					Tile curTile = null;
					selectedTiles.Clear ();
					for (float x = 0; x<=xLen; x++) {
						for (float z = 0; z<=zLen; z++) {
							curTile = GetTileAt (xOrig + x, zOrig + z);
							if (curTile != null) {
								selectedTiles.Add (curTile);
							}
						}
					}
					
				}

			}
		}
		private static  void MultiEntitySelect (Tile SelectedTile) {
			List<Entity> children = SelectedTile.Children;
			foreach (Entity child in children) {
				Thing thing;
				try {
					thing = (Thing)child;
				} catch (InvalidCastException) {
					thing = null;
				}
				if (thing != null) {
					Finder.SelectEntity (thing);
				}
			}
		}
		private static  void SingleEntitySelect (Tile SelectedTile) {
			if (Finder.SelectedEntities.Count == 0) {
				List<Entity> children = SelectedTile.Children;
				int checks = 0;
				bool success = false;
				Entity selectedEntity = lastSelectedEntity;
				while (!success && checks <= children.Count) {						
					success = true;
					if (children.Count > 0) {
						int index = children.Count - 1;
						if (children.Contains (selectedEntity)) {
							index = children.IndexOf (selectedEntity);
							index--;
							if (index < 0) {
								index = children.Count - 1;
							}
						}
						if (index >= 0) {
							selectedEntity = children [index];
							if (!selectedEntity.selectable) {
								success = false;
							} 
						} 
					}
					checks++;
				}
				if (selectedEntity != null) {
					Finder.SelectEntity (selectedEntity);		
					GUIController.SetFocus (selectedEntity);
				}
				lastSelectedEntity = selectedEntity;
			}
		}
		public static  void SendSelectedActorsToMouse () {
			Tile tileAtMouse = GetTileAtMouse ();
			foreach (Entity entity in Finder.SelectedEntities) {
				Actor actor = null;
				try {
					actor = (Actor)entity;
				} catch (InvalidCastException) {
					actor = null;
				}
				if (actor != null) {
					if (tileAtMouse != null) {						
						actor.brain.EndCurrentTasks ();
						string stimulusString = "UserMoveTo|" + tileAtMouse.Position.ToString ();
						actor.brain.AddStimulus (stimulusString);
					}
				}
			}
		}
		public static  Vector3 GetIsoPosAtMouse () {
			int y = DepthManager.CurrentDepth;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector3 IsoPos = MathI.WorldToIso (mousePos);
			return IsoPos;
		}
		public static  Tile GetTileAtMouse () {		
			Tile tile = null;
			int y = DepthManager.CurrentDepth;
			//while (y<ChunkManager.Dimensions.y&& (tile ==null||tile.Children.Count==0)) {
			Vector3 IsoPos = GetIsoPosAtMouse ();
			tile = Finder.TileDatabase.GetTile (IsoPos);
			y++;
			//}
			return tile;
		}
		public static  Tile GetTileAt (float x, float z) {	
			Tile tile = null;
			int y = DepthManager.CurrentDepth;
			//while (y<ChunkManager.Dimensions.y&& (tile ==null||tile.Children.Count==0)) {
			Vector3 pos = new Vector3 (x, y, z);
			tile = Finder.TileDatabase.GetTile (pos);
			y++;
			//}
			return tile;
		}
		public static void ActOnKeys () {
			if (IsKeyDown ("W")) {
				DrawManager.MoveCameraBy (new Vector2 (0.5f, 0.5f) * CameraSpeed);
			}
			if (IsKeyDown ("S")) {
				DrawManager.MoveCameraBy (new Vector2 (-0.5f, -0.5f) * CameraSpeed);
			}
			if (IsKeyDown ("A")) {
				DrawManager.MoveCameraBy (new Vector2 (-0.5f, 0.5f) * CameraSpeed);
			}
			if (IsKeyDown ("D")) {
				DrawManager.MoveCameraBy (new Vector2 (0.5f, - 0.5f) * CameraSpeed);
			}
			if (IsKeyDown ("SPACE")) {
				TickManager.SwitchPauseState ();
				RemoveKey ("SPACE");
			}
			if (IsKeyDown ("E")) {
				if (IsKeyDown ("LCONTROL")) {
					foreach (Entity entity in Finder.SelectedEntities) {
						Thing thing;
						try {
							thing = (Thing)entity;
						} catch (InvalidCastException) {
							thing = null;
						}
						if (thing != null) {
							thing.Destroy ();
						}
					}
				}
				foreach (Entity entity in Finder.SelectedEntities) {
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
		public  static void CheckKeyDown () {
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
			if (Input.GetKeyDown (KeyCode.P)) {
				AddKey ("P");
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				AddKey ("SPACE");
			}
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				AddKey ("LCONTROL");
			}			
			if (Input.GetKeyDown (KeyCode.E)) {
				AddKey ("E");
			}
		}
		public static void CheckKeyUp () {
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
			if (Input.GetKeyUp (KeyCode.P)) {
				RemoveKey ("P");
			}
			if (Input.GetKeyUp (KeyCode.Space)) {
				RemoveKey ("SPACE");
			}
			if (Input.GetKeyUp (KeyCode.LeftControl)) {
				RemoveKey ("LCONTROL");
			}	
			if (Input.GetKeyUp (KeyCode.E)) {
				RemoveKey ("E");
			}
		}
		public static void AddKey (string key) {
			if (KeyIsDown.ContainsKey (key)) {
				KeyIsDown [key] = true;
			} else {
				KeyIsDown.Add (key, true);
			}
		}
		public static void RemoveKey (string key) {
			if (KeyIsDown.ContainsKey (key)) {
				KeyIsDown [key] = false;
			} else {
				KeyIsDown.Add (key, false);
			}
		}
		public static bool IsKeyDown (string key) {			
			bool keyCheck = false;
			KeyIsDown.TryGetValue (key, out keyCheck);

			return keyCheck;
		}
	}
}