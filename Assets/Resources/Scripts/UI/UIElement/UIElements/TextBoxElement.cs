using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class TextBoxElement:UIElement
	{	
		Text textField = null;
		public override void SetUpElementProperties ()
		{
			base.SetUpElementProperties ();
			rectTransform.sizeDelta = new Vector2 (140, 20);
		}
		public override void UpdateElement ()
		{
			if (textField == null) {
				textField = gameObject.GetComponent<Text> ();
			}
			textField.text = value;
			if (value.Length * textField.fontSize / 2 > rectTransform.sizeDelta.x) {
				rectTransform.sizeDelta = new Vector2 ((value.Length + 1) * textField.fontSize / 2, rectTransform.sizeDelta.y);
			}
		}
	}
}