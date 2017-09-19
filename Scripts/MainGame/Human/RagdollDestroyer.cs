using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Linq;
using UniRx;
using UniRx.Triggers;

public class RagdollDestroyer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(destroy());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
