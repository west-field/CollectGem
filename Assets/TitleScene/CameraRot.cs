using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�J��������]������
public class CameraRot : MonoBehaviour
{
    float _x;//��]�X�s�[�h
    Vector3 _target;//�����_

    // Start is called before the first frame update
    void Start()
    {
        _target = new Vector3(0.0f, 1.0f, 0.0f);
        _x = -0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        this.transform.RotateAround(_target, Vector3.up, _x);
    }
}
