using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public float SoapHP = 100;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SoapSwipe()
    {
        SoapHP -= 0.5f;
        if(SoapHP <= 1)
        {
            SoapHP = 1;
        }
    }
}
