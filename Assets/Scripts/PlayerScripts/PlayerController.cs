using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer = default;//�n�ʃI�u�W�F�N�g�̃��C���[

    private Rigidbody2D _playerRb = default;

    private bool _isControll = false;//true�ő���\
    private bool _isInOperation = false;//true�Ńv���C���[�𑀍쒆
    private bool _isJump = false;//true�ŃW�����v��
    private bool _isOnConveyor = false;//true���ƃR���x�A�̏�ɍڂ��Ă���
    private bool _isOnIceFloorGimmic = default;//true�Ŋ��鏰�̏�ɍڂ��Ă���
    private bool _isOnMudFloorGimmic = false;//true�œD�̏��̏�ɂ���
    private bool _isDoubleJump = false;//true�Ń_�u���W�����v���\�ɂȂ�
    private bool _isInTheWater = false;
    private bool _isPlayerGravity = true;//false�͔��d��

    private float _leftMoveSpeed = 6f;//���ړ����x
    private float _rightMoveSpeed = 6f;//�E�ړ����x
    private float _jumpPower = 15f;//�W�����v��
    private float _boxRayPositionY = 0.025f;//�{�b�N�X���C��Y�|�W�V����
    private float _boxRayScaleX = 0.5f;
    private float _boxRayScaleY = 0.05f;


    private SpriteRenderer _playerSprite = default;

    private Animator _playerAnimation = default;

    private const float DEFAULT_MOVE_SPEED = 6f;//�ʏ�̈ړ����x

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

        //���Ɉړ�
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
        //�E�Ɉړ�
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
        //�ړ��L�[��b������
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            _playerAnimation.SetBool("isWalk", false);
            //�ړ����~�߂�
            _isInOperation = false;

            //���鏰�̏�ɍڂ��Ă��Ȃ�������
            if (!_isOnIceFloorGimmic)
            {
                _playerRb.velocity = new Vector2(0, _playerRb.velocity.y);
            }
        }

        //�����R���x�A�̏�ɂ̂��Ă��邩�A�v���C���[�𑀍쒆����Ȃ�������
        if (_isOnConveyor && !_isInOperation)
        {
            OnConveyorAutoMove();
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�����ł̃W�����v
            if (_isInTheWater)
            {
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpPower / 2);
            }
            //�ʏ�̃W�����v
            else if (!_isJump)
            {
                _isJump = true;
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpPower);
            }
            //��i�W�����v
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

        //���ɂ��Ă���
        if (isGrounded)
        {
            _isJump = false;
            _playerAnimation.SetBool("isJump", false);
        }
        //���ɂ��Ă��Ȃ�
        else
        {
            _isJump = true;
            _playerAnimation.SetBool("isJump", true);
        }
    }


    /// <summary>
    /// �R���x�A�̏�ɍڂ��Ă������̎����œ�������
    /// </summary>
    private void OnConveyorAutoMove()
    {
        float autoMoveSpeed = 4.5f;

        //�������ړ��������Ȃ��Ă�����
        if (_leftMoveSpeed > DEFAULT_MOVE_SPEED)
        {
            //�����ō��Ɉړ�
            _playerRb.velocity = new Vector2(-autoMoveSpeed, _playerRb.velocity.y);
        }
        //�����E�ړ��͑����Ȃ��Ă�����
        else
        {
            //�����ŉE�Ɉړ�
            _playerRb.velocity = new Vector2(autoMoveSpeed, _playerRb.velocity.y);
        }
    }

    public void ControllOff()
    {
        //���ׂĂ������l�ɖ߂�
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
        //��Ԃ����ׂă��Z�b�g
        this.gameObject.layer = 6;
        ExitConveyor();
        JudgeOnIceFloor(false);
    }

    public void Death()
    {
        //�ړ��ł��Ȃ����āA������ł��Ȃ�����
        _playerRb.velocity = Vector2.zero;
        _isControll = false;
        _isInOperation = false;
        _seAudio.PlaySE(_deathSE);

        //�v���C���[������
        //this.gameObject.SetActive(false);
    }
    public void AutoLeftMove()
    {
        float changeLeftMovespeed = 10f;
        float changeRightMovespeed = 3f;

        //���ړ��𑁂��A�E�ړ���x������
        _leftMoveSpeed = changeLeftMovespeed;
        _rightMoveSpeed = changeRightMovespeed;
        _isOnConveyor = true;
    }

    public void AutoRightMove()
    {
        float changeLeftMovespeed = 3f;
        float changeRightMovespeed = 10f;

        //�E�ړ��𑁂��A���ړ���x������
        _leftMoveSpeed = changeLeftMovespeed;
        _rightMoveSpeed = changeRightMovespeed;
        _isOnConveyor = true;
    }
    /// <summary>
    /// �v���C���[�ɕǊђʔ\�͂�^���郁�\�b�h
    /// </summary>
    public void ExitConveyor()
    {
        //�R���x�A���痣�ꂽ��ړ����x�����Ƃɖ߂�
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
    /// �v���C���[�̏d�͂�ύX
    /// </summary>
    /// <param name="isGravity">//true�������牺�����̏d�́Afalse�������������̏d��</param>
    public void ChangeGravityScale(bool isGravity)
    {
        int playerGravityScale = 5;

        //�������̏d�͂�������
        if (isGravity)
        {
            _playerRb.gravityScale = playerGravityScale;
            _playerSprite.flipY = false;
        }
        //������̏d�͂�������
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
        //�d�͂��ς�����Ƃ��A�v���C���[�̑����Ƀ{�b�N�X���C���Ȃ�������
        if ((isFlipY && _boxRayPositionY < 0) || (!isFlipY && _boxRayPositionY > 0))
        {
            //�{�b�X�N���C�̈ʒu�𒲐�����
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
