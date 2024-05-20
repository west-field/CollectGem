using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//�v���C���[
//�ړ��̓l�b�g�̋L�����Q�l��
public class PlayerScript : MonoBehaviour
{
    public PlayingSceneManager _manager;//�V�[����؂�ւ���

    GameObject _bullet, _grandChild;//�e�𐶐�,�e�����ʒu

    Animator _animator;//�A�j���[�^�[
    
    private Transform _camPos;//�J�����̃g�����X�t�H�[��
    
    //�O��̈ʒu, �O��̈ʒu����ǂꂭ�炢�i�񂾂�, �v���C���[�̉�],�J�����̌����Ă������, �v���C���[�ړ��p, �v���C���[�����]���p
    Vector3 _lastPos, _diff, _rot, _cameraforward, _velocity, _animdir;
    
    Vector2 _stick;//�ړ�����
    
    float _moveSpeed,_jump, _stageEdge;//�ړ��X�s�[�h�A�W�����v,�X�e�[�W�̒[

    bool _isWalkAnim,_isRunAnim, _isDeath, _isJump;//�����A�j���[�V����,����A�j���[�V����,�A�j���[�V���������Đ�������,�W�����v�ł��邩�ǂ���

    int _hp;//HP

    public AudioClip _hitSound,_jumpSound;//�Đ���������
    AudioSource _audioSource;//�T�E���h�Đ�

