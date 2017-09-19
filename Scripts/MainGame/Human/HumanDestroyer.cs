using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDestroyer : MonoBehaviour 
{
	public GameObject sorp;
	CubeInpact Cubeinp;



	void Start () 
	{
		Cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();
	}
	
	void Update () 
	{
		this.transform.position = new Vector3(
			Cubeinp.transform.position.x,
			Cubeinp.transform.position.y,
			Cubeinp.transform.position.z - 200f);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag ("Human")) 
		{
			Destroy (col.gameObject);
		}
	}
}
