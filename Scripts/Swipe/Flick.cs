using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class Flick : MonoBehaviour
{
    #region 
    private void Start()
    {
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
        // マウスを押したときの処理
        var mouseDown = this.UpdateAsObservable()
            // マウスを押したときに反応させるために入れる
            .Where(_ => Input.GetMouseButtonDown(0))
            // 次の関数を実行する（画面上のクリックした座標をスクリーン座標に変換する）
            .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
            // スクリーン座標を使って石鹸との当たり判定をチェックする
            .Select(x =>
            {
                RaycastHit rh;
                var hit = Physics.Raycast(x, out rh);
                return Tuple.Create(hit, rh);
            })
            // 計算結果をチェックする（Item1=>hit,Item2=>rh 石鹸にあたっていればItem1がtrue, Item12は当たったオブジェクトを指すのでその名前が対象の石鹸であればOK）
            .Where(x => x.Item1 && x.Item2.collider.gameObject == this.gameObject);

        // マウスを離した処理
        var mouseUp = this.UpdateAsObservable()
            // マウスを離す
            .Where(_ => Input.GetMouseButtonUp(0));

        // マウスの押し離しをチェックする
        this.UpdateAsObservable()
            // マウスを押しているかどうかのチェック
            .Where(_ => Input.GetMouseButton(0))
            // マウスを押したときの処理の結果を取得する
            .SkipUntil(mouseDown)
            // マウスを離すを検出する
            .TakeUntil(mouseUp)
            // いったん終わったら再度最初からやり直すように設定
            .Repeat()
            // マウス座標を取得する
            .Select(_ => Input.mousePosition)

            .Select(x =>
            {
                RaycastHit rh;
                Physics.Raycast(Camera.main.ScreenPointToRay(x), out rh, 1 << LayerMask.NameToLayer("Background"));
                return rh.point;
            })
            // マウスのクリックした座標にオブジェクトを移動させる座標を計算する（Zをオフジェクトから拾っているのでZ座標には移動しない）
            .Select(x => new Vector3(x.x, x.y, this.transform.position.z))
            // マウスの位置にオブジェクトを移動
            .Subscribe(x => this.transform.position = x);
    }
    #endregion

    #region
    
    #endregion

}
