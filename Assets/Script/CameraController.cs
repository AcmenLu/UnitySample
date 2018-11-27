using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Vector3 mDirection;

	// Use this for initialization
	void Start()
	{
		mDirection = new Vector3(0, -7, 10);
	}

	private void LateUpdate()
	{
		if (GameManager.sGameManager == null || GameManager.sGameManager.Player == null)
			return;

		transform.position = GameManager.sGameManager.Player.transform.position - mDirection;
	}

}
