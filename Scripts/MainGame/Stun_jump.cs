using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stun_jump : MonoBehaviour
{
	int SE = 0;

    public float Jumping = 0;
    public Text Soaptext;

    public GameObject Player;
    public Vector3 offset;

    CubeInpact cubeinp;

	private AudioSource sounds;
	public AudioClip Shoot;

    // Use this for initialization
    void Start()
    {
        cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();

		sounds = gameObject.GetComponent<AudioSource>();
		sounds.loop = false;

        PlayerPrefs.DeleteKey("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        Soaptext.text = "" + (Jumping + cubeinp.target);
        PlayerPrefs.SetFloat("Jump", Jumping);

        if (cubeinp.touchcount == 20)
        {
			this.transform.position = new Vector3(
                cubeinp.transform.position.x,
                cubeinp.transform.position.y + 253f,
                cubeinp.transform.position.z);

            if(SE == 0)
            {
                sounds.PlayOneShot(Shoot);
                SE = 1;
            }
			
        }
    }
    void OnTriggerEnter(Collider col)
    {
		if (col.tag == "Human" && cubeinp.touchcount == 20)
        {
            //Jumping += 1f;
            StartCoroutine(Allstan());

            
        }
		if (col.tag == "Objects" && Jumping > 0)
        {
            //Jumping -= 1f;
            StartCoroutine(Allstan());
        }
    }

    private IEnumerator Allstan()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.SetFloat("Jump", Jumping);
    }
}
