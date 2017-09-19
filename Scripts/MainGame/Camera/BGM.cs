using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour 
{
	AudioSource sounds;
	CubeInpact cubeinp;

	// Use this for initialization
	void Start () 
	{
		sounds = gameObject.GetComponent<AudioSource> ();
		cubeinp = GameObject.Find ("sorp_swipe").GetComponent<CubeInpact> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (cubeinp.touchcount == 50) 
		{
			sounds.volume = 0;
		}
	}
}
