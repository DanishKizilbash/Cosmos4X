using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
	public class Exposable {
		public Entity parent;
		public bool isDestroyed = false;
		public List<object> Persistents = new List<object> ();
		public List<string> Notes = new List<string> ();
		private string noteMessage = "";
		private string persistentMessage = "";
		public string cachedMessage = "";
		public string Message {
			get {
				cachedMessage = persistentMessage + noteMessage;
				return cachedMessage;
			}
		} 
		public Exposable (Entity iparent) {
			parent = iparent;
		}

		public string ExposePersistents () {
			string str = "";
			foreach (object p in Persistents) {
				if (p == null) {
					RemovePersistent (p);
				} else {
					str += p.ToString () + System.Environment.NewLine;				
				}
			}
			return str;
		}
		public string ExposeNotes () {
			string str = "";
			foreach (string note in Notes) {
				str += note + System.Environment.NewLine;
			}
			return str;
		}
		public void AddNote (string info) {
			if (!Notes.Contains (info)) {
				Notes.Add (info);
			}
		}
		public void AddPersistent (object persistent) {
			if (!Persistents.Contains (persistent)) {
				Persistents.Add (persistent);
			}
		}
		public void RemovePersistent (object persistent) {
			Persistents.Remove (persistent);
		}

		public void Clear () {
			Notes = new List<string> ();
		}
		public void Destroy () {
			Persistents = new List<object> ();
			Notes = new List<string> ();
			persistentMessage = "";
			noteMessage = "";
			isDestroyed = true;
		}
		public void Expose () {
			string cachedPersistents = ExposePersistents ();
			string cachedNotes = ExposeNotes ();
			if (cachedPersistents != "") {
				persistentMessage = cachedPersistents;
			}
			if (cachedNotes != "") {
				noteMessage = cachedNotes;
			}

			Clear ();
		}
	}
}