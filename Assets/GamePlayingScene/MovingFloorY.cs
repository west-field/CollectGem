using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//上下に移動する床
public class MovingFloorY : MonoBehaviour
{
    Vector3 _move;//移動
    bool _isSet;//移動方向を変更できるかどうか

    // Start is called before the first frame update
    void Start()
    {
        _move = new Vector3(0.0f, 0.01f, 0.0f);
        _isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isSet && (this.transform.position.y >= 10.0f || this.transform.position.y <= 7.7f))
        {
            _isSet = true;
            _move.y *= -1.0f;
        }
        else if (_isSet && this.transform.position.y <= 9.0f && this.transform.position.y >= 8.0f)
        {
            _isSet = false;
        }
    }

    void FixedUpdate()
    {
        this.transform.position += _move;
    }

    void OnCollisionEnter(Collision collision)
    {
        //プレイヤーと当たっている時
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit"); // ログを表示する
            //プレイヤーの位置を床の移動分、変更する
            collision.gameObject.transform.position += _move;
        }
    }

}
