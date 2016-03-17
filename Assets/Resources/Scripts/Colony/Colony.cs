using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class Colony
	{
		public string name;
		public CelestialBody parent;
		public Stockpile stockpile;
		public float population; //in millions
		public float constructionYards;
		public float shipYards;
		public Colony (CelestialBody parentcb)
		{
			parent = parentcb;
			stockpile = new Stockpile (parent);
			population = 0;
			constructionYards = 0;
			shipYards = 0;
			name = parent.Name + " station";
		}
		public void BuildShip (int qty=1)
		{
			EntityManager.AddShip (qty, (parent.Position + (new Vector3 (parent.scale.x, parent.scale.y, 0) / 2)), parent.system.ID, this);
		}
	}
}