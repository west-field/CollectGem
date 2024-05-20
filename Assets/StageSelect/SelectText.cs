using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//次へ行くためのボタン画像を表示する
public class SelectText : MonoBehaviour
{
    GameObject _gameObject;//ボタン画像を表示する

    //プレイヤーが選択できる位置にいるかどうかを取得するため
    public SelectScript _stage1Script, _stage2Script, _stage3Script, _bossScript,_backTitleScript;

    // Start is called before the first frame update
    void Start()
    {
        _gameObject = GameObject.Find("Canvas/next");
        //最初は非表示にしておく
        if (_gameObject.activeSelf)
        {
            _gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //いずれかのステージ選択ができるかどうか
        if (_stage1Script.IsColl() ||
           _stage2Script.IsColl() ||
           _stage3Script.IsColl() ||
           _bossScript.IsColl() ||
           _backTitleScript.IsColl())
        {
            ChangeActive(true);
        }
        else
        {
            ChangeActive(false);
        }
    }

    void FixedUpdate()
    {
       
    }

    //ボタン画像の表示非表示を変更する
    void ChangeActive(bool coll)
    {
        if(!coll && _gameObject.activeSelf)
        {
            _gameObject.SetActive(false);
        }
        else if(coll && !_gameObject.activeSelf)
        {
            _gameObject.SetActive(true);
        }
    }

}
