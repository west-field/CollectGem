using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�I����ʂł̃A�C�e��
public class SelectSceneItem : MonoBehaviour
{
    Vector3 _rot;//��]

    public AudioClip _sound;//�炵������
    AudioSource _audioSource;//����炷

    bool _isHit;//�v���C���[�Ɠ����������ǂ���

    MeshRenderer _mr;//�A�C�e���̃t�F�[�h�A�E�g�A�C���p

    // Start is called before the first frame update
    void Start()
    {
        _rot = new Vector3(0.0f, 0.0f, 0.5f);
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
        //��]
        this.gameObject.transform.Rotate(_rot);

        //�v���C���[�Ɠ�������
        if (_isHit)
        {
            //������
            if (_mr.material.color.a > 0.0f)
            {
                _mr.material.color = _mr.material.color - new Color32(0, 0, 0, 5);
            }
            //������I������瓖�����Ă��Ȃ��悤�ɂ���
            if (!_audioSource.isPlaying)
            {
                _isHit = false;
            }
        }
        //�v���C���[�Ɠ������Ă��Ȃ��@&&�@�����x��1�����������Ƃ�
        else if (!_isHit && _mr.material.color.a < 1.0f)
        {
            //�\��������(�����x��1��)
            _mr.material.color = _mr.material.color + new Color32(0, 0, 0, 10);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //�v���C���[�Ɠ������Ă���Ƃ�
        if (!_isHit && collision.gameObject.tag == "Player")
        {
            //����炷
            _audioSource.PlayOneShot(_sound);
            //�v���C���[�Ɠ������Ă���
            _isHit = true;
        }
    }
}
