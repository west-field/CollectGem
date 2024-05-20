using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W�{�b�N�X
public class SelectScript : MonoBehaviour
{
    bool _isColl;//�v���C���[���I��͈͂ɂ��邩�ǂ���

    public SelectPlayerMove _select;//�V�[����ύX���邽��

    // Start is called before the first frame update
    void Start()
    {
        _isColl = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //�v���C���[���I��͈͂ɂ���Ƃ��A�{�b�N�X��Y��]��90�Ŋ���؂�Ȃ��Ƃ�
        if (_isColl || this.transform.localEulerAngles.y % 90 != 0)
        {
            //��]
            this.transform.Rotate(0.0f, 1.0f, 0.0f);

            _isColl = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("TriggerStay");
        Debug.Log(collider.gameObject.tag);
        //�v���C���[���͈͓��ɂ���
        if (collider.gameObject.tag == "Player")
        {
            //�͈͂ɂ���
            _isColl = true;
        }

        //�V�[���̕ύX���ł����Ƌ�����
        _select.ChangeScene(this.gameObject.name);
    }

    //�͈͓��ɂ��邩�ǂ���
    public bool IsColl()
    {
        return _isColl;
    }

}
