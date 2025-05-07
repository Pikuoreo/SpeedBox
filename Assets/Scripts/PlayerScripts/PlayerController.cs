using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer = default;//地面オブジェクトのレイヤー

    private Rigidbody2D _playerRb = default;

    private bool _isControll = false;//trueで操作可能
    private bool _isInOperation = false;//trueでプレイヤーを操作中
    private bool _isJump = false;//trueでジャンプ中
    private bool _isOnConveyor = false;//trueだとコンベアの上に載っている
    private bool _isOnIceFloorGimmic = default;//trueで滑る床の上に載っている
    private bool _isOnMudFloorGimmic = false;//trueで泥の床の上にいる
    private bool _isDoubleJump = false;//trueでダブルジャンプが可能になる
    private bool _isInTheWater = false;
    private bool _isPlayerGravity = true;//falseは反重力

    private float _leftMoveSpeed = 6f;//左移動速度
    private float _rightMoveSpeed = 6f;//右移動速度
    private float _jumpPower = 15f;//ジャンプ力
    private float _boxRayPositionY = 0.025f;//ボックスレイのYポジション
    private float _boxRayScaleX = 0.5f;
    private float _boxRayScaleY = 0.05f;


    private SpriteRenderer _playerSprite = default;

    private Animator _playerAnimation = default;

    private const float DEFAULT_MOVE_SPEED = 6f;//通常の移動速度

    Play_UI_Select_SE _seAudio = default;
    private BoxCollider2D _playerCollider = default;
    [SerializeField] private AudioClip _deathSE = default;

    //[SerializeField] private GameObject _deathAnimation = default;

    void Start()
    {
        _playerRb = this.GetComponent<Rigidbody2D>();
        _playerSprite = this.GetComponent<SpriteRenderer>();
        _playerAnimation = this.GetComponent<Animator>();
        _seAudio = Camera.main.GetComponent<Play_UI_Select_SE>();
        _playerCollider = this.GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (_isControll)
        {
            PlayerMove();
            JudgeJump();
        }
    }

    private void PlayerMove()
    {

        //左に移動
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && !_isOnMudFloorGimmic)
        {
            _playerAnimation.SetBool("isWalk", true);
            _isInOperation = true;

            if (!_playerSprite.flipX)
            {
                _playerSprite.flipX = true;
            }
            _playerRb.velocity = new Vector2(-_leftMoveSpeed, _playerRb.velocity.y);
        }
        //右に移動
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && !_isOnMudFloorGimmic)
        {
            _playerAnimation.SetBool("isWalk", true);
            _isInOperation = true;

            if (_playerSprite.flipX)
            {
                _playerSprite.flipX = false;
            }
            _playerRb.velocity = new Vector2(_rightMoveSpeed, _playerRb.velocity.y);
        }
        //移動キーを話したら
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            _playerAnimation.SetBool("isWalk", false);
            //移動を止める
            _isInOperation = false;

            //滑る床の上に載っていなかったら
            if (!_isOnIceFloorGimmic)
            {
                _playerRb.velocity = new Vector2(0, _playerRb.velocity.y);
            }
        }

        //もしコンベアの上にのっているかつ、プレイヤーを操作中じゃなかったら
        if (_isOnConveyor && !_isInOperation)
        {
            OnConveyorAutoMove();
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //水中でのジャンプ
            if (_isInTheWater)
            {
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpPower / 2);
            }
            //通常のジャンプ
            else if (!_isJump)
            {
                _isJump = true;
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpPower);
            }
            //二段ジャンプ
            else if (_isDoubleJump)
            {
                _playerAnimation.SetTrigger("Flipping");
                _isDoubleJump = false;
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpPower);
            }
        }
    }

    private void JudgeJump()
    {
        bool isGrounded = Physics2D.OverlapBox(this.transform.position - new Vector3(0, _boxRayPositionY),
                                               new Vector2(_boxRayScaleX, _boxRayScaleY),
                                               0,
                                               _groundLayer);

        Debug.DrawRay(this.transform.position - new Vector3(0, _boxRayPositionY), new Vector2(_boxRayScaleX, _boxRayScaleY), Color.red);

        //床についている
        if (isGrounded)
        {
            _isJump = false;
            _playerAnimation.SetBool("isJump", false);
        }
        //床についていない
        else
        {
            _isJump = true;
            _playerAnimation.SetBool("isJump", true);
        }
    }


    /// <summary>
    /// コンベアの上に載っていた時の自動で動く処理
    /// </summary>
    private void OnConveyorAutoMove()
    {
        float autoMoveSpeed = 4.5f;

        //もし左移動が早くなっていたら
        if (_leftMoveSpeed > DEFAULT_MOVE_SPEED)
        {
            //自動で左に移動
            _playerRb.velocity = new Vector2(-autoMoveSpeed, _playerRb.velocity.y);
        }
        //もし右移動は早くなっていたら
        else
        {
            //自動で右に移動
            _playerRb.velocity = new Vector2(autoMoveSpeed, _playerRb.velocity.y);
        }
    }

    public void ControllOff()
    {
        //すべてを初期値に戻す
        _playerRb.velocity = Vector2.zero;
        ChangeGravityScale(true);
        _playerRb.gravityScale = 0;
        _isControll = false;
        this.gameObject.layer = 9;
        _isDoubleJump = false;
        _playerAnimation.SetBool("isWalk", false);
        _playerAnimation.SetBool("isJump", false);
    }

    public void controllOn()
    {
        _isControll = true;
        //状態をすべてリセット
        this.gameObject.layer = 6;
        ExitConveyor();
        JudgeOnIceFloor(false);
    }

    public void Death()
    {
        //移動できなくして、操作もできなくする
        _playerRb.velocity = Vector2.zero;
        _isControll = false;
        _isInOperation = false;
        _seAudio.PlaySE(_deathSE);

        //プレイヤーを消す
        //this.gameObject.SetActive(false);
    }
    public void AutoLeftMove()
    {
        float changeLeftMovespeed = 10f;
        float changeRightMovespeed = 3f;

        //左移動を早く、右移動を遅くする
        _leftMoveSpeed = changeLeftMovespeed;
        _rightMoveSpeed = changeRightMovespeed;
        _isOnConveyor = true;
    }

    public void AutoRightMove()
    {
        float changeLeftMovespeed = 3f;
        float changeRightMovespeed = 10f;

        //右移動を早く、左移動を遅くする
        _leftMoveSpeed = changeLeftMovespeed;
        _rightMoveSpeed = changeRightMovespeed;
        _isOnConveyor = true;
    }
    /// <summary>
    /// プレイヤーに壁貫通能力を与えるメソッド
    /// </summary>
    public void ExitConveyor()
    {
        //コンベアから離れたら移動速度をもとに戻す
        _leftMoveSpeed = DEFAULT_MOVE_SPEED;
        _rightMoveSpeed = DEFAULT_MOVE_SPEED;
        _isOnConveyor = false;

        if (!_isInOperation)
        {
            _playerRb.velocity = new Vector2(0, _playerRb.velocity.y);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isOnIceFloor"></param>
    public void JudgeOnIceFloor(bool isOnIceFloor)
    {
        _isOnIceFloorGimmic = isOnIceFloor;
    }

    public void JudgeOnMudFloor(bool isOnMudFloor)
    {
        _isOnMudFloorGimmic = isOnMudFloor;
        if (isOnMudFloor)
        {
            _playerRb.velocity = Vector2.zero;
        }
    }

    public void PossibleDoubleJump()
    {
        _isDoubleJump = true;
    }

    /// <summary>
    /// プレイヤーの重力を変更
    /// </summary>
    /// <param name="isGravity">//trueだったら下向きの重力、falseだったら上向きの重力</param>
    public void ChangeGravityScale(bool isGravity)
    {
        int playerGravityScale = 5;

        //下向きの重力だったら
        if (isGravity)
        {
            _playerRb.gravityScale = playerGravityScale;
            _playerSprite.flipY = false;
        }
        //上向きの重力だったら
        else
        {
            _playerRb.gravityScale = -playerGravityScale;
            _playerSprite.flipY = true;
        }

        _isPlayerGravity = isGravity;
        BoxRayPositionChange(isGravity);
    }
    private void BoxRayPositionChange(bool isFlipY)
    {
        //重力が変わったとき、プレイヤーの足元にボックスレイがなかったら
        if ((isFlipY && _boxRayPositionY < 0) || (!isFlipY && _boxRayPositionY > 0))
        {
            //ボッスクレイの位置を調整する
            _boxRayPositionY *= -1;
            _jumpPower *= -1;
            _playerCollider.offset *= new Vector2(1, -1);
        }
    }

    public void ChangeGrabityForWaterGimmic(bool isInTheWater)
    {
        const float WATER_GRAVITY = 3.5f;
        const int DEFAULT_GRAVITY = 5;

        if (isInTheWater)
        {
            if (_isPlayerGravity)
            {
                _playerRb.gravityScale = WATER_GRAVITY;
                _isInTheWater = true;
            }
            else
            {
                _playerRb.gravityScale = -WATER_GRAVITY;
                _isInTheWater = true;
            }
        }
        else
        {
            if (_isPlayerGravity)
            {
                _playerRb.gravityScale = DEFAULT_GRAVITY;
                _isInTheWater = false;
            }
            else
            {
                _playerRb.gravityScale = -DEFAULT_GRAVITY;
                _isInTheWater = false;
            }

        }
    }
}
