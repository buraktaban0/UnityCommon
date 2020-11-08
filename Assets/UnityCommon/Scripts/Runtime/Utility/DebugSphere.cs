using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{

	[ExecuteInEditMode]
	public class DebugSphere : MonoBehaviour
	{

#if UNITY_EDITOR

		public float radius = 0.5f;

		public Color color = Color.red;

		private void OnDrawGizmos()
		{
			Gizmos.color = color;
			Gizmos.DrawSphere(transform.position, radius);
		}

#endif

	}
}
