using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージボックス
public class SelectScript : MonoBehaviour
{
    bool _isColl;//プレイヤーが選択範囲にいるかどうか

    public SelectPlayerMove _select;//シーンを変更するため

    // Start is called before the first frame update
    void Start()
    {
        _isColl = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //プレイヤーが選択範囲にいるとき、ボックスのY回転が90で割り切れないとき
        if (_isColl || this.transform.localEulerAngles.y % 90 != 0)
        {
            //回転
            this.transform.Rotate(0.0f, 1.0f, 0.0f);

            _isColl = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("TriggerStay");
        Debug.Log(collider.gameObject.tag);
        //プレイヤーが範囲内にいる
        if (collider.gameObject.tag == "Player")
        {
            //範囲にいる
            _isColl = true;
        }

        //シーンの変更ができるよと教える
        _select.ChangeScene(this.gameObject.name);
    }

    //範囲内にいるかどうか
    public bool IsColl()
    {
        return _isColl;
    }

}
