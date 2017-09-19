using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Linq;
using UniRx;
using UniRx.Triggers;

#pragma strict;

public class CubeInpact : MonoBehaviour
{
    #region インスペクターフィールド-----------------------------------------------
    [SerializeField]
    private float _power = 5.0f;
    #endregion
    #region プロパティフィールド--------------------------------------------------
    //--- エフェクト関連加筆
    public enum SwiteType
    {
        RightSwipe,
        LeftSwipe
    }
    //--- エフェクト関連加筆終了
    #endregion
    #region インスペクターフィールド-----------------------------------------------
    #endregion
    #region データフィールド------------------------------------------------------
    #endregion

    //--- エフェクト関連加筆
    [SerializeField]
    private GameObject bubbleSlideObj;
    float ratetime = 25;
    //--- エフェクト関連加筆終了

    public GameObject Soap;         // 石鹸そのもの
    public GameObject Dummy;        // 移動距離を取得するための原点オブジェクト

    float powerPerPixel = 100;      // スワイプの方向
    public float maxPower;          // スワイプの力
    float sensitivity = 0.03f;      // 投げる時の左右の方向

    private Vector3 touchPos;
    private bool isSliding;         // 石鹸を飛ばす

    float FirstSoap;                // 飛ばす時の初期石鹸サイズ
    public float SoapFake = 100;    // 受け取った石鹸サイズ
    public float Soap_Move;
    public float target;            // 直接石鹸をぶつけた人のカウント
    public float Jump;              // 転ばせた人のカウント
    public Text SoapHP;             // 石鹸サイズのテキスト
    public Text SoapCM;             // 石鹸移動距離のテキスト
    public Text targetStun;         // 転ばせた人の数のテキスト
    public Text Ugokenai;
    public Text Kotu;

    public GameObject BubblePrefab; // 泡のエフェクト
    public GameObject HitPrefab;    // ヒットエフェクト
    public GameObject SoapClash;    // 石鹸が壊れるエフェクト
    public Vector3 effectRotation;

    Rigidbody rb;
    CharacterController controller;

    public int SoapLive = 0;		// カメラにゲームオーバーを伝えるための関数

    public int LeftSwipe = 0;       // 左に角度変更
    public int RightSwipe = 0;      // 右に角度変更
    public int forword = 0;

    Stun_jump stan;
    public Slider Gage;             // 石鹸サイズのゲージ
    float HP_gage;                  // ゲージの値

    private AudioSource sounds;
    public AudioClip BubbleSE;

    public int ReStart = 0;
    public int soapDead = 0;

    public Text fpsText;

	public int BubbleCors = 0;
	private IEnumerator bubbleCor;
	private IEnumerator bubbleCor2;
	private IEnumerator lagCor;

    // 石鹸のタッチ判定
    // 0->ノータッチ, 10->飛ばす前の左右移動, 20->石鹸飛ばし中
    public int touchcount = 0;

    #region 初期化—------------------------------------—————————————————————————-
    void Start()
    {
		bubbleCor = bubbles ();
		bubbleCor2 = bubbles2 ();
		lagCor = lag ();

        rb = GetComponent<Rigidbody>();

        isSliding = false;
        touchcount = 0;

        FirstSoap = SoapFake;

        StartCoroutine(loop());

        // スライダー取得
        Gage = GameObject.Find("Gage").GetComponent<Slider>();

        PlayerPrefs.DeleteKey("mertor");
        PlayerPrefs.DeleteKey("target");
    }
    #endregion

