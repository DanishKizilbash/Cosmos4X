using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Graphic
	{
		public string name;
		public SpriteAtlas spriteAtlas;
		public Vector2 atlasVector;
		public float scale;
		public Graphic (string Name, float Scale = 1f)
		{
			Debug.Log ("Making new graphic: " + Name);
			name = Name;
			scale = Scale;
			Init ();
			Finder.GraphicDatabase.Add (this);
		}

		public void Init ()
		{
			FindAtlas ();
		}
		private void FindAtlas ()
		{
			spriteAtlas = (SpriteAtlas)Finder.SpriteAtlasDatabase.Get (name);
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