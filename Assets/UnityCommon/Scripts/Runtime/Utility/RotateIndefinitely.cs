using System;
using UnityEngine;


public class RotateIndefinitely : MonoBehaviour
{
	public Vector3 velocity = Vector3.forward * 540f;

	private void Update()
	{
		transform.Rotate(velocity * Time.deltaTime);
	}
}