    #region 外部呼び出しメソッド--------------------------------------------------
    void OnMouseDrag()
    {
        // 飛ばす前の左右移動プログラム
        if (touchcount <= 10)
        {
            touchcount = 10;

            Vector3 objectPointInScreen
                = Camera.main.WorldToScreenPoint(this.transform.position);

            Vector3 mousePointInScreen = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                objectPointInScreen.z
            );

            Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
            // Y・Z軸へは動かない
            mousePointInWorld.y = this.transform.position.y;
            mousePointInWorld.z = this.transform.position.z;

            this.transform.position = mousePointInWorld;
            // 左右の範囲
            this.transform.position = (new Vector3(
                Mathf.Clamp(Soap.transform.position.x, -3.5f, 3.5f),
                Mathf.Clamp(Soap.transform.position.y, 0.06f, 0.06f),
                Mathf.Clamp(Soap.transform.position.z, -0.01f, 0.01f)
                //Soap.transform.position.z
                )
            );
            //--- エフェクト関連加筆
            // エフェクト
            var bubble = Instantiate<GameObject>(bubbleSlideObj, gameObject.transform);
            bubble.transform.localScale = Vector3.one;
            // パーティクルシステムを有効にする
            bubble.GetComponent<ParticleSystem>().Play();
            ratetime += 10;
            ParticleSystem.EmissionModule em = bubble.GetComponent<ParticleSystem>().emission;
            em.rateOverTime = new ParticleSystem.MinMaxCurve(ratetime);
            //--- エフェクト関連加筆終了
        }
        else if (touchcount == 20)
        {
			BubbleCors = 10;

            Vector3 objectPointInScreen
                = Camera.main.WorldToScreenPoint(this.transform.position);

            Vector3 mousePointInScreen = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                objectPointInScreen.z
            );

