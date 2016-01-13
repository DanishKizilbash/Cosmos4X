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
		public static List<ActorShip> ships;
		static int dir = 1;
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

			ships = EntityManager.AddShip (1);
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
			if (Random.Range (0, 100) > 98) {
				if (dir == 1) {
					dir = 2;
				} else {
					dir = 1;
				}
			}
			if (curGameState == GameState.GameRunning) {
				foreach (ActorShip ship in ships) {
					if (dir == 1) {
						ship.physicsObject.ApplyThrust (new Vector3 (0.01f, 0.0f, 0), new Vector3 (-0.5f, 0.5f, 0));
					} else {
						ship.physicsObject.ApplyThrust (new Vector3 (-0.01f, 0.0f, 0), new Vector3 (0.5f, 0.5f, 0));

					}
					ship.physicsObject.ApplyThrust (new Vector3 (0.0f, 0.05f, 0), new Vector3 (0.5f, -0.5f, 0));
					ship.physicsObject.ApplyThrust (new Vector3 (0.0f, 0.05f, 0), new Vector3 (-0.5f, -0.5f, 0));
				}
			}
			PhysicsManager.Update ();
			JobManager.Update ();
			TickManager.Update ();
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