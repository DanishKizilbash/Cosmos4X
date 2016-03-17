using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public abstract class Panel
	{
		private bool isactive;
		public bool active {
			get {
				return isactive;
			}
			set {
				SetVisibility (value);
			}
		}
		public bool setactiveInFrame;
		public PanelElement BasePanel;
		public TextBoxElement HeaderText;
		public string name;
		public Dictionary <string,UIElement> ChildElements;
		public float xBuffer = 5;
		public float yBuffer = 2;
		public Entity currentEntity;
		public Vector2 basePanelSize = new Vector2 (100, 100);
		public Panel (string Name = "")
		{
			if (Name == "") {
				name = this.GetType ().Name.ToString ();
			}
			GetChildren ();
			UpdateChildrenTransforms ();
			active = false;
		}
		public void GetChildren ()
		{
			ChildElements = new Dictionary<string,UIElement> ();
			AddBaseUIComponents ();
			AddPanelUIComponents ();
		}
		private void AddBaseUIComponents ()
		{
			BasePanel = (PanelElement)new PanelElement ().Init ("", null);
			BasePanel.Resize (basePanelSize);
			//
			HeaderText = (TextBoxElement)new TextBoxElement ().Init ("name", BasePanel);
			AddUIElement ("header", HeaderText);
			AddUIElement ("position", new TextBoxElement ().Init ("pos", BasePanel));
		}
		public abstract void AddPanelUIComponents ();
		public UIElement AddUIElement (string name, UIElement element)
		{
			ChildElements.Add (name, element);
			return  element;
		}
		public void UpdateChildrenTransforms ()
		{
			float maxX = 0;
			float totalY = 0;
			float curY = BasePanel.rectTransform.rect.height - yBuffer;
			foreach (UIElement child in ChildElements.Values) {
				if (child.isActive) {
					curY -= child.rectTransform.rect.height;
					child.rectTransform.anchoredPosition = new Vector2 (xBuffer, curY);
					maxX = MathI.Max (child.rectTransform.rect.width, maxX);
					totalY += child.rectTransform.rect.height;
				}
			}
			if (BasePanel.rectTransform.rect.width < maxX || BasePanel.rectTransform.rect.height < totalY) {
				BasePanel.Resize (new Vector2 (MathI.Max (maxX, basePanelSize.x), MathI.Max (totalY, basePanelSize.y)));
				UpdateChildrenTransforms ();
			}
		}
		public virtual void Update ()
		{
			UpdateValues ();
			UpdateChildrenTransforms ();
			setactiveInFrame = false;
		}
		public virtual void UpdateValues ()
		{
			if (currentEntity != null) {
				UpdateElement ("header", currentEntity.Name);
				Vector2 vec2dPos = new Vector2 (currentEntity.Position.x, currentEntity.Position.y);
				UpdateElement ("position", vec2dPos.ToString ());
			}

		}
		public virtual void UpdateElement (string elementName, string value)
		{
			UIElement element;
			ChildElements.TryGetValue (elementName, out element);
			if (element != null) {
				element.UpdateValue (value);
			}
		}
		private void SetVisibility (bool vis)
		{
			if (!setactiveInFrame) {
				isactive = vis;
				BasePanel.gameObject.SetActive (vis);
				if (vis) {
					setactiveInFrame = true;
				}
			}
		}
	}
}
