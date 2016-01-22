using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			//Create Galaxy
			GalaxyManager.Init ();
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
				DrawManager.Draw ();

			}
			//Profiler.ExposeStrings ();

		}

	}
}