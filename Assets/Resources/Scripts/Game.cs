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
		public List<ActorShip> ships;
		private int dir = 1;
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
			ships = EntityManager.AddShip (1);	
		}

		public void Update ()
		{
			if (Random.Range (0, 100) > 98) {
				if (dir == 1) {
					dir = 2;
				} else {
					dir = 1;
				}
			}
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
		private void SetUpZoomLevels ()
		{
			zoomLevels = new List<int> ();
			zoomLevels.Add (1);
			zoomLevels.Add (10);
			zoomLevels.Add (100);
			zoomLevels.Add (500);
			zoomLevels.Add (1000);
			zoomLevels.Add (2500);
			zoomLevels.Add (5000);
			zoomLevels.Add (7500);
			zoomLevels.Add (10000);
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
			currentSystem.SetEntityVisiblity (false);
			currentSystem = planetarySystems [id];
			currentSystemID = id;
			currentSystem.SetEntityVisiblity (true);
		}
	}
}
