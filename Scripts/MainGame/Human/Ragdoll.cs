using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Linq;
using UniRx;
using UniRx.Triggers;

public class Ragdoll : MonoBehaviour
{
    public GameObject Human;

    private const float speed = 3f;

    public int Walking;
    public float Slip = 0;

    public float korobi = 0;
    public float buttobi = 0;

    private AudioSource sounds;
    public AudioClip Voice1;

    float movePower = 0.2f;
    Rigidbody rb;
    Animator anim;

    public GameObject Slip_Human;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sounds = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(korobi == 10)
        {
            rb.AddForce(Vector3.up * 100);
            rb.AddForce(Vector3.forward * 20000);
            transform.position += new Vector3(0, 0, Time.deltaTime * 50);

            StartCoroutine(destroy());
        }

        if(buttobi == 10)
        {
            rb.AddForce(Vector3.up * 10000);
            transform.position += new Vector3(0, Time.deltaTime * 10, 0);
        }
        else if(buttobi == 20)
        {
            rb.AddForce(Vector3.down * 1);
            transform.position += new Vector3(0, Time.deltaTime * 0, 0);
            buttobi = 30;
        }
    }

    void OnTriggerEnter(Collider col)
    {
		if (col.tag == "Bubble" && Slip == 0)
        {
            rb.velocity = Vector3.zero;

            Slip = 5;
            korobi = 100;
            buttobi = 10;
        }
        else
        {
            if(korobi == 0)
            {
                StartCoroutine(spining());

                korobi = 10;

                sounds.PlayOneShot(Voice1);
            }
            
        }

        if (col.tag == "Sky" && Slip == 5)
        {
            Slip = 10;
            buttobi = 20;

            sounds.PlayOneShot(Voice1);
        }
        if (col.tag == "Floor" && Slip == 10)
        {
            if(Slip == 10)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.Rotate(new Vector3(0, 0, 0));
                this.transform.position += new Vector3(0, Time.deltaTime * -10f, 0);

                StartCoroutine(destroy());

                Slip = 20;
            }
            
            if(korobi == 10)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.Rotate(new Vector3(0, 0, 0));
                //Debug.Log("着地");
            }
        }
        if (col.tag == "Break" && Slip == 10)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.Rotate(new Vector3(0, 0, 0));
            this.transform.position += new Vector3(0, Time.deltaTime * -10f, 0);

            StartCoroutine(destroy());

            Slip = 20;
        }
        if (col.tag == "Wall" && Slip == 10)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            this.transform.position += new Vector3(0, Time.deltaTime * -10f, 0);

            transform.Rotate(new Vector3(0, 0, 0));

            StartCoroutine(destroy());

            Slip = 20;
        }
        if (col.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    private IEnumerator spining()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector3.up * 100);
        rb.AddForce(Vector3.forward * 10000);
    }
}
