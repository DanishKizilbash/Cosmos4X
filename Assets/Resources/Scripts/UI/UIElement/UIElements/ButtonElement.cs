using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class ButtonElement:UIElement
	{	
		public Text textField = null;
		public Button button = null;
		public float buttonBuffer = 15;
		public override void SetUpElementProperties ()
		{
			base.SetUpElementProperties ();
			if (button == null) {
				button = gameObject.GetComponent<Button> ();
			}
			rectTransform.sizeDelta = new Vector2 (140, 20);
		}
		public override void UpdateElement ()
		{
			if (textField == null) {
				textField = button.GetComponentInChildren<Text> ();
			}
			if (textField != null) {
				textField.text = header;
			}
		}
		public override void Autofit ()
		{
			if (textField != null && button != null) {
				Resize (new Vector2 (textField.preferredWidth + buttonBuffer, textField.preferredHeight + buttonBuffer));
				//button.transfor
			}
		}

		public void OnClick ()
		{
			Debug.Log ("Click");
			if (targetAction != null) {
				targetAction.Invoke ();
			}
		}
	}
}