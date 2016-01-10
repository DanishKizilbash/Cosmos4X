using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public abstract class Thing:Entity
	{
		public bool BypassDrops = false;
		public List<Drop> Drops;
		public PhysicsObject physicsObject;
		//
		public override Entity Init (string defID)
		{
			Entity tEntity = base.Init (defID);
			Drops = ((ThingDef)def).drops;
			physicsObject = new PhysicsObject (this);
			return tEntity;
		}
		public override void Destroy ()
		{
			if (!BypassDrops && !isDestroyed) {
				GenerateDrops ();
			}
			base.Destroy ();
		}
		public virtual void GenerateDrops ()
		{
			foreach (Drop drop in Drops) {
				drop.CreateNew (this);
			}
		}
	}
}