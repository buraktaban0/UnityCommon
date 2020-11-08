using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.Runtime.Utility
{

	public class TargetFrameRateSetter : MonoBehaviour
	{

		[SerializeField] private int value = 60;

		private void Awake()
		{
			Application.targetFrameRate = value;
		}


	}

}
