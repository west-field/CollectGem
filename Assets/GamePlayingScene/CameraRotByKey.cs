using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*参考　https://qiita.com/sakano/items/918c090f484c0610619d　他*/
public class CameraRotByKey : MonoBehaviour
{
    Vector2 _stick;//入力方向(回転)
    float _angle;//入力方向(拡大)

    public GameObject _target; // プレイヤー
    public Vector3 _offset; // ターゲットオブジェクトからのオフセット

    private float _distance; // 後続の物体との距離
    private float _polarAngle; // y軸との角度
    private float _azimuthalAngle; // x軸との角度

    //minDistance.ターゲットに近い最小値、maxDistance.ターゲットから遠ざかる最大値、minPolarAngle.縦回転の最小値、maxPolarAngle.横回転の最大値
    //rotXSpeed.X回転スピード、rotYSpeed.Y回転スピード、scrollSensitivity.ターゲットに近づくスピード
    private float _minDistance,_maxDistance, _minPolarAngle, _maxPolarAngle, _rotXSpeed, _rotYSpeed, _scrollSensitivity;


    void Start()
    {
        _stick = new Vector2(0.0f, 0.0f);
        _angle = 0.0f;

        _distance = 3.0f;//ターゲットとの距離
        _polarAngle = 180.0f;//y軸の角度
        _azimuthalAngle = -90.0f;//x軸の角度

        _minDistance = 1.0f;//ターゲットに近い最小値
        _maxDistance = 7.0f;//ターゲットに遠ざかる最大値

        _minPolarAngle = 5.0f;//縦回転の最小値
        _maxPolarAngle = 75.0f;//縦回転の最大値

        _rotXSpeed = 5.0f;//回転スピード
        _rotYSpeed = 5.0f;
        
        _scrollSensitivity = 0.5f;//ターゲットに近づくスピード
}

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        Angle(_stick.x, _stick.y);

        Distance(_angle);

        var lookAtPos = _target.transform.position + _offset;
        Position(lookAtPos);
        transform.LookAt(lookAtPos);
    }
    //カメラの回転
    public void OnRot(InputAction.CallbackContext context)
    {
        _stick = context.ReadValue<Vector2>();
    }
    //カメラのアップ
    public void OnScale(InputAction.CallbackContext context)
    {
        _angle = context.ReadValue<float>();
    }
    //rotationを変更する
    void Angle(float x ,float y)
    {
        x = _azimuthalAngle - x * _rotXSpeed;
        _azimuthalAngle = Mathf.Repeat(x, 360);

        y = _polarAngle + y * _rotYSpeed;
        _polarAngle = Mathf.Clamp(y, _minPolarAngle, _maxPolarAngle);
    }
    //プレイヤーに近づく
    void Distance (float scroll)
    {
        scroll = _distance - scroll * _scrollSensitivity;
        _distance = Mathf.Clamp(scroll, _minDistance, _maxDistance);
    }
    //カメラの位置を変更する
    void Position(Vector3 lookAtPos)
    {
        var da = _azimuthalAngle * Mathf.Deg2Rad;
        var dp = _polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + _distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + _distance * Mathf.Cos(dp),
            lookAtPos.z + _distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}