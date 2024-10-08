using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    private float _speed;//移動量

    [SerializeField] private Vector3 upPosition;//上の位置
    [SerializeField] private Vector3 downPosition;//下の位置

    bool _isMoveUp;//上に移動しているか
    bool _isTime;//待機時間中か
    float _time;//経過時間
    float _maxTime;//待機時間

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
