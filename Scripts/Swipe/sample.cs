using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sample : MonoBehaviour
{
    const float MaxHP = 100;
    const float length = 40;

    float SoapHP = 100;
    private Vector3 touchPos;
    
    [SerializeField]
    private GameObject _Cube;

    CubeInpact Cubeinp;

    void Start()
    {
        //Cubeinp = GameObject.Find("sorp_model").GetComponent<CubeInpact>();
		Cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();
    }

    void Update ()
    {
        if (Cubeinp.touchcount != 20)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.touchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                Vector3 sabun = Input.mousePosition - this.touchPos;
                float num = 0;

                if (sabun.magnitude >= length)
                {
                    num = 0.4f;
                }
                else if (sabun.magnitude >= length / 2)
                {
                    num = 0.2f;
                }
                else if (sabun.magnitude >= length / 8)
                {
                    num = 0.01f;
                }

                SoapHP = Mathf.Max(1, SoapHP - num);
                //Debug.Log(SoapHP);


                float par = (float)SoapHP / MaxHP;

                if(Cubeinp.touchcount != 50)
                {
                    _Cube.transform.localScale = new Vector3(par, par, par);
                }
            }
            this.touchPos = Input.mousePosition;

            PlayerPrefs.SetFloat("HP", SoapHP);
        }
        else if (Cubeinp.touchcount == 20)
        {
            SoapHP = PlayerPrefs.GetFloat("HP_return");

            float par = (float)SoapHP / MaxHP;
            _Cube.transform.localScale = new Vector3(par, par, par);
        }
    }
}
