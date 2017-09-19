using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageTransition : MonoBehaviour
{
    private Image BackColor;
    private Image RESULT;
    private Image Score;
    private Image Distance;
    private Image TotalScore;
    private Image Name;
    private Image BackTitle;

    public GameObject Result;

    SoapClash Clash;

    void Awake()
    {
        BackColor = GameObject.Find("BackColor").GetComponent<Image>();
        RESULT = GameObject.Find("Result").GetComponent<Image>();
        Score = GameObject.Find("Score").GetComponent<Image>();
        Distance = GameObject.Find("Distance").GetComponent<Image>();
        TotalScore = GameObject.Find("TotalScore").GetComponent<Image>();
        Name = GameObject.Find("name").GetComponent<Image>();
        BackTitle = GameObject.Find("BackTitle").GetComponent<Image>();

        Clash = GameObject.Find("SoapClash").GetComponent<SoapClash>();
    }


    void Start()
    {
        BackColor.gameObject.SetActive(false);
        RESULT.gameObject.SetActive(false);
        Score.gameObject.SetActive(false);
        Distance.gameObject.SetActive(false);
        TotalScore.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
        BackTitle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Clash.Result == 10)
        {
            StartCoroutine("imagedisplay");
            //Debug.Log("result_check");
        }
    }

    IEnumerator imagedisplay()
    {
        BackColor.gameObject.SetActive(true);
        RESULT.gameObject.SetActive(true);
        Score.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Distance.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        TotalScore.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1);
        Name.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1);
        BackTitle.gameObject.SetActive(true);
    }

    

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
