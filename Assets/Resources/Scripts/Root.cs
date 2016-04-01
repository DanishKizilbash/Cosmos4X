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
			GameManager.Update ();
		}

		void LateUpdate ()
		{
			GameManager.LateUpdate ();
		}
	}
}
