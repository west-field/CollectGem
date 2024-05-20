using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���E����O�Ɉړ�����
public class NormalRot : MonoBehaviour
{
    bool _isMovingZ,_isMovingX;//�������Ɉړ��ł��邩�ǂ����A�������Ɉړ��ł��邩�ǂ���
    float _moveX, _moveZ;//�������ւ̈ړ��A�������ւ̈ړ�

    // x�������ɂ��Ė��b2�x�A��]������Quaternion���쐬�i�ϐ���rot�Ƃ���j
    Quaternion _rot;
    int _rotIndex;//��]�����
    bool _isRot;//��]���邩�ǂ���

    Animator _animator;//�A�j���[�^�[

    bool _isHitBullet;//�e�Ɠ����������ǂ���

    public AudioClip _hitSound;//�Đ���������
    AudioSource _audioSource;//�T�E���h�Đ�

    // Start is called before the first frame update
    void Start()
    {
        //�ړ��ł���ʂ��w��
        _moveX = 2.0f;
        _moveZ = 3.0f;

        _isMovingZ = true;
        _isMovingX = false;

        _rot = Quaternion.AngleAxis(10, Vector3.up);
        _rotIndex = 0;
        _isRot = false;

        _animator = this.gameObject.GetComponent<Animator>();
        _animator.SetBool("walkFlag", true);//�����Ă���

        _isHitBullet = false;

        _audioSource = this.GetComponent<AudioSource>();//���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //�e�Ɠ���������
        if (_isHitBullet)
        {
            //Y�����̑傫����ύX����
            this.transform.localScale -= new Vector3(0.0f, 0.01f, 0.0f);
            //Y�����̑傫����0�����������Ȃ�����
            if (this.transform.localScale.y <= 0.0f)
            {
                //����
                Destroy(this.gameObject);
            }
            return;
        }

        //��]�ł���Ƃ�
        if (_isRot)
        {
            //��]
            this.transform.rotation = this.transform.rotation * _rot;
            _rotIndex++;

            //90�x��]����
            if (_rotIndex == 9)
            {
                _rotIndex = 0;
                _isRot = false;
            }
        }
        else
        {
            //�������Ɉړ��i���Ɉړ��j
            if (_isMovingZ && !_isMovingX)
            {
                if (_moveZ >= 0.0f)
                {
                    _moveZ -= 0.01f;
                    this.transform.position += new Vector3(0.0f, 0.0f, 0.01f);
                }
                else
                {
                    _isRot = true;
                    _moveZ = -3.0f;
                    _isMovingZ = false;
                }
            }
            //�������Ɉړ��i��O�Ɉړ��j
            if (!_isMovingZ && !_isMovingX)
            {
                if (_moveX >= 0.0f)
                {
                    _moveX -= 0.01f;
                    this.transform.position += new Vector3(0.01f, 0.0f, 0.0f);
                }
                else
                {
                    _isRot = true;
                    _moveX = -2.0f;
                    _isMovingX = true;
                }
            }
            //�������Ɉړ��i���Ɉړ��j
            if (!_isMovingZ && _isMovingX)
            {
                if (_moveZ <= 0.0f)
                {
                    _moveZ += 0.01f;
                    this.transform.position += new Vector3(0.0f, 0.0f, -0.01f);
                }
                else
                {
                    _isRot = true;
                    _moveZ = 3.0f;
                    _isMovingZ = true;
                }
            }
            //�������Ɉړ��i�E�Ɉړ��j
            if (_isMovingZ && _isMovingX)
            {
                if (_moveX <= 0.0f)
                {
                    _moveX += 0.01f;
                    this.transform.position += new Vector3(-0.01f, 0.0f, 0.0f);
                }
                else
                {
                    _isRot = true;
                    _moveX = 2.0f;
                    _isMovingX = false;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //�e�ɓ���������
        if (!_isHitBullet && collision.gameObject.tag == "bullet")
        {
            Debug.Log("bullet"); // ���O��\������
            //�A�j���[�V����
            _animator.SetBool("walkFlag", false);
            _animator.SetBool("deathTrigger", true);
            //�e�ɓ�������
            _isHitBullet = true;
            //����炷
            _audioSource.PlayOneShot(_hitSound);
            //�����蔻�������
            BoxCollider boxCol = this.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = false;
        }
    }
}
