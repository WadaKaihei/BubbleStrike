using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderLine : MonoBehaviour
{
    public int getSoap = 0;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Soap")
        {
            getSoap = 10;
        }
    }
}