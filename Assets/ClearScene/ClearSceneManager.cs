using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//�N���A
public class ClearSceneManager : MonoBehaviour
{
    public GameObject _panelfade;//�t�F�[�h�p�l��

    Image _fadealpha;//�t�F�[�h�p�l���̃C���[�W�擾�ϐ�

    private float _alpha;//�p�l���̃A���t�@�l�擾�ϐ�

    private bool _fadein;//�t�F�[�h�C���t���O
    private bool _fadeout;//�t�F�[�h�A�E�g�t���O

    public AudioSource _audioSource;//�T�E���h
    float _fadeSeconds;//�t�F�[�h�C��

    public AudioClip _Sound;//�炵������
    AudioSource _SEaudioSource;//����炷

    public TextMeshProUGUI _gameclear;//�N���A����
    float  _fontSizeChangeSpeed;//�N���A�����̃t�H���g�T�C�Y��ύX���邽�߂̕ϐ�

    // Start is called before the first frame update
    void Start()
    {
        _fadealpha = _panelfade.GetComponent<Image>();
        _alpha = 1.0f;
        _fadein = true;
        _fadeout = false;

        _audioSource.volume = 0;
        _fadeSeconds = 0.01f;

        _SEaudioSource = this.GetComponent<AudioSource>();//���Đ�

        MeshRenderer mr;
        GameObject gameobject;
        //�iPlayingSceneManager����j��΂��擾�ł��Ȃ������Ƃ�
        if (!PlayingSceneManager._isGreen)
        {
            gameobject = GameObject.Find("Gem_Green");
            mr = gameobject.GetComponent<MeshRenderer>();
            mr.material.color = mr.material.color - new Color32(0, 0, 0, 125);
            Debug.Log("�O���[��");
        }
        if(!PlayingSceneManager._isBlue)
        {
            gameobject = GameObject.Find("Gem_Blue");
            mr = gameobject.GetComponent<MeshRenderer>();
            mr.material.color = mr.material.color - new Color32(0, 0, 0, 125);
            Debug.Log("�u���[");
        }
        if(!PlayingSceneManager._isPink)
        {
            gameobject = GameObject.Find("Gem_Pink");
            mr = gameobject.GetComponent<MeshRenderer>();
            mr.material.color = mr.material.color - new Color32(0, 0, 0, 125);
            Debug.Log("�s���N");
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

        //�t�H���g�T�C�Y��ύX
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

    //�t�F�[�h�C��
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

    //�t�F�[�h�A�E�g
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

    //�V�[����ύX����
    public void ChangeScene()
    {
        if (_fadeout) return;

        _SEaudioSource.PlayOneShot(_Sound);

        _fadeout = true;
        _alpha = 0;
    }
}
