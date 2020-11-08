using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityCommon.Utilities
{

	public static class InputUtility
	{


		public static bool GetMouseDown()
		{
			return Input.GetMouseButtonDown(0) && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null;
		}

		public static bool GetMouseHold()
		{
			return Input.GetMouseButton(0) && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null;
		}
		public static bool GetMouseUp()
		{
			return Input.GetMouseButtonUp(0);
		}


	}


}
