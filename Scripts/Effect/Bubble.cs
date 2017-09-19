using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    ParticleSystem particle;

    public float bubble;
    public GameObject Cube;
	private IEnumerator destroy;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (bubble == 5) 
		{
			destroy = DestroyBubble ();
			StartCoroutine (destroy);

		} 
		else if (bubble == 10) 
		{
			StopCoroutine (destroy);
		}
        if (!GetComponent<Renderer>().isVisible)
        {
            bubble = 5;
            Instantiate(Cube, transform.position, transform.rotation);
        }
    }

    private IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(0.1f);
		bubble = 10;
        Destroy(gameObject);
    }
}
