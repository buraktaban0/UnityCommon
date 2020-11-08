using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	public static class ColliderExtensions
	{
		public static bool GetMouseDown(this Collider collider)
		{
			return Input.GetMouseButtonDown(0) &&
			       collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000f);
		}
		
		public static bool GetMouseHold(this Collider collider)
		{
			return Input.GetMouseButton(0) &&
			       collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000f);
		}
		
	}
}
