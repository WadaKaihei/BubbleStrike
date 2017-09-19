using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameras : MonoBehaviour
{
	public int OnOff = 0;

    // Use this for initialization
    void Start()
    {
		gameObject.SetActive (false);
		OnOff = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (OnOff == 10) 
		{
			StartCoroutine (CameraOnOff ());
		}
	}

	private IEnumerator CameraOnOff()
	{
		while(true)
		{
			yield return new WaitForSeconds(1.5f);
			OnOff = 0;
			gameObject.SetActive (false);
		}
	}
}
