using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	void OnCollisionEnter(Collision collision)
		{
		//エネミーの名前以外のオブジェクトを消す
		if (collision.gameObject.name == "sorp_model")
			{
			Destroy (gameObject);
			}
		}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Soap")
        {
            Destroy(gameObject);
        }
    }

}
