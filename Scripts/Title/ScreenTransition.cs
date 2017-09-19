using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour 
{
    public Text highscore;

    public void Start()
    {
        Application.targetFrameRate = 50;

        highscore.text = "High Score : " + PlayerPrefs.GetFloat("Total");
    }

    public void OnStartButtonClicked()
	{
		SceneManager.LoadScene("ShootScene");
	}
}
