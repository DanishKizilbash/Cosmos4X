using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Graphic
	{
		public string name;
		public SpriteAtlas spriteAtlas;
		public Vector2 atlasVector;
		public Vector2 scale;
		public Graphic (string Name, Vector2 Scale= default(Vector2))
		{
			Debug.Log ("Making new graphic: " + Name);
			name = Name;
			if (Scale == default(Vector2)) {
				Scale = new Vector2 (1, 1);
			}
			scale = Scale;
			Init ();
			Finder.graphicDatabase.Add (this);
		}

		public void Init ()
		{
			FindAtlas ();
		}
		private void FindAtlas ()
		{
			spriteAtlas = (SpriteAtlas)Finder.spriteAtlasDatabase.Get (name);
			if (spriteAtlas != null) {
				atlasVector = spriteAtlas.GetTextureVector (name);
			}
		}

		public override string ToString ()
		{
			return name;
		}
	}
}