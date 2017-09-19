using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public Text Score;
    private float score = 0;
    private float stan = 0;

	private AudioSource sounds;
	public AudioClip SE;

    void Start()
    {
        //score = Random.Range(0, 5);
        score = PlayerPrefs.GetFloat("target");
        stan = PlayerPrefs.GetFloat("Jump");

		sounds = gameObject.GetComponent<AudioSource>();
		sounds.loop = false;
		sounds.volume = 1.5f;

		sounds.PlayOneShot(SE);
    }

    void Update()
    {
        Score.text = "5 × " + (score + stan);

    }
}