            if (Input.GetMouseButtonDown(0))
            {
                this.touchPos = Input.mousePosition;
            }
            else
            {
                Vector3 sabun = Input.mousePosition - this.touchPos;

                if (sabun.x > 10) // 右にスワイプ
                {
                    if (RightSwipe == 0)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y + 0.5f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * -1) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * -1));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * -1) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * -1));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        RightSwipe = 1;
                        LeftSwipe = 0;
                    }
                    else if (RightSwipe == 1 && LeftSwipe == 0 && RightSwipe != 15)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y + 1f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * -2) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * -2));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * -2) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * -2));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        RightSwipe = 3;
                    }
                    else if (RightSwipe == 3 && LeftSwipe == 0 && RightSwipe != 15)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y + 2f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * -4) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * -4));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * -4) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * -4));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        RightSwipe = 7;
                    }
                    else if (RightSwipe == 7 && LeftSwipe == 0 && RightSwipe != 15)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y + 4f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * -8) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * -8));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * -8) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * -8));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        RightSwipe = 15;
                    }
                    else if (RightSwipe == 15 && LeftSwipe == 0)
                    {
                    }
                }
                else if (sabun.x < -10) // 左にスワイプ
                {
                    if (LeftSwipe == 0)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y - 0.5f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * 1) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * 1));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * 1) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * 1));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        LeftSwipe = 1;
                        RightSwipe = 0;
                    }
                    else if (LeftSwipe == 1 && RightSwipe == 0 && LeftSwipe != 15)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y - 1f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * 2) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * 2));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * 2) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * 2));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        LeftSwipe = 3;
                    }
                    else if (LeftSwipe == 3 && RightSwipe == 0 && LeftSwipe != 15)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y - 2f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * 4) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * 4));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * 4) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * 4));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        LeftSwipe = 7;
                    }
                    else if (LeftSwipe == 7 && RightSwipe == 0 && LeftSwipe != 15)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.localRotation.y - 4f, 0);
                        Rigidbody rb = Soap.GetComponent<Rigidbody>();
                        Vector3 vec = rb.velocity;
                        // 三角関数の計算式 cosX - sinY, sinX + cosZ
                        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * 8) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * 8));
                        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * 8) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * 8));
                        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);

                        LeftSwipe = 15;
                    }
                    else if (LeftSwipe == 15 && RightSwipe == 0)
                    {
                    }
                }

                this.touchPos = Input.mousePosition;
            }

            Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
            // Y・Z軸へは動かない

            mousePointInWorld.x = this.transform.position.x;
            mousePointInWorld.y = this.transform.position.y;
            mousePointInWorld.z = this.transform.position.z;

            this.transform.position = mousePointInWorld;
            // 左右の範囲
            this.transform.position = (new Vector3(
                Soap.transform.position.x,
                Mathf.Clamp(Soap.transform.position.y, 0.06f, 0.06f),
                Soap.transform.position.z)
            );
        }
    }

    //--- エフェクト関連加筆
    private void SwipeMove(int swipeNumber, SwiteType swipeType)
    {
        int[] SiwpeTable = { 1, 3, 7, 15 };
        var powCount = Mathf.Pow(2, swipeNumber);
        transform.rotation = Quaternion.Euler(0, transform.localRotation.y + 0.5f * powCount, 0);
        Rigidbody rb = Soap.GetComponent<Rigidbody>();
        Vector3 vec = rb.velocity;
        // 三角関数の計算式 cosX - sinY, sinX + cosZ
        float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * -powCount) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * -powCount));
        float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * -powCount) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * -powCount));
        rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);
        if (swipeType == SwiteType.RightSwipe)
        {
            RightSwipe = SiwpeTable[swipeNumber];
            LeftSwipe = 0;
        }
        else
        {
            LeftSwipe = SiwpeTable[swipeNumber];
            RightSwipe = 0;
        }
    }
    //--- エフェクト関連加筆終了

    void Update()
    {
        float fps = 1f / Time.deltaTime;
        //Debug.LogFormat("{0}fps", fps);

        // 以下逆走などの応急処置
        if (soapDead == 0 && touchcount == 20)
        {
            if (rb.velocity.z == 0)
            {
                //GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, (maxPower / 3)), ForceMode.Impulse);
                rb.AddForce(0, 0, maxPower / 3, ForceMode.Impulse);
            }
            else  if (rb.velocity.z < 7)
            {
                //GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, (maxPower / 3)), ForceMode.Impulse);
                rb.AddForce(0, 0, maxPower / 3, ForceMode.Impulse);
            }
        }
		if(forword == 10 && touchcount == 10)
        {
            if (rb.velocity.z <= 0.5f)
            {
                //Debug.Log("slide:");
                //rb.AddForce(0, 0, maxPower / 3, ForceMode.Impulse);
                //GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, (maxPower / 3)), ForceMode.Impulse);
                Ugokenai.text = "もう一度！";
                Kotu.text = "タッチ → 上へスライド → 離す でやってみよう！";
                isSliding = false;
            }
        }
        if(rb.velocity.z < 0)
        {
            //GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, (maxPower / 3)), ForceMode.Impulse);
            rb.AddForce(0, 0, maxPower / 3, ForceMode.Impulse);
        }
        if(touchcount == 50 && soapDead == 10)
        {
            rb.velocity = Vector3.zero;
        }

        if (SoapFake <= 0)
        {
			StopCoroutine (bubbleCor);

			if (soapDead == 0) 
			{
				Instantiate (SoapClash, transform.position, transform.rotation);
			} 
			else if (soapDead == 10) 
			{
				StopCoroutine (lagCor);
			}

            rb.velocity = Vector3.zero;

            PlayerPrefs.SetFloat("mertor", Soap_Move);
            PlayerPrefs.SetFloat("target", target);
            //StartCoroutine(SoapMertor());

            touchcount = 50;

            soapDead = 10;
            //StartCoroutine(lag());
			StartCoroutine(lagCor);

			StopCoroutine (bubbleCor);
			StopCoroutine (bubbleCor2);
        }

        if (touchcount < 20)
        {
            SoapFake = PlayerPrefs.GetFloat("HP");
            Jump = PlayerPrefs.GetFloat("Jump");

            // こすった回数による石鹸の速度変化
            for (int i = 0; i < 9; i++)
            {
                if ((90 - i * 10) <= SoapFake && SoapFake <= (99 - i * 10))
                {
                    maxPower = 10.0f + (float)i * 2.0f;
                }
            }
        }

        PlayerPrefs.SetFloat("HP_return", SoapFake);

        // Apos(石鹸)とBpos(ダミー)の距離を移動距離として取得する
        Vector3 Apos = Soap.transform.position;
        Vector3 Bpos = Dummy.transform.position;
        // 少数第三位まで表示するために計算しなおす
        Soap_Move = Mathf.Floor((Vector3.Distance(Apos, Bpos) / 1.5f) * 1000) / 1000;

        if (SoapFake > 0)
        {
            SoapCM.text = "" + Soap_Move;
            SoapHP.text = "" + SoapFake;
            targetStun.text = "" + (target + Jump);
        }

        HP_gage = SoapFake;
        Gage.value = HP_gage;

        if (touchcount == 20 && soapDead == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                SorpEffectMake(90 - 1 * 10, 100 - i * 10, i);
            }
        }

        Vector3 SoapPos = Soap.transform.position;

        if (!isSliding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // タップの場所
                touchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0) && touchcount == 10)
            {
                // タッチの停止
                Vector3 releasePos = Input.mousePosition;
                // スワイプY軸関連
                float swipeDistanceY = releasePos.y - touchPos.y;
                float power = swipeDistanceY * powerPerPixel;
                // スワイプX軸関連
                float swipeDistanceX = releasePos.x - touchPos.x;
                float throwDirection = swipeDistanceX * sensitivity;

                Mathf.Atan2(releasePos.y - touchPos.y, releasePos.x - touchPos.x);

                // 手前へのスワイプは無効
                if (swipeDistanceY > 0)
                {
                    if (power > maxPower)
                    {
                        // maxPowerを適用
                        power = maxPower;
                    }
                    // 石鹸に力を加える
                    forword = 10; // スワイプしたと他の処理に伝える
                    GetComponent<Rigidbody>().AddForce(new Vector3(throwDirection, 0, power), ForceMode.Impulse);
                }
            }

            if (touchcount == 20)
            {
                isSliding = true;
            }
        }

        if (forword == 10)
        {
            // 徐々に減速
            if (maxPower > 0)
            {
                maxPower -= 0.0005f;
            }
            else if (maxPower <= 0)
            {
                maxPower = 0;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                SoapFake = 0;
                Ugokenai.text = "もう動けない！";
            }
        }
    }

	private void SorpEffectMake(int firstSorpUnder, int firstSorpUpper, int moveTableNumber)
    {
		if (soapDead == 0) {
			float[,] sorpMoveTable = {
                {40, 120, 180, 240, 300, 360, 420, 480, 540},
                {40,  80, 120, 160, 200, 240, 280, 320, 360},
                {40,  80, 120, 200, 260, 320, 380, 440, 500},
                {40,  80, 120, 160, 240, 300, 360, 420, 480},
                {40,  80, 120, 160, 200, 280, 340, 400, 460},
                {40,  80, 120, 160, 200, 240, 320, 380, 440},
                {40,  80, 120, 160, 200, 240, 280, 360, 440},
                {40,  80, 120, 160, 200, 240, 280, 320, 400},
                {40,  80, 120, 160, 200, 240, 280, 320, 360},
            };

            if (firstSorpUnder < FirstSoap && FirstSoap <= firstSorpUpper)
            {
                for (var i = 0; i < 9; i++)
                {
                    if ((80 - i * 10) <= SoapFake && SoapFake <= (89 - i * 10) && Soap_Move <= sorpMoveTable[moveTableNumber, i])
                    {
                        //Instantiate(BubblePrefab, Soap.transform.position, Quaternion.Euler(effectRotation));
                        //StartCoroutine(bubbles());
						//StartCoroutine(bubbleCor);

						if(BubbleCors == 10)
						{
							StartCoroutine(bubbleCor);
							StopCoroutine(bubbleCor2);
						}
						else if(BubbleCors == 20)
						{
							StartCoroutine(bubbleCor2);
							StopCoroutine(bubbleCor);
						}
						//Debug.Log("BubbleCors:"+BubbleCors);
                    }
                }
            }
        }
		else if(soapDead == 10)
		{
			StopCoroutine(bubbleCor);
			StopCoroutine(bubbleCor2);
		}
    }

    private IEnumerator SoapMertor()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.SetFloat("mertor", Soap_Move);
		PlayerPrefs.SetFloat("target", target);
    }

    private IEnumerator bubbles()
    {
        while (true)
        {
			yield return new WaitForSeconds(0.5f);
			Instantiate(BubblePrefab, Soap.transform.position, Quaternion.Euler(effectRotation));
			yield return new WaitForSeconds(2f);
			BubbleCors = 20;

        }
    }
	private IEnumerator bubbles2()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			Instantiate(BubblePrefab, Soap.transform.position, Quaternion.Euler(effectRotation));
			yield return new WaitForSeconds(2f);
			BubbleCors = 10;

		}
	}

    private IEnumerator loop()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);
            onTimer();
        }
    }
    private IEnumerator lag()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            soapDead = 10;
            //gameObject.SetActive(false);
            
        }
    }

    private void onTimer()
    {
        if(touchcount == 20)
        {
			//Debug.Log ("OnTimer!!!");
            SoapFake -= 1;
        }
    }

    void OnTriggerEnter(Collider col)
    {
		if (col.tag == "Border")
		{
			touchcount = 20;
            isSliding = true;
            Ugokenai.text = "";
            Kotu.text = "";
			//this.transform.DetachChildren();
		}
        if (col.tag == "Wall")
        {
            SoapFake -= 5;
            Instantiate(HitPrefab, Soap.transform.position, Quaternion.Euler(effectRotation));
        }
        else if (col.tag == "Break")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SoapFake = 0;
        }
        if(col.tag == "Objects")
        {
            SoapFake -= 3;

            if(target + Jump > 0)
            {
                target -= 1;
            }
        }
		if (col.tag == "Ragdoll") 
		{
            //Debug.Log("go!");
            ReStart = 10;
            GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, (maxPower / 3)), ForceMode.Impulse);
        }
        if (col.tag == "Human")
        {
            SoapFake -= 3;
            target += 1f;
            Instantiate(HitPrefab, Soap.transform.position, Quaternion.Euler(effectRotation));

            if(SoapFake >= 70)
            {
            }
            else if(SoapFake <= 69 && SoapFake >= 30)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, (maxPower / 3)), ForceMode.Impulse);

                Rigidbody rb = Soap.GetComponent<Rigidbody>();
                Vector3 vec = rb.velocity;
                // 三角関数の計算式 cosX - sinY, sinX + cosZ
                float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * 15) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * 15));
                float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * 15) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * 15));
                rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);
            }
            else if (SoapFake <= 29)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(sensitivity, 0, maxPower), ForceMode.Impulse);

                Rigidbody rb = Soap.GetComponent<Rigidbody>();
                Vector3 vec = rb.velocity;
                // 三角関数の計算式 cosX - sinY, sinX + cosZ
                float next_x = vec.x * Mathf.Cos(Mathf.Deg2Rad * 15) - vec.z * (Mathf.Sin(Mathf.Deg2Rad * 15));
                float next_z = vec.x * Mathf.Sin(Mathf.Deg2Rad * 15) + vec.z * (Mathf.Cos(Mathf.Deg2Rad * 15));
                rb.velocity = new Vector3(next_x, rb.velocity.y, next_z);
            }
        }
    }

    void GoToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    #endregion
    #region ローカルメソッド------------------------------------------------------
    #endregion
}

