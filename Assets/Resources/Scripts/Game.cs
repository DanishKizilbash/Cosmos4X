using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{

	public class Game
	{		
		

		//Systems
		public List<PlanetarySystem> planetarySystems;
		public int currentSystemID;		
		public  PlanetarySystem currentSystem;
		private List<int> zoomLevels;
		public float currentZoomScale = 1;
		private int cachedZoom;
		public int zoom {
			get {
				return cachedZoom;
			}
			set {
				cachedZoom = value;
				if (cachedZoom < 0) {
					cachedZoom = 0;
				}
				if (cachedZoom > zoomLevels.Count - 1) {
					cachedZoom = zoomLevels.Count - 1;
				}
				GetCurrentZoomScale ();
			}
		}

		public Game ()
		{			
			planetarySystems = new List<PlanetarySystem> ();
			SetUpZoomLevels ();			

		}
		public void Start ()
		{
			EntityManager.AddPlanetarySystem (5);
			planetarySystems [0].SetVisiblity (true);
			TickManager.AddPersistant (Update);
		}

		public void Update ()
		{/*
			if (Random.Range (0, 100) > 98) {
				if (dir == 1) {
					dir = 2;
				} else {
					dir = 1;
				}
			}
			foreach (object obj in Finder.ActorDatabase.GetAll()) {

				try {

					ActorShip ship = (ActorShip)obj;
					if (dir == 1) {
						ship.physicsObject.ApplyThrust (new Vector3 (0.01f, 0.0f, 0), new Vector3 (-0.5f, 0.5f, 0));
					} else {
						ship.physicsObject.ApplyThrust (new Vector3 (-0.01f, 0.0f, 0), new Vector3 (0.5f, 0.5f, 0));
					
					}
					ship.physicsObject.ApplyThrust (new Vector3 (0.0f, 0.05f, 0), new Vector3 (0.5f, -0.5f, 0));
					ship.physicsObject.ApplyThrust (new Vector3 (0.0f, 0.05f, 0), new Vector3 (-0.5f, -0.5f, 0));
				} catch (UnityException e) {

				}
				Actor actor = (Actor)obj;


			}
			*/
		}
		private void SetUpZoomLevels ()
		{
			zoomLevels = new List<int> ();
			zoomLevels.Add (1);
			zoomLevels.Add (2);
			zoomLevels.Add (4);
			zoomLevels.Add (6);
			zoomLevels.Add (8);
			zoomLevels.Add (10);
			zoomLevels.Add (25);
			zoomLevels.Add (50);
			zoomLevels.Add (75);
			zoomLevels.Add (100);
			zoomLevels.Add (250);
			zoomLevels.Add (500);
			zoomLevels.Add (750);
			zoomLevels.Add (1000);
			zoomLevels.Add (2500);
			zoomLevels.Add (5000);
			zoomLevels.Add (7500);
			zoomLevels.Add (10000);
			zoomLevels.Add (15000);
			zoomLevels.Add (20000);
			zoomLevels.Add (30000);
			zoomLevels.Add (50000);
		}
		private void GetCurrentZoomScale ()
		{
			currentZoomScale = zoomLevels [zoom];
		}
		public void ChangeSystem (int id)
		{
			if (id < 0) {
				id = 0;
			}
			if (id > planetarySystems.Count - 1) {
				id = planetarySystems.Count - 1;
			}
			currentSystem.SetVisiblity (false);
			currentSystem = planetarySystems [id];
			currentSystemID = id;
			currentSystem.SetVisiblity (true);
		}
	}
}
