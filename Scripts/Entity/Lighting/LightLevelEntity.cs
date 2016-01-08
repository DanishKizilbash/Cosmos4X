using UnityEngine;
using System.Collections;
namespace Eniso {
	public class LightLevelEntity :Entity {
		public int lightLevel = 0;
		public override Entity Init (string defID) {
			selectable = false;
			parentDrawSort = -50;
			return base.Init (defID);
		}
		public int ModifyLightLevel (int val) {
			lightLevel += val;
			if (lightLevel > 9) {
				lightLevel = 9;
			}
			if (lightLevel < 0) {
				lightLevel = 0;
			}
			QueForDraw ();
			return lightLevel;
		}
		public override void QueForDraw () {
			if (isInitiated) {
				Vector2 lightLevelAtlasVector = TextureManager.LightingAtlas.GetTextureVector ("Lighting_LightLevel" + lightLevel.ToString ());
				MeshManager.UpdateEntity (this, lightLevelAtlasVector);
			}
		}
		public override void QueForUpdate () {
			QueForDraw ();
		}
		public override string DefaultID () {
			return "Lighting_LightDisplay_LightLevel_LightLevel";
		}
	}
}
