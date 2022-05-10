using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LookAtCamera : MonoBehaviour
{
	public Camera CameraToLookAt;
	public string FindCameraTag;

	private void Start()
	{
		if (CameraToLookAt == null)
		{
			CameraToLookAt = GameObject.FindGameObjectWithTag(FindCameraTag).GetComponent<Camera>();
		}
	}

	private void Update()
	{
		transform.LookAt(CameraToLookAt.transform);

		// when we need more rotation control
		// vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv

		//Vector3 lookVector = CameraToLookAt.transform.position - transform.position;
		//Quaternion lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);
		//transform.rotation = lookRotation;
	}

}
