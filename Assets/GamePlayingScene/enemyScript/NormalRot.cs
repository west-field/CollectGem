using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//左右奥手前に移動する
public class NormalRot : MonoBehaviour
{
    bool _isMovingZ,_isMovingX;//ｚ方向に移動できるかどうか、ｘ方向に移動できるかどうか
    float _moveX, _moveZ;//ｘ方向への移動、ｚ方向への移動

    // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
    Quaternion _rot;
    int _rotIndex;//回転する回数
    bool _isRot;//回転するかどうか

    Animator _animator;//アニメーター

    bool _isHitBullet;//弾と当たったかどうか

    public AudioClip _hitSound;//再生したい音
    AudioSource _audioSource;//サウンド再生

    // Start is called before the first frame update
    void Start()
    {
        //移動できる量を指定
        _moveX = 2.0f;
        _moveZ = 3.0f;

        _isMovingZ = true;
        _isMovingX = false;

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
        //弾と当たった時
        if (_isHitBullet)
        {
            //Y方向の大きさを変更する
            this.transform.localScale -= new Vector3(0.0f, 0.01f, 0.0f);
            //Y方向の大きさが0よりも小さくなった時
            if (this.transform.localScale.y <= 0.0f)
            {
                //消す
                Destroy(this.gameObject);
            }
            return;
        }

        //回転できるとき
        if (_isRot)
        {
            //回転
            this.transform.rotation = this.transform.rotation * _rot;
            _rotIndex++;

            //90度回転する
            if (_rotIndex == 9)
            {
                _rotIndex = 0;
                _isRot = false;
            }
        }
        else
        {
            //ｚ方向に移動（奥に移動）
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
            //ｚ方向に移動（手前に移動）
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
            //ｘ方向に移動（左に移動）
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
            //ｘ方向に移動（右に移動）
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
        //弾に当たった時
        if (!_isHitBullet && collision.gameObject.tag == "bullet")
        {
            Debug.Log("bullet"); // ログを表示する
            //アニメーション
            _animator.SetBool("walkFlag", false);
            _animator.SetBool("deathTrigger", true);
            //弾に当たった
            _isHitBullet = true;
            //音を鳴らす
            _audioSource.PlayOneShot(_hitSound);
            //当たり判定を消す
            BoxCollider boxCol = this.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = false;
        }
    }
}
