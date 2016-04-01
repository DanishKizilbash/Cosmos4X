using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Cosmos
{
	public static class Finder
	{
		private static List<object> removalQue = new List<object> ();
		public static double globalCount = 0;
		public static List<Entity> selectedEntities = new List<Entity> ();
		public static Fleet selectedFleet = null;
		//
		public static GameObjectsDatabase gameObjectDatabase = new GameObjectsDatabase ();
		public static ResourcesDatabase resourceDatabase = new ResourcesDatabase ();
		public static EntitiesDatabase entityDatabase = new EntitiesDatabase ();
		public static GraphicsDatabase graphicDatabase = new GraphicsDatabase ();
		public static TexturesDatabase textureDatabase = new TexturesDatabase ();
		public static SpriteAtlasesDatabase spriteAtlasDatabase = new SpriteAtlasesDatabase ();
		public static ActorsDatabase actorDatabase = new ActorsDatabase ();
		public static FleetsDatabase fleetDatabase = new FleetsDatabase ();
		//
		public static int exCount = 0;
		public static void ClearSelectedEntities ()
		{
			foreach (Entity entity in selectedEntities) {
				if (entity != null) {
					entity.isSelected = false;
				}
			}
			selectedEntities.Clear ();
		}
		public static void SelectEntity (Entity entity)
		{
			entity.isSelected = true;
			selectedEntities.Add (entity);
		}
		public static void DeselectEntity (Entity entity)
		{
			entity.isSelected = false;
			selectedEntities.Remove (entity);
		}
		public static void DeselectAll ()
		{
			if (Finder.selectedFleet != null) {
				Finder.selectedFleet.isSelected = false;
			}
			while (selectedEntities.Count>0) {
				selectedEntities [0].isSelected = false;
			}
		}
		public static void Update ()
		{
			ProcessRemovalQue ();
		}
		//
		private static void ProcessRemovalQue ()
		{
			foreach (object obj in removalQue) {
				TryRemove (obj);
			}
			removalQue.Clear ();
		}
		private static void TryRemove (object obj)
		{
			try {
				Entity eObj = (Entity)obj;
				entityDatabase.Remove (eObj); 
			} catch (InvalidCastException) { 
				//Debug.Log ("Cannot convert to Entity");
			}
			try {
				resourceDatabase.Remove ((Resource)obj);
			} catch (InvalidCastException) { 
				//Debug.Log ("Cannot convert to Resource");
			}

		}
		public static void RemoveAllInstancesOf (object obj)
		{
			removalQue.Add (obj);
		}


		#region Databases
		public static class DefDatabase
		{
			private static  Dictionary<string,Def> DefDictionary = new Dictionary<string,Def> ();
			
			public static void Add (Def def)
			{
				String key = def.Name;
				Def value;
				DefDictionary.TryGetValue (key, out value);
				if (value == null) {
					Debug.Log ("Adding Def " + key);
					DefDictionary.Add (key, def);
				} else {
					Debug.Log ("Def of name " + key + " already exists. Setting to new value.");
					DefDictionary [key] = def;
				}
			}

			public static void Add (string baseClass, string type, string category, string id, Dictionary<string,string> attributes)
			{
				string defString = baseClass + "Def";
				string query = "Cosmos." + defString + ", " + typeof(Def).Assembly.FullName.ToString ();
				
				Type defType = Type.GetType (query);
				if (defType != null) {
					Def newDef = (Def)Activator.CreateInstance (defType);
					newDef.init (baseClass, type, category, id, attributes);
				} else {
					Debug.Log ("Type of name " + defString + " not found with search query: " + query + ". Setting to default Def type");
					new DefaultDef ().init (baseClass, type, category, id, attributes);
				}
			}
			
			public static Def Get (string key)
			{
				Def value;
				DefDictionary.TryGetValue (key, out value);
				if (value == null) {
					if (key != "") {
						Debug.Log ("No Def for " + key + " found");
					}
				}
				return value;
			}

			public static void Print (string key="")
			{
				if (key == "") {
					foreach (Def def in DefDictionary.Values) {
						def.Print ();
					}
				} else {
					Get (key).Print ();
				}

			}
		}

		//
		public class GraphicsDatabase:Database
		{
			public void Add (Graphic graphic, string name="")
			{
				if (name == "") {
					name = graphic.name;
				}
				dictionary.Add (name, graphic);
			}

			public Graphic Get (string path, Def def, bool skipCreateNew =false)
			{
				object val = null;
				dictionary.TryGetValue (path, out val);
				Graphic value = (Graphic)val;
				if (value == null && !skipCreateNew) {
					value = new Graphic (def.GetAttribute ("TextureID"));
				}
				return value;
			}
		}
		//

		public class TexturesDatabase:Database
		{
			public override void Add (object entry)
			{
				string name = ((Texture2D)entry).name;
				if (!dictionary.ContainsKey (name)) {
					dictionary.Add (name, entry);
				}
			}
			public override object Get (string path)
			{
				if (path == "") {
					return null;
				}
				object val = null;
				dictionary.TryGetValue (path, out val);
				Texture value = (Texture)val;
				if (value == null) {
					Debug.Log ("Texture " + path + " Not Found, attempting load.");
					try {
						value = (Texture)Loader.LoadPath (path) [0];
					} catch (Exception) {
						Debug.Log ("No Texture at Path: " + path);
					}
					if (dictionary.ContainsKey (path)) {
						dictionary [path] = value;
					} else {
						dictionary.Add (path, value);
					}
				}
				return value;
			}

			public List<Texture2D> GetTexturesOfType (string id)
			{
				List<Texture2D> texList = new List<Texture2D> ();
				foreach (object obj in this.dictionary.Values) {
					Texture2D tex = (Texture2D)obj;
					string[] str = tex.name.Split (new String[]{"_"}, StringSplitOptions.None);
					if (str [0] == id) {
						texList.Add (tex);
					}
				}
				return texList;
			}
		}


		public class SpriteAtlasesDatabase:Database
		{
			public override object Get (string id)
			{
				if (id == "") {
					return null;
				}
				object val = null;
				dictionary.TryGetValue (id, out val);

				SpriteAtlas value = (SpriteAtlas)val;
				if (value == null) {
					Debug.Log ("No atlas for " + id + " found, searching by texture ID");
					value = FindAtlasWithTexture (id);
				}
				if (value == null) {
					Debug.Log ("Failed to find atlas");
				}
				return value;
			}
			public SpriteAtlas FindAtlasWithTexture (string id)
			{
				foreach (object obj in this.dictionary.Values) {
					SpriteAtlas spriteAtlas = (SpriteAtlas)obj;
					Vector2 texPos = spriteAtlas.GetTextureVector (id);
					if (texPos != Vector2.zero) {
						Debug.Log ("Found texture in atlas: " + spriteAtlas.name + " at position" + texPos.ToString ());
						return spriteAtlas;
					}
				}
				Debug.Log ("Could not find " + id + " Reverting to Default");
				return (SpriteAtlas)Get ("Default");
			}
		}


		public class EntitiesDatabase:Database
		{
			
			private Dictionary<string,Entity>  updateQueDictionary = new Dictionary<string,Entity> ();

			public override void Set (string name, object obj)
			{
				if (name != null) {
					if (dictionary.ContainsKey (name)) {
						dictionary [name] = obj;
						AddToQue ((Entity)obj);
					}
				}
			}

			public override void Remove (object entry)
			{
				base.Remove (entry);
				if (entry.ToString () != null) {
					if (updateQueDictionary.ContainsKey (entry.ToString ())) {
						updateQueDictionary.Remove (entry.ToString ());
					}
				}
			}

			public Entity GetByGameObject (string name)
			{
				if (name == null) {
					return null;
				}
				Entity value = (Entity)Get (name);
				return value;
			}

			public void AddToQue (Entity entity)
			{
				if (entity.ToString () != null) {
					if (!updateQueDictionary.ContainsKey (entity.ToString ())) {
						updateQueDictionary.Add (entity.ToString (), entity);
					}
				}
			}

			public List<Entity> GetUpdatePending ()
			{
				List<Entity> entityList = updateQueDictionary.Values.ToList ();
				updateQueDictionary.Clear ();
				return entityList;
			}
		}

		public class GameObjectsDatabase:Database
		{
			public void RemoveInstances ()
			{
				foreach (object obj in dictionary.Values) {
					((GameObject)obj).transform.parent = null;
					GameObject.Destroy ((GameObject)obj);
				}
			}
		}

		public class ActorsDatabase:Database
		{

		}
		public class FleetsDatabase:Database
		{
			
		}

		public class ResourcesDatabase:Database
		{
			public List<Resource> GetAllWithFunction (string function)
			{
				if (function == null) {
					return null;
				}
				List<Resource> value = null;
				List<object> allResources = new List<object> ();
				allResources.AddRange (dictionary.Values);
				List<object> targetResources = allResources.FindAll (
					delegate(object iResource) {
					return ((Resource)iResource).function == function;
				});
				value = targetResources.ConvertAll ((new Converter<object,Resource> (delegate(object input) {
					return (Resource)input;
				})));
				return value;
			}
		}
		#endregion

	}
}