using UnityEngine;
using System.Collections;
namespace Eniso {
	public class SelectionIndicator : GUIEntity {
		
		public Entity targetEntity;
		public override Entity Init (string defID) { 
			Entity entity = base.Init (defID);
			if (targetEntity != null) {
				targetEntity.AddLinkedEntity (this);
				MoveTo (targetEntity.Position);
			} else {
				Debug.Log ("No target Entity for selection Indicator: " + Name);
				Destroy ();
			}
			return entity;
		}
		public override Vector3 ScreenPosition {
			get {
				return MathI.IsoToWorld (Center) + Vector3.back * 0.5f;
			}
		}
		public override void Destroy () {
			if (targetEntity != null) {
				targetEntity.RemoveLinkedEntity (this);
			}
			base.Destroy ();
		}
		public override string DefaultID () {
			return "GUI_Indicator_Selection_SelectionIndicator";
		}
	}
}