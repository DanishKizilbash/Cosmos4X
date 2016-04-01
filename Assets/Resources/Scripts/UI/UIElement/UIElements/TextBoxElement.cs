using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class TextBoxElement:UIElement
	{	
		public Text textField = null;
		public float textBuffer = 4;
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
			if (header == "") {
				textField.text = value;
			} else {
				textField.text = header + " : " + value;
			}

		}
		public override void Autofit ()
		{
			Resize (new Vector2 (textField.preferredWidth + textBuffer, rectTransform.rect.height));
		}
	}
}