    // Start is called before the first frame update
    void Start()
    {
        _lastPos = new Vector3(-5.0f, 1.508f, 5.0f);//�O��̈ʒu
        _moveSpeed = 0.05f;//�ړ��X�s�[�h
        _jump = 0.0f;//�W�����v

        //�J����������Ƃ��̓g�����X�t�H�[�����擾����B�Ȃ��Ƃ��͌x����\��
        if (Camera.main != null)
        {
            _camPos = Camera.main.transform;//�J�����̃g�����X�t�H�[��
        }
        else
        {
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }
        _cameraforward = Vector3.zero;//�J�����̌����Ă������
        _velocity = Vector3.zero;//�v���C���[�ړ��p
        _animdir = Vector3.zero;//�v���C���[�����]���p

        _animator = this.gameObject.GetComponent<Animator>();//�A�j���[�^�[
        _isWalkAnim = false;//�����A�j���[�V�����Đ�
        _isRunAnim = false;//����A�j���[�V�����Đ�

        _stageEdge = 12 / 2 - 0.2f;//�X�e�[�W�̒[

        _rot = new Vector3(0.0f, 0.0f, 0.0f);//�v���C���[�̉�]

        _bullet = (GameObject)Resources.Load("bullet");//�e�𐶐�����

        //�e�����ʒu
        _grandChild = transform.Find("CharacterArmature/Root/Body/Hips/Abdomen/Torso/Shoulder.R/UpperArm.R/LowerArm.R/Fist.R/Gun/Cannonball").gameObject;

        _hp = 3;//Hp
        _isDeath = false;//�������Đ�
        _isJump = false;//�W�����v

        _audioSource = this.GetComponent<AudioSource>();//���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDeath) return;

        _rot = this.transform.localEulerAngles;//�I�C���[�p
        this.transform.rotation = Quaternion.Euler(0.0f, _rot.y, 0.0f);

        _animator.SetBool("walkFlag", _isWalkAnim);//�����A�j���[�V����
        _animator.SetBool("runFlag", _isRunAnim);//����A�j���[�V����

        _diff = transform.position - _lastPos;//�O�񂩂�ǂ��ɐi�񂾂����擾
        _lastPos = transform.position;//�O���position�̍X�V

        //�x�N�g���̑傫����0.01�ȏ�̎�������ς���
        if (_diff.sqrMagnitude > 0.001f)
        {
            this.transform.rotation = Quaternion.LookRotation(_diff);

            _rot = this.transform.localEulerAngles;
            this.transform.rotation = Quaternion.Euler(0.0f, _rot.y, 0.0f);
        }
    }
    private void FixedUpdate()
    {
        if (_isDeath) return;
        //�J������Transform���擾����Ă���Ύ��s
        if (_camPos != null)
        {
            //��̃x�N�g���̊e�����̏�Z(Vector3.Scale)�B�P�ʃx�N�g����(normalized)
            _cameraforward = Vector3.Scale(_camPos.forward, new Vector3(1, 0, 1)).normalized;
            //�ړ��x�N�g��
            _velocity = _stick.y * _cameraforward * _moveSpeed + _stick.x * _camPos.right * _moveSpeed;
        }

        if(this.transform.position.x + _velocity.x >= _stageEdge || this.transform.position.x + _velocity.x <= -_stageEdge)
        {
            _velocity.x = 0.0f;
        }
        if (this.transform.position.z + _velocity.z >= _stageEdge || this.transform.position.z + _velocity.z <= -_stageEdge)
        {
            _velocity.z = 0.0f;
        }

        //�W�����v
        _jump -= 0.01f;
        if (_jump < 0.0f)
        {
            _jump = 0.0f;
        }

        this.transform.position += new Vector3(_velocity.x, _jump, _velocity.z);

        Vector3 AnimDir = _velocity;
        _animdir.y = 0.0f;
        //�����]��
        if(_animdir.sqrMagnitude >0.001f)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, AnimDir, 5.0f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        //�X�e�B�b�N���g���Ă��Ȃ��Ƃ��́A�����Ă��Ȃ�
        if(_isWalkAnim && _stick.x == 0 && _stick.y == 0)
        {
            _isWalkAnim = false;
            _isRunAnim = false;
        }
        //�����Ă��Ȃ����ǈړ��X�s�[�h������X�s�[�h�̎�
        if (!_isRunAnim && _moveSpeed != 0.05f)
        {
            //�����X�s�[�h�ɕύX
            _moveSpeed = 0.05f;
        }

        //Hp��0�ɂȂ�����Q�[���I�[�o�[
        if (_hp <= 0 && !_isDeath)
        {
            _animator.SetBool("deleteTrigger", true);
            _manager.ChangeScene("GameoverScene");
            _isDeath = true;
        }
    }
    //�W�����v
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && _isJump)
        {
            _animator.SetBool("jumpTrigger", true);
            _isJump = false;
            _jump = 0.15f;
            _audioSource.PlayOneShot(_jumpSound);
        }
    }
    //�ړ�
    public void OnMove(InputAction.CallbackContext context)
    {
        // �������ςȂ��̓���́A���ڃI�u�W�F�N�g�𓮂����̂ł͂Ȃ��������݂̂�o�^����
        _stick = context.ReadValue<Vector2>();
        
        if(!_isWalkAnim )
        {
            _moveSpeed = 0.05f;
            _isWalkAnim = true;
        }
    }
    //�V���b�g
    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _animator.SetBool("shotTrigger", true);
            //�e�𐶐�����
            Instantiate(_bullet,
                _grandChild.transform.position,
                Quaternion.identity);
        }
    }
    //����
    public void OnRun(InputAction.CallbackContext context)
    {
        if (!_isRunAnim && !_animator.GetBool("punchTrigger"))
        {
            _moveSpeed *= 1.5f;
            _isRunAnim = true;
        }
    }
    //�������Ă��邩�ǂ���
    void OnCollisionEnter(Collision collision)
    {
        //�G�Ɠ������Ă��邩
        if (collision.gameObject.tag == "enemy")
        {
            //�G�Ɠ������Ă�����HP������
            _hp--;

            //HP�ɂ���ĕ\��������
            if (_hp == 2)
            {
                GameObject gameObject = GameObject.Find("Canvas/hurt03");
                Image image_ = gameObject.GetComponent<Image>();
                image_.color = new Color32(0, 0, 0, 255);
            }
            else if(_hp == 1)
            {
                GameObject gameObject = GameObject.Find("Canvas/hurt02");
                Image image_ = gameObject.GetComponent<Image>();
                image_.color = new Color32(0, 0, 0, 255);
            }
            else if(_hp == 0)
            {
                GameObject gameObject = GameObject.Find("Canvas/hurt01");
                Image image_ = gameObject.GetComponent<Image>();
                image_.color = new Color32(0, 0, 0, 255);
            }

            //�q�b�g�A�j���[�V����
            _animator.SetBool("hitTrigger", true);
            //�q�b�g��
            _audioSource.PlayOneShot(_hitSound);
            //�ړ����Ȃ��悤��
            _velocity.x = 0.0f;
            _velocity.z = 0.0f;
        }

        //�W�����v���Ă��āA�n�ʂɓ������Ă�����
        if (!_isJump && collision.gameObject.tag == "ground")
        {
            //���ɃW�����v�ł���悤��
            _isJump = true;
        }
    }
    //�������Ă��邩
    void OnTriggerEnter(Collider collision)
    {
        //�X�^�[�ɓ������Ă�����
        if (collision.gameObject.tag == "star")
        {
            //�Q�[���N���A
            _manager.ChangeScene("ClearScene");
        }
    }
}
