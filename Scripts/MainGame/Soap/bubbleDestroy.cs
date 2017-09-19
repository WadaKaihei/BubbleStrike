using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleDestroy : MonoBehaviour
{
    // Use this for initialization
	void Start ()
    {
        StartCoroutine(timer());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private IEnumerator timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(25f);
            Destroy(gameObject);
        }
    }
}
