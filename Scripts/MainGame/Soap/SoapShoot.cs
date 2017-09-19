using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapShoot : MonoBehaviour 
{
	public GameObject sorp;
	CubeInpact Cubeinp;

	void Start () 
	{
		Cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();

		this.transform.position = new Vector3(
			Cubeinp.transform.position.x,
			Cubeinp.transform.position.y,
			Cubeinp.transform.position.z);
	}
	
	void Update ()
	{
		//transform.parent = GameObject.Find ("sorp_swipe").transform;

		this.transform.position = new Vector3(
			Cubeinp.transform.position.x,
			Cubeinp.transform.position.y,
			Cubeinp.transform.position.z);

		if (Cubeinp.soapDead == 10) 
		{
			Destroy (gameObject);
		}
	}
}
