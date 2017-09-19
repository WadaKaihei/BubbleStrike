using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornJump : MonoBehaviour
{
    Rigidbody rb;
    int sky;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(sky > 0)
        {
            transform.Rotate(new Vector3(1, 0, 0));
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Soap")
        {
            sky = 5;
            rb.AddForce(Vector3.up * 600);
        }
        if (col.tag == "Sky")
        {
            sky = 10;
        }
        if (col.tag == "Floor" && sky == 10)
        {
            Destroy(gameObject);
        }
    }
}
