using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    [SerializeField] private Transform _horizontalFulcrum = default;
    [SerializeField] private Transform _varticalFulcrum = default;

    [SerializeField] private Transform _circularSawObject = default;

    [SerializeField] private bool _isUpMove = false;
    [SerializeField] private bool _isDownMove = false;
    [SerializeField] private bool _isRightMove = false;
    [SerializeField] private bool _isLeftMove = false;

    private float _varticalMoveSpeed = 0;
    private float _horizontalMoveSpeed = 0;
    [SerializeField] private float _maxMoveSpeed = 0;

    private void Start()
    {
        _circularSawObject.position = this.transform.position;
    }
    void Update()
    {
        if (_circularSawObject != null)
        {
            CircularSawMoveJudge();
        }
    }

    private void CircularSawMoveJudge()
    {
        if (_isUpMove)
        {
            //丸のこが支点の一番上まで来ていたら
            if (_circularSawObject.localPosition.y > _varticalFulcrum.lossyScale.y / 2)
            {
                //下移動に切り替える
                _isUpMove = false;
                _isDownMove = true;
                _varticalMoveSpeed /= 2.5f;
            }
            else
            {
                CircularSawUpMove();
            }

        }
        else if (_isDownMove)
        {
            //丸のこが支点の一番上まで来ていたら
            if (_circularSawObject.localPosition.y < -_varticalFulcrum.lossyScale.y / 2)
            {
                //上移動に切り替える
                _isUpMove = true;
                _isDownMove = false;
                _varticalMoveSpeed /= 2.5f;
            }
            else
            {
                CircularSawDownMove();
            }
        }

        if (_isRightMove)
        {
            //丸のこが支点の一番右まで来ていたら
            if (_circularSawObject.localPosition.x > _horizontalFulcrum.lossyScale.x / 2)
            {
                //左移動に切り替える
                _isRightMove = false;
                _isLeftMove = true;
                _horizontalMoveSpeed /= 2.5f;
            }
            else
            {
                CircularSawRightMove();
            }
        }
        else if (_isLeftMove)
        {
            //丸のこが支点の一番左まで来ていたら
            if (_circularSawObject.localPosition.x < -_horizontalFulcrum.lossyScale.x / 2)
            {
                //右移動に切り替える
                _isRightMove = true;
                _isLeftMove = false;
                _horizontalMoveSpeed /= 2.5f;
            }
            else
            {
                CircularSawLeftMove();
            }
        }
    }

    private void CircularSawUpMove()
    {
        if (_varticalMoveSpeed > _maxMoveSpeed)
        {
            _circularSawObject.position += new Vector3(0, _maxMoveSpeed) * Time.deltaTime;
        }
        else
        {
            _varticalMoveSpeed += Time.deltaTime * _maxMoveSpeed;
            _circularSawObject.position += new Vector3(0, _varticalMoveSpeed) * Time.deltaTime;
        }
    }

    private void CircularSawDownMove()
    {
        if (_varticalMoveSpeed < -_maxMoveSpeed)
        {
            _circularSawObject.position += new Vector3(0, -_maxMoveSpeed) * Time.deltaTime;
        }
        else
        {
            _varticalMoveSpeed -= Time.deltaTime * _maxMoveSpeed;
            _circularSawObject.position += new Vector3(0, _varticalMoveSpeed) * Time.deltaTime;
        }
    }

    private void CircularSawLeftMove()
    {
        if (_horizontalMoveSpeed < -_maxMoveSpeed)
        {
            _circularSawObject.position += new Vector3(-_maxMoveSpeed, 0) * Time.deltaTime;
        }
        else
        {
            _horizontalMoveSpeed -= Time.deltaTime * _maxMoveSpeed;
            _circularSawObject.position += new Vector3(_horizontalMoveSpeed, 0) * Time.deltaTime;
        }

    }

    private void CircularSawRightMove()
    {
        if (_horizontalMoveSpeed > _maxMoveSpeed)
        {
            _circularSawObject.position += new Vector3(_maxMoveSpeed, 0) * Time.deltaTime;
        }
        else
        {
            _horizontalMoveSpeed += Time.deltaTime * _maxMoveSpeed;
            _circularSawObject.position += new Vector3(_horizontalMoveSpeed, 0) * Time.deltaTime;
        }
    }
}
