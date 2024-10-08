using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoveScript : MonoBehaviour
{
    public AudioClip _hitSound;//当たった時に出す音
    private AudioSource _audioSource;//サウンド再生

    Animator _animator;//アニメーター

    bool _isHitBullet;//弾と当たったかどうか

    float _moveSpeed;//移動量

    Quaternion _rot;//X軸を軸に毎秒2度、回転させるQuaternion
    bool _isRot;//回転するかどうか

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
            //Y方向の大きさを変更する
            this.transform.localScale -= new Vector3(0.0f, 0.1f, 0.0f);
            //Y方向の大きさが0よりも小さくなった時
            if(this.transform.localScale.y <= 0.0f)
            {
                //消す
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
                //回転
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
                //回転
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
        //弾に当たった時
        if(!_isHitBullet && collision.gameObject.tag == "bullet")
        {
            _isHitBullet = true;
            //アニメーションを変更
            _animator.SetBool("walkFlag", false);
            _animator.SetBool("deathTrigger", true);
            //音を鳴らす
            _audioSource.PlayOneShot(_hitSound);
            //当たり判定を消す
            BoxCollider boxCol = this.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = false;
        }
    }
}
