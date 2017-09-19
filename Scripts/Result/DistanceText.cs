using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    public Text Score;
    float score;

    int num = 0;

	private AudioSource sounds;
	public AudioClip SE;

    void Start()
    {
        score = PlayerPrefs.GetFloat("mertor");

		sounds = gameObject.GetComponent<AudioSource>();
		sounds.loop = false;
		sounds.volume = 1.5f;

		sounds.PlayOneShot(SE);
    }

    void Update()
    {
        Score.text = "" + score;
    }
}
