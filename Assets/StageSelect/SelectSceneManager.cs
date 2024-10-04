using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SelectSceneManager : MonoBehaviour
{
    public GameObject _panelfade;//フェードパネル

    GameObject _selectStage;//ステージへ移動するかどうかのパネルの表示非表示

    Image _fadealpha;//フェードパネルのイメージ取得変数

    private float _alpha;//パネルのアルファ値取得変数

    private bool _fadein, _fadeout;//フェードインフラグ,フェードアウトフラグ,はいをおしているかどうか

    private bool _isChangeScene, _isYes;//シーンを変更するかどうか

    string _name;//次に移動するスクリーン

    public AudioSource _BGMSource;//サウンド
    public AudioClip _decisionSound;//決定音
    AudioSource _SESource;

    float _fadeSeconds;//フェードイン

    TextMeshProUGUI _yes, _no,_stageName;//選択、ステージテキストの表示
    float _fontSizeChangeSpeed;//フォントサイズ変更のスピード

    public PlayerInput _playerInput;//操作Mapを変更する

    private float _changeFontSize;//フォントサイズを変更する

    // Start is called before the first frame update
    void Start()
    {
        _selectStage = GameObject.Find("Canvas/select");
        GameObject obj = GameObject.Find("Canvas/select/YesText");
        _yes = obj.GetComponent<TextMeshProUGUI>();
        obj = GameObject.Find("Canvas/select/NoText");
        _no = obj.GetComponent<TextMeshProUGUI>();
        obj = GameObject.Find("Canvas/select/OnStageText");
        _stageName = obj.GetComponent<TextMeshProUGUI>();

        if (_selectStage.activeSelf)
        {
            _selectStage.SetActive(false);
        }

        _fadealpha = _panelfade.GetComponent<Image>();
        _alpha = 1.0f;
        _fadein = true;
        _fadeout = false;
        _isChangeScene = false;
        _isYes = false;

        _name = "GameoverScene";

        _BGMSource.volume = 0;

        _SESource = this.GetComponent<AudioSource>();

        _fadeSeconds = 0.1f;

        //_playerInputActions = new PlayerInputActions();
        //_playerInput.SwitchCurrentActionMap("Player");
        _changeFontSize = 0.5f;
        _fontSizeChangeSpeed = _changeFontSize;
    }

    void FixedUpdate()
    {
        //シーンを変更するとき
        if(_isChangeScene)
        {
            if(_isYes)
            {
                _isChangeScene = false;
                //フェードアウト
                _fadeout = true;
                _alpha = 0;
            }
            else
            {
                Init();
            }
        }
        //ステージへ移動するかどうかのパネルが表示されている時
        if(_selectStage.activeSelf)
        {
            if (_isYes)
            {
                //フォントの大きさを変えることで選択中を分かりやすく
                if(_yes.fontSize < 30)
                {
                    _fontSizeChangeSpeed = _changeFontSize;
                }
                else if(_yes.fontSize > 55)
                {
                    _fontSizeChangeSpeed = -_changeFontSize;
                }
                _yes.fontSize += _fontSizeChangeSpeed;

                //選択している時は色を赤にする
                if(_yes.color != Color.red)
                {
                    _yes.color = Color.red;
                }

                //元の大きさではないとき
                if(_no.fontSize != 30)
                {
                    _no.fontSize = 30;
                }
                //黒色ではないとき
                if(_no.color != Color.black)
                {
                    _no.color = Color.black;
                }
            }
            else
            {
                //元の大きさではないとき
                if (_yes.fontSize != 30)
                {
                    _yes.fontSize = 30;
                }
                //元の色（黒）ではないとき
                if(_yes.color != Color.black)
                {
                    _yes.color = Color.black;
                }

                if (_no.fontSize < 30)
                {
                    _fontSizeChangeSpeed = _changeFontSize;
                }
                else if (_no.fontSize > 55)
                {
                    _fontSizeChangeSpeed = -_changeFontSize;
                }
                _no.fontSize += _fontSizeChangeSpeed;
                //色を青に変更する
                if(_no.color != Color.blue)
                {
                    _no.color = Color.blue;
                }
            }
        }


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

        if (_BGMSource.volume < 0.5)
        {
            _BGMSource.volume += _fadeSeconds;
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

        if (_BGMSource.volume > 0.0)
        {
            _BGMSource.volume -= _fadeSeconds;
        }

        if (_alpha >= 1)
        {
            _fadeout = false;
            SceneManager.LoadScene(_name);
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

    //シーンを変更するかどうかのパネルを表示させる
    public void ChangeScene(string name)
    {
        if (_fadeout) return;

        _name = name;

        if(name == "Stage1Scene")
        {
            _stageName.text ="ステージ1へ\n移動しますか";
        }
        else if(name == "TitleScene")
        {
            _stageName.text = "タイトルへ\n移動しますか";
        }
        //音を鳴らす
        _SESource.PlayOneShot(_decisionSound);
        //パネルが非表示の時は表示させる
        if (!_selectStage.activeSelf)
        {
            _selectStage.SetActive(true);
        }
        //操作方法を変更
        _playerInput.SwitchCurrentActionMap("UI");
    }
    //「はい」と「いいえ」を変更する
    public void ChageSelect(InputAction.CallbackContext context)
    {
        if(!_isYes && context.ReadValue<Vector2>().x > 0.0f)
        {
            _isYes = true;
        }
        else if(_isYes && context.ReadValue<Vector2>().x < 0.0f)
        {
            _isYes = false;
        }
    }

    //シーンを変更する
    public void Select()
    {
        _isChangeScene = true;
        //操作方法を変更
        _playerInput.SwitchCurrentActionMap("Player");

        _SESource.PlayOneShot(_decisionSound);
    }

    //初期化
    void Init()
    {
        if (_selectStage.activeSelf)
        {
            _selectStage.SetActive(false);
        }

        _fadeout = false;
        _isChangeScene = false;
        _isYes = false;

    }

}
