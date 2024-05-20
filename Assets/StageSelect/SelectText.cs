using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���֍s�����߂̃{�^���摜��\������
public class SelectText : MonoBehaviour
{
    GameObject _gameObject;//�{�^���摜��\������

    //�v���C���[���I���ł���ʒu�ɂ��邩�ǂ������擾���邽��
    public SelectScript _stage1Script, _stage2Script, _stage3Script, _bossScript,_backTitleScript;

    // Start is called before the first frame update
    void Start()
    {
        _gameObject = GameObject.Find("Canvas/next");
        //�ŏ��͔�\���ɂ��Ă���
        if (_gameObject.activeSelf)
        {
            _gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�����ꂩ�̃X�e�[�W�I�����ł��邩�ǂ���
        if (_stage1Script.IsColl() ||
           _stage2Script.IsColl() ||
           _stage3Script.IsColl() ||
           _bossScript.IsColl() ||
           _backTitleScript.IsColl())
        {
            ChangeActive(true);
        }
        else
        {
            ChangeActive(false);
        }
    }

    void FixedUpdate()
    {
       
    }

    //�{�^���摜�̕\����\����ύX����
    void ChangeActive(bool coll)
    {
        if(!coll && _gameObject.activeSelf)
        {
            _gameObject.SetActive(false);
        }
        else if(coll && !_gameObject.activeSelf)
        {
            _gameObject.SetActive(true);
        }
    }

}
