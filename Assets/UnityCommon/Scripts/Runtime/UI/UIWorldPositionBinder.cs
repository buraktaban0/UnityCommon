using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldPositionBinder : MonoBehaviour
{

	public Transform targetWorldTransform;

	public float sharpness = 15f;

	void Update()
    {

		transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(targetWorldTransform.transform.position), Time.deltaTime * sharpness);
		
    }

}
