using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour {

	//時間↓
	private float time = 10;

	// Use this for initialization
	void Start () {

		GetComponent<Text>().text = ((int)time).ToString();

	}

	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;

		if (time <= 0)
        {
            time = 0;
            SceneManager.LoadScene("ShootScene");
        }

		GetComponent<Text>().text = ((int)time).ToString();
	}
}