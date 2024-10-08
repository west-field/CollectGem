using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//奥と手前に移動
public class Normal : MonoBehaviour
{
    bool _isMovingZ;//奥に移動できるか
    float _moveZ;//移動量

    // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
    Quaternion _rot;
    int _rotIndex;//回転できる回数
    bool _isRot;//回転できるか

    Animator _animator;//アニメーター

    bool _isHitBullet;//弾と当たったかどうか

    public AudioClip _hitSound;//再生したい音
    AudioSource _audioSource;//サウンド再生

    // Start is called before the first frame update
    void Start()
    {
        _moveZ = 2.5f;

        _isMovingZ = true;

        _rot = Quaternion.AngleAxis(10, Vector3.up);
        _rotIndex = 0;
        _isRot = false;

        _animator = this.gameObject.GetComponent<Animator>();
        _animator.SetBool("walkFlag", true);//歩いている

        _isHitBullet = false;

        _audioSource = this.GetComponent<AudioSource>();//音再生
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        //弾と当たった
        if (_isHitBullet)
        {
            //Y方向の大きさを変更
            this.transform.localScale -= new Vector3(0.0f, 0.01f, 0.0f);
            //Y方向の大きさが0よりも小さくなったら
            if (this.transform.localScale.y <= 0.0f)
            {
                //消す
                Destroy(this.gameObject);
            }
            //弾と当たったらほかの処理をしないように
            return;
        }
        //回転できる
        if (_isRot)
        {
            //回転
            this.transform.rotation = this.transform.rotation * _rot;
            _rotIndex++;

            //反対方向を向く
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

            //奥に移動
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
            //手前に移動
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
        //弾に当たった時
        if (!_isHitBullet && collision.gameObject.tag == "bullet")
        {
            Debug.Log("bullet"); // ログを表示する
            //アニメーション
            _animator.SetBool("walkFlag", false);
            _animator.SetBool("deleteTrigger", true);
            //弾と当たった
            _isHitBullet = true;
            //音を鳴らす
            _audioSource.PlayOneShot(_hitSound);
            //当たり判定を消す
            BoxCollider boxCol = this.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = false;
        }
    }
}