using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace Cosmos
{
	public abstract class Entity:Tickable
	{
		public string cachedDefID = "";
		public int lightLevel = 0;
		public Exposable Exposer;
		private Vector3 position = Vector3.zero;
		private Graphic graphic;		
		public MeshDisplay meshDisplay;
		public SelectionIndicator selectionEntity;
		private string name;
		public Def def;
		public bool isDestroyed = false;
		public bool isVisible = true;
		public bool isInitiated = false;
		private bool selected = false;
		public bool selectable = true;
		public bool forceChunkUpdate = false;
		public List<Entity> linkedEntities = new List<Entity> ();
		public bool adoptable = true;
		public bool isSelected {
			get {
				return selected;
			}
			set {
				if (!selected) {
					OnSelected (true);
				} else {						
					OnSelected (false);	
				}	
				selected = value;
			}
		}
		//	
		public virtual Vector3 TrueCenter ()
		{
			return Position + new Vector3 (MainGraphic.scale / 2, 0, MainGraphic.scale / 2);
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
				Vector3 pos = Position;
				float scale = DrawManager.tileSize / 100;
				Vector3 tileCenter = new Vector3 (0, 0, pos.z);
				tileCenter.x = pos.x + scale / 2f;//Left+halfX
				tileCenter.y = pos.y + scale / 2f;//Bottom +halfY
				return tileCenter;
			}
		}
		public virtual Vector3 ScreenPosition {
			get {

				return MathI.IsoToWorld (Center);

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
				if (selectionEntity == null) {
					selectionEntity = new SelectionIndicator ();
				}				
				selectionEntity.targetEntity = this;
				selectionEntity.Init ("GUI_Indicator_Selection_SelectionIndicator");				
			} else {
				if (selectionEntity != null) {
					selectionEntity.Destroy ();
				}
			}
		}
		public virtual Entity Init (string defID)
		{
			Exposer = new Exposable (this);
			getDef (defID);
			if (def == null) {
				return null;
			}
			Name = String.Join ("", new string[]{def.ID,Finder.GlobalCount.ToString ()});
			getGraphics ();
			SetAttributes ();
			if (Interval == 0) {
				Interval = 60;
			}
			Finder.EntityDatabase.Add (this);
			Finder.GlobalCount++;
			isInitiated = true;
			QueForUpdate ();
			linkedEntities = new List<Entity> ();
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
		}
		public virtual void Destroy ()
		{
			OnSelected (false);

			MeshManager.CleanMeshDisplay (meshDisplay);
			TickManager.RemoveTicker (this);
			Finder.RemoveAllInstancesOf (this);
			if (Exposer != null) {
				Exposer.Destroy ();
			}
			isDestroyed = true;
		}
		//
		public virtual void SetAttributes ()
		{

		}
		public virtual void SetLateAttributes ()
		{
		}
		public abstract string DefaultID ();
		//
		public override string ToString ()
		{
			return Name;
		
		}
	}
}