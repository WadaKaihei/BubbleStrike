using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreText : MonoBehaviour {

    public Text Score;
    public Text Name;
    public Text Comment;
    float score;
    float stan;
    float target;

    float total;
    private string naming;
    private string comments;

	private AudioSource sounds;
	public AudioClip SE;

    void Start ()
    {
        score = PlayerPrefs.GetFloat("mertor");
        target = PlayerPrefs.GetFloat("target");
        stan = PlayerPrefs.GetFloat("Jump");

        total = (score + ((target + stan) * 5));

		sounds = gameObject.GetComponent<AudioSource>();
		sounds.loop = false;

		sounds.PlayOneShot(SE);
    }
	
	void Update ()
    {
		if(PlayerPrefs.GetFloat("Total") < total)
        {
            PlayerPrefs.SetFloat("Total", total);
        }

        Score.text = "" + total;
        Name.text = naming;
        Comment.text = comments;

        if(total >= 800)
        {
            naming = "ソープキング";
            comments = "最高ランクだ！ やったね！";
        }
        else if (total >= 600 && total <= 799)
        {
            float nextPoint;
            nextPoint = 800 - total;

            naming = "サイコパス";
            comments = "「ソープキング」まで" + nextPoint + "ポイント！";
        }
        else if (total >= 400 && total <= 599)
        {
            float nextPoint;
            nextPoint = 600 - total;

            naming = "性悪";
            comments = "「サイコパス」まで" + nextPoint + "ポイント！";
        }
        else if (total >= 200 && total <= 399)
        {
            float nextPoint;
            nextPoint = 400 - total;

            naming = "悪戯っ子";
            comments = "「性悪」まで" + nextPoint + "ポイント！";
        }
        else
        {
            float nextPoint;
            nextPoint = 200 - total;

            naming = "天使";
            comments = "「悪戯っ子」まで" + nextPoint + "ポイント！";
        }
    }
}
