using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI;

namespace Cosmos
{
	public class ActorShip : Actor
	{
		private Fleet _fleet;
		public Fleet fleet {
			get {
				return _fleet;
			}
			set {
				Fleet oldFleet = _fleet;
				_fleet = value;
				if (_fleet != oldFleet) {
					if (oldFleet != null) {
						oldFleet.RemoveShip (this);
					}
					if (_fleet != null) {
						_fleet.AddShip (this);
					}
				}
			}
		}
		private Colony _homeColony;
		public Colony homeColony {
			get {
				if (_homeColony != null) {
					if (_homeColony.parent == null) {
						homeColony = null;
					}
				}
				return _homeColony;
			}
			set {
				if (_homeColony != null) {
					_homeColony.RemoveGarrisonedShip (this);
				}
				_homeColony = value;
				if (_homeColony != null && isConstructed) {
					_homeColony.AddGarrisonedShip (this);
				}
			}
		}

		public override Entity Init (string defID)
		{
			return base.Init (defID);
		}
		public override void AddBrain ()
		{
			brain = new BrainShip ();
			brain.Initiate (this, 10);
		}
		public override void AddAnatomy ()
		{
			anatomy = new Anatomy ();
			anatomy.Initiate (this, Parser.StringToFloat (def.GetAttribute ("Life")));
			anatomy.AddLimb ("Bridge", "Limb_Ship_Bridge_BasicBridge");
			AddThruster (2, new Vector3 (0f, 0.02f, 0f), new Vector3 (-0.5f, -0.5f, 0f));
			AddThruster (2, new Vector3 (0f, 0.02f, 0f), new Vector3 (0.5f, -0.5f, 0f));
			//
			AddThruster (2, new Vector3 (0.05f, 0f, 0f), new Vector3 (-1f, 1f, 0f), true);
			AddThruster (2, new Vector3 (-0.05f, 0f, 0f), new Vector3 (1f, 1f, 0f), true);
			AddThruster (2, new Vector3 (0.05f, 0f, 0f), new Vector3 (-1f, -1f, 0f), true);
			AddThruster (2, new Vector3 (-0.05f, 0f, 0f), new Vector3 (1f, -1f, 0f), true);
			anatomy.Invincible = true;
			//Exposer.AddPersistent (anatomy);
		}
		public void AddThruster (float condition, Vector3 force, Vector3 offset, bool isRCS=false)
		{
			string defIDString = "Limb_Ship_Thruster_";
			ThrusterLimb thrusterLimb;
			if (isRCS) {
				thrusterLimb = (ThrusterLimb)new ThrusterLimb ().Init (defIDString + "BasicRCSThruster");
			} else {
				thrusterLimb = (ThrusterLimb)new ThrusterLimb ().Init (defIDString + "BasicThruster");
			}
			anatomy.AddLimb ("Thruster", "", thrusterLimb);
			thrusterLimb.force = force;
			thrusterLimb.offset = offset;	
		}
		public override void Tick ()
		{
			base.Tick ();
		}
		public override void SetConstructed (bool value)
		{
			base.SetConstructed (value);
			homeColony = _homeColony;
		}

		public override void OnSelected (bool selected)
		{
			base.OnSelected (selected);
			if (selected) {
				if (isSelected) {
					if (fleet != null) {
						Finder.selectedFleet = fleet;
					}
				}
			} 
		}
		public override void Destroy ()
		{
			base.Destroy ();
			fleet.RemoveShip (this);
			if (homeColony != null) {
				homeColony.RemoveGarrisonedShip (this);
				homeColony = null;
			}
		}
		public override string DefaultID ()
		{
			return "Thing_Actor_Ship_Escort";
		}
	}
}