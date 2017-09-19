using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destroyer : MonoBehaviour
{
    CubeInpact cubeinp;

    // Use this for initialization
    void Start()
    {
        cubeinp = GameObject.Find("sorp_swipe").GetComponent<CubeInpact>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cubeinp.touchcount == 20)
        {
            this.transform.position = new Vector3(
                cubeinp.transform.position.x,
                cubeinp.transform.position.y - 255f,
                cubeinp.transform.position.z);
        }
    }
}
