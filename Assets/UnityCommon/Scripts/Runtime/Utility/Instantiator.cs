using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{

	public class Instantiator : MonoBehaviour
	{

		public GameObject prefab;

		public float destroyAfter = -1f;

		public bool onAwake = false;

		private void Awake()
		{
			if (onAwake)
			{
				Instantiate();
			}
		}

		public void Instantiate()
		{
			var obj = Instantiate(prefab);

			if (destroyAfter > 0f)
			{
				Destroy(obj, destroyAfter);
			}

		}

	}

}
