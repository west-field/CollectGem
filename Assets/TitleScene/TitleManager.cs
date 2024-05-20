using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject _panelfade;//フェードパネル

    Image _fadealpha;//フェードパネルのイメージ取得変数

    float _alpha;//パネルのアルファ値取得変数

    bool _fadein, _fadeout;//フェードインフラグ,フェードアウトフラグ

    public AudioSource _BGMaudioSource;//サウンド
    float _fadeSeconds;//フェードイン

    public AudioClip _Sound;//鳴らしたい音
    AudioSource _SEaudioSource;//サウンドを鳴らす場所

    // Start is called before the first frame update
    void Start()
    {
        _fadealpha = _panelfade.GetComponent<Image>();
        _alpha = 1.0f;
        _fadein = true;
        _fadeout = false;

        _BGMaudioSource.volume = 0;
        _fadeSeconds = 0.01f;

        _SEaudioSource = this.GetComponent<AudioSource>();//音再生
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(_fadein)
        {
            FadeIn();
        }
        if(_fadeout)
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

        //サウンドの音量を大きく
        if (_BGMaudioSource.volume < 0.5)
        {
            _BGMaudioSource.volume += _fadeSeconds;
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

        //サウンドの音量を小さく
        if (_BGMaudioSource.volume > 0.0)
        {
            _BGMaudioSource.volume -= _fadeSeconds;
        }

        if (_alpha >= 1)
        {
            _fadeout = false;
            SceneManager.LoadScene("StageSelect");
            //SceneManager.LoadScene("PlayingScene");
            return;
        }
    }

    //フェード用パネルの表示非表示を変更する
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

    //シーンを変更する(フェードアウトする)
    public void ChangeScene()
    {
        if (_fadeout) return;

        _SEaudioSource.PlayOneShot(_Sound);

        _fadeout = true;
        _alpha = 0;
    }
}
