using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class PanelElement:UIElement
	{	
		public bool moveWithMouse = false;
		private Vector3 lastMousePos;
		public override Rect rect {
			get {
				return new Rect (rectTransform.anchoredPosition3D, rectTransform.sizeDelta);				
			}
		}
		public override void SetUpElementProperties ()
		{
			base.SetUpElementProperties ();
			rectTransform.sizeDelta = new Vector2 (50, 50);
		}
		public override void UpdateElement ()
		{
			if (moveWithMouse) {
				if (parentElement == null) {
					value = (position + Input.mousePosition - lastMousePos).ToString ();
				} else {
					PanelElement parentPanel = null;
					try {
						parentPanel = (PanelElement)parentElement;
					} catch (UnityException ex) {
					}
					if (parentPanel != null) {
						UIElement newPanel = null;
						UIElement curPanel = parentPanel;
						while (curPanel!=null) {
							newPanel = curPanel;
							curPanel = newPanel.parentElement;
						}
						PanelElement basePanel = (PanelElement)newPanel;
						basePanel.value = (basePanel.position + Input.mousePosition - lastMousePos).ToString ();
					}
				}
				lastMousePos = Input.mousePosition;
			}
			if (value != "") {
				Vector3 parsedPos = Parser.StringToVector3 (value);
				if (parsedPos != MathI.InfinityVector) {
					MoveTo (parsedPos);
				}
			}
			if (!Input.GetMouseButton (1)) {
				moveWithMouse = false;
			}
		}
		public override void OnMouseDown (int button)
		{
			base.OnMouseDown (button);
			if (button == 1) {
				moveWithMouse = true;
				lastMousePos = Input.mousePosition;
			}
		}
		public override void Autofit ()
		{

		}
	}
}