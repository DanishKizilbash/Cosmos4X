using UnityEngine;
using System.Collections;
namespace Eniso
{
	public class MouseIndicator : GUIEntity
	{
		public override Entity Init (string defID)
		{
			adoptable = false;
			return base.Init (defID);
		}
		public override Vector3 Center {
			get {
				Vector3 pos = Position;
				float scale = DrawManager.tileSize / 100;
				Vector3 tileCenter = new Vector3 (0, 0, pos.z);
				tileCenter.x = pos.x + scale / 2f;//Left+halfX
				tileCenter.y = pos.y + scale / 2f;//Bottom +halfY
				return tileCenter;
			}
		}
		public override string DefaultID ()
		{
			return "GUI_Indicator_Mouse_MouseTile";
		}
	}
}