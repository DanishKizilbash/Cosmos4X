using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace Cosmos
{
	public class Fleet:Tickable
	{
		public List<ActorShip> ships;
		public ActorShip flagShip;
		//
		public Colony homeColony;
		private bool _isSelected;
		public bool isSelected {
			get {
				return _isSelected;
			}
			set {
				OnSelected (value);
			}
		}
		
		private VectorLine selectedLine;
		public Fleet (ActorShip Ship):this(new List<ActorShip>(){Ship})
		{

		}
		public Fleet (List<ActorShip> Ships = null, ActorShip FlagShip =null, Colony HomeColony =null)
		{
			name = "Fleet " + (Finder.fleetDatabase.Count + 1);
			ships = new List<ActorShip> ();
			AddMultipleShips (Ships);
			SetDefaultFlagShip ();
			homeColony = HomeColony;
			Interval = 5;
			//
			selectedLine = new VectorLine ("Selected_" + name, new Vector3[8], null, 3.0f);
			selectedLine.color = Color.magenta;
			Finder.fleetDatabase.Add (this);
		}
		private void DrawSelectedLine ()
		{
			if (flagShip != null) {
				float minX = flagShip.Position.x;
				float minY = flagShip.Position.y;
				float maxX = flagShip.Position.x;
				float maxY = flagShip.Position.y;
				foreach (ActorShip ship in ships) {
					minX = MathI.Min (minX, ship.Position.x);
					minY = MathI.Min (minY, ship.Position.y - ship.scale.y);
					maxX = MathI.Max (maxX, ship.Position.x + ship.scale.x);
					maxY = MathI.Max (maxY, ship.Position.y);
				}
				selectedLine.MakeRect (new Rect (minX, minY, maxX - minX, maxY - minY));	
				selectedLine.Draw3DAuto ();
			}
		}
		public void AddMultipleShips (List<ActorShip> Ships)
		{
			foreach (ActorShip ship in Ships) {
				AddShip (ship);
			}
		}
		public void AddShip (ActorShip Ship)
		{
			if (!ships.Contains (Ship)) {
				ships.Add (Ship);				
				Ship.fleet = this;
			}
		}
		public void RemoveShip (ActorShip Ship)
		{
			if (ships.Contains (Ship)) {
				ships.Remove (Ship);
				if (Ship.fleet == this) {
					Ship.fleet = null;
				}
			}
		}
		public void SetHomeColony (Colony HomeColony)
		{
			homeColony = HomeColony;
		}
		public void SetDefaultFlagShip ()
		{
			if (ships != null && ships.Count > 0) {
				flagShip = ships [0];
			} else {
				flagShip = null;
			}
		}
		public void FleetMoveTo (Vector3 Pos)
		{
			string posString = Pos.ToString ();
			foreach (ActorShip actor in ships) {
				actor.brain.EndCurrentTasks ();
				actor.brain.AddCommand ("UserMoveTo|" + posString);				
			}
		}
		public override void Tick ()
		{
			if (ships.Count <= 0) {
				Destroy ();
			}
			if (flagShip.fleet != this) {
				SetDefaultFlagShip ();
			}
			//if (isSelected) {
			if (GameManager.currentGame.currentSystem == flagShip.system) {
				DrawSelectedLine ();
				selectedLine.active = true;
			} else {
				selectedLine.active = false;
			}
			//}
			TickRequired = true;
		}
		public override void OnSelected (bool value)
		{
			if (value) {
				Finder.DeselectAll ();
				flagShip.isSelected = true;
				Finder.selectedFleet = this;
				_isSelected = true;
				//selectedLine.active = true;
				//DrawSelectedLine ();
			} else {
				if (Finder.selectedFleet == this) {
					Finder.selectedFleet = null;
				}
				_isSelected = false;
				//selectedLine.active = false;
			}
		}
		public void ClearAllShips ()
		{
			while (ships.Count>0) {
				RemoveShip (ships [0]);
			}
		}
		public override void Destroy ()
		{
			base.Destroy ();
			ClearAllShips ();
			ships = null;
			flagShip = null;
			homeColony = null;
			Finder.fleetDatabase.Remove (this);
		}
		public override string ToString ()
		{
			return name;
		}
	}
}
