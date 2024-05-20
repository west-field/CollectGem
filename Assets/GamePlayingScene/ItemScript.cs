using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�A�C�e��
public class ItemScript : MonoBehaviour
{
    Vector3 _rot;//��]

    public AudioClip _sound;//�v���C���[�Ɠ����������ɖ炷��
    AudioSource _audioSource;//����炷

    bool _isHit;//�v���C���[�Ɠ����������ǂ���

    MeshRenderer _meshRenderer;//�t�F�[�h�A�E�g�����邽��

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
        //��]
        this.gameObject.transform.Rotate(_rot);

        //�v���C���[�Ɠ���������
        if (_isHit)
        {
            //�}�e���A���̃��l��0�Ɂi�����ɂ���j
            if (_meshRenderer.material.color.a > 0.0f)
            {
                _meshRenderer.material.color = _meshRenderer.material.color - new Color32(0, 0, 0, 5);
            }
            //������I����������
            if (!_audioSource.isPlaying)
            { 
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //���������݂��Ă��āA�v���C���[�Ɠ���������
        if (!_isHit && collision.gameObject.tag == "Player")
        {
            //����炷
            _audioSource.PlayOneShot(_sound);
            //�v���C���[�Ɠ�������
            _isHit = true;

            //�����̖��O�𒲂ׂ�
            if (this.name == "Gem_Green")
            {
                //�G��\��������
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
