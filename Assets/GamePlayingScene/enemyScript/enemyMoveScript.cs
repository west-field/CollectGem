using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoveScript : MonoBehaviour
{
    public AudioClip _hitSound;//�����������ɏo����
    private AudioSource _audioSource;//�T�E���h�Đ�

    Animator _animator;//�A�j���[�^�[

    bool _isHitBullet;//�e�Ɠ����������ǂ���

    float _moveSpeed;//�ړ���

    Quaternion _rot;//X�������ɖ��b2�x�A��]������Quaternion
    bool _isRot;//��]���邩�ǂ���

    public Transform _firstTransform;
    public Transform _secondTransform;

    private bool _isMoveUp;

    private void Start()
    {
        this.transform.position = _secondTransform.position;

        _moveSpeed = 0.01f;

        _audioSource = GetComponent<AudioSource>();
        
        _animator = GetComponent<Animator>();
        _animator.SetBool("walkFlag", true);

        _isHitBullet = false;

        _rot = Quaternion.AngleAxis(10, Vector3.up);

        _isRot = false;

        _isMoveUp = true;
    }

    private void FixedUpdate()
    {
        if (_isHitBullet)
        {
            //Y�����̑傫����ύX����
            this.transform.localScale -= new Vector3(0.0f, 0.1f, 0.0f);
            //Y�����̑傫����0�����������Ȃ�����
            if(this.transform.localScale.y <= 0.0f)
            {
                //����
                Destroy(this.gameObject);
            }

            return;
        }

        if(_isMoveUp && !_isRot)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, _firstTransform.position, _moveSpeed);
            if (this.transform.position == _firstTransform.position)
            {
                _isRot = true;
            }
        }
        else if(!_isMoveUp && !_isRot)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, _secondTransform.position, _moveSpeed);
            if (this.transform.position == _secondTransform.position)
            {
                _isRot = true;
            }
        }
        else if(_isRot)
        {
            if(!_isMoveUp)
            {
                //��]
                this.transform.rotation = this.transform.rotation * _rot;
                if (this.transform.localRotation.y <= 0.0f)
                {
                    _isRot = false;
                    _isMoveUp = !_isMoveUp;
                    _rot = Quaternion.AngleAxis(10, Vector3.up);
                }
            }
            else if(_isMoveUp)
            {
                //��]
                this.transform.rotation = this.transform.rotation * _rot;
                if (this.transform.localRotation.y >= 1.0f)
                {
                    _isRot = false;
                    _isMoveUp = !_isMoveUp;
                    _rot = Quaternion.AngleAxis(-10, Vector3.up);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�e�ɓ���������
        if(!_isHitBullet && collision.gameObject.tag == "bullet")
        {
            _isHitBullet = true;
            //�A�j���[�V������ύX
            _animator.SetBool("walkFlag", false);
            _animator.SetBool("deathTrigger", true);
            //����炷
            _audioSource.PlayOneShot(_hitSound);
            //�����蔻�������
            BoxCollider boxCol = this.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = false;
        }
    }
}
