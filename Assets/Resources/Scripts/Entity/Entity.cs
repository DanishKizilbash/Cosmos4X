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
		private string name;
		public Def def;
		public bool isDestroyed = false;
		public bool isVisible = true;
		public bool isInitiated = false;
		//
		private bool selected = false;
		public bool selectable = true;
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
				return selected;
			}
			set {
				OnSelected (value);
				selected = value;
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

				return Center;

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
		public Rect bounds {
			get {
				return new Rect (Position.x, Position.y, scale.x * 2, scale.y * 2);
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
				Finder.EntityDatabase.UpdateKey (name, value);
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
		public virtual void OnSelected (bool selected)
		{
			if (selected) {
				if (!Finder.SelectedEntities.Contains (this)) {
					Finder.SelectedEntities.Add (this);
					selectedLine.active = true;
					drawSelectedLine ();
				}
				Expose ();
			} else {
				Finder.SelectedEntities.Remove (this);
				selectedLine.active = false;
				selectedLine.StopDrawing3DAuto ();
			}
		}
		private void drawSelectedLine ()
		{
			float rad = scale.x > scale.y ? scale.x / 2 : scale.y / 2;
			selectedLine.MakeCircle (Center, rad);	
			selectedLine.Draw3DAuto ();
		}
		public virtual Entity Init (string defID)
		{
			//Exposer = new Exposable (this);
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
			Name = String.Join ("", new string[]{id,Finder.GlobalCount.ToString ()});
			getGraphics ();
			scale = MainGraphic.scale;
			SetAttributes ();
			if (Interval == 0) {
				Interval = 60;
			}
			Finder.EntityDatabase.Add (this);
			Finder.GlobalCount++;
			isInitiated = true;
			QueForUpdate ();
			linkedEntities = new List<Entity> ();
			stockpile = new Stockpile (this);
			selectedLine = new VectorLine ("Selected_" + name, new Vector3[36], null, 3.0f);
			selectedLine.color = Color.green;
			return this;
		}
		public virtual void getGraphics ()
		{
			string textureID = def.GetAttribute ("TextureID");	
			MainGraphic = Finder.GraphicDatabase.Get (textureID, def);
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
		public virtual void MoveTo (Vector3 iposition, bool skipParentTile=false, bool skipTick=false, bool skipDraw=false)
		{
			position = iposition;
			foreach (Entity link in linkedEntities) {
				link.MoveTo (iposition, skipTick, skipDraw);
			}
			if (isSelected) {
				drawSelectedLine ();
			}
			if (!skipTick) {
				QueForTick ();
			} 
			if (!skipDraw) {
				QueForDraw ();
			}
		}
		public virtual void MoveTo (float x, float y, float z, bool skipParentTile=false, bool skipTick=false, bool skipDraw=false)
		{
			MoveTo (new Vector3 (x, y, z), skipParentTile, skipTick, skipDraw);			
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
		public virtual void Destroy ()
		{
			OnSelected (false);
			system.RemoveEntity (this);
			MeshManager.CleanMeshDisplay (meshDisplay);
			TickManager.RemoveTicker (this);
			Finder.RemoveAllInstancesOf (this);
			/*if (Exposer != null) {
				Exposer.Destroy ();
			}
			*/
			isDestroyed = true;
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
		public virtual bool InRect (Rect rect)
		{
			return MathI.RectIntersect (rect, bounds);
		}
		public virtual void Print ()
		{
			Debug.Log ("----Printing attributes of Entity : " + name + "----");
		}
		public abstract string DefaultID ();
		//
		public override void Expose ()
		{
			string s = this.GetType ().BaseType.ToString ();
			string[] ss = s.Split (new string[1]{"."}, StringSplitOptions.None);
			Debug.Log (ss [1]);
			base.Expose ();

		}
		public override string ToString ()
		{
			return Name;
		
		}
	}
}