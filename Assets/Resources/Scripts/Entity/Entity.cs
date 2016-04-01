using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Vectrosity;
namespace Cosmos
{
	public abstract class Entity:Tickable
	{
		public string cachedDefID = "";
		private Vector3 position = Vector3.zero;
		private Graphic graphic;	
		private Vector2 cachedScale = Vector2.one;
		public MeshDisplay meshDisplay;
		public float displayDepth;
		public Def def;
		public bool isVisible = true;
		public bool isInitiated = false;
		//
		private bool _selected = false;
		private VectorLine selectedLine;
		//
		public List<Entity> linkedEntities = new List<Entity> ();
		public bool adoptable = true;
		public float rotation;
		public Vector3 rotationPoint;
		//
		public Stockpile stockpile;
		//
		public PlanetarySystem system;
		public bool isSelected {
			get {
				return _selected;
			}
			set {
				OnSelected (value);
				_selected = value;
			}
		}
		public virtual Vector3 Position {
			get {
				return position;
			}
			set {
				MoveTo (value);
			}
		}
		public virtual Vector3 Center {
			get {
				Vector3 pos = position;
				Vector3 tileCenter = new Vector3 (0, 0, pos.z);
				tileCenter.x = pos.x + scale.x / 2f;//Left+halfX
				tileCenter.y = pos.y - scale.y / 2f;//Bottom +halfY
				return tileCenter;
			}
		}
		public virtual Vector3 ScreenPosition {
			get {
				return Position + Vector3.back * displayDepth;
			}
		}
		public Vector2 scale {
			get {
				return cachedScale;
			}
			set {
				cachedScale = value;
				if (meshDisplay != null) {
					meshDisplay.UpdateScale ();
				}
			}
		}
		public Rect rect {
			get {
				return new Rect (Position.x, Position.y, scale.x, scale.y);
			}
		}
		public virtual Graphic MainGraphic {
			get {
				if (graphic == null) {
					getGraphics ();
				}
				return graphic;
			}
			set {
				graphic = value;
			}
		}
		public virtual string Name {
			get {
				return name;
			}
			set {
				Finder.entityDatabase.UpdateKey (name, value);
				name = value;
			}
		}
		public float parentDrawSort = 0;



		public virtual void QueForUpdate ()
		{	
			QueForTick ();
			QueForDraw ();
		}
		public virtual void QueForDraw ()
		{
			if (isInitiated) {
				MeshManager.UpdateEntity (this);
			}
		}
		public virtual void QueForTick ()
		{
			TickRequired = true;
			TickManager.AddTicker (this);
			
		}
		public override void Tick ()
		{
		}
		public override void OnSelected (bool value)
		{
			if (value) {
				if (!Finder.selectedEntities.Contains (this)) {
					Finder.selectedEntities.Add (this);
					selectedLine.active = true;
					DrawSelectedLine ();
				}
			} else {
				Finder.selectedEntities.Remove (this);
				selectedLine.active = false;
				selectedLine.StopDrawing3DAuto ();
			}
		}
		private void DrawSelectedLine ()
		{
			float rad = scale.x > scale.y ? scale.x / 2 : scale.y / 2;
			selectedLine.MakeRect (new Rect (position + Vector3.down * scale.y, scale));	
			selectedLine.Draw3DAuto ();
		}
		public virtual Entity Init (string defID)
		{
			system = GameManager.currentGame.currentSystem;
			system.AddEntity (this);
			getDef (defID);
			if (def == null) {
				return null;
			}
			string id = def.ID;
			if (id == "") {
				id = "Entity";
			}
			Name = String.Join ("", new string[]{id,Finder.globalCount.ToString ()});
			getGraphics ();
			scale = MainGraphic.scale;
			SetAttributes ();
			if (Interval == 0) {
				Interval = 60;
			}
			Finder.entityDatabase.Add (this);
			Finder.globalCount++;
			isInitiated = true;
			QueForUpdate ();
			linkedEntities = new List<Entity> ();
			stockpile = new Stockpile (this);
			selectedLine = new VectorLine ("Selected_" + name, new Vector3[8], null, 3.0f);
			selectedLine.color = Color.green;
			return this;
		}
		public virtual void getGraphics ()
		{
			string textureID = def.GetAttribute ("TextureID");	
			MainGraphic = Finder.graphicDatabase.Get (textureID, def);
		}
		public virtual void getDef (string defID)
		{
			def = Finder.DefDatabase.Get (defID);
			if (def == null) {
				if (defID != "") {
					Debug.Log ("Invalid defID: " + defID + " provided, reverting to default");
				}
				defID = DefaultID ();
				def = Finder.DefDatabase.Get (defID);
				if (def == null) {
					Debug.Log ("Default def Invalid, could not create Entity: " + defID);

				}
			}
		}
		public virtual void AddLinkedEntity (Entity entity)
		{
			if (!linkedEntities.Contains (entity)) {
				linkedEntities.Add (entity);
			}
		}
		public virtual void RemoveLinkedEntity (Entity entity)
		{
			linkedEntities.Remove (entity);
		}
		public virtual void MoveCenterTo (Vector3 iposition)
		{
			Vector3 posOffset = new Vector3 (-scale.x / 2, scale.y / 2, 0);
			MoveTo (iposition + posOffset);
		}
		public virtual void MoveTo (Vector3 iposition)
		{
			position = iposition;
			foreach (Entity link in linkedEntities) {
				link.MoveTo (iposition);
			}
			if (isSelected) {
				DrawSelectedLine ();
			}
			QueForTick ();			
			QueForDraw ();			
		}
		public virtual void MoveTo (float x, float y, float z)
		{
			MoveTo (new Vector3 (x, y, z));			
		}
		public virtual void SetVisible (bool visible)
		{
			isVisible = visible;
			foreach (Entity linkedEntity in linkedEntities) {
				linkedEntity.SetVisible (visible);
			}
			if (meshDisplay != null) {
				meshDisplay.setVisibility (visible);
			}
			if (!isVisible) {
				isSelected = false;
			}
		}
		public override void Destroy ()
		{
			base.Destroy ();
			OnSelected (false);
			system.RemoveEntity (this);
			MeshManager.CleanMeshDisplay (meshDisplay);
			TickManager.RemoveTicker (this);
			Finder.RemoveAllInstancesOf (this);
		}
		//
		public virtual void SetAttributes ()
		{

		}
		public virtual void SetLateAttributes ()
		{
		}
		public virtual void ChangeSystem (PlanetarySystem newSystem)
		{
			system.RemoveEntity (this);
			newSystem.AddEntity (this);
			system = newSystem;
		}
		public virtual bool InRect (Rect tRect)
		{
			return MathI.RectIntersect (tRect, rect);
		}
		public virtual bool PointInside (Vector3 point)
		{
			return MathI.PointInRectangle (point + new Vector3 (0, rect.height, 0), rect);
		}
		public virtual void Print ()
		{
			Debug.Log ("----Printing attributes of Entity : " + name + "----");
		}
		public abstract string DefaultID ();
		//
		public override string ToString ()
		{
			return Name;
		
		}
	}
}