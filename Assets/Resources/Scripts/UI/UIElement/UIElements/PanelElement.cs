using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class PanelElement:UIElement
	{	
		public Vector3 position = new Vector3 (0, 0, 0);
		public override void SetUpElementProperties ()
		{
			base.SetUpElementProperties ();
			rectTransform.sizeDelta = new Vector2 (150, 200);
		}
		public override void UpdateElement ()
		{
			if (value != "") {
				position = Parser.StringToVector3 (value);
				transform.position = position;
			}
		}
	}
}