using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//選択画面でのアイテム
public class SelectSceneItem : MonoBehaviour
{
    Vector3 _rot;//回転

    public AudioClip _sound;//鳴らしたい音
    AudioSource _audioSource;//音を鳴らす

    bool _isHit;//プレイヤーと当たったかどうか

    MeshRenderer _mr;//アイテムのフェードアウト、イン用

    // Start is called before the first frame update
    void Start()
    {
        _rot = new Vector3(0.0f, 0.0f, 0.5f);
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
        //回転
        this.gameObject.transform.Rotate(_rot);

        //プレイヤーと当たった
        if (_isHit)
        {
            //透明に
            if (_mr.material.color.a > 0.0f)
            {
                _mr.material.color = _mr.material.color - new Color32(0, 0, 0, 5);
            }
            //音が鳴り終わったら当たっていないようにする
            if (!_audioSource.isPlaying)
            {
                _isHit = false;
            }
        }
        //プレイヤーと当たっていない　&&　透明度が1よりも小さいとき
        else if (!_isHit && _mr.material.color.a < 1.0f)
        {
            //表示させる(透明度を1に)
            _mr.material.color = _mr.material.color + new Color32(0, 0, 0, 10);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //プレイヤーと当たっているとき
        if (!_isHit && collision.gameObject.tag == "Player")
        {
            //音を鳴らす
            _audioSource.PlayOneShot(_sound);
            //プレイヤーと当たっている
            _isHit = true;
        }
    }
}
