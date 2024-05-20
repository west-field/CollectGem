using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�@���Ԑ���
public class TimerScript : MonoBehaviour
{
    int _time, _count;//�\�����鎞�ԁA�J�E���g

    int _oldSeconds;//�O��Update�̎��̕b��

    Text _timerText;//�^�C�}�[�\���p�e�L�X�g

    public PlayingSceneManager _manager;//�V�[����؂�ւ���

    bool _isGameover;//���Ԃ�0�ɂȂ�����true

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
        //_count��60�ɂȂ�����A�\������b����1������
        _count++;
        if (_count >= 60)
        {
            _count = 0;
            _time--;
        }

        //�l���ς�����������e�L�X�gUI���X�V
        if(_time != _oldSeconds)
        {
            if (_isGameover) return;

            _timerText.text = _time.ToString();

            //���Ԃ�60�b��؂�����ԐF�ɕύX����
            if (_time == 60)
            {
                _timerText.color = new Color32(255, 0, 0, 255);
            }
            //���Ԃ�0�ɂȂ�����Q�[���I�[�o�[��
            else if (_time <= 0)
            {
                _manager.ChangeScene("GameoverScene");
                _isGameover = true;
                return;
            }
        }

        //���̎��Ԃ������Ă���
        _oldSeconds = _time;
    }
}
