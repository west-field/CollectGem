using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*�Q�l�@https://qiita.com/sakano/items/918c090f484c0610619d�@��*/
public class CameraRotByKey : MonoBehaviour
{
    Vector2 _stick;//���͕���(��])
    float _angle;//���͕���(�g��)

    public GameObject _target; // �v���C���[
    public Vector3 _offset; // �^�[�Q�b�g�I�u�W�F�N�g����̃I�t�Z�b�g

    private float _distance; // �㑱�̕��̂Ƃ̋���
    private float _polarAngle; // y���Ƃ̊p�x
    private float _azimuthalAngle; // x���Ƃ̊p�x

    //minDistance.�^�[�Q�b�g�ɋ߂��ŏ��l�AmaxDistance.�^�[�Q�b�g���牓������ő�l�AminPolarAngle.�c��]�̍ŏ��l�AmaxPolarAngle.����]�̍ő�l
    //rotXSpeed.X��]�X�s�[�h�ArotYSpeed.Y��]�X�s�[�h�AscrollSensitivity.�^�[�Q�b�g�ɋ߂Â��X�s�[�h
    private float _minDistance,_maxDistance, _minPolarAngle, _maxPolarAngle, _rotXSpeed, _rotYSpeed, _scrollSensitivity;


    void Start()
    {
        _stick = new Vector2(0.0f, 0.0f);
        _angle = 0.0f;

        _distance = 3.0f;//�^�[�Q�b�g�Ƃ̋���
        _polarAngle = 180.0f;//y���̊p�x
        _azimuthalAngle = -90.0f;//x���̊p�x

        _minDistance = 1.0f;//�^�[�Q�b�g�ɋ߂��ŏ��l
        _maxDistance = 7.0f;//�^�[�Q�b�g�ɉ�������ő�l

        _minPolarAngle = 5.0f;//�c��]�̍ŏ��l
        _maxPolarAngle = 75.0f;//�c��]�̍ő�l

        _rotXSpeed = 5.0f;//��]�X�s�[�h
        _rotYSpeed = 5.0f;
        
        _scrollSensitivity = 0.5f;//�^�[�Q�b�g�ɋ߂Â��X�s�[�h
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
    //�J�����̉�]
    public void OnRot(InputAction.CallbackContext context)
    {
        _stick = context.ReadValue<Vector2>();
    }
    //�J�����̃A�b�v
    public void OnScale(InputAction.CallbackContext context)
    {
        _angle = context.ReadValue<float>();
    }
    //rotation��ύX����
    void Angle(float x ,float y)
    {
        x = _azimuthalAngle - x * _rotXSpeed;
        _azimuthalAngle = Mathf.Repeat(x, 360);

        y = _polarAngle + y * _rotYSpeed;
        _polarAngle = Mathf.Clamp(y, _minPolarAngle, _maxPolarAngle);
    }
    //�v���C���[�ɋ߂Â�
    void Distance (float scroll)
    {
        scroll = _distance - scroll * _scrollSensitivity;
        _distance = Mathf.Clamp(scroll, _minDistance, _maxDistance);
    }
    //�J�����̈ʒu��ύX����
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