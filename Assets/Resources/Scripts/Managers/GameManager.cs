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

			InitManagers ();
			AddManagersToTicker ();
			currentGame = new Game ();
			currentGame.Start ();
			curGameState = GameState.GameRunning;
		}
		private static void InitManagers ()
		{
			Loader.Init ();
			//Load textures and generate atlases
			TextureManager.Init (128);
			//Initiate render managers
			DrawManager.Init ();					
			//Initiate Tick Manager
			TickManager.Init ();
			//Initiate User Control
			InputManager.Init ();
			//Prepare Resources
			ResourceManager.Init ();
			//Create Galaxy
			GalaxyManager.Init ();
			UIManager.Init ();
		}
		private static void AddManagersToTicker ()
		{
			//Add Fps Independant managers to ticker
			TickManager.AddPersistant (PhysicsManager.Update);
			TickManager.AddPersistant (JobManager.Update);
			TickManager.AddPersistant (Finder.Update);
		}
		public static void Update ()
		{
			TickManager.Update ();
			InputManager.Update ();
		}
		public static void LateUpdate ()
		{
			DrawManager.Draw ();
			UIManager.Update ();
			//Profiler.ExposeStrings ();
			//DrawDebugLines ();
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