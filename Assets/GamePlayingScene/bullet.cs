using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�e
public class bullet : MonoBehaviour
{
    float _shotSpeed;//�X�s�[�h

    GameObject _playerObject;//�v���C���[�I�u�W�F�N�g
    protected Vector3 _forward;

    int _time;//�\������

    AudioSource _audioSource;//�炵������
    bool _isHit;//�����Ɠ��������ǂ���

    MeshRenderer _mr;//�����ɂ��邽��

    // Start is called before the first frame update
    void Start()
    {
        _shotSpeed = 0.1f;
        _playerObject = GameObject.Find("Character_Gun");
        _forward = _playerObject.transform.forward;

        _time = 0;

        _audioSource = this.GetComponent<AudioSource>();
        _isHit = false;

        _mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //���Ԃ�������������悤�ɂ���
        _time++;
        if (_time >= 2 * 60)
        {
            _isHit = true;
        }
        //�ړ�
        Vector3 vel = _forward * _shotSpeed;
        this.transform.position += vel;

        //������I����āA�����Ɠ���������(���Ԃ�������)
        if (!_audioSource.isPlaying && _isHit)
        {
            //����
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //�����Ɠ�������
        _isHit = true;
        //�����Ȃ��悤�ɂ���
        _mr.material.color = _mr.material.color - new Color32(0, 0, 0, 255);
    }
}
