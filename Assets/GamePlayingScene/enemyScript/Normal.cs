using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ǝ�O�Ɉړ�
public class Normal : MonoBehaviour
{
    bool _isMovingZ;//���Ɉړ��ł��邩
    float _moveZ;//�ړ���

    // x�������ɂ��Ė��b2�x�A��]������Quaternion���쐬�i�ϐ���rot�Ƃ���j
    Quaternion _rot;
    int _rotIndex;//��]�ł����
    bool _isRot;//��]�ł��邩

    Animator _animator;//�A�j���[�^�[

    bool _isHitBullet;//�e�Ɠ����������ǂ���

    public AudioClip _hitSound;//�Đ���������
    AudioSource _audioSource;//�T�E���h�Đ�

    // Start is called before the first frame update
    void Start()
    {
        _moveZ = 2.5f;

        _isMovingZ = true;

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
        //�e�Ɠ�������
        if (_isHitBullet)
        {
            //Y�����̑傫����ύX
            this.transform.localScale -= new Vector3(0.0f, 0.01f, 0.0f);
            //Y�����̑傫����0�����������Ȃ�����
            if (this.transform.localScale.y <= 0.0f)
            {
                //����
                Destroy(this.gameObject);
            }
            //�e�Ɠ���������ق��̏��������Ȃ��悤��
            return;
        }
        //��]�ł���
        if (_isRot)
        {
            //��]
            this.transform.rotation = this.transform.rotation * _rot;
            _rotIndex++;

            //���Ε���������
            if (_rotIndex == 18)
            {
                _rotIndex = 0;
                _isRot = false;
            }
        }
        else
        {
            ////-7.5
            ////-5
            //this.transform.position += _addPos;
            //if (_isMovingZ && this.transform.position.z + _addPos.z <= -7.5f)
            //{
            //    _addPos *= -1.0f;
            //    _isMovingZ = false;
            //}
            //if (_isMovingZ && (this.transform.position.z >= -5.0f || this.transform.position.z <= -7.5f))
            //{
            //    //_isRot = true;
            //    _addPos *= -1.0f;
            //    _isMovingZ = false;
            //}
            //if (!_isMovingZ && (this.transform.position.z < -6.0f || this.transform.position.z > -6.5f))
            //{
            //    _isMovingZ = true;
            //}

            //���Ɉړ�
            if (_isMovingZ)
            {
                if (_moveZ >= 0.0f)
                {
                    _moveZ -= 0.01f;
                    this.transform.position += new Vector3(0.0f, 0.0f, 0.01f);
                }
                else
                {
                    _isRot = true;
                    _moveZ = -2.5f;
                    _isMovingZ = false;
                }
            }
            //��O�Ɉړ�
            if (!_isMovingZ)
            {
                if (_moveZ <= 0.0f)
                {
                    _moveZ += 0.01f;
                    this.transform.position += new Vector3(0.0f, 0.0f, -0.01f);
                }
                else
                {
                    _isRot = true;
                    _moveZ = 2.5f;
                    _isMovingZ = true;
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
            _animator.SetBool("deleteTrigger", true);
            //�e�Ɠ�������
            _isHitBullet = true;
            //����炷
            _audioSource.PlayOneShot(_hitSound);
            //�����蔻�������
            BoxCollider boxCol = this.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = false;
        }
    }
}