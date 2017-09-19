using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public string WalkHuman;
    public GameObject WalkHuman_2;   // 歩く人
    public GameObject StanHuman; // 転んだ人（ラグドール）

    Rigidbody rb;
    Animator anim;
    

    // Use this for initialization
    void Start ()
    {
        GameObject.Find(WalkHuman).GetComponent<testMove>();

        WalkHuman_2.SetActive(true);
        StanHuman.SetActive(false);

        rb = GetComponentInChildren<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
