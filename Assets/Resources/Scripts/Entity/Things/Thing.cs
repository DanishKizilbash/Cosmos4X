using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public abstract class Thing:Entity
	{
		public bool BypassDrops = false;
		public List<Drop> Drops;
		public PhysicsObject physicsObject;
		public float constructionPointsRequired;
		private bool _isConstructed;
		public bool isConstructed {
			get {
				return _isConstructed;
			}
			set {
				SetConstructed (value);
			}
		}
		//
		public override Entity Init (string defID)
		{
			base.Init (defID);
			Drops = ((ThingDef)def).drops;
			physicsObject = new PhysicsObject (this, new Vector3 (1, 1, 1));
			return this;
		}
		public override void Destroy ()
		{
			if (Drops != null && !BypassDrops && !isDestroyed) {
				GenerateDrops ();
			}
			physicsObject.Destroy ();
			base.Destroy ();
		}
		public virtual void GenerateDrops ()
		{
			foreach (Drop drop in Drops) {
				drop.CreateNew (this);
			}
		}
		public override void SetAttributes ()
		{
			base.SetAttributes ();
			constructionPointsRequired = Parser.StringToFloat (def.GetAttribute ("ConstructionPoints"));
		}
		public override void Tick ()
		{
			base.Tick ();
		}
		public virtual void SetConstructed (bool value)
		{
			_isConstructed = value;
			SetVisible (value);
		}
	}
}