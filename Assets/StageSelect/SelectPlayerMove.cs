using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤー
public class SelectPlayerMove : MonoBehaviour
{
    public SelectSceneManager _manager;//シーンを切り替える
    Animator _animator;//アニメーター
    private Transform _camPos;//カメラのトランスフォーム
    //前回の位置, 前回の位置からどれくらい進んだか, プレイヤーの回転,カメラの向いている方向, プレイヤー移動用, プレイヤー方向転換用
    Vector3 _lastPos, _diff, _rot, _cameraforward, _velocity, _animdir;
    Vector2 _stick;//移動方向
    float _moveSpeed, _jump, _stageEdge;//移動スピード、ジャンプ,ステージの端
    bool _isWalkAnim, _isRunAnim, _isDeath, _isJump,_isPunch;//歩きアニメーション,走るアニメーション,アニメーションを一回再生させる,ジャンプできるかどうか
    public AudioClip _hitSound, _jumpSound;//再生したい音
    AudioSource _audioSource;//サウンド再生

    // Start is called before the first frame update
    void Start()
    {
        _lastPos = new Vector3(-5.0f, 1.508f, 5.0f);//前回の位置
        _moveSpeed = 0.05f;//移動スピード
        _jump = 0.0f;//ジャンプ

        //カメラがあるときはトランスフォームを取得する。ないときは警告を表示
        if (Camera.main != null)
        {
            _camPos = Camera.main.transform;//カメラのトランスフォーム
        }
        else
        {
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }
        _cameraforward = Vector3.zero;//カメラの向いている方向
        _velocity = Vector3.zero;//プレイヤー移動用
        _animdir = Vector3.zero;//プレイヤー方向転換用

        _animator = this.gameObject.GetComponent<Animator>();//アニメーター
        _isWalkAnim = false;//歩くアニメーション再生
        _isRunAnim = false;//走るアニメーション再生

        _stageEdge = 12 / 2 - 0.2f;//ステージの端

        _rot = new Vector3(0.0f, 0.0f, 0.0f);//プレイヤーの回転

        _isDeath = false;//音を一回再生
        _isJump = false;//ジャンプ
        _isPunch = false;//パンチ

        _audioSource = this.GetComponent<AudioSource>();//音再生
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDeath) return;

        _rot = this.transform.localEulerAngles;//オイラー角
        this.transform.rotation = Quaternion.Euler(0.0f, _rot.y, 0.0f);

        _animator.SetBool("WalkFlag", _isWalkAnim);//歩きアニメーション
        _animator.SetBool("RunFlag", _isRunAnim);//走りアニメーション

        _diff = transform.position - _lastPos;//前回からどこに進んだかを取得
        _lastPos = transform.position;//前回のpositionの更新

        //ベクトルの大きさが0.01以上の時向きを変える
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
        //カメラのTransformが取得されていれば実行
        if (_camPos != null)
        {
            //二つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(normalized)
            _cameraforward = Vector3.Scale(_camPos.forward, new Vector3(1, 0, 1)).normalized;
            //移動ベクトル
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

        //ジャンプ
        _jump -= 0.01f;
        if (_jump < 0.0f)
        {
            _jump = 0.0f;
        }

        this.transform.position += new Vector3(_velocity.x, _jump, _velocity.z);

        Vector3 AnimDir = _velocity;
        _animdir.y = 0.0f;
        //方向転換
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

    //ジャンプ
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
    //移動
    public void OnMove(InputAction.CallbackContext context)
    {
        // 押しっぱなしの動作は、直接オブジェクトを動かすのではなく方向性のみを登録する
        _stick = context.ReadValue<Vector2>();
        
        _isPunch = false;

        if (!_isWalkAnim /*&& !_animator.GetBool("punchTrigger")*/)
        {
            _moveSpeed = 0.05f;
            _isWalkAnim = true;
        }
    }
    //パンチ
    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
           _animator.SetBool("PunchTrigger", true);
            _isPunch = true;
        }
        _stick = Vector2.zero;
    }
    //走る
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
        //ジャンプしていて、地面についている時
        if (!_isJump && collision.gameObject.tag == "ground")
        {
            _isJump = true;
        }
    }

    //シーン変更
    public void ChangeScene(string name)
    {
        //パンチしていないときはシーンを変更しない
        if (!_isPunch) return;

        //パンチしていないようにする
        _isPunch = false;

        //ステージボックスの名前から、変更するシーンを選ぶ
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
