using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public abstract class Panel
	{
		private bool active;
		public bool isActive {
			get {
				return active;
			}
			set {
				SetVisibility (value);
			}
		}
		public string name;
		public bool setactiveInFrame;
		public Panel parentPanel;
		public List<Panel> childPanels;
		public Dictionary <string,UIElement> childElements;
		public float xBuffer = 10;
		public float yBuffer = 10;	
		public Vector2 defaultPanelSize = new Vector2 (50, 10);
		public PanelElement panelElement;
		public TextBoxElement headerText;
		public Exposable currentExposable;
		public bool isDependant;
		//
		public float height {
			get {
				float totalY = panelElement.height;
				foreach (Panel panel in childPanels) {
					if (panel.active) {
						totalY += panel.height;
					}
				}
				return totalY;
			}
		}
		public float width {
			get {
				float maxX = panelElement.width; 
				foreach (Panel panel in childPanels) {
					if (panel.active) {
						maxX = MathI.Max (maxX, panel.width);
					}
				}
				return maxX;
			}
		}
		public Vector3 position {
			get {
				return panelElement.position;
			}
		}

		public virtual Panel Init (string Name = "", Panel ParentPanel = null, bool IsDependant=false)
		{
			if (Name == "") {
				name = this.GetType ().Name.ToString ();
			} else {
				name = Name;
			}
			isDependant = IsDependant;
			panelElement = (PanelElement)new PanelElement ().Init ("", null, "");
			childPanels = new List<Panel> ();
			parentPanel = ParentPanel;
			if (parentPanel != null) {
				AddSelfToParentPanel (parentPanel);
			}
			GetChildren ();
			Autofit ();
			isActive = false;
			return this;
		}
		public void AddPanel (Panel child)
		{
			childPanels.Add (child);
		}
		private void AddSelfToParentPanel (Panel tPanel)
		{
			panelElement.parentElement = tPanel.panelElement;
			tPanel.AddPanel (this);
		}
		public virtual void Update ()
		{	
			if (currentExposable == null) {
				isActive = false;
			} else {
				panelElement.UpdateElement ();
				UpdateChildPanels ();
				if (parentPanel != null) {
					UpdateParentPanel ();
				}
				UpdateValues ();
				Autofit ();
			}
			setactiveInFrame = false;
		}
		public virtual void CatchChildUpdate ()
		{
			if (parentPanel != null) {
				UpdateParentPanel ();
			}
			UpdateValues ();
			Autofit ();
		}

		private void UpdateParentPanel ()
		{

			parentPanel.currentExposable = currentExposable;
			parentPanel.isActive = isActive;
			parentPanel.CatchChildUpdate ();

		}
		private void UpdateChildPanels ()
		{
			foreach (Panel panel in childPanels) {
				if (panel.isDependant) {
					panel.currentExposable = currentExposable;					
					panel.Update ();
				}
			}			
		}
		public virtual void Autofit ()
		{
			AutofitElements ();
			AutofitPanels ();
		}
		private void GetChildren ()
		{
			childElements = new Dictionary<string,UIElement> ();
			AddUIElements ();
		}
		public UIElement AddUIElement (string name, UIElement element, bool hideIfEmpty=false)
		{
			element.hideIfEmpty = hideIfEmpty;
			childElements.Add (name, element);
			return  element;
		}
		public void UpdateMouseStatus ()
		{
			Vector3 mousePos = Input.mousePosition;
			UIElement mouseElement = null;
			foreach (UIElement child in childElements.Values) {
				if (child.isActive) {
					if (MathI.PointInRectangle (mousePos, child.rect)) {
						mouseElement = child;
						break;
					}
				}
			}
			if (mouseElement != null) {
				if (Input.GetMouseButtonUp (0)) {
					mouseElement.OnMouseUp (0);
				}
				if (Input.GetMouseButtonDown (0)) {
					mouseElement.OnMouseDown (0);
				}
				if (Input.GetMouseButtonUp (1)) {
					mouseElement.OnMouseUp (1);
				}
				if (Input.GetMouseButtonDown (1)) {
					mouseElement.OnMouseDown (1);
				}
			}
			if (Input.GetMouseButtonUp (0)) {
				panelElement.OnMouseUp (0);
			}
			if (Input.GetMouseButtonDown (0)) {
				panelElement.OnMouseDown (0);
			}
			if (Input.GetMouseButtonUp (1)) {
				panelElement.OnMouseUp (1);
			}
			if (Input.GetMouseButtonDown (1)) {
				panelElement.OnMouseDown (1);
			}
		}		
		public void UpdateElement (string elementName, object value=null)
		{
			if (value == null) {
				UpdateElementString (elementName, "None");
			} else {
				UpdateElementString (elementName, value.ToString ());
			}
		}
		public void UpdateElementString (string elementName, string value="")
		{
			UIElement element;
			childElements.TryGetValue (elementName, out element);
			if (element != null) {
				element.UpdateValue (value);
			}
		}
		public void MoveTo (Vector3 pos)
		{
			panelElement.MoveTo (pos);
		}
		public void SetVisibility (bool vis)
		{
			if (!setactiveInFrame) {
				active = vis;
				panelElement.isActive = vis;
				if (vis) {
					setactiveInFrame = true;
				}
			}
		}
		public virtual void AutofitPanels ()
		{
			float maxX = 0;
			float totalY = 0;
			float curY = 0;
			foreach (Panel panel in childPanels) {
				if (panel.isActive) {
					panel.MoveTo (new Vector3 (position.x, position.y - panel.panelElement.height - totalY, 0));
					totalY += panel.height;
					maxX = MathI.Max (maxX, panel.width);
				}
			}
			//keep on screen
			bool forcedMove = false;
			float trueYPos = position.y - totalY;
			if (trueYPos < 0) {
				MoveTo (position + new Vector3 (0, -trueYPos, 0));
				forcedMove = true;
			}
			if (position.x < 0) {
				MoveTo (position + new Vector3 (-position.x, 0, 0));
				forcedMove = true;
			}
			if (position.y + panelElement.height > Camera.main.pixelHeight) {
				MoveTo (position - new Vector3 (0, position.y + panelElement.height - Camera.main.pixelHeight, 0));
				forcedMove = true;
			}
			if (position.x + panelElement.width > Camera.main.pixelWidth) {
				MoveTo (position - new Vector3 (position.x + panelElement.width - Camera.main.pixelWidth, 0, 0));
				forcedMove = true;
			}
			panelElement.Resize (new Vector2 (MathI.Max (maxX, panelElement.width), 0));
			if (forcedMove) {
				Autofit ();
			}
		}
		public virtual void AutofitElements ()
		{
			float maxX = xBuffer;
			float totalY = yBuffer;
			float curY = panelElement.height - yBuffer;
			foreach (UIElement child in childElements.Values) {
				if (child.isActive) {
					child.Autofit ();
					curY -= child.height;
					child.MoveTo (new Vector2 (xBuffer, curY));
					maxX = MathI.Max (child.width + xBuffer * 2, maxX);
					totalY += child.height;
				}
			}
			totalY += yBuffer;
			panelElement.Resize (new Vector2 (MathI.Max (maxX, defaultPanelSize.x), MathI.Max (totalY, defaultPanelSize.y)));				
		}
		public virtual void AddUIElements ()
		{
			if (parentPanel == null) {
				AddUIElement ("closepanel", new ButtonElement ().Init ("closepanel", panelElement, "X", ClosePanel));
			}
		}
		public virtual void UpdateValues ()
		{
			UpdateElement ("closepanel");
		}
		public void ClosePanel ()
		{
			Debug.Log ("close");
			currentExposable.OnSelected (false);
			isActive = false;
			currentExposable = null;
		}
	
	}
}
