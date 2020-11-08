using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.UI
{

	public class CanvasWorldCameraSetter : MonoBehaviour
	{

		[HideInInspector] [SerializeField] private Canvas canvas;

		[Tooltip("If empty or not found, uses Camera.main")]
		public string overrideCameraName = "";

		private void OnValidate()
		{
			canvas = GetComponent<Canvas>();
		}


		private void Start()
		{
			if (overrideCameraName == "")
			{
				canvas.worldCamera = Camera.main;
			}
			else
			{
				GameObject camObj = GameObject.Find(overrideCameraName);
				if (camObj == null)
				{
					canvas.worldCamera = Camera.main;
				}
				else
				{
					canvas.worldCamera = camObj.GetComponent<Camera>();
				}

			}
		}


	}

}
