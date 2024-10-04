using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SelectSceneManager : MonoBehaviour
{
    public GameObject _panelfade;//�t�F�[�h�p�l��

    GameObject _selectStage;//�X�e�[�W�ֈړ����邩�ǂ����̃p�l���̕\����\��

    Image _fadealpha;//�t�F�[�h�p�l���̃C���[�W�擾�ϐ�

    private float _alpha;//�p�l���̃A���t�@�l�擾�ϐ�

    private bool _fadein, _fadeout;//�t�F�[�h�C���t���O,�t�F�[�h�A�E�g�t���O,�͂��������Ă��邩�ǂ���

    private bool _isChangeScene, _isYes;//�V�[����ύX���邩�ǂ���

    string _name;//���Ɉړ�����X�N���[��

    public AudioSource _BGMSource;//�T�E���h
    public AudioClip _decisionSound;//���艹
    AudioSource _SESource;

    float _fadeSeconds;//�t�F�[�h�C��

    TextMeshProUGUI _yes, _no,_stageName;//�I���A�X�e�[�W�e�L�X�g�̕\��
    float _fontSizeChangeSpeed;//�t�H���g�T�C�Y�ύX�̃X�s�[�h

    public PlayerInput _playerInput;//����Map��ύX����

    private float _changeFontSize;//�t�H���g�T�C�Y��ύX����

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
        //�V�[����ύX����Ƃ�
        if(_isChangeScene)
        {
            if(_isYes)
            {
                _isChangeScene = false;
                //�t�F�[�h�A�E�g
                _fadeout = true;
                _alpha = 0;
            }
            else
            {
                Init();
            }
        }
        //�X�e�[�W�ֈړ����邩�ǂ����̃p�l�����\������Ă��鎞
        if(_selectStage.activeSelf)
        {
            if (_isYes)
            {
                //�t�H���g�̑傫����ς��邱�ƂőI�𒆂𕪂���₷��
                if(_yes.fontSize < 30)
                {
                    _fontSizeChangeSpeed = _changeFontSize;
                }
                else if(_yes.fontSize > 55)
                {
                    _fontSizeChangeSpeed = -_changeFontSize;
                }
                _yes.fontSize += _fontSizeChangeSpeed;

                //�I�����Ă��鎞�͐F��Ԃɂ���
                if(_yes.color != Color.red)
                {
                    _yes.color = Color.red;
                }

                //���̑傫���ł͂Ȃ��Ƃ�
                if(_no.fontSize != 30)
                {
                    _no.fontSize = 30;
                }
                //���F�ł͂Ȃ��Ƃ�
                if(_no.color != Color.black)
                {
                    _no.color = Color.black;
                }
            }
            else
            {
                //���̑傫���ł͂Ȃ��Ƃ�
                if (_yes.fontSize != 30)
                {
                    _yes.fontSize = 30;
                }
                //���̐F�i���j�ł͂Ȃ��Ƃ�
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
                //�F��ɕύX����
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

    //�t�F�[�h�C��
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
    //�t�F�[�h�A�E�g
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

    //�t�F�[�h�p�p�l���̕\����\��
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

    //�V�[����ύX���邩�ǂ����̃p�l����\��������
    public void ChangeScene(string name)
    {
        if (_fadeout) return;

        _name = name;

        if(name == "Stage1Scene")
        {
            _stageName.text ="�X�e�[�W1��\n�ړ����܂���";
        }
        else if(name == "TitleScene")
        {
            _stageName.text = "�^�C�g����\n�ړ����܂���";
        }
        //����炷
        _SESource.PlayOneShot(_decisionSound);
        //�p�l������\���̎��͕\��������
        if (!_selectStage.activeSelf)
        {
            _selectStage.SetActive(true);
        }
        //������@��ύX
        _playerInput.SwitchCurrentActionMap("UI");
    }
    //�u�͂��v�Ɓu�������v��ύX����
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

    //�V�[����ύX����
    public void Select()
    {
        _isChangeScene = true;
        //������@��ύX
        _playerInput.SwitchCurrentActionMap("Player");

        _SESource.PlayOneShot(_decisionSound);
    }

    //������
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
