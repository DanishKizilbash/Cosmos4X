using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public abstract class UIElement
	{
		public string name;
		public Transform transform;
		public RectTransform rectTransform;
		public GameObject gameObject;
		public string value;
		public bool isActive;
		public virtual UIElement Init (string value = "", UIElement ParentElement=null)
		{
			if (ParentElement != null) {
				return Init (ParentElement.gameObject, value);
			} else {
				return Init (null, value);
			}
		}
		public virtual UIElement Init (GameObject ParentGameObject, string value="")
		{
			name = this.GetType ().Name.ToString ();
			value = value;
			//
			gameObject = GameObject.Instantiate (GameObject.Find (name));
			transform = gameObject.transform;
			gameObject.name = name;
			//
			if (ParentGameObject != null) {
				transform.SetParent (ParentGameObject.transform, false);
			} else if (UIManager.UICanvas != null) {
				transform.SetParent (UIManager.UICanvas.gameObject.transform, false);
			}
			SetUpElementProperties ();
			return this;
		}
		public virtual void SetUpElementProperties ()
		{
			rectTransform = gameObject.GetComponent<RectTransform> ();
			rectTransform.pivot = new Vector2 (0, 0);
			rectTransform.anchorMin = new Vector2 (0, 0);
			rectTransform.anchorMax = new Vector2 (0, 0);
			rectTransform.anchoredPosition = new Vector2 (0, 0);
			rectTransform.localScale = new Vector3 (1, 1, 1);
			rectTransform.sizeDelta = new Vector2 (100, 100);

		}
		public virtual void UpdateValue (string newValue)
		{
			Show ();
			value = newValue;
			UpdateElement ();
		}
		public virtual void Resize (Vector2 size)
		{
			rectTransform.sizeDelta = size;
		}
		public void Hide ()
		{
			gameObject.SetActive (false);
			isActive = false;
		}
		public void Show ()
		{
			gameObject.SetActive (true);
			isActive = true;
		}
		public abstract void UpdateElement ();
	}
}