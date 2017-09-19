using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoapClash : MonoBehaviour
{
	private AudioSource sounds;
	public AudioClip Clash;

    CubeInpact cubeinp;

    public int Result;

    int burst;

    // Use this for initialization
    void Start ()
    {
		sounds = gameObject.GetComponent<AudioSource>();
		sounds.loop = false;

        cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(cubeinp.soapDead == 10 && burst == 0)
        {
            sounds.PlayOneShot(Clash);

            burst = 10;
        }
        if(burst == 10)
        {
            Invoke("GoToResult", 2f);
        }
    }

    void GoToResult()
    {
        //SceneManager.LoadScene("ResultScene");
        Result = 10;
    }
}
