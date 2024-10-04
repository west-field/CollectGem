using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//�v���C���[
public class SelectPlayerMove : MonoBehaviour
{
    public SelectSceneManager _manager;//�V�[����؂�ւ���
    Animator _animator;//�A�j���[�^�[
    private Transform _camPos;//�J�����̃g�����X�t�H�[��
    //�O��̈ʒu, �O��̈ʒu����ǂꂭ�炢�i�񂾂�, �v���C���[�̉�],�J�����̌����Ă������, �v���C���[�ړ��p, �v���C���[�����]���p
    Vector3 _lastPos, _diff, _rot, _cameraforward, _velocity, _animdir;
    Vector2 _stick;//�ړ�����
    float _moveSpeed, _jump, _stageEdge;//�ړ��X�s�[�h�A�W�����v,�X�e�[�W�̒[
    bool _isWalkAnim, _isRunAnim, _isDeath, _isJump,_isPunch;//�����A�j���[�V����,����A�j���[�V����,�A�j���[�V���������Đ�������,�W�����v�ł��邩�ǂ���
    public AudioClip _hitSound, _jumpSound;//�Đ���������
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

        _isDeath = false;//�������Đ�
        _isJump = false;//�W�����v
        _isPunch = false;//�p���`

        _audioSource = this.GetComponent<AudioSource>();//���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDeath) return;

        _rot = this.transform.localEulerAngles;//�I�C���[�p
        this.transform.rotation = Quaternion.Euler(0.0f, _rot.y, 0.0f);

        _animator.SetBool("WalkFlag", _isWalkAnim);//�����A�j���[�V����
        _animator.SetBool("RunFlag", _isRunAnim);//����A�j���[�V����

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

        if (this.transform.position.x + _velocity.x >= _stageEdge || this.transform.position.x + _velocity.x <= -_stageEdge)
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
        if (_animdir.sqrMagnitude > 0.001f)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, AnimDir, 5.0f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        if (_isWalkAnim && _stick.x == 0 && _stick.y == 0)
        {
            _isWalkAnim = false;
            _isRunAnim = false;
        }
        if (!_isRunAnim && _moveSpeed != 0.05f)
        {
            _moveSpeed = 0.05f;
        }

        if (_isDeath)
        {
            _animator.SetBool("YesFlag", true);
            _manager.ChangeScene("GameoverScene");
            _isDeath = true;
        }
    }

    //�W�����v
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && _isJump)
        {
            _animator.SetBool("JumpTrigger", true);
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
        
        _isPunch = false;

        if (!_isWalkAnim /*&& !_animator.GetBool("punchTrigger")*/)
        {
            _moveSpeed = 0.05f;
            _isWalkAnim = true;
        }
    }
    //�p���`
    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
           _animator.SetBool("PunchTrigger", true);
            _isPunch = true;
        }
        _stick = Vector2.zero;
    }
    //����
    public void OnRun(InputAction.CallbackContext context)
    {
        if (!_isRunAnim && !_animator.GetBool("PunchTrigger"))
        {
            _moveSpeed *= 1.5f;
            _isRunAnim = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //�W�����v���Ă��āA�n�ʂɂ��Ă��鎞
        if (!_isJump && collision.gameObject.tag == "ground")
        {
            _isJump = true;
        }
    }

    //�V�[���ύX
    public void ChangeScene(string name)
    {
        //�p���`���Ă��Ȃ��Ƃ��̓V�[����ύX���Ȃ�
        if (!_isPunch) return;

        //�p���`���Ă��Ȃ��悤�ɂ���
        _isPunch = false;

        //�X�e�[�W�{�b�N�X�̖��O����A�ύX����V�[����I��
        if (name == "stage1")
        {
            _manager.ChangeScene("Stage1Scene");
        }
        if(name == "stage2")
        {
            _manager.ChangeScene("Stage2Scene");
        }
        if (name == "stage3")
        {
            //_manager.ChangeScene("PlayingScene");
        }
        if(name == "boss")
        {
            //_manager.ChangeScene("PlayingScene");
        }
        if(name == "title")
        {
            _manager.ChangeScene("TitleScene");
        }
    }

}
