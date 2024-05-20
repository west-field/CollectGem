using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//アイテム
public class ItemScript : MonoBehaviour
{
    Vector3 _rot;//回転

    public AudioClip _sound;//プレイヤーと当たった時に鳴らす音
    AudioSource _audioSource;//音を鳴らす

    bool _isHit;//プレイヤーと当たったかどうか

    MeshRenderer _meshRenderer;//フェードアウトさせるため

    // Start is called before the first frame update
    void Start()
    {
        _rot = new Vector3(0.0f, 0.0f, 0.5f);
        _audioSource = this.GetComponent<AudioSource>();
        _isHit = false;

        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //回転
        this.gameObject.transform.Rotate(_rot);

        //プレイヤーと当たった時
        if (_isHit)
        {
            //マテリアルのα値を0に（透明にする）
            if (_meshRenderer.material.color.a > 0.0f)
            {
                _meshRenderer.material.color = _meshRenderer.material.color - new Color32(0, 0, 0, 5);
            }
            //音が鳴り終わったら消す
            if (!_audioSource.isPlaying)
            { 
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //自分が存在していて、プレイヤーと当たった時
        if (!_isHit && collision.gameObject.tag == "Player")
        {
            //音を鳴らす
            _audioSource.PlayOneShot(_sound);
            //プレイヤーと当たった
            _isHit = true;

            //自分の名前を調べる
            if (this.name == "Gem_Green")
            {
                //絵を表示させる
                GameObject gameObject = GameObject.Find("Canvas/GemGreen");
                Image image_ = gameObject.GetComponent<Image>();
                image_.color = new Color32(255, 255, 255, 255);
            }
            else if(this.name == "Gem_Blue")
            {
                GameObject gameObject = GameObject.Find("Canvas/GemBlue");
                Image image_ = gameObject.GetComponent<Image>();
                image_.color = new Color32(255, 255, 255, 255);
            }
            else if (this.name == "Gem_Pink")
            {
                GameObject gameObject = GameObject.Find("Canvas/GemPink");
                Image image_ = gameObject.GetComponent<Image>();
                image_.color = new Color32(255, 255, 255, 255);
            }
        }
    }
}
