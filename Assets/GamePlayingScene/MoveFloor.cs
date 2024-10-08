using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    private float _speed;//�ړ���

    [SerializeField] private Vector3 upPosition;//��̈ʒu
    [SerializeField] private Vector3 downPosition;//���̈ʒu

    bool _isMoveUp;//��Ɉړ����Ă��邩
    bool _isTime;//�ҋ@���Ԓ���
    float _time;//�o�ߎ���
    float _maxTime;//�ҋ@����

    private void Start()
    {
        this.transform.position = downPosition;

        _speed = 0.05f;
        _isMoveUp = true;
        _isTime = false;
        _time = 0;
        _maxTime = 60;
    }

    private void FixedUpdate()
    {
        if(_isMoveUp && !_isTime)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, upPosition, _speed);
            if(this.transform.position == upPosition)
            {
                _isTime = true;
            }
        }
        else if(!_isMoveUp && !_isTime)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, downPosition, _speed);
            if (this.transform.position == downPosition)
            {
                _isTime = true;
            }
        }
        else if(_isTime)
        {
            if (_time++ >= _maxTime)
            {
                _isTime = false;
                _isMoveUp = !_isMoveUp;
                _time = 0;
            }
        }
    }
}
