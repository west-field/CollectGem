using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//プレイヤー
//移動はネットの記事を参考に
public class PlayerScript : MonoBehaviour
{
    public PlayingSceneManager _manager;//シーンを切り替える

    GameObject _bullet, _grandChild;//弾を生成,弾生成位置

    Animator _animator;//アニメーター
    
    private Transform _camPos;//カメラのトランスフォーム
    
    //前回の位置, 前回の位置からどれくらい進んだか, プレイヤーの回転,カメラの向いている方向, プレイヤー移動用, プレイヤー方向転換用
    Vector3 _lastPos, _diff, _rot, _cameraforward, _velocity, _animdir;
    
    Vector2 _stick;//移動方向
    
    float _moveSpeed,_jump, _stageEdge;//移動スピード、ジャンプ,ステージの端

    bool _isWalkAnim,_isRunAnim, _isDeath, _isJump;//歩きアニメーション,走るアニメーション,アニメーションを一回再生させる,ジャンプできるかどうか

    int _hp;//HP

    public AudioClip _hitSound,_jumpSound;//再生したい音
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

        _bullet = (GameObject)Resources.Load("bullet");//弾を生成する

        //弾生成位置
        _grandChild = transform.Find("CharacterArmature/Root/Body/Hips/Abdomen/Torso/Shoulder.R/UpperArm.R/LowerArm.R/Fist.R/Gun/Cannonball").gameObject;

        _hp = 3;//Hp
        _isDeath = false;//音を一回再生
        _isJump = false;//ジャンプ

        _audioSource = this.GetComponent<AudioSource>();//音再生
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDeath) return;

        _rot = this.transform.localEulerAngles;//オイラー角
        this.transform.rotation = Quaternion.Euler(0.0f, _rot.y, 0.0f);

        _animator.SetBool("walkFlag", _isWalkAnim);//歩きアニメーション
        _animator.SetBool("runFlag", _isRunAnim);//走りアニメーション

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

        if(this.transform.position.x + _velocity.x >= _stageEdge || this.transform.position.x + _velocity.x <= -_stageEdge)
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
        if(_animdir.sqrMagnitude >0.001f)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, AnimDir, 5.0f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        //スティックを使っていないときは、歩いていない
        if(_isWalkAnim && _stick.x == 0 && _stick.y == 0)
        {
            _isWalkAnim = false;
            _isRunAnim = false;
        }
        //走っていないけど移動スピードが走りスピードの時
        if (!_isRunAnim && _moveSpeed != 0.05f)
        {
            //歩きスピードに変更
            _moveSpeed = 0.05f;
        }

        //Hpが0になったらゲームオーバー
        if (_hp <= 0 && !_isDeath)
        {
            _animator.SetBool("deleteTrigger", true);
            _manager.ChangeScene("GameoverScene");
            _isDeath = true;
        }
    }
    //ジャンプ
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
    //移動
    public void OnMove(InputAction.CallbackContext context)
    {
        // 押しっぱなしの動作は、直接オブジェクトを動かすのではなく方向性のみを登録する
        _stick = context.ReadValue<Vector2>();
        
        if(!_isWalkAnim )
        {
            _moveSpeed = 0.05f;
            _isWalkAnim = true;
        }
    }
    //ショット
    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _animator.SetBool("shotTrigger", true);
            //弾を生成する
            Instantiate(_bullet,
                _grandChild.transform.position,
                Quaternion.identity);
        }
    }
    //走り
    public void OnRun(InputAction.CallbackContext context)
    {
        if (!_isRunAnim && !_animator.GetBool("punchTrigger"))
        {
            _moveSpeed *= 1.5f;
            _isRunAnim = true;
        }
    }
    //当たっているかどうか
    void OnCollisionEnter(Collision collision)
    {
        //敵と当たっているか
        if (collision.gameObject.tag == "enemy")
        {
            //敵と当たっていたらHPを引く
            _hp--;

            //HPによって表示を消す
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

            //ヒットアニメーション
            _animator.SetBool("hitTrigger", true);
            //ヒット音
            _audioSource.PlayOneShot(_hitSound);
            //移動しないように
            _velocity.x = 0.0f;
            _velocity.z = 0.0f;
        }

        //ジャンプしていて、地面に当たっていたら
        if (!_isJump && collision.gameObject.tag == "ground")
        {
            //次にジャンプできるように
            _isJump = true;
        }
    }
    //当たっているか
    void OnTriggerEnter(Collider collision)
    {
        //スターに当たっていたら
        if (collision.gameObject.tag == "star")
        {
            //ゲームクリア
            _manager.ChangeScene("ClearScene");
        }
    }
}
