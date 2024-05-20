using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//弾
public class bullet : MonoBehaviour
{
    float _shotSpeed;//スピード

    GameObject _playerObject;//プレイヤーオブジェクト
    protected Vector3 _forward;

    int _time;//表示時間

    AudioSource _audioSource;//鳴らしたい音
    bool _isHit;//何かと当たったどうか

    MeshRenderer _mr;//透明にするため

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
        //時間がたったら消すようにする
        _time++;
        if (_time >= 2 * 60)
        {
            _isHit = true;
        }
        //移動
        Vector3 vel = _forward * _shotSpeed;
        this.transform.position += vel;

        //音が鳴り終わって、何かと当たった時(時間が来た時)
        if (!_audioSource.isPlaying && _isHit)
        {
            //消す
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //何かと当たった
        _isHit = true;
        //見えないようにする
        _mr.material.color = _mr.material.color - new Color32(0, 0, 0, 255);
    }
}
