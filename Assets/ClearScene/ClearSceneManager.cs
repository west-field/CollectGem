using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//クリア
public class ClearSceneManager : MonoBehaviour
{
    public GameObject _panelfade;//フェードパネル

    Image _fadealpha;//フェードパネルのイメージ取得変数

    private float _alpha;//パネルのアルファ値取得変数

    private bool _fadein;//フェードインフラグ
    private bool _fadeout;//フェードアウトフラグ

    public AudioSource _audioSource;//サウンド
    float _fadeSeconds;//フェードイン

    public AudioClip _Sound;//鳴らしたい音
    AudioSource _SEaudioSource;//音を鳴らす

    public TextMeshProUGUI _gameclear;//クリア文字
    float  _fontSizeChangeSpeed;//クリア文字のフォントサイズを変更するための変数

    // Start is called before the first frame update
    void Start()
    {
        _fadealpha = _panelfade.GetComponent<Image>();
        _alpha = 1.0f;
        _fadein = true;
        _fadeout = false;

        _audioSource.volume = 0;
        _fadeSeconds = 0.01f;

        _SEaudioSource = this.GetComponent<AudioSource>();//音再生

        MeshRenderer mr;
        GameObject gameobject;
        //（PlayingSceneManagerから）宝石を取得できなかったとき
        if (!PlayingSceneManager._isGreen)
        {
            gameobject = GameObject.Find("Gem_Green");
            mr = gameobject.GetComponent<MeshRenderer>();
            mr.material.color = mr.material.color - new Color32(0, 0, 0, 125);
            Debug.Log("グリーン");
        }
        if(!PlayingSceneManager._isBlue)
        {
            gameobject = GameObject.Find("Gem_Blue");
            mr = gameobject.GetComponent<MeshRenderer>();
            mr.material.color = mr.material.color - new Color32(0, 0, 0, 125);
            Debug.Log("ブルー");
        }
        if(!PlayingSceneManager._isPink)
        {
            gameobject = GameObject.Find("Gem_Pink");
            mr = gameobject.GetComponent<MeshRenderer>();
            mr.material.color = mr.material.color - new Color32(0, 0, 0, 125);
            Debug.Log("ピンク");
        }
        
        _fontSizeChangeSpeed = 0.1f;
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

        //フォントサイズを変更
        if (_gameclear.fontSize >= 150)
        {
            _fontSizeChangeSpeed = -1.0f;
        }
        else if (_gameclear.fontSize <= 100)
        {
            _fontSizeChangeSpeed = 1.0f;
        }
        _gameclear.fontSize += _fontSizeChangeSpeed;
    }

    //フェードイン
    private void FadeIn()
    {
        _alpha -= 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

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

        if (_audioSource.volume > 0.0)
        {
            _audioSource.volume -= _fadeSeconds;
        }

        if (_alpha >= 1)
        {
            _fadeout = false;
            SceneManager.LoadScene("StageSelect");
            return;
        }
    }

    //フェード用パネルの表示非表示
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
    public void ChangeScene()
    {
        if (_fadeout) return;

        _SEaudioSource.PlayOneShot(_Sound);

        _fadeout = true;
        _alpha = 0;
    }
}
