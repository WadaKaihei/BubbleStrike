using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeTest : ScrollRect
{
    public Text SoapLevel;

    private Vector2 swipeDistance = new Vector2(30, 30);
    private Damage _damage;

    protected override void Start()
    {
        PlayerPrefs.DeleteKey("HP");
    }
    protected override void Awake()
    {
        _damage = GetComponent<Damage>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag (eventData);
		//Debug.Log ("on Drag");
	} 

	public override void OnDrag(PointerEventData eventData)
	{
        Vector2 startPos = eventData.pressPosition;
        Vector2 nowPos = eventData.position;

        //--加筆部分開始 石鹸のみにスワイプ判定
        Vector2 objectPointInScreen
               = Camera.main.WorldToScreenPoint(this.transform.position);

        Vector2 mousePointInScreen = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y
        );
        //--加筆部分終わり

        // Vector2 idlePos
        base.OnDrag (eventData);
        // Debug.Log("startPos:" + eventData.pressPosition);
        // Debug.Log("Dragging " + eventData.position);
        // Debug.Log("Distance:" + Mathf.Abs(Vector2.Distance(startPos, nowPos)));

        if (Mathf.Abs(Vector2.Distance(startPos, nowPos)) > 30)
        {
            _damage.SoapSwipe();
            //Debug.Log("HP:" + _damage.SoapHP);
            if(_damage.SoapHP == 100)
            {
                PlayerPrefs.SetFloat("HP", 100);
                
            }
            else
            {
                PlayerPrefs.SetFloat("HP", _damage.SoapHP);
            }
        }
    }

	public override void OnEndDrag (PointerEventData eventData)
	{
		base.OnEndDrag (eventData);
		//Debug.Log ("off Drag");
	}
}
