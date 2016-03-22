using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace Cosmos
{
	public static class GameManager
	{
		public enum GameState
		{
			MainMenu,
			GameRunning,
			GamePaused,
			GameLoading
		}
		public static GameState curGameState = GameState.MainMenu;		
		public static Vector3 screenPos = new Vector3 (0, 0, 0);
		public static Game currentGame;
		private static List<DebugLine> debugLineList = new List<DebugLine> ();
		private static List<VectorLine> debugVectorLines = new List<VectorLine> ();
		public static void NewGame (float Seed, Vector3 MapSize)
		{

			Profiler.Start ();
			Debug.Log ("Loading New Game");
			curGameState = GameState.GameLoading;
			Loader.Init ();
			//Load textures and generate atlases
			TextureManager.Init (128);
			//Initiate render managers
			DrawManager.Init ();

			//Generate/Parse World
			//Gen.WorldGen.Initiate ();			
			//Initiate Tick Manager
			TickManager.Init ();
			//Initiate User Control
			InputManager.Init ();
			//Prepare Resources
			ResourceManager.Init ();
			//Create Galaxy
			GalaxyManager.Init ();
			UIManager.Init ();
			currentGame = new Game ();
			currentGame.Start ();
			curGameState = GameState.GameRunning;
		}
		public static void PauseGame ()
		{
			curGameState = GameState.GamePaused;
		}
		public static void ResumeGame ()
		{
			curGameState = GameState.GameRunning;
		}
		public static void Update ()
		{
			if (curGameState == GameState.GameRunning) {
				PhysicsManager.Update ();
				JobManager.Update ();
				TickManager.Update ();
				currentGame.Update ();
			}
			InputManager.Update ();
		}
		public static void LateUpdate ()
		{
			if (curGameState == GameState.GameRunning) {	
				Finder.Update ();
			}
			DrawManager.Draw ();
			UIManager.Update ();
			//Profiler.ExposeStrings ();
			DrawDebugLines ();

		}

		private static void DrawDebugLines ()
		{
			VectorLine.Destroy (debugVectorLines);
			debugVectorLines = new List<VectorLine> ();
			foreach (DebugLine dLine in debugLineList) {

				Vector3[] pts = new Vector3[2]{dLine.Start,dLine.End};
				VectorLine debugLine = VectorLine.SetLine3D (dLine.lColor, pts);
				debugVectorLines.Add (debugLine);
			}
			debugLineList.Clear ();
		}

		public static void DrawLine (Vector3 start, Vector3 end, Color color)
		{
			debugLineList.Add (new DebugLine (start, end, color));
		}

		private class DebugLine
		{
			public Vector3 Start;
			public Vector3 End;
			public Color lColor;
			public DebugLine (Vector3 start, Vector3 end, Color color)
			{
				Start = start;
				End = end;
				lColor = color;
			}
		}
	}

}