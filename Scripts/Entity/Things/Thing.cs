using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Eniso {
	public abstract class Thing:Entity {
		public bool BypassDrops = false;
		public List<Drop> Drops;
		//
		public int workRequired = 0;
		public int completedWork = 0;
		//
		public Job assignedJob = null;
		public override Entity Init (string defID) {
			Entity tEntity = base.Init (defID);
			Drops = ((ThingDef)def).drops;
			int defWork = Parser.StringToInt (def.GetAttribute ("WorkRequired"));
			if (defWork != 0) {
				workRequired = defWork;
			}
			return tEntity;
		}
		public override void Destroy () {
			if (!BypassDrops && !isDestroyed) {
				GenerateDrops ();
			}
			base.Destroy ();
		}
		public virtual void Work (int amount) {
			if (workRequired > 0) {
				completedWork += amount;
				if (completedWork >= workRequired) {
					assignedJob.ReportWorkSuccess ();
					Destroy ();
				}
			}
			Exposer.AddNote ("Work Left: " + (workRequired - completedWork));
		}
		public virtual void GenerateDrops () {
			foreach (Drop drop in Drops) {
				drop.CreateNew (this);
			}
		}
	}
}