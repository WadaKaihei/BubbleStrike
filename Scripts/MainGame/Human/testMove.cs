using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Linq;
using UniRx;
using UniRx.Triggers;

public class testMove : MonoBehaviour
{
    public GameObject RagdollHuman;
    public GameObject StanRagdoll;
    public GameObject StanRagdoll2;

    private const float speed = 3f;

    public int Walking;
    public float Slip = 0;
    public int Bubbly = 0;
	public int StanAnim = 0;

    private AudioSource sounds;
    public AudioClip Voice1;

    float movePower = 0.2f;
    Rigidbody rb;
    Animator anim;
    AnimatorStateInfo animState;

    public GameObject Slip_Human;

	public GameObject SubCam;
	public Stun_jump Jumping;
	public SubCameras CameraWipe;
    public CubeInpact Cubeinp;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sounds = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        System.Random r = new System.Random(1000);
        int i = r.Next(10);

        if (Slip == 0)
        {
            if (Walking == 1)
            {
                // 右向き
                this.transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
            }
            else if (Walking == 2)
            {
                // 左向き
                this.transform.position += new Vector3(Time.deltaTime * -speed, 0, 0);
            }
            else if (Walking == 3)
            {
                // 奥向き
                this.transform.position += new Vector3(0, 0, Time.deltaTime * speed);
            }
            else if (Walking == 4)
            {
                // 手前向き
                this.transform.position += new Vector3(0, 0, Time.deltaTime * -speed);
            }
        }
        else if(Slip == 10)
        {
            Destroy(gameObject);
        }
        else if (Slip == 20)
        {
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (Slip == 0)
        {
            if (col.tag == "Wall" && Bubbly == 0 && Cubeinp.soapDead == 0)
            {
                if (Walking == 1)
                {
                    Walking = 2;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                else if (Walking == 2)
                {
                    Walking = 1;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                else if (Walking == 3)
                {
                    Walking = 4;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                else if (Walking == 4)
                {
                    Walking = 3;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
            }
            if (col.tag == "Objects" && Bubbly == 0 && Cubeinp.soapDead == 0)
            {
                if (Walking == 1)
                {
                    Walking = 2;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                else if (Walking == 2)
                {
                    Walking = 1;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                else if (Walking == 3)
                {
                    Walking = 4;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                else if (Walking == 4)
                {
                    Walking = 3;
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
            }
        }
        if (col.tag == "Bubble" && Bubbly == 0 && Cubeinp.soapDead == 0)
        {
			anim.SetBool("Stan", true);
			transform.position += transform.up * 0.03f;
			StartCoroutine(Stan());
            StartCoroutine(destroy());

			if (CameraWipe.OnOff == 0) 
			{
				SubCam.SetActive (true);
				SubCam.transform.position = new Vector3(
					this.transform.position.x,
					this.transform.position.y,
					this.transform.position.z - 3.5f);
				
				CameraWipe.OnOff = 10;
			}
            sounds.PlayOneShot(Voice1);
            Jumping.Jumping += 1;
            Bubbly = 10;
            StartCoroutine(Ragdoll());
        }
		if(col.tag == "Human")
		{
			if (Slip < 10)
			{
				Slip = 10;
				rb.AddForce(Vector3.up * 10);
				rb.AddForce(Vector3.forward * 10);

				anim.speed = 0;
			}
		}
        if (col.tag == "Ragdoll")
        {
            if (Slip < 10)
            {
                Slip = 10;
                rb.AddForce(Vector3.up * 10);
                rb.AddForce(Vector3.forward * 10);

                anim.speed = 0;
            }
        }
        if (col.tag == "Soap" && Cubeinp.soapDead == 0)
        {
            if (Slip < 10)
            {
                Instantiate(RagdollHuman, this.transform.position, Quaternion.identity);
                Slip = 10;
            }
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
	private IEnumerator Stan()
	{
		yield return new WaitForSeconds(1.5f);
		StanAnim = 10;
	}
    private IEnumerator Ragdoll()
    {
        if(Walking == 1)
        {
            yield return new WaitForSeconds(0.65f);
            Instantiate(StanRagdoll, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (Walking == 2)
        {
			yield return new WaitForSeconds(0.65f);
            Instantiate(StanRagdoll2, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
