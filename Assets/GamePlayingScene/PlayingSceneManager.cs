using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayingSceneManager : MonoBehaviour
{
    public GameObject _panelfade;//フェードパネル

    Image _fadealpha;//フェードパネルのイメージ取得変数

    private float _alpha;//パネルのアルファ値取得変数

    private bool _fadein;//フェードインフラグ
    private bool _fadeout;//フェードアウトフラグ

    string _name;//次に移動するスクリーン

    public AudioSource _audioSource;//サウンド
    float _fadeSeconds;//フェードイン

    public static bool _isGreen, _isBlue, _isPink;//緑色の宝石を取得したかどうか、青色の宝石を取得したかどうか、ピンク色の宝石を取得したかどうか

    // Start is called before the first frame update
    void Start()
    {
        _fadealpha = _panelfade.GetComponent<Image>();
        _alpha = 1.0f;
        _fadein = true;
        _fadeout = false;

        _name = "GameoverScene";

        _audioSource.volume = 0;
        _fadeSeconds = 0.01f;

        _isGreen = false;
        _isBlue = false;
        _isPink = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (_fadein)
        {
            FadeIn();
        }
        if (_fadeout)
        {
            FadeOut();
        }
    }

    //フェードイン
    private void FadeIn()
    {
        _alpha -= 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

        //音を大きく
        if (_audioSource.volume < 0.5)
        {
            _audioSource.volume += _fadeSeconds;
        }

        if (_alpha <= 0)
        {
            _fadein = false;
            return;
        }
    }

    //フェードアウト
    private void FadeOut()
    {
        _alpha += 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

        //音を小さく
        if (_audioSource.volume > 0.0)
        {
            _audioSource.volume -= _fadeSeconds;
        }

        if (_alpha >= 1)
        {
            _fadeout = false;
            SceneManager.LoadScene(_name);
            return;
        }
    }

    //フェード用パネルの表示非表示の変更
    private void ChangePanelEnabled()
    {
        if (_alpha <= 0)
        {
            _panelfade.SetActive(false);
        }
        else
        {
            _panelfade.SetActive(true);
        }
    }

    //シーンを変更する
    public void ChangeScene(string name)
    {
        if (_fadeout) return;

        //取得した宝石だけを表示するために、カンバスで表示しているかを調べる

        GameObject gameobject = GameObject.Find("Canvas/GemGreen");
        Image objectimage = gameobject.GetComponent<Image>();
        Color32 color = new Color32(255, 255, 255, 255); ;
        //_isGreen = _green.activeSelf;
        if (objectimage.color == color)
        {
            _isGreen = true;
            Debug.Log("グリーンG");
        }

        gameobject = GameObject.Find("Canvas/GemBlue");
        objectimage = gameobject.GetComponent<Image>();
        //_isBlue = _green.activeSelf;
        if(objectimage.color == color)
        {
            _isBlue = true ;
            Debug.Log("ブルーG");
        }

        gameobject = GameObject.Find("Canvas/GemPink");
        objectimage = gameobject.GetComponent<Image>();
        //_isPink = _green.activeSelf;
        if( objectimage.color == color)
        {
            _isPink = true;
            Debug.Log("ピンクG");
        }

        _name = name;

        _fadeout = true;
        _alpha = 0;
    }

}
