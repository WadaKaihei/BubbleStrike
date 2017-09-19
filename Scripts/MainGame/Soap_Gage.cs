using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Soap_Gage: MonoBehaviour
{
	Slider Gage;					// 石鹸サイズのゲージ
	float HP_gage;					// ゲージの値

	void Start ()
	{
		// スライダー取得
		Gage = GameObject.Find("Gage").GetComponent<Slider>();
		HP_gage = PlayerPrefs.GetFloat("HP");
	}

	void Update ()
	{
		Gage.value = HP_gage;
	}
}