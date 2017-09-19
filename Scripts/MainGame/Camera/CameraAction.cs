using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;
using UniRx;

public class CameraAction : MonoBehaviour
{
	#region インスペクターフィールド-----------------------------------------------

	[SerializeField]
	private Transform _player;

	#endregion

	#region データフィールド------------------------------------------------------

	public float baseDistance = 1f;
	// 停止時のカメラ―プレイヤー間の距離[m]
	public float baseCameraHeight = 1f;
	// 停止時のカメラの高さ[m]
	public float chaseDamper = 1f;
	// カメラの追跡スピード（追跡時のカメラ―プレイヤー間の距離がきまる）
	private Transform _cam;

    [SerializeField]
    CubeInpact soap;

	#endregion

	#region 初期化—------------------------------------—————————————————————————-

	void Start ()
	{
		_cam = GetComponent<Camera> ().transform;

        //soap = GameObject.Find("Soap_Empty").GetComponentInChildren<CubeInpact>();
    }

	void FixedUpdate ()
	{
        if(soap.SoapLive == 0)
        {
            // カメラの位置を設定
            var desiredPos = _player.position - _player.forward * baseDistance + Vector3.up * baseCameraHeight;
            _cam.position = Vector3.Lerp(_cam.position, desiredPos, chaseDamper);

            // カメラの向きを設定
            _cam.LookAt(_player);
        }
		
	}

	#endregion
}
