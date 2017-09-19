using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDestroy : MonoBehaviour {

    CubeInpact cubeinp;

	// Use this for initialization
	void Start ()
    {
        cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(cubeinp.touchcount == 20)
        {
            Destroy(gameObject);
        }
	}
}
