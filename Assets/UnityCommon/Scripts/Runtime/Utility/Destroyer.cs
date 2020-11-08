using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{

	public class Destroyer : MonoBehaviour
	{

		[SerializeField] private UnityEngine.Object objectToDestroy = null;

		[SerializeField] private float delay = 1f;

		[SerializeField] private bool onAwake = false;

		private void Awake()
		{
			if (onAwake)
			{
				if (delay > 0f)
					Destroy(objectToDestroy, delay);
				else
					Destroy(objectToDestroy);

			}

		}


		public void Destroy()
		{
			GameObject.Destroy(objectToDestroy, delay);
		}



	}

}
