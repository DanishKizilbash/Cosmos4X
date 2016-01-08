using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public static class GUIController {
		public static GameObject GUIObject = GameObject.Find ("GUI");
		public static GameObject PanelMessage = GameObject.Find ("PanelMessage");
		public static GameObject TextMessage = GameObject.Find ("TextMessage");
		public static Entity CurrentEntity = null;

		public static void UpdateMessage (Exposable exposables) {
			exposables.Expose ();
			TextMessage.GetComponent<Text> ().text = "";
			TextMessage.GetComponent<Text> ().text += exposables.parent.Name + System.Environment.NewLine;
			TextMessage.GetComponent<Text> ().text += exposables.parent.Position + System.Environment.NewLine;
			if (exposables.Message != "") {
				TextMessage.GetComponent<Text> ().text += exposables.Message;
			} else {
				TextMessage.GetComponent<Text> ().text += exposables.cachedMessage;
			}
		}
		public static void UpdateMessage (string str) {
			TextMessage.GetComponent<Text> ().text = str;
		}

		public static void SetFocus (Entity entity) {
			CurrentEntity = entity;
		}
		public static void SetFocus () {
			CurrentEntity = Finder.SelectedEntities [0];
		}
		public static void DrawSelectionUI (Entity entity) {
			//GUITexture guiTex = new GUITexture ();
			//guiTex.
			//entity.gameObject;
		}
		public static void OnGUI () {
		}
		public static void Update () {
			if (CurrentEntity != null) {
				if (CurrentEntity.Exposer.isDestroyed) {
					CurrentEntity = null;
					Finder.SelectedEntities.Remove (CurrentEntity);
				} else {
					DrawSelectionUI (CurrentEntity);
					UpdateMessage (CurrentEntity.Exposer);
				}
			} else {
				UpdateMessage ("");
			}
		}
	}
}