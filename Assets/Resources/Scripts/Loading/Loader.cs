using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;

namespace Cosmos
{
	public static class Loader
	{
		public enum LoadState
		{
			Loading,
			Finished
		}
		public static LoadState curLoadState = LoadState.Finished;
		public static void Init ()
		{
			curLoadState = LoadState.Loading;
			LoadTextureFolders ();
			LoadXMLPaths ();
			curLoadState = LoadState.Finished;
		}

		public static void LoadXMLPaths ()
		{
			Debug.Log (Application.dataPath);
			DirectoryInfo dir = new DirectoryInfo (Application.dataPath + "/Resources/XMLDefs");
			FileInfo[] info = dir.GetFiles ("*.xml");
			var pathname = info.Select (f => f.FullName).ToArray ();
			foreach (string f in pathname) {
				int lastSlash = f.LastIndexOf ("\\");
				string fileName = f.Substring (lastSlash + 1);
				LoadXML ("/Resources/XMLDefs/" + fileName);
			}
			LoadJobsXML ("/Resources/XMLDefs/JobDefs.xml");
		}

		public static void LoadTextureFolders ()
		{		
			LoadTextures ("Textures");
		}

		public static void LoadTextures (string path)
		{
			object[] LoadedTextures = LoadPath (path, typeof(Texture2D));
			foreach (object t in LoadedTextures) {
				Texture2D tTex = (Texture2D)t;
				string tempName = tTex.name;
				tTex.name = tempName;
				Debug.Log ("Adding Texture: " + tempName);
				Finder.textureDatabase.Add (tTex);
			}
		}

		public static object[] LoadPath (string path, System.Type type=null)
		{
			Debug.Log ("Loading " + path);
			object[] Obj;
			if (type == null) {
				Obj = Resources.LoadAll (path);
			} else {
				Obj = Resources.LoadAll (path, type);
			}
			if (Obj == null) {
				Debug.Log ("Resource not found or Path invalid");
			}
			return Obj;
		}

		public static void LoadXML (string path)
		{
			//Load xml doc
			XmlDocument doc = new XmlDocument ();
			doc.Load (Application.dataPath + path);
			XmlNode root = doc.DocumentElement;
			//
			string baseClass;
			string type;
			string category;
			string id;
			Dictionary<string,string> attributes;

			baseClass = root.Name;

			XmlNodeList xmlTypes = root.ChildNodes;
			foreach (XmlNode Type in xmlTypes) {	
				type = Type.Name;
				foreach (XmlNode Category in Type.ChildNodes) {				
					category = Category.Name;
					foreach (XmlNode ID in Category.ChildNodes) {
						id = ID.Name;
						attributes = new Dictionary<string, string> ();
						foreach (XmlNode Attribute in ID.ChildNodes) {
							attributes.Add (Attribute.Name, Attribute.InnerText);
						}

						Finder.DefDatabase.Add (baseClass, type, category, id, attributes);
					}				
				}
			}
		}
		public static void LoadJobsXML (string path)
		{
			//Load xml doc
			XmlDocument doc = new XmlDocument ();
			doc.Load (Application.dataPath + path);
			XmlNode root = doc.DocumentElement;
			//
			string skillCategory;
			string skillValue;
			string skillName;
			XmlNodeList xmlTypes = root.ChildNodes;
			foreach (XmlNode Cat in xmlTypes) {	
				skillCategory = Cat.Name;
				foreach (XmlNode Skill in Cat.ChildNodes) {
					skillName = Skill.Name;
					skillValue = Skill.InnerText;
					SkillsTemplate.AddSkill (skillName, Parser.StringToInt (skillValue), skillCategory);
				}

			}
		}
	}
}