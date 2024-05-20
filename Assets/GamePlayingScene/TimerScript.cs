using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//　時間制限
public class TimerScript : MonoBehaviour
{
    int _time, _count;//表示する時間、カウント

    int _oldSeconds;//前のUpdateの時の秒数

    Text _timerText;//タイマー表示用テキスト

    public PlayingSceneManager _manager;//シーンを切り替える

    bool _isGameover;//時間が0になったらtrue

    // Start is called before the first frame update
    void Start()
    {
        _time = 60 * 5;
        _count = 0;
        _oldSeconds = 0;
        _timerText = GetComponentInChildren<Text>();
        _isGameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //_countが60になったら、表示する秒数が1増える
        _count++;
        if (_count >= 60)
        {
            _count = 0;
            _time--;
        }

        //値が変わった時だけテキストUIを更新
        if(_time != _oldSeconds)
        {
            if (_isGameover) return;

            _timerText.text = _time.ToString();

            //時間が60秒を切ったら赤色に変更する
            if (_time == 60)
            {
                _timerText.color = new Color32(255, 0, 0, 255);
            }
            //時間が0になったらゲームオーバーに
            else if (_time <= 0)
            {
                _manager.ChangeScene("GameoverScene");
                _isGameover = true;
                return;
            }
        }

        //今の時間を持っておく
        _oldSeconds = _time;
    }
}
