﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI.Pathfinding;
namespace Cosmos
{
	public static class InputManager
	{
		public static float CameraSpeed = 0.3f;
		public static float CameraEase = 0.1f;
		public static Dictionary<string,bool> KeyIsDown = new Dictionary<string, bool> ();
		private static Entity lastSelectedEntity;
		private static int depthChangeDelay = 2;
		private static int depthChangeTime = 0;
		public static void Init ()
		{
		}
	
		public static void Update ()
		{
			CheckKeyDown ();
			CheckKeyUp ();
			ActOnKeys ();

			//

			if (Input.GetMouseButtonUp (1)) {
			}
			if (Input.GetMouseButtonUp (0)) {							

			}
			if (Input.GetMouseButtonDown (0)) {

			}
			float d = Input.GetAxis ("Mouse ScrollWheel");
			if (d > 0f) {
				GameManager.currentGame.zoom--;
				//GameManager.viewScale;
			} else if (d < 0f) {
				GameManager.currentGame.zoom++;
			}
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
				DrawManager.MoveCameraBy (new Vector2 (0.0f, 1.0f) * CameraSpeed * (float)Math.Sqrt (Camera.main.orthographicSize));
			}
			if (IsKeyDown ("S")) {
				DrawManager.MoveCameraBy (new Vector2 (0.0f, -1.0f) * CameraSpeed * (float)Math.Sqrt (Camera.main.orthographicSize));
			}
			if (IsKeyDown ("A")) {
				DrawManager.MoveCameraBy (new Vector2 (-1.0f, 0.0f) * CameraSpeed * (float)Math.Sqrt (Camera.main.orthographicSize));
			}
			if (IsKeyDown ("D")) {
				DrawManager.MoveCameraBy (new Vector2 (1.0f, 0.0f) * CameraSpeed * (float)Math.Sqrt (Camera.main.orthographicSize));
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
			if (IsKeyDown ("PAGEUP")) {
				GameManager.currentGame.ChangeSystem (GameManager.currentGame.currentSystemID + 1);
				Debug.Log (GameManager.currentGame.currentSystemID);
				//Debug.Log (GalaxyManager.currentSystem.name);
				RemoveKey ("PAGEUP");
			}
			if (IsKeyDown ("PAGEDOWN")) {
				GameManager.currentGame.ChangeSystem (GameManager.currentGame.currentSystemID - 1);
				Debug.Log (GameManager.currentGame.currentSystemID);
				//Debug.Log (GalaxyManager.currentSystem.name);
				RemoveKey ("PAGEDOWN");
			}

		}
		public  static void CheckKeyDown ()
		{
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
			if (Input.GetKeyDown (KeyCode.PageUp)) {
				AddKey ("PAGEUP");
			}
			if (Input.GetKeyDown (KeyCode.PageDown)) {
				AddKey ("PAGEDOWN");
			}
		}
		public static void CheckKeyUp ()
		{
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
			if (Input.GetKeyUp (KeyCode.PageUp)) {
				RemoveKey ("PAGEUP");
			}
			if (Input.GetKeyUp (KeyCode.PageDown)) {
				RemoveKey ("PAGEDOWN");
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
		public static bool IsKeyDown (string key)
		{			
			bool keyCheck = false;
			KeyIsDown.TryGetValue (key, out keyCheck);

			return keyCheck;
		}
	}
}