using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CameraFit
{
	public static Vector3 GetFitPosition(this Camera cam, Quaternion camRotation, GameObject gameObject,
	                                     float percentPadding = 1.1f, float minDistance = 0f) =>
		GetFitPosition(cam, camRotation, new[] {gameObject}, percentPadding, minDistance);

	public static Vector3 GetFitPosition(this Camera cam, Quaternion camRotation, IEnumerable<GameObject> gameObjects,
	                                     float percentPadding = 1.1f, float minDistance = 0f)
	{
		var bounds = gameObjects.SelectMany(s => s.GetComponentsInChildren<Renderer>()).Select(r => r.bounds);
		var min = bounds.Select(b => b.min).Aggregate(Vector3.Min);
		var max = bounds.Select(b => b.max).Aggregate(Vector3.Max);
		var center = (min + max) / 2f;
		var size = max - min;

		var right = camRotation * Vector3.right;
		var forward = camRotation * Vector3.forward;
		var up = camRotation * Vector3.up;

		var frustumWidth = Mathf.Abs(Vector3.Dot(right, size));
		var frustumHeight = frustumWidth / cam.aspect;
		var distanceX = percentPadding * frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

		frustumHeight = Mathf.Abs(Vector3.Dot(up, size));
		var distanceY = percentPadding * frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

		var distance = Mathf.Max(distanceX, distanceY);

		distance = Mathf.Max(minDistance, distance);


		return center - distance * forward;
	}
}
