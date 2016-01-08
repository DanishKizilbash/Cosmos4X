using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class Root : MonoBehaviour
	{
		void Start ()
		{			
			Application.targetFrameRate = 60;
			GameManager.NewGame (0, new Vector3 (1, 0, 1));
		}

		void Update ()
		{
			if (GameManager.curGameState == GameManager.GameState.GameRunning) {
				GameManager.Update ();
			}
		}

		void LateUpdate ()
		{
			if (GameManager.curGameState == GameManager.GameState.GameRunning) {
				GameManager.LateUpdate ();
			}
		}
	}
}
