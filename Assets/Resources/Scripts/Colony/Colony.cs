using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class Colony:Tickable
	{
		public CelestialBody parent;
		public Stockpile stockpile;
		public float population; //in millions
		public float constructionYards;
		public float baseConstructionPts = 5;
		public float shipYards;
		public List<ActorShip> garrisonedShips;
		public List<ConstructionJob> constructionJobs;
		public Colony (CelestialBody parentcb)
		{
			parent = parentcb;
			stockpile = new Stockpile (parent);
			population = 0;
			constructionYards = 500;
			shipYards = 0;
			garrisonedShips = new List<ActorShip> ();
			constructionJobs = new List<ConstructionJob> ();
			name = parent.Name + " Colony";
			Interval = 60;
		}
		public void BuildConstructionYard (int qty=1)
		{
			for (int i = 0; i<qty; i++) {
				constructionYards++;
				//ConstructionJob newJob = new ConstructionJob (this, EntityManager.AddShip (1, parent.Position, parent.system.ID, this) [0]);
				//AddConstructionJob (newJob);
			}	
		}
		public void BuildShip (int qty=1)
		{
			for (int i = 0; i<qty; i++) {
				ConstructionJob newJob = new ConstructionJob (this, EntityManager.AddShip (1, parent.Center, parent.system.ID, this) [0]);
				AddConstructionJob (newJob);
			}			
		}

		public void AddConstructionJob (ConstructionJob job)
		{
			constructionJobs.Add (job);
		}
		public void RemoveConstructionJob (ConstructionJob job)
		{
			constructionJobs.Remove (job);
		}
		public void AddGarrisonedShip (ActorShip ship)
		{
			garrisonedShips.Add (ship);
		}
		public void RemoveGarrisonedShip (ActorShip ship)
		{
			garrisonedShips.Remove (ship);
		}
		public override void Destroy ()
		{
			base.Destroy ();
			garrisonedShips.Clear ();
			stockpile.Destroy ();
			parent = null;
			foreach (ActorShip ship in garrisonedShips) {
				ship.homeColony = null;
			}
		}
		public override void Tick ()
		{
			PerformConstructionJobs ();
			TickRequired = true;
		}
		private void PerformConstructionJobs ()
		{
			float availablePts = constructionYards + baseConstructionPts;
			while (availablePts>0&&constructionJobs.Count > 0) {
				ConstructionJob curJob = constructionJobs [0];
				float reqPts = curJob.constructionPoints;
				curJob.constructionPoints -= availablePts;
				availablePts -= reqPts;
			}
		}
		public override void OnSelected (bool value)
		{

		}
		public override string ToString ()
		{
			return name;
		}
	}
}