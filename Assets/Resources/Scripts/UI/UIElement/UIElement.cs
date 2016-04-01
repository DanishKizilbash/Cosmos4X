using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public abstract class UIElement
	{
		public string name;		
		public Action targetAction;
		public Transform transform;
		public RectTransform rectTransform;		
		public Vector3 position {
			get {
				return rectTransform.anchoredPosition3D;
			}
		}
		public GameObject gameObject;
		public UIElement parentElement;
		public string value;
		public string header;
		private bool active;
		public bool isActive {
			get {
				return active;
			}
			set {
				if (value) {
					Show ();
				} else {
					Hide ();
				}
			}
		}
		public bool hideIfEmpty;
		public Vector2 edgeBuffer;
		public float width {
			get {
				return rectTransform.rect.width + edgeBuffer.x;
			}
		}
		public float height {
			get {
				return rectTransform.rect.height + edgeBuffer.y;
			}
		}
		public virtual Rect rect {
			get {
				if (parentElement == null) {
					return new Rect (rectTransform.anchoredPosition3D, rectTransform.sizeDelta);
				} else {
					return new Rect (parentElement.rectTransform.anchoredPosition3D + rectTransform.anchoredPosition3D, rectTransform.sizeDelta);
				}
			}
		}
		public virtual UIElement Init (string Value = "", UIElement ParentElement=null, string Header="", Action action=null)
		{

			if (ParentElement != null) {
				parentElement = ParentElement;
				return Init (ParentElement.gameObject, Value, Header, action);
			} else {
				return Init (null, Value, Header, action);
			}
		}
		public virtual UIElement Init (GameObject ParentGameObject, string Value="", string Header="", Action action=null)
		{
			targetAction = action;
			name = this.GetType ().Name.ToString ();
			value = Value;
			header = Header;
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
			edgeBuffer = new Vector2 (2, 2);

		}
		public virtual void UpdateValue (string newValue)
		{
			value = newValue;
			UpdateElement ();

			if (hideIfEmpty && (value == "0" || value == "")) {
				Hide ();
			} else {
				Show ();
			}
		}
		public abstract void Autofit ();
		public virtual void Resize (Vector2 size)
		{
			float xSize = size.x;
			float ySize = size.y;
			if (xSize <= 0) {
				xSize = rectTransform.sizeDelta.x;
			}
			if (ySize <= 0) {
				ySize = rectTransform.sizeDelta.y;
			}
			rectTransform.sizeDelta = new Vector2 (xSize, ySize);
		}
		public virtual void MoveTo (Vector2 pos)
		{
			rectTransform.anchoredPosition = pos;
		}
		public virtual void OnMouseUp (int button)
		{
			if (button == 0) {
				if (targetAction != null) {
					targetAction.Invoke ();
				}
			} else if (button == 1) {				
			}
		}
		public virtual void OnMouseDown (int button)
		{
			if (button == 0) {
			} else if (button == 1) {				
			}
		}
		public void Hide ()
		{
			gameObject.SetActive (false);
			active = false;
		}
		public void Show ()
		{
			gameObject.SetActive (true);
			active = true;
		}
		public abstract void UpdateElement ();
	}
}