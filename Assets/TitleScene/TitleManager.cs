using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject _panelfade;//�t�F�[�h�p�l��

    Image _fadealpha;//�t�F�[�h�p�l���̃C���[�W�擾�ϐ�

    float _alpha;//�p�l���̃A���t�@�l�擾�ϐ�

    bool _fadein, _fadeout;//�t�F�[�h�C���t���O,�t�F�[�h�A�E�g�t���O

    public AudioSource _BGMaudioSource;//�T�E���h
    float _fadeSeconds;//�t�F�[�h�C��

    public AudioClip _Sound;//�炵������
    AudioSource _SEaudioSource;//�T�E���h��炷�ꏊ

    // Start is called before the first frame update
    void Start()
    {
        _fadealpha = _panelfade.GetComponent<Image>();
        _alpha = 1.0f;
        _fadein = true;
        _fadeout = false;

        _BGMaudioSource.volume = 0;
        _fadeSeconds = 0.01f;

        _SEaudioSource = this.GetComponent<AudioSource>();//���Đ�
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

    //�t�F�[�h�C��
    private void FadeIn()
    {
        _alpha -= 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

        //�T�E���h�̉��ʂ�傫��
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

    //�t�F�[�h�A�E�g
    private void FadeOut()
    {
        _alpha += 0.01f;

        var color = _fadealpha.color;
        color.a = _alpha;
        _fadealpha.color = color;

        ChangePanelEnabled();

        //�T�E���h�̉��ʂ�������
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

    //�t�F�[�h�p�p�l���̕\����\����ύX����
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

    //�V�[����ύX����(�t�F�[�h�A�E�g����)
    public void ChangeScene()
    {
        if (_fadeout) return;

        _SEaudioSource.PlayOneShot(_Sound);

        _fadeout = true;
        _alpha = 0;
    }
}
