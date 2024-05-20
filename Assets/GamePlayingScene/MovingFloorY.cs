using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�㉺�Ɉړ����鏰
public class MovingFloorY : MonoBehaviour
{
    Vector3 _move;//�ړ�
    bool _isSet;//�ړ�������ύX�ł��邩�ǂ���

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
        //�v���C���[�Ɠ������Ă��鎞
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit"); // ���O��\������
            //�v���C���[�̈ʒu�����̈ړ����A�ύX����
            collision.gameObject.transform.position += _move;
        }
    }

}
