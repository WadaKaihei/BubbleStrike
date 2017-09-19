using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Origin : MonoBehaviour
{
    CubeInpact Cubeinp;

    void Start ()
    {
        Cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();
    }
	
	void Update ()
    {
        if(Cubeinp.touchcount == 20)
        {
            this.transform.parent = null;
        }
	}
}
