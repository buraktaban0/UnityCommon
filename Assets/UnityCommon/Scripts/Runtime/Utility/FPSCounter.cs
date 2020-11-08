using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.Utility
{
	[RequireComponent(typeof(Text))]
	public class FPSCounter : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Text text;

		private TimedAction timer;

		private void OnValidate()
		{
			text = GetComponent<Text>();
		}


		private void Awake()
		{
			timer = new TimedAction(PushFPS, 0.5f, 0.5f);
		}


		private void PushFPS()
		{
			text.text = $"FPS: {(int) (1f / Time.smoothDeltaTime)}";
		}

		private void Update()
		{
			timer.Update(Time.deltaTime);
		}
	}
}
