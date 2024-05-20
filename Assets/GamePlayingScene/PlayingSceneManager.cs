using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayingSceneManager : MonoBehaviour
{
    public GameObject _panelfade;//�t�F�[�h�p�l��

    Image _fadealpha;//�t�F�[�h�p�l���̃C���[�W�擾�ϐ�

    private float _alpha;//�p�l���̃A���t�@�l�擾�ϐ�

    private bool _fadein;//�t�F�[�h�C���t���O
    private bool _fadeout;//�t�F�[�h�A�E�g�t���O

    string _name;//���Ɉړ�����X�N���[��

    public AudioSource _audioSource;//�T�E���h
    float _fadeSeconds;//�t�F�[�h�C��

    public static bool _isGreen, _isBlue, _isPink;//�ΐF�̕�΂��擾�������ǂ����A�F�̕�΂��擾�������ǂ����A�s���N�F�̕�΂��擾�������ǂ���

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

    //�t�F�[�h�C��
    private void FadeIn()
    {
        _alpha -= 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

        //����傫��
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

    //�t�F�[�h�A�E�g
    private void FadeOut()
    {
        _alpha += 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

        //����������
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

    //�t�F�[�h�p�p�l���̕\����\���̕ύX
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

    //�V�[����ύX����
    public void ChangeScene(string name)
    {
        if (_fadeout) return;

        //�擾������΂�����\�����邽�߂ɁA�J���o�X�ŕ\�����Ă��邩�𒲂ׂ�

        GameObject gameobject = GameObject.Find("Canvas/GemGreen");
        Image objectimage = gameobject.GetComponent<Image>();
        Color32 color = new Color32(255, 255, 255, 255); ;
        //_isGreen = _green.activeSelf;
        if (objectimage.color == color)
        {
            _isGreen = true;
            Debug.Log("�O���[��G");
        }

        gameobject = GameObject.Find("Canvas/GemBlue");
        objectimage = gameobject.GetComponent<Image>();
        //_isBlue = _green.activeSelf;
        if(objectimage.color == color)
        {
            _isBlue = true ;
            Debug.Log("�u���[G");
        }

        gameobject = GameObject.Find("Canvas/GemPink");
        objectimage = gameobject.GetComponent<Image>();
        //_isPink = _green.activeSelf;
        if( objectimage.color == color)
        {
            _isPink = true;
            Debug.Log("�s���NG");
        }

        _name = name;

        _fadeout = true;
        _alpha = 0;
    }

}
