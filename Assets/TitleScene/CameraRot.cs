using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラを回転させる
public class CameraRot : MonoBehaviour
{
    float _x;//回転スピード
    Vector3 _target;//注視点

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
