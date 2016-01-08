using UnityEngine;
using System.Collections;

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
			/*
			//Generate/Parse World
			Gen.WorldGen.Initiate ();			
			//Initiate Tick Manager
			TickManager.Init ();
			//Initiate User Control
			InputManager.Init ();

			*/
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
				/*
				 JobManager.Update ();
			TickManager.Update ();
			InputManager.Update ();
				 */
			}
		}
		public static void LateUpdate ()
		{
			if (curGameState == GameState.GameRunning) {	
				/*
				Finder.Update ();
				DrawManager.Draw ();
				 */
			}
			//Profiler.ExposeStrings ();

		}

	}
}