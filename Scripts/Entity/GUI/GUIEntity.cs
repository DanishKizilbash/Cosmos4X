using UnityEngine;
using System.Collections;
namespace Eniso {
	public abstract class GUIEntity : Entity {

		public override Entity Init (string defID) {

			isVisible = true;
			forceChunkUpdate = false;
			selectable = false;
			parentDrawSort = -100;
			return base.Init (defID);
		}
		public override Vector3 ScreenPosition {
			get {
				return MathI.IsoToWorld (Center) + Vector3.back * 0.5f;
			}
		}
		public override void QueForUpdate () {
			QueForDraw ();
		}
		public override string DefaultID () {
			return "GUI_Indicator_Mouse_MouseTile";
		}
	}
}